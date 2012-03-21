using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {
	
	public struct PlatformData{
		GameObject platform;
		int size;
	}
	
	public enum GameTypes{
		None,
		Surival,
		Story
	}
	
	
	public GameObject[] platformPrefabs;
	public GameObject[] platformSpawnPoints;
	
	public Component platformSpawnHeights;
	
	private DataController h;
	
	private float startTime = 0f;
	private float curTime= 0f;
	
	float data;
	
	// Use this for initialization
	void Start () {
		h = (DataController)GameObject.Find("ThinkGear").GetComponent(typeof(DataController));
	}
	//data=h.headsetData.highAlpha;
	// Update is called once per frame
	void Update () {
		curTime = Time.deltaTime;
		data=h.headsetData.highAlpha;
		GameHelper.SendMessageToAll("onRequestAlpha", data,null);
	/*	data=h.headsetData.highBeta;
		GameHelper.SendMessageToAll(getBeta, data);
		data=h.headsetData.highGamma;
		GameHelper.SendMessageToAll(getGamma, data);
		data=h.headsetData.theta;
		GameHelper.SendMessageToAll(getTheta, data);
		data=h.headsetData.attention;
		GameHelper.SendMessageToAll(getAttention, data);
		data=h.headsetData.meditation;
		GameHelper.SendMessageToAll(getMeditation, data);
		*/
	}
}
