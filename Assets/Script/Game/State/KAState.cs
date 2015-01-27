using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class KAState
{
	public abstract class KASubState : KAState
	{
		protected const int SUBSTATE_END = 0;
		protected KAState m_parent;

		protected KASubState(KAState parent)
			: base(parent.StateMgr)
		{
			m_parent = parent;
		}

		public override bool check(KAState lastState)
		{
			return true;
		}

		protected override void UpdateNextState()
		{
			if ( m_info.nextState == null )
			{
				if (!m_roleStateMgr.role.AnmationMgr.isPlaying)
				{
					AfterAction();
				}
			}
			else
			{
				foreach (int type in m_info.nextState)
				{
					if (type == SUBSTATE_END)
					{
						if (!m_roleStateMgr.role.AnmationMgr.isPlaying)
						{
							AfterAction();
						}
					}
					else
					{
						KAState nextState = m_parent.GetSubStateByType(type);
						if (nextState != null && nextState.check(this))
						{
							if (m_parent.ChangeToSubState(type))
							{
								return;
							}
						}

					}

				}
			}
		}
	}

	public class StateInfo
	{
		public int type = (int)KAStateType.S_NONE;
		public int hash = 0;
		public int[] nextState = null;
	}

	// 状态所属的角色
	protected KARoleStateMgr m_roleStateMgr = null;
	protected StateInfo m_info = new StateInfo();
	protected KASubState m_subState = null;
	protected Dictionary<int, KASubState> m_mapSubState = new Dictionary<int, KASubState>();
	// 表示是否开始响应下一连续技的按键
	protected float m_fParam1 = 0;
	protected float m_fParam2 = 0;
	protected float m_fParam3 = 0;
	protected float m_fParam4 = 0;

	public KARoleStateMgr StateMgr
	{
		get { return m_roleStateMgr; }
	}

	// 状态类型
	public int type
	{
		get { return m_info.type; }
	}

	// Animation Hash
	public int hash
	{
		get { return m_info.hash; }
	}

	public int[] NextState
	{
		get { return m_info.nextState; }
	}

	public KASubState GetSubStateByType(int type)
	{
		return m_mapSubState[type];
	}

	public float Param1
	{
		get
		{
			if (m_subState != null)
			{
				return m_subState.Param1;
			}
			else
			{
				return m_fParam1;
			}
		}
		set
		{
			if (m_subState != null)
			{
				m_subState.Param1 = value;
			}
			else
			{
				m_fParam1 = value;
			}
		}
	}

	public float Param2
	{
		get
		{
			if (m_subState != null)
			{
				return m_subState.Param2;
			}
			else
			{
				return m_fParam2;
			}
		}
		set
		{
			if (m_subState != null)
			{
				m_subState.m_fParam2 = value;
			}
			else
			{
				m_fParam2 = value;
			}
		}
	}

	public float Param3
	{
		get
		{
			if (m_subState != null)
			{
				return m_subState.Param3;
			}
			else
			{
				return m_fParam3;
			}
		}
		set
		{
			if (m_subState != null)
			{
				m_subState.Param3 = value;
			}
			else
			{
				m_fParam3 = value;
			}
		}
	}

	public float Param4
	{
		get
		{
			if (m_subState != null)
			{
				return m_subState.Param4;
			}
			else
			{
				return m_fParam4;
			}
		}
		set
		{
			if (m_subState != null)
			{
				m_subState.Param4 = value;
			}
			else
			{
				m_fParam4 = value;
			}
		}
	}

	public abstract bool check(KAState lastState);

	public virtual void PreAction()
	{
		Clear();
	}

	public virtual void AfterAction()
	{
		Clear();
	}

	protected void Clear()
	{
		m_fParam1 = 0;
		m_fParam2 = 0;
		m_fParam3 = 0;
		m_fParam4 = 0;
	}

	protected virtual void UpdateSelf()
	{
		if (m_subState != null)
		{
			m_subState.Update();
		}

	}

	protected KAState(KARoleStateMgr StateMgr)
	{
		if (StateMgr == null)
		{
			Debug.LogError("State Construct Error. StateMgr is null.");
		}

		m_roleStateMgr = StateMgr;
	}

	public void Update()
	{
		UpdateSelf();
		UpdateNextState();
	}

	protected bool ChangeToSubState(int type)
	{
		KASubState state = null;
		if (!m_mapSubState.TryGetValue(type, out state))
		{
			return false;
		}

		if (m_subState != null)
		{
			m_subState.AfterAction();
		}
		m_subState = state;
		if (m_subState.hash != 0)
		{
			m_roleStateMgr.role.AnmationMgr.Play(m_subState.hash);
		}
		m_subState.PreAction();
		return true;
	}

	protected virtual void UpdateNextState()
	{
		if (m_info.nextState == null)
		{
			if (!m_roleStateMgr.role.AnmationMgr.isPlaying)
			{
				AfterAction();
			}
		}
		else
		{
			foreach (int type in m_info.nextState)
			{
				KAState state = m_roleStateMgr.getStateByType(type);
				if ( state.check(this) )
				{
					if ( m_roleStateMgr.ChangeToState(type) )
					{
						return;
					}

				}

			}

		}

	}

}