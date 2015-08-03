using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleStarComponent : MonoBehaviour {
	public	RawImage[]	stars;
	public	Texture2D	textureDisable;
	public	Texture2D	textureEnable;
	public	Texture2D	textureSuccess;

	public	void SetThemeInfo(ThemeInfo themeInfo) {
		int starSuccess = PlayerInfoManager.instance.stars[themeInfo.order];
		int starEnable = Mathf.Min(Mathf.Max(themeInfo.star, starSuccess+1), stars.Length);

		int i = 1;
		foreach (RawImage image in stars) {
			if (i <= starSuccess) {
				image.texture = textureSuccess;
			} else if (i <= starEnable) {
				image.texture = textureEnable;
			} else {
				image.texture = textureDisable;
			}
			i++;
		}
	}
}
