using UnityEngine;
using System.Collections;

public class EffectComponent : MonoBehaviour {
	private	static EffectComponent _instance;
	public	static EffectComponent GetInstance() {
		return _instance;
	}

	public	Transform	faceIn;
	public	Transform	faceOut;
	public	Transform	bangLv1;
	public	Transform	bangLv2;
	public	Transform	bangLv3;
	public	Transform	comboLv1;
	public	Transform	comboLv2;
	public	Transform	comboLv3;
	public	Transform	coin;

	void Start() {
		_instance = this;
	}

	public	static Transform Show(EffectType type, Vector3 position) {
		Transform t = null;
		switch(type) {
		case EffectType.FACE_DRAG_IN:
			t = _instance.faceIn;
			break;
		case EffectType.FACE_DRAG_OUT:
			t = _instance.faceOut;
			break;
		case EffectType.BANG_LV1:
			t = _instance.bangLv1;
			break;
		case EffectType.BANG_LV2:
			t = _instance.bangLv2;
			break;
		case EffectType.BANG_LV3:
			t = _instance.bangLv3;
			break;
		case EffectType.COMBO_LV1:
			t = _instance.comboLv1;
			break;
		case EffectType.COMBO_LV2:
			t = _instance.comboLv2;
			break;
		case EffectType.COMBO_LV3:
			t = _instance.comboLv3;
			break;
		case EffectType.COIN:
			t = _instance.coin;
			break;
		default:
			return null;
		}
		return Instantiate(t, position, Quaternion.identity) as Transform;
	}
}
