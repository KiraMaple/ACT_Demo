using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class KAState
{
	public class StateInfo
	{
		public KAStateType state = KAStateType.S_NONE;
		public int hash = 0;
		public KAStateType[] nextState = null;
	}

	// 状态所属的角色
	protected KARoleStateMgr m_roleStateMgr = null;
	protected StateInfo m_info = new StateInfo();
	// 状态类型
	public KAStateType state
	{
		get { return m_info.state; }
	}

	// Animation Hash
	public int hash
	{
		get { return m_info.hash; }
	}

	public KAStateType[] NextState
	{
		get { return m_info.nextState; }
	}
	public abstract bool check(KAState lastState);

	public virtual void PreAction()
	{
	}

	protected KAState(KARoleStateMgr StateMgr)
	{
		if (StateMgr == null)
		{
			Debug.LogError("State Construct Error. StateMgr is null.");
		}

		m_roleStateMgr = StateMgr;
	}

	public void UpdateState()
	{
		foreach (KAStateType type in m_info.nextState)
		{
			KAState state = m_roleStateMgr.getStateByType(type);
			if ( state.check(this) )
			{
				m_roleStateMgr.ChangeToState(type);
				return;
			}
		}
	}

}
