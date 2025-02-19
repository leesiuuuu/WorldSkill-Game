using UnityEngine;
using UnityEngine.UI;

public class EngineItemMove : ItemSelete
{
	public Transform Camera;
	private int MAX = 3;
	private float zPos = 3.75f;
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
		if (index > 0) --index;
		Move();
		TitleInfo();
	}

	public override void MoveRight()
	{
		if (index < MAX - 1) ++index;
		Move();
		TitleInfo();
	}
	void Move()
	{
		Camera.position =
			new Vector3(
				Camera.position.x,
				Camera.position.y,
				zPos + (index * -3f) + 5.28f);
	}
	private void Update()
	{
		if (!GameSystem.instance.EngineStore[index])
		{
			button.text = "Buy";
		}
		else
		{
			if (GameSystem.instance.GetItemData<GameSystem.Engine>().Equals(GameSystem.Engine.Normal))
			{
				switch (index)
				{
					case 0:
						button.text = "Equipped"; break;
					case 1:
						button.text = "Equip"; break;
					case 2:
						button.text = "Equip"; break;
				}
			}
			else if (GameSystem.instance.GetItemData<GameSystem.Engine>().Equals(GameSystem.Engine._6Engine))
			{
				switch (index)
				{
					case 0:
						button.text = "Equip"; break;
					case 1:
						button.text = "Equipped"; break;
					case 2:
						button.text = "Equip"; break;
				}
			}
			else if (GameSystem.instance.GetItemData<GameSystem.Engine>().Equals(GameSystem.Engine._8Engine))
			{
				switch (index)
				{
					case 0:
						button.text = "Equip"; break;
					case 1:
						button.text = "Equip"; break;
					case 2:
						button.text = "Equipped"; break;
				}
			}
		}
	}

	public void BuyItem()
	{
		//돈이 충분한지 확인하는 조건문 추가
		GameSystem.instance.EngineStore[index] = true;
		switch (index)
		{
			case 0:
				GameSystem.instance.SetItemData<GameSystem.Engine>(GameSystem.Engine.Normal); break;
			case 1:
				GameSystem.instance.SetItemData<GameSystem.Engine>(GameSystem.Engine._6Engine); break;
			case 2:
				GameSystem.instance.SetItemData<GameSystem.Engine>(GameSystem.Engine._8Engine); break;
		}
	}
	private void TitleInfo()
	{
		switch (index)
		{
			case 0:
				ItemTitle.text = "평범한 엔진";
				Info.text = "흔하디 흔한 엔진이다.";
				break;
			case 1:
				ItemTitle.text = "6기통 엔진";
				Info.text = "평범한 엔진보다는 좋아보인다.\n이동 시 부스터 게이지가 더 많이 차오른다.";
				break;
			case 2:
				ItemTitle.text = "8기통 엔진";
				Info.text = "모든 레이서들이 바라는 엔진이다.\n이동 시 부스터 게이지가 더 많이 차오른다.";
				break;

		}
	}
}
