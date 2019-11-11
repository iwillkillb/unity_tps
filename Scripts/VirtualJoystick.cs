using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

	Image imgBg, imgStick;
	Vector3 vecInput;
	public string axisNameX = "Horizontal", axisNameY = "Vertical";

	void Awake () {
		imgBg = GetComponent<Image> ();
		imgStick = transform.GetChild (0).GetComponent<Image> ();
	}

	public virtual void OnDrag (PointerEventData ped) {
		Vector2 pos;

		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (
			    imgBg.rectTransform,
			    ped.position,
			    ped.pressEventCamera,
			    out pos)) {
			pos.x = (pos.x / imgBg.rectTransform.sizeDelta.x);
			pos.y = (pos.y / imgBg.rectTransform.sizeDelta.y);

			float inputX = (imgBg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
			float inputY = (imgBg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

			vecInput = new Vector3 (inputX, 0, inputY);
			vecInput = (vecInput.magnitude > 1f) ? vecInput.normalized : vecInput;

			// Move stick
			imgStick.rectTransform.anchoredPosition =
				new Vector2 (
					vecInput.x * (imgBg.rectTransform.sizeDelta.x / 3),
					vecInput.z * (imgBg.rectTransform.sizeDelta.y / 3)
				);
		}
	}

	public virtual void OnPointerDown (PointerEventData ped) {
		OnDrag (ped);
	}

	public virtual void OnPointerUp (PointerEventData ped) {
		vecInput = Vector3.zero;
		imgStick.rectTransform.anchoredPosition = Vector3.zero;
	}

	public Vector2 GetInputAxis () {
		return new Vector2 (vecInput.x, vecInput.z);
	}
}