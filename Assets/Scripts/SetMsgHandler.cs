using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMsgHandler : MonoBehaviour {

    Button button;
    GameObject msgHandler;
	void Start () {
        msgHandler = GameObject.Find("MessageHandlerServerBrowser");
        string username = GetComponent<Button>().transform.parent.transform.FindChild("Username").GetComponent<Text>().text;
        GetComponent<Button>().onClick.AddListener(() => msgHandler.GetComponent<MH_ServerBrowser>().joinLobby(username));
    }

}
