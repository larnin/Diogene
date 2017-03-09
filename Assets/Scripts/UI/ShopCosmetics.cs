using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum CosmeticsType
{
	DEFAULT = 0,
	BLACK = 1,
	BLUE = 2,
	CYAN = 3,
	GREEN = 4,
	GRAY = 5,
	ORANGE = 6,
	PINK = 7,
	WHITE = 8,
	YELLOW = 9,
	RAINBOW = 10,
	COSMETICS_MAX = RAINBOW
}


public class ShopCosmetics : MonoBehaviour {

	public string Title;
	public string Description;
	public int Cost;
	public CosmeticsType MyCosmetics;
	public ShopMaster MyMaster;
	public Text TitleZone;
	public Text DescriptionZone;
	public Text CostZone;
	public GameObject CannotBuy;
	public GameObject CanBuy;
	public GameObject Buyed;
	public GameObject Equipped;

	Button _myButton;
	public bool _state = false;

	SubscriberList _subscriberList = new SubscriberList();

	void Awake () {
		TitleZone.text = Title;
		DescriptionZone.text = Description;
		CostZone.text = Cost.ToString ();
	}

	void OnEnable () {
		_myButton = gameObject.GetComponent <Button> ();
		_subscriberList.Add(new Event<ShopResetEvent>.Subscriber(Refresh));
		_subscriberList.Subscribe();
		Refresh (new ShopResetEvent());
	}

	public void Refresh (ShopResetEvent e) {

		if (MyCosmetics == CosmeticsType.DEFAULT) {
			_state = true;
		}
		else {
			_state = G.Sys.dataMaster.CosmeticsLevel (MyCosmetics);
		}

		if (!_state) {
			Buyed.SetActive (false);
			Equipped.SetActive (false);

			if (G.Sys.dataMaster.Coins >= Cost) {
				CannotBuy.SetActive (false);
				CanBuy.SetActive (true);
				_myButton.interactable = true;
			}
			else {
				CannotBuy.SetActive (true);
				CanBuy.SetActive (false);
				_myButton.interactable = false;
			}
		}
		else {
			CostZone.text = string.Empty;
			CannotBuy.SetActive (false);
			CanBuy.SetActive (false);
			Buyed.SetActive (true);
			_myButton.interactable = true;

			if (G.Sys.dataMaster.EquippedCosmetic == MyCosmetics) {
				Equipped.SetActive (true);
			}
			else {
				Equipped.SetActive (false);
			}
		}
	}

	public void IWannaBuy () {

		if (!_state) {
			MyMaster.OpenWindowCosmetics (Title, Cost, MyCosmetics);
		}
		else if (G.Sys.dataMaster.EquippedCosmetic != MyCosmetics) {
			G.Sys.dataMaster.EquippedCosmetic = MyCosmetics;
			Debug.Log (MyCosmetics.ToString () + " equipped " + G.Sys.dataMaster.EquippedCosmetic.ToString ());
			Event<ShopResetEvent>.Broadcast(new ShopResetEvent());
		}

	}
}
