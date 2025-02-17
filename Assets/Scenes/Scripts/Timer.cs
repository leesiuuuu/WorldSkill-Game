using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float TimerCount = 0f;

    [SerializeField]
    private Text TimerText;

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
            TimerText.text = $"{((int)(TimerCount / 60)).ToString("00")}:{((int)(TimerCount % 60)).ToString("00")}.{((TimerCount % 1) * 100).ToString("00")}";
		}
	}
}
