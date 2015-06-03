using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitlePurchaseComponent : MonoBehaviour {
	public	Text		textName;
	public	Text		textDescription;
	public	Button		buttonCoin;
	public	Text		textCoin;
	private	ThemeInfo	info;

	public	void ClearThemeInfo() {
		textName.gameObject.SetActive(false);
		textDescription.gameObject.SetActive(false);
		buttonCoin.gameObject.SetActive(false);
	}

	public	void SetThemeInfo(ThemeInfo info, bool interactable = true) {
		this.info = info;
		textName.text = info.name;
		textDescription.text = info.description;
		textCoin.text = info.coin.ToString();

		textName.gameObject.SetActive(true);
		textDescription.gameObject.SetActive(true);
		buttonCoin.gameObject.SetActive(true);
		buttonCoin.interactable = interactable;
	}

	public	void OnClick() {
		SendMessageUpwards("OnClickBuy", info);
	}
}
