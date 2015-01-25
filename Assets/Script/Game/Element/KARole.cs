using UnityEngine;
using System.Collections;

public class KARole : MonoBehaviour {

	protected bool m_bLand = true;
	protected bool m_bLeft = false;
	protected float m_fWalkXSpeed = 10f;
	protected float m_fWalkZSpeed = 8f;
	protected float m_fRunXSpeed = 20f;
	protected float m_fRunZSpeed = 16f;
	protected float m_fJumpForce = 500.0f;

	protected KAAnimationMgr m_animationMgr = null;
	protected KAKeyboardMgr m_keyboardMgr = null;
	protected KARoleStateMgr m_roleStateMgr = null;

	public KAAnimationMgr AnmationMgr
	{
		get { return m_animationMgr; }
	}
	public KAKeyboardMgr KeyboardMgr
	{
		get { return m_keyboardMgr; }
	}
	public KARoleStateMgr RoleStateMgr
	{
		get { return m_roleStateMgr; }
	}

	public float JumpForce
	{
		get { return m_fJumpForce; }
	}

	// TODO OnLand的判断
	public bool OnLand
	{
		get { return m_bLand; }
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
		m_roleStateMgr.Update();
		UpdateDir();
		UpdateRoleMove();
	}

	protected virtual void UpdateDir()
	{
		bool Left = Input.GetKey(KeyCode.LeftArrow);
		bool Right = Input.GetKey(KeyCode.RightArrow);
		if (!(Left && Right))
		{
			if (Left && !m_bLeft)
			{
				FaceTo(KACommon.Direction.D_LEFT);
			}

			if (Right && m_bLeft)
			{
				FaceTo(KACommon.Direction.D_RIGHT);
			}
		}
	}

	protected virtual void UpdateRoleMove()
	{
		// TODO 加入状态机
		/*
		if (Input.GetKeyDown(KeyCode.C))
		{
			Vector3 vForce = Vector3.zero;
			vForce.y = m_fJumpForce;
			transform.rigidbody.AddForce(vForce);
		}
		*/

		if (m_roleStateMgr.IsCanMove() == false)
		{
			return;
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

		KAStateType state = m_roleStateMgr.StateType;
		float fXSpeed = 0, fZSpeed = 0;
		if (state == KAStateType.S_RUN)
		{
			fXSpeed = m_fRunXSpeed;
			fZSpeed = m_fRunZSpeed;
		}
		else if (state == KAStateType.S_WALK || state == KAStateType.S_JUMP)
		{
			fXSpeed = m_fWalkXSpeed;
			fZSpeed = m_fWalkZSpeed;
		}

		velcity.x = vInput.x * fXSpeed;
		velcity.z = vInput.z * fZSpeed;

		transform.rigidbody.velocity = velcity;
	}

	public void FaceTo(KACommon.Direction dir)
	{
		Vector3 scale = transform.localScale;
		if (dir == KACommon.Direction.D_LEFT)
		{
			m_bLeft = true;
			scale.x = -Mathf.Abs(scale.x);
		}
		else if (dir == KACommon.Direction.D_RIGHT)
		{
			m_bLeft = false;
			scale.x = Mathf.Abs(scale.x);
		}
		transform.localScale = scale;

	}

}
