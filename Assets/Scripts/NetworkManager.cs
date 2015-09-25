using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{

    public Console console;
    public ButtonHandler buttonHandler;
    bool GameReady = false;

    private Room room;

	// Use this for initialization
	void Start ()
    {
        Connect();
	
	}


    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("Sannemer 0.01");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }


    // First try to join a random room
    // if that fails make a room
    void OnConnectedToMaster()
    {
        //Debug.Log("Joined Lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        //Debug.Log("OnPhotonRandomJoinFailed");
        PhotonNetwork.CreateRoom("Sannemer");
    }


    // Everything worked so figure out how to start the game.
    void OnJoinedRoom()
    {
        room = PhotonNetwork.room;
    }

    void GameStart()
    {

    }

    void Update()
    {
        if (!GameReady && room != null)
        {
            if (room.playerCount == 1)
            {
                //Debug.Log("Waiting for other player");
                console.Display("Waiting for other player");
            }

            if (room.playerCount == 2)
            {
                //Debug.Log("Everyone is seated, press the button to begin");
                console.Display("Everyone is seated press the button to begin", new List<string>() { "Play" });
                console.buttons[0].onClick.AddListener(() => buttonHandler.StartMulti());
                GameReady = true;
            }
        }
    }

}
