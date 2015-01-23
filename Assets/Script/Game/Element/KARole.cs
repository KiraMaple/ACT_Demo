using UnityEngine;
using System.Collections;

public class KARole : MonoBehaviour {

	protected bool m_bLeft = false;
	protected float m_fWalkXSpeed = 10f;
	protected float m_fWalkZSpeed = 8f;
	protected float m_fRunXSpeed = 20f;
	protected float m_fRunZSpeed = 16f;

	protected KAAnimationMgr m_animationMgr = null;
	protected KAKeyboardMgr m_keyboardMgr = null;
	protected KARoleStateMgr m_roleStateMgr = null;

	public KAAnimationMgr AnmationMgr
	{
		get { return m_animationMgr; }
	}
	public KAKeyboardMgr CombKeyMgr
	{
		get { return m_keyboardMgr; }
	}
	public KARoleStateMgr RoleStateMgr
	{
		get { return m_roleStateMgr; }
	}

	// Use this for initialization
	public virtual void Start() {
		m_animationMgr = new KAAnimationMgr(this);
		m_roleStateMgr = new KARoleStateMgr(this);
		m_keyboardMgr = new KAKeyboardMgr(this);
	}
	
	// Update is called once per frame
	public virtual void Update () {
		m_keyboardMgr.UpdateKeyboadInput();
		m_roleStateMgr.UpdateKeyboadInput();
		UpdateRoleMove();
	}

	public virtual void UpdateRoleMove()
	{
		if (m_roleStateMgr.IsCanMove() == false)
		{
			return;
		}

		if (m_bLeft)
		{
			Vector3 scale = transform.localScale;
			scale.x = -Mathf.Abs(scale.x);
			transform.localScale = scale;
		}
		else
		{
			Vector3 scale = transform.localScale;
			scale.x = Mathf.Abs(scale.x);
			transform.localScale = scale;
		}

		Vector3 velcity = transform.rigidbody.velocity;
		Vector3 vInput = Vector3.zero;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			vInput.x--;
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			vInput.x++;
		}

		if (Input.GetKey(KeyCode.UpArrow))
		{
			vInput.z++;
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			vInput.z--;
		}

		KARoleStateMgr.State state = m_roleStateMgr.state;
		float fXSpeed = 0, fZSpeed = 0;
		if (vInput == Vector3.zero)
		{
			if (m_roleStateMgr.IsCanIdel())
			{
				m_roleStateMgr.ChangeToState(KARoleStateMgr.State.S_IDLE);
			}

		}
		else
		{
			if (state == KARoleStateMgr.State.S_RUN)
			{
				fXSpeed = m_fRunXSpeed;
				fZSpeed = m_fRunZSpeed;
			}
			else if (state == KARoleStateMgr.State.S_WALK || state == KARoleStateMgr.State.S_JUMP)
			{
				fXSpeed = m_fWalkXSpeed;
				fZSpeed = m_fWalkZSpeed;
			}
			/*
			if (m_keyboardMgr.IsRunKey() && m_roleStateMgr.IsCanRun())
			{
				m_roleStateMgr.ChangeToState(KARoleStateMgr.State.S_RUN);
			}
			else
			{
				if (m_roleStateMgr.IsCanWalk())
				{
					m_roleStateMgr.ChangeToState(KARoleStateMgr.State.S_WALK);
				}
			}
			*/

		}

		velcity.x = vInput.x * fXSpeed;
		velcity.z = vInput.z * fZSpeed;

		transform.rigidbody.velocity = velcity;
	}

	public void FaceTo(KACommon.Direction dir)
	{
		if (dir == KACommon.Direction.D_LEFT)
		{
			m_bLeft = true;
		}

		if (dir == KACommon.Direction.D_RIGHT)
		{
			m_bLeft = false;
		}
	}

}
