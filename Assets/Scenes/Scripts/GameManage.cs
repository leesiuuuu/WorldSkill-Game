using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UIElements;

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

	[Header("Cheat")]
	public GameObject CheatCanvas;
	public Text CheatLog;
	public GameObject Cheat5Log;
	public GameObject SkipPos;
	public GameObject ItemSelete;

	private bool Pause = false;
	[HideInInspector]
	public bool _RandomItem = true;

	public bool GameDone = false;

	public Sprite ItemSeletedSprite;
	private void Awake()
	{
		if (PlayerPrefs.GetInt("Cheat3") == 1)
		{
			StartCoroutine(_cheatLog("치트 3 : 스테이지 재시작"));
		}
		PlayerPrefs.DeleteKey("Cheat3");
	}

	private void OnEnable()
	{
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
		GameDone = true;
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
		int rand = Random.Range(0, 6);
		return ItemImage[rand];
	}
	public Sprite ItemByName(int index)
	{
		return ItemImage[index];
	}

	public void StageLoad(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void Cheat1()
	{
		Time.timeScale = 0f;
		ItemSelete.SetActive(true);
	}

	public void Cheat2()
	{
		StartCoroutine(_cheatLog("치트 2 : 아이템 무료 구매 가능"));
		Store.GetComponent<StoreManage>().FreeUse = true;
	}

	public void Cheat3()
	{
		PlayerPrefs.SetInt("Cheat3", 1);
		PlayerPrefs.Save();
		Time.timeScale = 1f;
		StageLoad(SceneManager.GetActiveScene().name);
	}

	public void Cheat4()
	{
		controller.gameObject.GetComponent<Rigidbody>().MovePosition(SkipPos.transform.position);
		StartCoroutine(_cheatLog("치트 4 : 다음 스테이지 이동"));
	}

	public void Cheat5()
	{
		Pause = !Pause;
		Cheat5Log.SetActive(Pause);
		Time.timeScale = Pause ? 0 : 1;
	}

	public void OnClickCheat1(int index)
	{
		_RandomItem = false;
		ItemSeletedSprite = ItemByName(index);
		Time.timeScale = 1f;
		ItemSelete.SetActive(false);
		ItemPanel.SetActive(true);
		_RandomItem = true;
	}
	
	public IEnumerator _cheatLog(string value)
	{
		CheatLog.gameObject.SetActive(true);
		CheatLog.text = value;
		yield return new WaitForSecondsRealtime(1f);
		CheatLog.text = "";
		CheatLog.gameObject.SetActive(false);
	}
}
