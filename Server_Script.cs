using UnityEngine;
using System.Collections;

public class Server_Script : MonoBehaviour {
	private const string NAME_OF_GAME = "Beat_Trip";
	private const string SERVER_NAME = "SXSW_Server";
	private HostData[] hosts;
	
	void OnServerInitialized()								//virtual function
	{
		Debug.Log("Server Initializied");
	}
	void OnConnectedToServer()								//virtual function
	{
		Debug.Log("Server Joined");
	}
	void OnMasterServerEvent(MasterServerEvent msEvent)		//virtual function
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hosts = MasterServer.PollHostList();
	}
	
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
			{
				//MasterServer.ipAddress = "10.20.16.43";
				Network.InitializeServer(2, 25588, !Network.HavePublicAddress());
				MasterServer.RegisterHost(NAME_OF_GAME, SERVER_NAME);
			}

			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				MasterServer.RequestHostList(NAME_OF_GAME);
			
			if (hosts != null)
			{
				for (int i = 0; i < hosts.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hosts[i].gameName))
						Network.Connect(hosts[i]);
				}
			}
		}
	}


}
