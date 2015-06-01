using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TutorialComponent : UIComponent {
	public	void OnClickClose() {
		OnUIReserve(parent);
		OnUIChange();
	}
}
