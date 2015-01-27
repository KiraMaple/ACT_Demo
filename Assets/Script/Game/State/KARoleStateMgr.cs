using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum KAStateType
{
	S_NONE,
	S_IDLE,
	S_WALK,
	S_RUN,
	S_JUMP,
	S_ATTACK,
	S_ATTACKED,
	S_SKILL
}

public class KARoleStateMgr
{
	private KARole m_role = null;
	private KAState m_lastState = null;
	private KAState m_state = null;
	private Dictionary<int, KAState> m_mapState = new Dictionary<int, KAState>();

	public KARoleStateMgr(KARole role)
	{
		m_role = role;

		KAState IdleState = new KAStateIdle(this);
		m_mapState.Add(IdleState.type, IdleState);

		KAState WalkState = new KAStateWalk(this);
		m_mapState.Add(WalkState.type, WalkState);

		KAState RunState = new KAStateRun(this);
		m_mapState.Add(RunState.type, RunState);

		KAState JumpState = new KAStateJump(this);
		m_mapState.Add(JumpState.type, JumpState);

		KAState AttackState = new KAStateAttack(this);
		m_mapState.Add(AttackState.type, AttackState);

		ChangeToState((int)KAStateType.S_IDLE);
	}

	public KARole role
	{
		get { return m_role; }
	}

	public int StateType
	{
		get { return m_state.type; }
	}

	public int LastStateType
	{
		get { return m_lastState != null ? m_lastState.type : (int)KAStateType.S_NONE; }
	}

	public KAState state
	{
		get { return m_state; }
	}

	public KAState getStateByType(int type)
	{
		return m_mapState[type];
	}

	public void Update()
	{
		m_state.Update();
	}

	public bool ChangeToState(int StateType)
	{
		KAState state = null;
		if ( !m_mapState.TryGetValue(StateType, out state) )
		{
			return false;
		}

		if (m_state != null)
		{
			m_state.AfterAction();
		}
		m_lastState = m_state;
		m_state = state;
		if (m_state.hash != 0)
		{
			m_role.AnmationMgr.Play(m_state.hash);
		}
		m_state.PreAction();
		return true;
	}

	public bool IsCanDeal(KACombKeyData data)
	{
		return true;
	}

	public bool IsCanMove()
	{
		if (StateType == (int)KAStateType.S_WALK || StateType == (int)KAStateType.S_RUN || StateType == (int)KAStateType.S_JUMP || StateType == (int)KAStateType.S_ATTACK)
		{
			return true;
		}

		return false;
	}

	public void DealCombKeyData(KACombKeyData data)
	{
		string log = "DataType:";
		switch(data.type)
		{
			case KACombKeyData.DataType.TYPE_SKILL:
			{
				log += "Skill";
				;
			}
			break;
			default:
			{
				;
			}
			break;
		}
		Debug.Log(log);
	}
}
