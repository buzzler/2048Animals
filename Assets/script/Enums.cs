using UnityEngine;
using System.Collections;

public	enum CostType {
	NONE = 0,
	FREE,
	CONNECT,
	COIN
}

public	enum AnimalType {
	NONE = 0,
	BEAR,
	BOO,
	SANTA,
	SOAN,
	NARU,
	LOLI,
	POMERANIAN,
	ALPACA,
	CAMELEON,
	CHICK,
	CHICKEN,
	COW,
	CROCODILE,
	ELEPHANT,
	FROG,
	FURSEAL,
	GIBBON,
	GIRAFFE,
	GREENTURTLE,
	HIPPOPOTAMUS,
	HORSE,
	JINDO,
	LION,
	LIONESS,
	OTTER,
	PANDA,
	PENGUIN,
	PERSIAN,
	PIG,
	PIGBLACK,
	POLARBEAR,
	POMEWHITE,
	RABBIT,
	RACCOON,
	SEALION,
	SHEEP,
	WELSHCORGI
}

public	enum AnimalSize {
	SMALL = 0,
	MEDIUM = 1,
	LARGE,
	EXTRA
}

public	enum BackgroundType {
	NONE = 0,
	ALASKA,
	ANTARCTICA,
	SAVANNAH,
	AMAZON
}

public	enum BackgroundStatus {
	NONE = 0,
	A,
	B,
	C,
	D
}

public	enum UIType {
	NONE = 0,
	TITLE,
	GAME,
	RESULT,
	SETTING,
	COINPACK,
	TUTORIAL,
	PAUSE,
	PRELOAD
}

public	enum UIState {
	STOPPED = 0,
	START,
	PAUSED
}

public	enum MaskState {
	OPEN = 0,
	CLOSE
}

public	enum AudioType {
	NONE = 0,
	BGM,
	FX
}

public	enum ThemeSelectorState {
	NONE = 0,
	BLINDED,
	LOCKED,
	UNLOCKED
}

public	enum BuffType {
	NONE = 0,
	COIN,
	SCORE,
	REWARD
}

public	enum EffectType {
	NONE = 0,
	FACE_DRAG_IN,
	FACE_DRAG_OUT,
	COIN,
	COMBO_LV1,
	COMBO_LV2,
	COMBO_LV3,
	BANG_LV1,
	BANG_LV2,
	BANG_LV3
}
