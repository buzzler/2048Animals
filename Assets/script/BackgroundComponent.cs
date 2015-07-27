using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BackgroundComponent : MonoBehaviour {
	public	Animator animator;

	public	void SetStatusNormal() {
		SetTheme(PlayerInfoManager.instance.GetThemeInfo().bgNormal);
	}

	public	void SetStatusFever() {
		SetTheme(PlayerInfoManager.instance.GetThemeInfo().bgFever);
	}

	public	void SetTheme(BackgroundStatus status) {
		switch(status) {
		case BackgroundStatus.A: A();break;
		case BackgroundStatus.B: B();break;
		case BackgroundStatus.C: C();break;
		case BackgroundStatus.D: D();break;
		case BackgroundStatus.NONE: A ();break;
		}
	}

	public	void SetTheme(bool d, bool c, bool b) {
		animator.SetBool("bool_d", d);
		animator.SetBool("bool_c", c);
		animator.SetBool("bool_b", b);
	}

	public	void A() {
		SetTheme(false, false, false);
	}

	public	void D() {
		SetTheme(true, false, false);
	}

	public	void C() {
		SetTheme(false, true, false);
	}

	public	void B() {
		SetTheme(false, false, true);
	}
}
