using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopUpgrade : MonoBehaviour {

	public string Title;
	public string Description;
	public PowerupType MyPowerType;
	public int[] CostTable = new int[5];
	public GameObject[] Bar = new GameObject[5];
	public ShopMaster MyMaster;
	public Text TitleZone;
	public Text DescriptionZone;
	public Text CostZone;
	public GameObject CannotBuy;
	public GameObject CanBuy;
	public GameObject Buyed;

	Button _myButton;
	int _currentLevel = 0;

	SubscriberList _subscriberList = new SubscriberList();

	void Awake () {
		TitleZone.text = Title;
		DescriptionZone.text = Description;
	}

	void OnEnable () {
		_myButton = gameObject.GetComponent <Button> ();
		_subscriberList.Add(new Event<ShopResetEvent>.Subscriber(Refresh));
		_subscriberList.Subscribe();
		Refresh (new ShopResetEvent());
	}

	public void Refresh (ShopResetEvent e) {
		_currentLevel = G.Sys.dataMaster.PowerupLevel (MyPowerType);
		CostZone.text = CostTable [_currentLevel].ToString ();
		for (int i = 0; i < Bar.Length; i++) {
			if (i + 1 <= _currentLevel) {
				Bar [i].SetActive (true);
			}
			else {
				Bar [i].SetActive (false);
			}
		}

		if (_currentLevel == CostTable.Length) {
			CannotBuy.SetActive (false);
			CanBuy.SetActive (false);
			Buyed.SetActive (true);
			_myButton.interactable = false;
		}
		else {
			Buyed.SetActive (false);
			if (G.Sys.dataMaster.Coins >= CostTable [_currentLevel]) {
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
	}

	public void IWannaBuy () {
		MyMaster.OpenWindowPower (Title, CostTable[_currentLevel], MyPowerType);
	}
}
