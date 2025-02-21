using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
	public Text Title;
	public Text Score;
	public Text buttonText;

	public Button NextStage;
	public Button Main;

	public GameManage gameManage;
	private void OnEnable()
	{
		if (gameManage.PlayerWin)
		{
			Title.text = "GAME CLEAR!";
			int score = (int)(1000000 / gameManage.timer.TimerCount - 50);
			//점수 합계 추가하기
			PlayerPrefs.SetInt("score", score);
			PlayerPrefs.Save();
			Score.text = $"Score : {score.ToString("000000")}";
			buttonText.text = "Next Stage";
			Main.gameObject.SetActive(false);
			NextStage.onClick.RemoveAllListeners();
			NextStage.onClick.AddListener(() =>
			{
				string name = SceneManager.GetActiveScene().name;
				int count = int.Parse(name[name.Length - 1].ToString()) + 1;
				gameManage.StageLoad(name.Remove(name.Length-1) + count);
			});
		}
		else
		{
			Title.text = "GAME OVER...";
			int score = (int)(1000000 / gameManage.timer.TimerCount - 50);
			//점수 합계 추가하기
			PlayerPrefs.SetInt("score", score);
			PlayerPrefs.Save();
			Score.text = $"Score : {score.ToString("000000")}";
			buttonText.text = "Retry";
			Main.gameObject.SetActive(true);
			Main.onClick.AddListener(() =>
			{
				gameManage.StageLoad("Main");
			});
			NextStage.onClick.RemoveAllListeners();
			NextStage.onClick.AddListener(() =>
			{
				gameManage.StageLoad(SceneManager.GetActiveScene().name);
			});
		}
	}
}
