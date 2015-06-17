using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityJellySprite))]
public class HeadComponent : MonoBehaviour {
	private	UnityJellySprite jelly;

	void Start() {
		jelly = GetComponent<UnityJellySprite> ();

		Invoke("TestForce", Random.Range(2, 5));
	}

	public	void TestForce() {
		jelly.Boing ();
		Invoke("TestForce", Random.Range(2, 5));
	}
}
