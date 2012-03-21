using UnityEngine;
using System.Collections;
//DataController h;

public class GameHelper : MonoBehaviour {
	DataController h;
  public static void SendMessageToAll(string methodName, object argument, SendMessageOptions options){
    foreach(GameObject go in FindObjectsOfType(typeof(GameObject)))
      go.SendMessage(methodName, argument, options);
  }
}
