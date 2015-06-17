using UnityEngine;
using System.Collections;

public class JellySpriteReferencePoint : MonoBehaviour 
{
	public GameObject ParentJellySprite { get; set; }
	public bool SendCollisionMessages { get { return m_SendCollisionMessages; } set { m_SendCollisionMessages = value; } }
	public int Index { get; set; }

	JellySprite.JellyCollision m_JellyCollision = new JellySprite.JellyCollision();
	JellySprite.JellyCollision2D m_JellyCollision2D = new JellySprite.JellyCollision2D();

	JellySprite.JellyCollider m_JellyCollider = new JellySprite.JellyCollider();
	JellySprite.JellyCollider2D m_JellyCollider2D = new JellySprite.JellyCollider2D();

	bool m_SendCollisionMessages = true;

	public JellySpriteReferencePoint()
	{
		m_JellyCollision.ReferencePoint = this;
		m_JellyCollision2D.ReferencePoint = this;
		m_JellyCollider.ReferencePoint = this;
		m_JellyCollider2D.ReferencePoint = this;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollision.Collision = collision;
			ParentJellySprite.SendMessage("OnJellyCollisionEnter", m_JellyCollision, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollision2D.Collision2D = collision;
			ParentJellySprite.SendMessage("OnJellyCollisionEnter2D", m_JellyCollision2D, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollision.Collision = collision;
			ParentJellySprite.SendMessage("OnJellyCollisionExit", m_JellyCollision, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollision2D.Collision2D = collision;
			ParentJellySprite.SendMessage("OnJellyCollisionExit2D", m_JellyCollision2D, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnCollisionStay(Collision collision)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollision.Collision = collision;
			ParentJellySprite.SendMessage("OnJellyCollisionStay", m_JellyCollision, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollision2D.Collision2D = collision;
			ParentJellySprite.SendMessage("OnJellyCollisionStay2D", m_JellyCollision2D, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollider.Collider = collider;
			ParentJellySprite.SendMessage("OnJellyTriggerEnter", m_JellyCollider, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollider2D.Collider2D = collider;
			ParentJellySprite.SendMessage("OnJellyTriggerEnter2D", m_JellyCollider2D, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollider.Collider = collider;
			ParentJellySprite.SendMessage("OnJellyTriggerExit", m_JellyCollider, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerExit2D(Collider2D collider)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollider2D.Collider2D = collider;
			ParentJellySprite.SendMessage("OnJellyTriggerExit2D", m_JellyCollider2D, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerStay(Collider collider)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollider.Collider = collider;
			ParentJellySprite.SendMessage("OnJellyTriggerStay", m_JellyCollider, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerStay2D(Collider2D collider)
	{
		if(ParentJellySprite && SendCollisionMessages)
		{
			m_JellyCollider2D.Collider2D = collider;
			ParentJellySprite.SendMessage("OnJellyTriggerStay2D", m_JellyCollider2D, SendMessageOptions.DontRequireReceiver);
		}
	}

	public	bool dragging;
	public	Vector2 oriPos;
	public	SpringJoint2D joint;

	void Start()
	{
		joint = GetComponent<SpringJoint2D>();
	}

	public	Vector2 GetAnchor() {
		if (joint!=null) {
			return joint.anchor;
		} else {
			return Vector2.zero;
		}
	}

	public	void Boing(float scalar) {
		if ((!dragging) && (joint!=null)) {
			Vector2 ca = joint.connectedAnchor.normalized;
			joint.anchor = ca * scalar;
		}
	}

	public void DragOn()
	{
		if ((!dragging) && (joint!=null)) {
			oriPos = transform.position;
			dragging = true;
		}
	}

	public void DragOff()
	{
		if (dragging && (joint!=null)) {
			joint.anchor = Vector2.zero;
			dragging = false;
		}
	}

	void Update()
	{
		if (dragging) {
			Vector2 curPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 anc = oriPos - curPos;
			anc.x *= 0.7f;
			anc.y *= 0.7f;
			joint.anchor = anc;
		} else if ((joint!=null) && (joint.anchor!=Vector2.zero)) {
			joint.anchor = Vector2.zero;
		}
	}
}
