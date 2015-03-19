using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	#region fields

	private Transform mTransform = null;
	private float speed = 10f;

	#endregion

	#region Unity Methods

	// Use this for initialization
	void Start () 
	{
		if (!GetComponent<NetworkView>().isMine)
			enabled = false;
		mTransform = this.transform;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<NetworkView>().isMine)
			return;

		if (Input.GetKey (KeyCode.W))
			mTransform.Translate (Vector3.forward * speed * Time.deltaTime);

		if (Input.GetKey (KeyCode.S))
			mTransform.Translate (-Vector3.forward * speed * Time.deltaTime);

		if (Input.GetKey (KeyCode.A))
			mTransform.Translate (-Vector3.right * speed * Time.deltaTime);

		if (Input.GetKey (KeyCode.D))
			mTransform.Translate (Vector3.right * speed * Time.deltaTime);
	
	}
	void OnDestroy()
	{
		Debug.Log (name + " has their sample player destroyed");
	}
	#endregion
}
