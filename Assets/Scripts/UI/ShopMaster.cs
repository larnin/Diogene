using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopMaster : MonoBehaviour {

	public GameObject ShopWindow;
	public Text WindowText;

	int _currentPrice;
	PowerupType _currentPowerUp;
	CosmeticsType _currentCosmetics;

	public int NbOfPowerPage;
	public GameObject[] Masters;
	int _currentPage = 1;

	public GameObject RightArrow;
	public GameObject LeftArrow;
	public Text UnderTitle;

	bool _isPowerUp = true;

	void OnEnable () {
		_currentPage = 1;
		Masters [0].SetActive (true);
		for (int i = 1; i < Masters.Length; i++) {
			Masters [i].SetActive (false);
		}
		LeftArrow.SetActive (false);
		RightArrow.SetActive (true);

		UnderTitle.text = "POWER-UP UPGRADES 1/" + NbOfPowerPage;
	}

	public void OnArrow(int value) {
		Masters [_currentPage - 1].SetActive (false);
		_currentPage += value;
		Masters [_currentPage - 1].SetActive (true);
		if (_currentPage <= 1) {
			LeftArrow.SetActive (false);
			RightArrow.SetActive (true);
		}
		else if (_currentPage >= Masters.Length) {
			LeftArrow.SetActive (true);
			RightArrow.SetActive (false);
		}
		else {
			LeftArrow.SetActive (true);
			RightArrow.SetActive (true);
		}

		if (_currentPage <= NbOfPowerPage) {
			UnderTitle.text = "POWER-UP UPGRADES" + _currentPage + "/" + NbOfPowerPage;
		}
		else {
			UnderTitle.text = "COSMETICS" + (_currentPage - NbOfPowerPage) + "/" + (Masters.Length - NbOfPowerPage);
		}
	}

	public void OpenWindowPower (string title, int price, PowerupType powerUp) {
		ShopWindow.SetActive (true);
		WindowText.text = "DO YOU WANT TO BUY" + title + " FOR " + price + " ?";
		_currentPrice = price;
		_currentPowerUp = powerUp;
		_isPowerUp = true;
	}

	public void OpenWindowCosmetics (string title, int price) {
		ShopWindow.SetActive (true);
		WindowText.text = "DO YOU WANT TO BUY" + title + " FOR " + price + " ?";
		_currentPrice = price;
		_isPowerUp = false;
	}

	public void CloseWindow () {
		ShopWindow.SetActive (false);
	}

	public void Buy() {

		G.Sys.dataMaster.Coins -= _currentPrice;

		if (_isPowerUp) {
			G.Sys.dataMaster.SetPowerupLevel (G.Sys.dataMaster.PowerupLevel (_currentPowerUp) + 1, _currentPowerUp);
			ShopWindow.SetActive (false);
			Event<ShopResetEvent>.Broadcast(new ShopResetEvent());
		}
		else {
			G.Sys.dataMaster.SetCosmeticsLevel (true, _currentCosmetics);
			ShopWindow.SetActive (false);
			Event<ShopResetEvent>.Broadcast(new ShopResetEvent());
		}

	}
}
