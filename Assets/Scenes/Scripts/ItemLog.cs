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
		Title.text = ItemImage.sprite.name + " 획득!";
		store.ItemRespond(ItemImage.sprite.name);
		Invoke("nnn", 1f);
	}
	private void nnn()
	{
		gameObject.SetActive(false);
	}
}
