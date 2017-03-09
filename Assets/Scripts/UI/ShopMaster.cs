using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopMaster : MonoBehaviour {

	public MenuState WhereGoing;
	public GameObject MainMenu;

	public GameObject ShopWindow;
	public Text WindowText;
	public Text WindowPrice;

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
		Event<ResetEvent>.Broadcast(new ResetEvent());

		ShopWindow.SetActive (false);

		_currentPage = 1;
		Masters [0].SetActive (true);
		for (int i = 1; i < Masters.Length; i++) {
			Masters [i].SetActive (false);
		}
		LeftArrow.SetActive (false);
		RightArrow.SetActive (true);

		UnderTitle.text = "POWER-UP UPGRADES 1/" + NbOfPowerPage;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (ShopWindow.activeSelf) {
				ShopWindow.SetActive (false);
			}
			else {
				Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(WhereGoing));
				Event<InitializeEvent>.Broadcast(new InitializeEvent(G.Sys.gameManager.playerStartLocation));
				MainMenu.SetActive (true);
				gameObject.SetActive (false);
			}
		}
	}

	public void BackToMainScreen () {
		Event<ChangeMenuEvent>.Broadcast(new ChangeMenuEvent(WhereGoing));
		Event<InitializeEvent>.Broadcast(new InitializeEvent(G.Sys.gameManager.playerStartLocation));
		MainMenu.SetActive (true);
		gameObject.SetActive (false);
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
			UnderTitle.text = "POWER-UP UPGRADES " + _currentPage + "/" + NbOfPowerPage;
		}
		else {
			UnderTitle.text = "COSMETICS " + (_currentPage - NbOfPowerPage) + "/" + (Masters.Length - NbOfPowerPage);
		}
	}

	public void OpenWindowPower (string title, int price, PowerupType powerUp) {
		ShopWindow.SetActive (true);
		WindowText.text = "DO YOU WANT TO BUY" + title + " FOR " + price + " ?";
		WindowPrice.text = price.ToString ();
		_currentPrice = price;
		_currentPowerUp = powerUp;
		_isPowerUp = true;
	}

	public void OpenWindowCosmetics (string title, int price, CosmeticsType cosmetics) {
		ShopWindow.SetActive (true);
		WindowText.text = "DO YOU WANT TO BUY" + title + " FOR " + price + " ?";
		WindowPrice.text = price.ToString ();
		_currentCosmetics = cosmetics;
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
			Debug.Log (_currentPowerUp.ToString () + " " + G.Sys.dataMaster.PowerupLevel (_currentPowerUp));
		}
		else {
			G.Sys.dataMaster.SetCosmeticsLevel (true, _currentCosmetics);
			ShopWindow.SetActive (false);
			Event<ShopResetEvent>.Broadcast(new ShopResetEvent());
			Debug.Log (_currentCosmetics.ToString () + " " + G.Sys.dataMaster.CosmeticsLevel(_currentCosmetics).ToString ());
		}

	}
}
