using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class FakeMindSetOutput : IBrainwaveDataPlayer {
  private System.Random r;
  private DateTime startTime;
  private DateTime currentTime;
  private DateTime lastFrameTime;

  private float blinkValue;
  private double deltaSeconds;
  private double elapsedSeconds;

  private const float BLINK_PERIOD = 0.5f; 

  public FakeMindSetOutput(){
    r = new System.Random();
    startTime = currentTime = lastFrameTime = DateTime.Now;
    blinkValue = 0.0f;
  }

  public ThinkGearData DataAt(double secondsFromBeginning){
    currentTime = DateTime.Now;
    
    deltaSeconds = currentTime.Subtract(startTime).TotalSeconds; 

    elapsedSeconds += currentTime.Subtract(lastFrameTime).TotalSeconds; 

    if(elapsedSeconds > BLINK_PERIOD){
      blinkValue = (int)(256.0f * (float)r.NextDouble());
      elapsedSeconds -= BLINK_PERIOD;
    }

    lastFrameTime = currentTime;

    return new ThinkGearData(secondsFromBeginning,
                             (int)(Math.Sin(deltaSeconds / 3.0f) * 50.0f + 50.0f),
                             (int)(Math.Cos(deltaSeconds / 3.0f) * 50.0f + 50.0f), 
                             (int)(Math.Sin(deltaSeconds / 7.0f) * 50.0f + 50.0f),
                             0,
                             RandomValue(), RandomValue(),
                             RandomValue(), RandomValue(),
                             RandomValue(), RandomValue(),
                             RandomValue(), RandomValue(),
                             512.0f * (float)r.NextDouble(),
                             blinkValue);
  }

  public int dataPoints {
    get { return -1; }
  }

  public double duration {
    get { return 0.0; }
  }

  private float RandomValue(){
    return (float)Math.Exp(r.NextDouble() * 7.0 - 6.0);
  }
}
