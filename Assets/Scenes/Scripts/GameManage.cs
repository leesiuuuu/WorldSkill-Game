using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.Animations;

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

	[Header("Item")]
	public Sprite[] ItemImage = new Sprite[5];
	public GameObject ItemPanel;

	[Header("End")]
	public GameObject Camera;
	public Vector3 CameraPos;
	public Vector3 CameraRot;
	public bool PlayerWin = true;
	public GameObject ResultPanel;

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
		controller.enabled = false;
		Camera.transform.position = CameraPos;
		Camera.transform.rotation = Quaternion.Euler(CameraRot);
		Camera.GetComponent<CameraFollowPlayer>().enabled = false;
		ResultPanel.SetActive(true);
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

	public Sprite RendomItem()
	{
		ItemPanel.SetActive(true);
		int rand = Random.Range(0, 5);
		return ItemImage[rand];
	}

	public void StageLoad(string name)
	{
		SceneManager.LoadScene(name);
	}
}
