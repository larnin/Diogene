using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CanvasRay : MonoBehaviour {

	public LayerMask Ring;
	GraphicRaycaster _graphicRaycaster;
	SubscriberList _subscriberList = new SubscriberList();
	bool _pause = false;

	void Awake()
	{
		_subscriberList.Add (new Event<PausePlayerEvent>.Subscriber (Pause));
		_subscriberList.Subscribe ();
		_graphicRaycaster = gameObject.GetComponent <GraphicRaycaster> ();
	}

	void Pause (PausePlayerEvent e) {
		_pause = e.State;
	}

	void Update () {
		if (!_pause) {
			var touches = Input.touches;

			if (touches.Length > 0)
			{
				if (touches[0].phase == TouchPhase.Began)
				{
					Ray ray = Camera.main.ScreenPointToRay(touches[0].position);
					RaycastHit hit;
					if (!Physics.Raycast(ray, out hit, 1000, Ring))
					{
						bool willJump = true;
						PointerEventData pointerData = new PointerEventData(EventSystem.current);
						pointerData.position = touches[0].position;
						List<RaycastResult> results = new List<RaycastResult>();
						_graphicRaycaster.Raycast (pointerData, results);
						for (int i = 0; i < results.Count; i++) {
							if (results[i].gameObject.tag == "UI")
							{
								willJump = false;
							}
						}

						if (willJump) {
							Event<PlayerJumpEvent>.Broadcast(new PlayerJumpEvent());
						}
					}
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (!Physics.Raycast(ray, out hit, 1000, Ring))
				{
					bool willJump = true;
					PointerEventData pointerData = new PointerEventData(EventSystem.current);
					pointerData.position = Input.mousePosition;
					List<RaycastResult> results = new List<RaycastResult>();
					_graphicRaycaster.Raycast (pointerData, results);
					for (int i = 0; i < results.Count; i++) {
						if (results[i].gameObject.tag == "Pause")
						{
							willJump = false;
						}
					}

					if (willJump) {
						Event<PlayerJumpEvent>.Broadcast(new PlayerJumpEvent());
					}
				}
			}
		}
	}
}
