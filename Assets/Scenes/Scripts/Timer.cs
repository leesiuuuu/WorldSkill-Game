using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [HideInInspector]
    public float TimerCount = 0f;

    [SerializeField]
    private GameObject CountDownObj;

    [SerializeField]
    private Text TimerText;

    [SerializeField]
    private GameObject BGM;

    private GameObject obj;

    private bool Counting = true;

    public void InitCount()
    {
        TimerCount = 0f;
        Counting = false;
    }

    public void StartCount()
    {
        Counting = true;
    }

	public void StopCount()
    {
        Counting = false;
    }
	private void Update()
	{
		if (Counting)
		{
			TimerCount += Time.deltaTime;
            TimerText.text = $"{((int)(TimerCount / 60)).ToString("00")}:{((int)(TimerCount % 60)).ToString("00")}.{((int)((TimerCount % 1) * 100)).ToString("00")}";
		}
	}

    public IEnumerator CountDown(Action onComplete)
    {
        obj = Instantiate(CountDownObj);
        obj.transform.SetParent(GameObject.Find("Canvas").transform, false);
        
        Text text = obj.GetComponent<Text>();
        CountDownSound cds = text.GetComponent<CountDownSound>();

		cds.SoundLoad();
		text.text = "3";
        yield return new WaitForSeconds(1);
		cds.SoundLoad();
		text.text = "2";
		yield return new WaitForSeconds(1);
		cds.SoundLoad();
		text.text = "1";
		yield return new WaitForSeconds(1);
        text.text = "GO!";
		cds.SoundLoad();
        BGM.SetActive(true);
		onComplete?.Invoke();
        yield break;
    }
    public IEnumerator Disappear(float Delay)
    {
		yield return new WaitForSeconds(Delay);
        
        Text text = obj.GetComponent<Text>();

		float ElapsedTime = 0f;
		while (ElapsedTime < 1f)
		{
			ElapsedTime += Time.unscaledDeltaTime;
			float t = ElapsedTime / 1f;
			text.color = new Color(
				text.color.r,
				text.color.g,
				text.color.b,
				1 - t);
			yield return null;
		}
        Destroy(obj);
	}
}
