using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownSound : MonoBehaviour
{
	public AudioClip Ready;
	public AudioClip Go;
	public AudioSource AS;
	public void SoundLoad()
	{
		if (gameObject.GetComponent<Text>().text.Equals("GO!"))
		{
			AS.clip = Go;
			AS.Play();
		}
		else
		{
			AS.clip = Ready;
			AS.Play();
		}
	}
}
