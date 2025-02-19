using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManage : MonoBehaviour
{
	[Header("Ready")]
    public Timer timer;
    public KartController controller;
    public BezierFollow beizerFollow;
	public bool GameStart = false;

	[Header("Store")]
	public GameObject Store;
	public bool isStore = false;


	private void OnEnable()
	{
		GameSystem.instance.Init();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	private void StartGame()
	{
		StartCoroutine(timer.Disappear(0.5f));
		controller.enabled = true;
		beizerFollow.enabled = true;
		timer.StartCount();
		controller.Startboost();
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//처음 씬이 로드될 때 초기화 시킬 것들과 준비해야 할 것들 적기
		controller.enabled = false;
		beizerFollow.enabled = false;
		timer.StopCount();

		StartCoroutine(timer.CountDown(StartGame));
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	/// <summary>
	/// 등수 및 점수와 우승상금 등 게임이 종료된 후에 실행될 로직
	/// </summary>
	public void GameOver()
	{
		Debug.Log("Game is Done!");
		timer.StopCount();
	}

	public void OpenStore()
	{
		isStore = true;
		Store.SetActive(true);
	}
	public void CloseStore()
	{
		isStore = false;
		Store.SetActive(false);
	}
}
