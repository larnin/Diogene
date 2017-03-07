using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CanvasRay : MonoBehaviour {

	public LayerMask Ring;
    public float ringInputWidth;

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

	void Update ()
    {
		if (!_pause)
        {
			if (Input.GetMouseButtonDown(0))
			{
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                _graphicRaycaster.Raycast(pointerData, results);
                foreach (var r in results)
                    if (r.gameObject.tag == "Pause")
                        return;
                if (Input.mousePosition.x < ringInputWidth)
                    Event<MoveRingEvent>.Broadcast(new MoveRingEvent(1));
                else if (Input.mousePosition.x > Screen.width - ringInputWidth)
                    Event<MoveRingEvent>.Broadcast(new MoveRingEvent(-1));
                else Event<PlayerJumpEvent>.Broadcast(new PlayerJumpEvent());

                /*
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
				}*/
            }
		}
	}
}
