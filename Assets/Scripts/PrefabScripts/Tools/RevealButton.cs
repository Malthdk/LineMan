using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealButton : MonoBehaviour {

	public List <GameObject> revealObjects;
	public List <SpriteRenderer> fadeObjects;

//	public bool isEnabled;
	private SpriteRenderer spriteRend;
	private Color color;

	void Start () 
	{
		spriteRend = transform.GetComponentInChildren<SpriteRenderer>();
		color = Color.black;
		//FLipState(revealObjects);
	}
	

	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			spriteRend.enabled = false;
			StartCoroutine("HideOrShow");
			Debug.Log("yoyo");
		}
	}


	IEnumerator HideOrShow()
	{
		FLipState(revealObjects);

		yield return new WaitForSeconds(1f);

		while (color.a > 0)
		{
			Fade(fadeObjects);
			yield return new WaitForEndOfFrame();
		}
	}

	void Fade(List<SpriteRenderer> theList)
	{
		for (int i = 0; i < theList.Count; i++)
		{
			Debug.Log("Fading Color");
			color = theList[i].material.color;
			color.a -= 0.01f;
			theList[i].material.color = color;
		}
	}

	void FLipState(List<GameObject> theList)
	{
		Debug.Log("Showing or hiding");
		for (int i = 0; i < theList.Count; i++)
		{
//			if (!isEnabled)
//			{
			theList[i].SetActive(true);
//				isEnabled = true;
//			}
//			else if (isEnabled)
//			{
//				theList[i].SetActive(false);
//				isEnabled = true;
//			}
		}
	}
}
