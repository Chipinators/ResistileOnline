using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResistileClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MH_ServerBrowser : MonoBehaviour, MessageHanderInterface {
    public GameObject panelManager, contentView;
    public GameObject serverInfoPrefab;
    private int msgFromThread = -1;
    private ResistileMessage messageFromThread;

    void Start()
    {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().messageInterface = this;
        panelManager = GameObject.FindGameObjectWithTag("PanelManager");
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.getHostList, ""));
    }

    public void doAction(ResistileMessage message)
    {
        messageFromThread = message;
        switch (message.messageCode)
        {
            case ResistileMessageTypes.hostList:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.hostDeclined:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.startGame:
                msgFromThread = message.messageCode;
                break;
            case ResistileMessageTypes.hostNotFound:
                msgFromThread = message.messageCode;
                break;
            default:
                Debug.Log("Unrecognized Message Type: " + message.messageCode + " --- " + message.message);
                break;

        }
    }

    void Update()
    {
        switch (msgFromThread)
        {
            case ResistileMessageTypes.hostList:
                hostList(messageFromThread);
                break;
            case ResistileMessageTypes.hostDeclined:
                hostDeclined(messageFromThread);
                break;
            case ResistileMessageTypes.startGame:
                startGame(messageFromThread);
                break;
            case ResistileMessageTypes.hostNotFound:
                hostNotFound(messageFromThread);
                break;
        }
        msgFromThread = -1;
    }

    //RECEIVE MESSAGES FROM SERVER
    private void hostList(ResistileMessage message)
    {
        var hosts = message.messageArray;
        foreach (string host in hosts)
        {
            addHost(host);
        }
    }

    private void hostDeclined(ResistileMessage message)
    {
        //TODO: Tell user that host declined
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
    }

    private void startGame(ResistileMessage message)
    {
        SceneManager.LoadScene("Board");
    }

    private void hostNotFound(ResistileMessage message)
    {
        //TODO Add alert
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
        getHostList();
    }

    //SEND MESSAGES TO SERVER
    public void getHostList()
    {
        foreach(Transform obj in contentView.transform)
        {
            Destroy(obj.gameObject);
        }
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.getHostList, ""));
    }

    public void joinLobby(string username)
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.requestJoinGame, username));
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = false;
    }

    public void cancelRequest()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.cancelJoinRequest, ""));
        panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting = true;
    }

    public void goBack() //uses for back button, if viewing server list it sends a ping, if waiting for host sends cancelJoinRequest
    {
        if (panelManager.GetComponent<HostScreenPanelAdapter>().isWaiting) ping();
        else NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.cancelJoinRequest, ""));
        SceneManager.LoadScene("MainMenu");
    }

    private void ping()
    {
        NetworkManager.networkManager.sendMessage(new ResistileMessage(0, ResistileMessageTypes.ping, ""));
    }

    private void addHost(string username)
    {
        var host = Instantiate(serverInfoPrefab);
        host.transform.SetParent(contentView.transform, false);
        host.transform.FindChild("Username").GetComponent<Text>().text = username;
    }
}
