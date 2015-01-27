using UnityEngine;
using System.Collections;

public class KARole : MonoBehaviour {

	protected bool m_bLand = true;
	protected bool m_bSkill = false;
	protected bool m_bLeft = false;
	protected float m_fWalkXSpeed = 10f;
	protected float m_fWalkZSpeed = 8f;
	protected float m_fRunXSpeed = 20f;
	protected float m_fRunZSpeed = 16f;
	protected float m_fAttackMoveSpeed = 2.0f;
	protected float m_fAttackMoveSpeedX = 4.0f;
	protected float m_fJumpBase = 28.0f;
	protected float m_fJumpForce = 0.0f;

	protected KAAnimationMgr m_animationMgr = null;
	protected KAKeyboardMgr m_keyboardMgr = null;
	protected KARoleStateMgr m_roleStateMgr = null;

	public KAAnimationMgr AnmationMgr { get { return m_animationMgr; } }

	public KAKeyboardMgr KeyboardMgr { get { return m_keyboardMgr; } }

	public KARoleStateMgr RoleStateMgr { get { return m_roleStateMgr; } }

	public float JumpForce
	{
		get { return m_fJumpForce; }
		set { m_fJumpForce = value; }
	}

	public float JumpBase
	{
		get { return m_fJumpBase; }
	}

	public bool OnLand
	{
		get { return m_bLand; }
		set { m_bLand = value; }
	}

	public bool IsSkill
	{
		get { return m_bSkill; }
		set { m_bSkill = value; }
	}

	// Use this for initialization
	public virtual void Start() {
		m_animationMgr = new KAAnimationMgr(this);
		m_roleStateMgr = new KARoleStateMgr(this);
		m_keyboardMgr = new KAKeyboardMgr(this);
		JumpForce = JumpBase;
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

		int state = m_roleStateMgr.StateType;
		int lastState = m_roleStateMgr.LastStateType;
		float fXSpeed = 0, fZSpeed = 0;
		if (state == (int)KAStateType.S_RUN || (state == (int)KAStateType.S_JUMP && lastState == (int)KAStateType.S_RUN))
		{
			fXSpeed = m_fRunXSpeed;
			fZSpeed = m_fRunZSpeed;
		}
		else if (state == (int)KAStateType.S_WALK || (state == (int)KAStateType.S_JUMP && lastState != (int)KAStateType.S_RUN))
		{
			fXSpeed = m_fWalkXSpeed;
			fZSpeed = m_fWalkZSpeed;
		}
		else if (state == (int)KAStateType.S_ATTACK)
		{
			if (m_roleStateMgr.state.Param1 == 0)
			{
				if (((vInput.x > 0) && !m_bLeft)
					|| ((vInput.x < 0) && m_bLeft))
				{
					fXSpeed = m_fAttackMoveSpeedX;
				}
				else if (vInput.x == 0)
				{
					vInput.x = m_bLeft ? -1 : 1;
					fXSpeed = m_fAttackMoveSpeed;
				}
			}
		}

		velcity.x = vInput.x * fXSpeed;
		velcity.z = vInput.z * fZSpeed;

		transform.rigidbody.velocity = velcity;
	}

	public void FaceTo(KACommon.Direction dir)
	{
		KAStateType type = (KAStateType)m_roleStateMgr.StateType;
		if (type == KAStateType.S_ATTACK || type == KAStateType.S_SKILL || type == KAStateType.S_ATTACKED)
		{
			return;
		}

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

	public float StateParam1 { get { return m_roleStateMgr.state.Param1; } set { m_roleStateMgr.state.Param1 = value; } }
	public float StateParam2 { get { return m_roleStateMgr.state.Param2; } set { m_roleStateMgr.state.Param2 = value; } }
	public float StateParam3 { get { return m_roleStateMgr.state.Param3; } set { m_roleStateMgr.state.Param3 = value; } }
	public float StateParam4 { get { return m_roleStateMgr.state.Param4; } set { m_roleStateMgr.state.Param4 = value; } }

}
