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

	public Text GetMoney;

	public GameManage gameManage;
	public StoreManage storeManage;

	public AudioClip Win;
	public AudioClip Lose;

	private void OnEnable()
	{
		if (gameManage.PlayerWin)
		{
			gameManage.BGM.Stop();
			storeManage.sound.SoundPlay("Win", Win);
			Title.text = "GAME CLEAR!";
			int score = (int)(1000000 / gameManage.timer.TimerCount - 50);
			//점수 합계 추가하기
			GameSystem.instance.Score += score;

			int AddMoney = 0;

			switch (SceneManager.GetActiveScene().name)
			{
				case "Stage1":
					AddMoney = 50000;
					break;
				case "Stage2":
					AddMoney = 100000;
					break;
				case "Stage3":
					AddMoney = 150000;
					break;
			}

			GameSystem.instance.Money += AddMoney;
			GameSystem.instance.SaveItemData();
			GetMoney.text = $"You got {AddMoney}$!";
			Debug.Log("Current Score : " + GameSystem.instance.Score);
			Score.text = $"Score : {score.ToString("000000")}";
			if (SceneManager.GetActiveScene().name.Equals("Stage3"))
			{
				buttonText.text = "Next";
			}
			else
			{
				buttonText.text = "Next Stage";
			}
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
			gameManage.BGM.Stop();
			storeManage.sound.SoundPlay("Lose", Lose);
			Title.text = "GAME OVER...";
			GetMoney.text = "";
			int score = (int)(1000000 / gameManage.timer.TimerCount - 50);
			//점수 합계 추가하기
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
