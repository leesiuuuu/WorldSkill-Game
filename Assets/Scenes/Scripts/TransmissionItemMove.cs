using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransmissionItemMove : ItemSelete
{
	public Transform Camera;
	public StoreManage storeManage;
	private int MAX = 3;
	private float zPos = 1.75f;
	[SerializeField]
	private Text button;
	[SerializeField]
	private Text ItemTitle;
	[SerializeField]
	private Text Info;
	[SerializeField]
	private GameObject WarnLog;
	[SerializeField]
	private Text costText;
	[SerializeField]
	private Image Main;

	public AudioClip UIMoveSound;
	public AudioClip UISeleteSound;
	public AudioClip UIWarnSound;

	private int[] costs =
{
		0,
		20000,
		50000
	};

	private void Awake()
	{
		LoadIndex();
		TitleInfo();
		Move();
	}
	public override void MoveLeft()
	{
		if (index > 0) 
		{
			--index;
			storeManage.sound.SoundPlay("UIMove", UIMoveSound);
		}
		Move();
		TitleInfo();
	}

	public override void MoveRight()
	{
		if (index < MAX - 1)
		{
			++index;
			storeManage.sound.SoundPlay("UIMove", UIMoveSound);
		}
		Move();
		TitleInfo();
	}
	void Move()
	{
		Camera.position =
			new Vector3(
				Camera.position.x,
				Camera.position.y,
				zPos + (index * -3f) + 7.24f);
	}
	private void Update()
	{
		if (storeManage.StoreClosed)
		{
			return;
		}
		if(costs[index] == 0)
		{
			costText.text = "FREE";
		}
		else
		{
			costText.text = $"{costs[index]}$";
		}

		if (!GameSystem.instance.TransmissionStore[index])
		{
			button.text = "Buy";
		}
		else
		{
			if (GameSystem.instance.GetItemData<GameSystem.Transmission>().Equals(GameSystem.Transmission.Normal))
			{
				switch (index)
				{
					case 0:
						button.text = "Equipped";
						ChangeColor(0.5f);
						break;
					case 1:
						button.text = "Equip";
						ChangeColor(1f); 
						break;
					case 2:
						button.text = "Equip";
						ChangeColor(1f); 
						break;
				}
			}
			else if (GameSystem.instance.GetItemData<GameSystem.Transmission>().Equals(GameSystem.Transmission.EnforcedTransmission))
			{
				switch (index)
				{
					case 0:
						button.text = "Equip";
						ChangeColor(1f); 
						break;
					case 1:
						button.text = "Equipped";
						ChangeColor(0.5f); 
						break;
					case 2:
						button.text = "Equip";
						ChangeColor(1f); 
						break;
				}
			}
			else if (GameSystem.instance.GetItemData<GameSystem.Transmission>().Equals(GameSystem.Transmission.AutoTransmission))
			{
				switch (index)
				{
					case 0:
						button.text = "Equip";
						ChangeColor(1f);
						break;
					case 1:
						button.text = "Equip";
						ChangeColor(1f); 
						break;
					case 2:
						button.text = "Equipped";
						ChangeColor(0.5f); 
						break;
				}
			}
		}
	}

	public void BuyItem()
	{
		if (storeManage.FreeUse)
		{
			storeManage.sound.SoundPlay("Buy", UISeleteSound);
			GameSystem.instance.TransmissionStore[index] = true;
			switch (index)
			{
				case 0:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.Normal); break;
				case 1:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.EnforcedTransmission); break;
				case 2:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.AutoTransmission); break;
			}
			storeManage.FreeUse = false;
			storeManage.UpdateMoneyUI();
			return;
		}
		else if (GameSystem.instance.TransmissionStore[index])
		{
			storeManage.sound.SoundPlay("Buy", UISeleteSound);
			switch (index)
			{
				case 0:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.Normal); break;
				case 1:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.EnforcedTransmission); break;
				case 2:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.AutoTransmission); break;
			}
		}
		else if(!GameSystem.instance.TransmissionStore[index] && costs[index] <= GameSystem.instance.Money)
		{
			storeManage.sound.SoundPlay("Buy", UISeleteSound);
			GameSystem.instance.TransmissionStore[index] = true;
			switch (index)
			{
				case 0:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.Normal); break;
				case 1:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.EnforcedTransmission); break;
				case 2:
					GameSystem.instance.SetItemData<GameSystem.Transmission>(GameSystem.Transmission.AutoTransmission); break;
			}
			GameSystem.instance.Money -= costs[index];
			storeManage.UpdateMoneyUI();
		}
		else
		{

			StartCoroutine(WarnLogAppear());
		}
	}
	private IEnumerator WarnLogAppear()
	{
		storeManage.sound.SoundPlay("Warn", UIWarnSound);
		WarnLog.SetActive(true);
		yield return new WaitForSecondsRealtime(0.5f);
		WarnLog.SetActive(false);
		yield break;
	}

	public void TitleInfo()
	{
		switch (index)
		{
			case 0:
				ItemTitle.text = "낡은 변속기";
				Info.text = "어디서 뜯어온건지, 너무 낡았다.";
				break;
			case 1:
				ItemTitle.text = "강화된 변속기";
				Info.text = "전 변속기보다는 좋은 것 같다.\n드리프트 시 부스터 게이지가 더욱 많이 차오른다.";
				break;
			case 2:
				ItemTitle.text = "자동 변속기";
				Info.text = "말로만 듣던 자동 변속기.\n드리프트 시 부스터 게이지가 훨씬 많이 차오른다.";
				break;

		}
	}
	public override void SaveIndex()
	{
		PlayerPrefs.SetInt("index_transmission", index);
		PlayerPrefs.Save();
	}
	public override void LoadIndex()
	{
		if(PlayerPrefs.HasKey("index_transmission"))
		index = PlayerPrefs.GetInt("index_transmission");
	}
	private void ChangeColor(float value)
	{
		Main.color = new Color(
			Main.color.r,
			Main.color.g,
			Main.color.b,
			value);
	}
}
