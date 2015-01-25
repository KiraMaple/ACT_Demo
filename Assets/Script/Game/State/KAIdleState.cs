using UnityEngine;
using System.Collections;

public class KAIdleState : KAState
{
	public override bool check(KAState lastState)
	{
		KARole role = m_roleStateMgr.role;
		if (role.OnLand && !KAKeyboardMgr.IsArrowKey())
		{
			return true;
		}

		return false;
	}

	public KAIdleState(KARoleStateMgr StateMgr)
		: base(StateMgr)
	{
		m_info.state = KAStateType.S_IDLE;
		m_info.hash = Animator.StringToHash("Idle");
		m_info.nextState = new KAStateType[] { KAStateType.S_WALK, KAStateType.S_RUN, KAStateType.S_JUMP };
	}

	public override void PreAction()
	{
		KARole role = m_roleStateMgr.role;
		role.transform.rigidbody.velocity = Vector3.zero;
	}

}

public class KAWalkState : KAState
{
	public override bool check(KAState lastState)
	{
		KARole role = m_roleStateMgr.role;
		if (role.OnLand && KAKeyboardMgr.IsArrowKey() && !m_roleStateMgr.role.KeyboardMgr.IsRunKey())
		{
			return true;
		}

		return false;
	}

	public KAWalkState(KARoleStateMgr StateMgr)
		: base(StateMgr)
	{
		m_info.state = KAStateType.S_WALK;
		m_info.hash = Animator.StringToHash("Walk");
		m_info.nextState = new KAStateType[] { KAStateType.S_IDLE, KAStateType.S_RUN, KAStateType.S_JUMP };
	}

}

public class KARunState : KAState
{
	public override bool check(KAState lastState)
	{
		KARole role = m_roleStateMgr.role;
		if (role.OnLand && KAKeyboardMgr.IsArrowKey() && m_roleStateMgr.role.KeyboardMgr.IsRunKey())
		{
			return true;
		}

		return false;
	}

	public KARunState(KARoleStateMgr StateMgr)
		: base(StateMgr)
	{
		m_info.state = KAStateType.S_RUN;
		m_info.hash = Animator.StringToHash("Run");
		m_info.nextState = new KAStateType[] { /*KAStateType.S_WALK,*/ KAStateType.S_IDLE, KAStateType.S_JUMP };
	}

}

public class KAJumpState : KAState
{
	public override bool check(KAState lastState)
	{
		KARole role = m_roleStateMgr.role;
		if (role.OnLand  && Input.GetKeyDown(KeyCode.C))
		{
			return true;
		}

		return false;
	}

	public KAJumpState(KARoleStateMgr StateMgr)
		: base(StateMgr)
	{
		m_info.state = KAStateType.S_JUMP;
		m_info.hash = Animator.StringToHash("Idle");
		m_info.nextState = new KAStateType[] { /*KAStateType.S_WALK,*/ KAStateType.S_IDLE };
	}

	public override void PreAction()
	{
		KARole role = m_roleStateMgr.role;
		role.rigidbody.AddForce(new Vector3(0, role.JumpForce, 0));
	}

}