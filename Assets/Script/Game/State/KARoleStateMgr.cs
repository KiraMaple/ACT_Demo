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
	S_ATTACKED,
	S_SKILL
}

public class KARoleStateMgr
{
	private KARole m_role = null;
	private KAState m_state = null;
	private Dictionary<KAStateType, KAState> m_mapState = new Dictionary<KAStateType, KAState>();

	public KARoleStateMgr(KARole role)
	{
		m_role = role;
		m_mapState.Add(KAStateType.S_IDLE, new KAIdleState(this));
		m_mapState.Add(KAStateType.S_WALK, new KAWalkState(this));
		m_mapState.Add(KAStateType.S_RUN, new KARunState(this));
		m_mapState.Add(KAStateType.S_JUMP, new KAJumpState(this));
		ChangeToState(KAStateType.S_IDLE);
	}

	public KARole role
	{
		get { return m_role; }
	}

	public KAStateType StateType
	{
		get { return m_state.state; }
	}

	public KAState state
	{
		get { return m_state; }
	}

	public KAState getStateByType(KAStateType type)
	{
		return m_mapState[type];
	}

	public void Update()
	{
		m_state.UpdateState();
	}

	public void ChangeToState(KAStateType StateType)
	{
		KAState state = null;
		if ( !m_mapState.TryGetValue(StateType, out state) )
		{
			return;
		}

		m_state = state;
		m_role.AnmationMgr.Play(m_state.hash);
		m_state.PreAction();
	}

	public bool IsCanDeal(KACombKeyData data)
	{
		return true;
	}

	public bool IsCanMove()
	{
		if (StateType == KAStateType.S_WALK || StateType == KAStateType.S_RUN || StateType == KAStateType.S_JUMP)
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
