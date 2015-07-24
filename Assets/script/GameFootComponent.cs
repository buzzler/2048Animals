using UnityEngine;
using UnityEngine.UI;
//using System.Collections;
using Soomla.Store;
using SmartLocalization;

public class GameFootComponent : MonoBehaviour {
    public  Text    textTitle;
    public  Text    textFoot;
    public  Text    textMessage;
    public  Text    textYes;

    void OnEnable() {
		OnUIChangeLanguage (LanguageManager.Instance);
        LanguageManager.Instance.OnChangeLanguage += OnUIChangeLanguage;
        Observer.GetInstance().inventoryChange += OnUpdateInventory;
    }

    void OnDisable() {
        LanguageManager.Instance.OnChangeLanguage -= OnUIChangeLanguage;
        Observer.GetInstance().inventoryChange -= OnUpdateInventory;
    }

    private void OnUIChangeLanguage (SmartLocalization.LanguageManager lm) {
        textTitle.text = lm.GetTextValue("fnf.ui.gameover") + "?";
        textFoot.text = StoreInventory.GetItemBalance(StoreAssetInfo.FOOT).ToString();
        textMessage.text = lm.GetTextValue("fnf.ui.nomoremove") + "\n" + lm.GetTextValue("fnf.ui.paw");
        textYes.text = lm.GetTextValue("fnf.ui.yes");
    }

    private void OnUpdateInventory(string id, int balance, int delta) {
        if (id == StoreAssetInfo.FOOT) {
            textFoot.text = balance.ToString();
        }
    }
}
