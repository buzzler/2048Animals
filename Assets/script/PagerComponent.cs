using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PagerComponent : ScrollRect {

	private	float	leftMost;
	private float	rightMost;
	private	float	snap;
	private	int		currentPage;
	private	int		totalPage;

	protected override void Start() {
		base.Start();
		float w		= (transform as RectTransform).rect.width;
		float cw	= content.rect.width;
		rightMost	= (cw - w) / 2;
		leftMost	= -rightMost;
		snap		= w;
		currentPage	= 1;
		totalPage	= Mathf.RoundToInt(cw/w);
	}

	public override void OnEndDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnEndDrag (eventData);

		float abs = Mathf.Abs(eventData.delta.x);
		if (abs < 10) {
			NearPage();
		} else if (eventData.delta.x<0) {
			NextPage();
		} else if (eventData.delta.x>0) {
			PrevPage();
		}
	}

	public	void NearPage() {
		float now = content.localPosition.x;
		float distance = float.MaxValue;
		int index = -1;
		int count = 0;
		for (float i = rightMost ; i >= leftMost ; i -= snap) {
			float dist = Mathf.Abs(i-now);
			if (dist <= distance) {
				distance = dist;
				index = count;
			}
			count++;
		}
		if (index>=0) {
			currentPage = index+1;
			SetPage(currentPage);
		}
	}

	public	void NextPage() {
		currentPage = Mathf.Min(currentPage+1,totalPage);
		SetPage(currentPage);
	}

	public	void PrevPage() {
		currentPage = Mathf.Max(currentPage-1,1);
		SetPage(currentPage);
	}

	public	void SetPage(int page) {
		float targetPos = rightMost - snap * ((float)page-1f);

		horizontal = false;
		StopMovement();

		Hashtable hash = new Hashtable();
		hash.Add("from", content.localPosition.x);
		hash.Add("to", targetPos);
		hash.Add("speed", 2500f);
		hash.Add("easetype", iTween.EaseType.linear);
		hash.Add("onupdate", "OnMoveUpdate");
		hash.Add("onupdatetarget", gameObject);
		hash.Add("oncomplete", "OnMoveComplete");
		hash.Add("oncompletetarget", gameObject);
		iTween.ValueTo(gameObject, hash);
	}

	public	void OnMoveUpdate(float value) {
		content.localPosition = new Vector3(value,0f,0f);
	}

	public	void OnMoveComplete() {
		horizontal = true;
	}
}
