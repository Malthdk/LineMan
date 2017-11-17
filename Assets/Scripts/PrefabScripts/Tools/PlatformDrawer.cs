using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDrawer : MonoBehaviour {

	public Transform otherTrans;

	public float position;

	public float minScale = 1.175789f;
	public float maxScale = 5.675f;
	public float minPlatPos = 48.19f;
	public float maxPlatPos = 58.19f;

	void Start () 
	{
		position = transform.position.x;
	}
	

	void Update () 
	{
		position = transform.position.x;
		HandleSize();
	}

	void HandleSize()
	{
		otherTrans.localScale = new Vector3(0.437499f, Map(position, minPlatPos, maxPlatPos, minScale, maxScale), 1f);
	}

	public float Map(float value, float inMin, float inMax, float outMin, float outMax)
	{
		return(value-inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

}
