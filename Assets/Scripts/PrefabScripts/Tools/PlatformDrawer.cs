using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDrawer : MonoBehaviour {

	public Transform otherTrans;

	public float position;

	private float minScale = 1.175789f;
	private float maxScale = 5.675f;
	private float minPlatPos = 48.19f;
	private float maxPlatPos = 58.19f;

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
