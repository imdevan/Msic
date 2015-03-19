using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	string registeredGameName = "SXSW_test_server";
	bool isRefreshing = false;
	float refreshRequestLength = 3.0f;
	HostData[] hostData;
	
	
	private void StartServer()
	{
		Network.InitializeServer (2, 25003, !Network.HavePublicAddress());
		MasterServer.RegisterHost (registeredGameName, "Test Game", "Test implementation of server code");
		
	}
	
	/*void OnServerInitialized()
	{
		Debug.Log ("Server has been initialized");
	}
	
	void OnMasterServerEvent( MasterServerEvent masterServerEvent)
	{
		if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
			Debug.Log ("Registration Successful");
	}*/
	
	public IEnumerator RefreshHostList()
	{
		
		Debug.Log ("Refreshing...");
		MasterServer.RequestHostList (registeredGameName);
		float timeStarted = Time.time;
		float timeEnd = Time.time + refreshRequestLength;
		
		while (Time.time < timeEnd)
		{
			hostData = MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}
		
		if (hostData == null || hostData.Length == 0) {
			Debug.Log ("No active servers have been found");
		} else
			Debug.Log (hostData.Length + "have been found");
	}

	private void SpawnPlayer()
	{
		Debug.Log ("Spawning Player..");
		Network.Instantiate(Resources.Load("Prefabs/SamplePlayer"), new Vector3(0f, 2.5f, 0f), Quaternion.identity, 0);
	}

	/// /////Call backs from client and server	///

	void OnConnectedToServer()
	{
	}

	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
	}

	void OnFailedToConnect(NetworkConnectionError error)
	{
	}

	void OnMasterServerEvent(MasterServerEvent masterServerEvent)
	{
		if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
			Debug.Log ("Registration Successful");
	}

	void OnServerInitialized()
	{
		SpawnPlayer ();
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log ("Player disconnected from: " + player.ipAddress + ":" + player.port);
		Network.RemoveRPCs (player);
		Network.DestroyPlayerObjects (player);
	}

	void OnFailedConnectToMasterServer(NetworkConnectionError info)
	{
	}

	void OnNetworkInstantiate(NetworkMessageInfo info)
	{
	}

	void OnApplicationQuit()
	{
		if (Network.isServer) 
		{
			Network.Disconnect(200);
			MasterServer.UnregisterHost();
		}
		if (Network.isClient) 
		{
			Network.Disconnect(200);
		}
	}

	
	public void OnGUI()
	{

		if (Network.isServer)
			GUILayout.Label ("Running as a Server.");
		else if (Network.isClient)
			GUILayout.Label ("Running as a Client.");

		if (Network.isClient) 
		{
			if (GUI.Button (new Rect (25f, 25f, 150f, 30f), "Spawn"))
				SpawnPlayer();
		}

		if (!Network.isClient && !Network.isServer) 
		{
			if (GUI.Button (new Rect (25f, 25f, 150f, 30f), "Start New Server")) {
				StartServer ();
			}
		
			if (GUI.Button (new Rect (25f, 65f, 150f, 30f), "Refresh Server List")) {
				StartCoroutine ("RefreshHostList");
			}
		
			if (hostData != null) {
				for (int i = 0; i < hostData.Length; i++) {
					if (GUI.Button (new Rect (Screen.width / 2, 65f + (30f * i), 300f, 30f), hostData [i].gameName)) {
						Network.Connect (hostData [i]);
					}
				}
			}
		}
		
	}
	
}
