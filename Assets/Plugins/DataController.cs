using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class DataController : MonoBehaviour {

  public ThinkGearData headsetData;
  public IBrainwaveDataPlayer dataPlayer;
  public IBrainwaveDataPlayer standbyPlayer;

  private long lastPacketReadTime;

  private Thread updateThread;
  private bool updateDataThreadActive = true;

  private double elapsedTime = 0.0;
  private bool enableDemoMode = false;
  private bool isConnected = false;

  private const float TIMEOUT_INTERVAL = 1.5f;
  private const long TIMEOUT_INTERVAL_TICKS = (long)(TIMEOUT_INTERVAL * 10000000);

  private string headsetDataLog = "";

  private bool hasBlinked = false;
  private int blinkStrength = 0;

  public bool IsOffHead {
    get { return !isConnected || headsetData.poorSignalValue >= 200; }
  }

  public bool IsHeadsetInitialized {
    get { return isConnected; }
  }

  public bool IsDemo {
    get { return enableDemoMode; }
  }

  public bool IsESenseReady {
    get { return isConnected && (headsetData.attention != 0 && headsetData.meditation != 0); }
  }

  void Awake(){
    dataPlayer = new FakeMindSetOutput();
    updateThread = new Thread(UpdateDataValuesThread);
  }

  void Update(){
    elapsedTime += Time.deltaTime;

    if(hasBlinked){
      GameHelper.SendMessageToAll("OnBlinked", headsetData.blink, SendMessageOptions.DontRequireReceiver);
      hasBlinked = false;
    }
  }

  void OnHeadsetConnected(string portName){
    isConnected = true;
    
    enableDemoMode = portName == "DemoMode";

    standbyPlayer = dataPlayer;

    dataPlayer = enableDemoMode ? 
                    (IBrainwaveDataPlayer)new FakeMindSetOutput() : 
                    (IBrainwaveDataPlayer)new MindSetOutput(MindSetVersions.ASIC);

    lastPacketReadTime = DateTime.Now.Ticks;

    Invoke("UpdateDataValues", 0.0f); 
  }

  void OnHeadsetDisconnected(){
    isConnected = false;
    enableDemoMode = false;

    updateDataThreadActive = false;

    dataPlayer = standbyPlayer;
    headsetDataLog = "";

    if(updateThread != null && updateThread.IsAlive)
      updateThread.Abort();
  }

  private void UpdateDataValues(){
    updateDataThreadActive = true;

    if(updateThread == null || !updateThread.IsAlive){
      updateThread = new Thread(UpdateDataValuesThread);
      updateThread.Start();
    }
  }

  private void UpdateDataValuesThread(){
    while(updateThread.IsAlive && updateDataThreadActive){
      // Update/refresh the data values
      int readResult = enableDemoMode ? 0 : ThinkGear.ReadPackets(-1);

      if(readResult < 0){
        // if we haven't seen a valid packet in a while, then the headset was probably
        // disconnected. do a cleanup.
        if(DateTime.Now.Ticks - lastPacketReadTime > TIMEOUT_INTERVAL_TICKS){
          Debug.Log("Headset data receipt timed out.");
          GameHelper.SendMessageToAll("OnRequestHeadsetDisconnect", null, SendMessageOptions.DontRequireReceiver);
        }
      }
      else {
        lock(this){
          headsetData = dataPlayer.DataAt(elapsedTime);
          
          if(headsetData.blink != blinkStrength){
            blinkStrength = (int)headsetData.blink;
            hasBlinked = true;
          }
        }

        lastPacketReadTime = DateTime.Now.Ticks;
      }

      Thread.Sleep(20);
    }
  }
}

