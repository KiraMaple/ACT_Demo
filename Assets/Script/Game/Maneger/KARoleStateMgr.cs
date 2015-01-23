using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KARoleStateMgr {

	public enum State
	{
		S_NONE,
		S_IDLE,
		S_WALK,
		S_RUN,
		S_JUMP,
		S_ATTACKED,
		S_SKILL
	}

	private KARole m_role = null;
	private KAAnimationMgr m_pAnimationMgr = null;
	private State m_state = State.S_NONE;
	private Dictionary<State, string> m_stateAnima = new Dictionary<State, string>();

	public KARoleStateMgr(KARole role)
	{
		m_role = role;
		m_pAnimationMgr = role.AnmationMgr;
		m_stateAnima.Add(State.S_IDLE, "Idle");
		m_stateAnima.Add(State.S_WALK, "Walk");
		m_stateAnima.Add(State.S_RUN, "Run");
		ChangeToState(State.S_IDLE);
	}

	public State state
	{
		get { return m_state; }
	}

	public void UpdateKeyboadInput()
	{
		if ( !KAKeyboardMgr.IsMoveKey() )
		{
			// 没有速度
			if ( IsCanIdel() )
			{
				ChangeToState(State.S_IDLE);
			}
		}

		// TODO 创建State类
	}

	public void ChangeToState(State state)
	{
		m_state = state;
		string AnimaName = string.Empty;
		if ( m_stateAnima.TryGetValue(state, out AnimaName) )
		{
			m_pAnimationMgr.Play(AnimaName);
		}
	}

	public bool IsCanDeal(KACombKeyData data)
	{
		return true;
	}

	public bool IsCanIdel()
	{
		if (m_state == State.S_WALK || m_state == State.S_RUN)
		{
			return true;
		}
		return false;
	}

	public bool IsCanMove()
	{
		if (m_state == State.S_IDLE || m_state == State.S_WALK || m_state == State.S_RUN || m_state == State.S_JUMP)
		{
			return true;
		}
		return false;
	}

	public bool IsCanWalk()
	{
		if (m_state == State.S_IDLE || m_state == State.S_RUN)
		{
			return true;
		}
		return false;
	}

	public bool IsCanRun()
	{
		if (m_state == State.S_IDLE || m_state == State.S_WALK)
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
