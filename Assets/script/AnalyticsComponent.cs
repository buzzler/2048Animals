using UnityEngine;
using System.Collections;

public class AnalyticsComponent : MonoBehaviour {
    public  GoogleAnalyticsV3 google; 

    void Awake() {
        google.StartSession();
    }
}
