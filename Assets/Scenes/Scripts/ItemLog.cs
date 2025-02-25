using UnityEngine;
using UnityEngine.UI;

public class ItemLog : MonoBehaviour
{
	public GameManage gm;
	public StoreManage store;
	public Image ItemImage;
	public Text Title;
	private void OnEnable()
	{
		ItemImage.sprite = gm.RendomItem();
		if (ItemImage.sprite.name.Equals("Store"))
		{
			Title.text = "상점 이동";
			Invoke("Go", 1f);
		}
		else
		{
			Title.text = ItemImage.sprite.name + " 획득!";
			store.ItemRespond(ItemImage.sprite.name);
			Invoke("nnn", 1f);
		}
	}
	private void nnn()
	{
		gameObject.SetActive(false);
	}
	private void Go()
	{
		gameObject.SetActive(false);
		gm.OpenStore();
	}
}
