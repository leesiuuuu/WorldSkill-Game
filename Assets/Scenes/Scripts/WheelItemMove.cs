using UnityEngine;
using UnityEngine.UI;

public class WheelItemMove : ItemSelete
{
	public Transform Camera;
	private int MAX = 4;
	private float zPos =1.75f;
	[SerializeField]
	private Text button;
	[SerializeField]
	private Text ItemTitle;
	[SerializeField]
	private Text Info;
	private void Awake()
	{
		TitleInfo();
	}
	public override void MoveLeft()
	{
		if(index > 0) --index;
		Move();
		TitleInfo();
	}

	public override void MoveRight()
	{
		if (index < MAX-1) ++index;
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
		if (!GameSystem.instance.WheelStore[index])
		{
			button.text = "Buy";
		}
		else
		{
			if (GameSystem.instance.GetItemData<GameSystem.WheelType>().Equals(GameSystem.WheelType.Normal))
			{
				switch (index)
				{
					case 0:
						button.text = "Equipped"; break;
					case 1:
						button.text = "Equip"; break;
					case 2:
						button.text = "Equip"; break;
					case 3:
						button.text = "Equip"; break;
				}
			}
			else if (GameSystem.instance.GetItemData<GameSystem.WheelType>().Equals(GameSystem.WheelType.Sand))
			{
				switch (index)
				{
					case 0:
						button.text = "Equip"; break;
					case 1:
						button.text = "Equipped"; break;
					case 2:
						button.text = "Equip"; break;
					case 3:
						button.text = "Equip"; break;
				}
			}
			else if (GameSystem.instance.GetItemData<GameSystem.WheelType>().Equals(GameSystem.WheelType.Mountain))
			{
				switch (index)
				{
					case 0:
						button.text = "Equip"; break;
					case 1:
						button.text = "Equip"; break;
					case 2:
						button.text = "Equipped"; break;
					case 3:
						button.text = "Equip"; break;
				}
			}
			else if (GameSystem.instance.GetItemData<GameSystem.WheelType>().Equals(GameSystem.WheelType.Road))
			{
				switch (index)
				{
					case 0:
						button.text = "Equip"; break;
					case 1:
						button.text = "Equip"; break;
					case 2:
						button.text = "Equip"; break;
					case 3:
						button.text = "Equipped"; break;
				}
			}
		}
	}

	public void BuyItem()
	{
		//돈이 충분한지 확인하는 조건문 추가
		GameSystem.instance.WheelStore[index] = true;
		switch (index)
		{
			case 0:
				GameSystem.instance.SetItemData<GameSystem.WheelType>(GameSystem.WheelType.Normal); break;
			case 1:
				GameSystem.instance.SetItemData<GameSystem.WheelType>(GameSystem.WheelType.Sand); break;
			case 2:
				GameSystem.instance.SetItemData<GameSystem.WheelType>(GameSystem.WheelType.Mountain); break;
			case 3:
				GameSystem.instance.SetItemData<GameSystem.WheelType>(GameSystem.WheelType.Road); break;
		}
	}

	private void TitleInfo()
	{
		switch (index)
		{
			case 0:
				ItemTitle.text = "오래된 바퀴";
				Info.text = "너무 낡은 바퀴이다.";
				break;
			case 1:
				ItemTitle.text = "사막 전용 바퀴";
				Info.text = "사막 지형에 뛰어난 바퀴이다.\n사막 지형에서 속도가 늘어난다.";
				break;
			case 2:
				ItemTitle.text = "산악 전용 바퀴";
				Info.text = "산악 지형에 뛰어난 바퀴이다.\n산악 지형에서 속도가 늘어난다.";
				break;
			case 3:
				ItemTitle.text = "도로 전용 바퀴";
				Info.text = "전 챔피언 자동차의 바퀴이다.\n도시 지형에서 속도가 늘어난다.";
				break;

		}
	}
}
