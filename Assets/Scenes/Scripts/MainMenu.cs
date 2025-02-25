using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject Car;
	
	public void GoRanking()
	{
		StartCoroutine(move(-1,0.5f));
	}

	public void GoMain()
	{
		StartCoroutine(move(1, 0.5f));
	}

	private IEnumerator move(int d, float duration)
	{
		Vector3 curPos = Car.transform.position;

		float ElapsedTime = 0f;
		while (ElapsedTime < duration)
		{
			ElapsedTime += Time.deltaTime;
			float t = ElapsedTime / duration;
			Car.transform.position = Vector3.Lerp(curPos, curPos + new Vector3(d, 0, 0), t);
			yield return null;
		}
	}
	public void ExitGame()
	{
		Application.Quit();
	}
	public void StartGame()
	{
		SceneManager.LoadScene("Stage1");
	}
}
