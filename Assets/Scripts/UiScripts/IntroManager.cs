using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {

	public Text actText, quoteText, personText;
	public string nextLevel;

	void Awake () 
	{
		
	}

	void Start()
	{
		StartCoroutine("InitiateScene");
	}

	void Update () 
	{
		
	}

	IEnumerator InitiateScene()
	{

		yield return new WaitForSeconds(2f);

		while (actText.color.a < 1f)
		{
			Fade(actText);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(3f);

		while (quoteText.color.a < 1f)
		{
			Fade(quoteText);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(7f);

		while (personText.color.a <= 1f)
		{
			Fade(personText);
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(5f);

		SceneManager.LoadScene(nextLevel);
	}

	void Fade(Text text)
	{
		Color color;
		color = text.color;
		color.a += 0.01f;
		text.color = color;
	}
}
