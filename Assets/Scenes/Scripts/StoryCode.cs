using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryCode : MonoBehaviour
{
    public Text storyText;
    public Image storyImage;
    public Text Press;

    public Sprite[] storyImages;
    public string[] storyLine;

    private int count = 0;
    void Start()
    {
        GameSystem.instance.Init();
        StartCoroutine(Story());
        StartCoroutine(Blink());
    }
    IEnumerator Story()
    {
        while(count < storyImages.Length)
        {
            storyImage.sprite = storyImages[count];
            storyText.text = storyLine[count];
            if (Input.GetKeyDown(KeyCode.Space)) count++;
            yield return null;
        }
        SceneManager.LoadScene("Stage1");
    }
    IEnumerator Blink()
    {
		float ElapsedTime = 0f;
		while (count < storyImages.Length)
        {
            while(ElapsedTime < 0.5f)
            {
                ElapsedTime += Time.deltaTime;
                float t = ElapsedTime / 0.5f;
                Press.color = new Color(Press.color.r,
                    Press.color.g,
                    Press.color.b,
                    t);
                yield return null;
            }
            ElapsedTime = 0f;
			while (ElapsedTime < 0.5f)
			{
				ElapsedTime += Time.deltaTime;
				float t = ElapsedTime / 0.5f;
				Press.color = new Color(Press.color.r,
					Press.color.g,
					Press.color.b,
					1-t);
				yield return null;
			}
            ElapsedTime = 0f;
		}
    }
}
