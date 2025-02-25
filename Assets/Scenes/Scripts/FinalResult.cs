using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalResult : MonoBehaviour
{
    public Text Score;
    public InputField NameInput;
    public Text WarningLog;

    public GameObject Button;
    void Awake()
    {
        Score.text = "최종 스코어 : " + GameSystem.instance.Score.ToString("000000");
    }
    public void NameCheck()
    {
        if(NameInput.text.Length > 6)
        {
            Button.SetActive(false);
            WarningLog.gameObject.SetActive(true);

        }
        else if(NameInput.text.Length < 2)
        {
            Button.SetActive(false);
        }
        else
        {
            Button.SetActive(true);
            WarningLog.gameObject.SetActive(false);
        }
    }
    public void NameDone()
    {
        GameSystem.instance.RegisterRanking(NameInput.text, GameSystem.instance.Score);
        SceneManager.LoadScene("StartScene");
    }
}
