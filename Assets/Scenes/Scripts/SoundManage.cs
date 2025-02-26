using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
	public void SoundPlay(string name, AudioClip ac)
	{
		GameObject gameObject = new GameObject(name);
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();

		audioSource.clip = ac;
		audioSource.loop = false;
		audioSource.ignoreListenerPause = true;
		audioSource.Play();

		// TimeScale 영향을 받지 않는 코루틴 실행
		StartCoroutine(DestroyAfterTime(gameObject, ac.length));
	}

	private IEnumerator DestroyAfterTime(GameObject obj, float duration)
	{
		yield return new WaitForSecondsRealtime(duration); // TimeScale 영향 X
		Destroy(obj);
	}

}
