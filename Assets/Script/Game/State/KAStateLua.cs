using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using KAct;

namespace KAct
{
	public class KAState
	{
		public class StateInfo
		{
			public int type = (int)KAStateType.S_NONE;
			public int hash = 0;
			public int[] nextState = null;
			public LuaFunction check = null;
			public LuaFunction PreAction = null;
			public LuaFunction AfterAction = null;
		}

		// 状态所属的角色
		protected string m_path = "";
		protected KARoleStateMgr m_roleStateMgr = null;
		protected StateInfo m_info = new StateInfo();

		protected const int SUBSTATE_END = -1;
		protected KAState m_subState = null;
		protected KAState m_parent = null;
		protected Dictionary<int, KAState> m_mapSubState = new Dictionary<int, KAState>();
		
		// 表示是否开始响应下一连续技的按键
		protected float m_fParam1 = 0;
		protected float m_fParam2 = 0;
		protected float m_fParam3 = 0;
		protected float m_fParam4 = 0;

		public KAState(KARoleStateMgr StateMgr, KAState parent = null)
		{
			if (StateMgr == null)
			{
				Debug.LogError("State Construct Error. StateMgr is null.");
			}

			m_roleStateMgr = StateMgr;

			if (parent != null)
			{
				m_parent = parent;
			}
		}

		public KAState(KARoleStateMgr StateMgr, string luaFile, KAState parent = null)
			: this(StateMgr, parent)
		{
			LoadFromFile(luaFile);
		}

		public bool LoadFromFile(string luaFile, string stateName="State")
		{
			LuaState ls = new LuaState();
			TextAsset luaContent = (TextAsset)Resources.Load(luaFile);
			if (luaContent == null)
			{
				Debug.LogError("State Init Error. Failed to Load File. File:" + luaFile);
				return false;
			}
			ls.DoString(luaContent.text);
			m_path = luaFile;

			LuaTable state = (LuaTable)ls[stateName];
			if (state == null)
			{
				Debug.LogError("State Init Error. Can not find State Info in lua. File:" + luaFile);
				return false;
			}

			if (state["type"] == null || state["type"].GetType().ToString() != "System.Double")
			{
				Debug.LogError("State Init Error. Can not find type in State Info. File:" + luaFile);
				return false;
			}
			int type = Convert.ToInt32((double)state["type"]);
			m_info.type = type;

			if (state["role"] == null)
			{
				Debug.LogError("State Init Error. Can not find role in State Info. File:" + luaFile);
				return false;
			}
			state["role"] = m_roleStateMgr.role;

			string animation = (string)state["animation"];
			if (animation != null && animation != "")
			{
				m_info.hash = Animator.StringToHash(animation);
			}

			LuaTable nextState = (LuaTable)state["nextState"];
			if (nextState == null || nextState.Values.Count == 0)
			{
				Debug.LogError("State Init Error. Can not find Next State in State Info. File:" + luaFile);
				return false;
			}

			int nNextStateCnt = nextState.Values.Count;
			m_info.nextState = new int[nNextStateCnt];
			for (int i = 0; i < nNextStateCnt; i++)
			{
				m_info.nextState[i] = Convert.ToInt32(nextState[i+1]);
			}

			object oCnt = state["SubStateCount"];
			object oPath = state["SubState"];
			int nSubStateCnt = oCnt != null ? Convert.ToInt32(oCnt) : 0;
			string SubStatePath = oPath != null ? Convert.ToString(oPath) : "";
			if (nSubStateCnt != 0 && SubStatePath != "")
			{
				int success = ReadSubState(nSubStateCnt, SubStatePath);
				if (success < nSubStateCnt)
				{
					Debug.LogError("State Init Error. SubState Info is invalid. File:" + luaFile);
					return false;
				}
			}

			LuaFunction check = (LuaFunction)state["Check"];
			if (check != null)
			{
				m_info.check = check;
			}

			LuaFunction PreAction = (LuaFunction)state["PreAction"];
			if (PreAction != null)
			{
				m_info.PreAction = PreAction;
			}

			LuaFunction AfterAction = (LuaFunction)state["AfterAction"];
			if (AfterAction != null)
			{
				m_info.AfterAction = AfterAction;
			}

			return true;
		}

		private int ReadSubState(int nSubSTateCnt, string path)
		{
			if (path == null || nSubSTateCnt == 0 || path == "")
			{
				Debug.LogError("SubState Init Error. SubState Info is invalid. File:" + path);
				return -1;
			}

			if (nSubSTateCnt < 0 || nSubSTateCnt >= 100)
			{
				Debug.LogError("SubState Init Error. SubState Count is invalid. File:" + path);
				return -1;
			}

			int ret = 0;
			for (int i = 0; i < nSubSTateCnt; i++ )
			{
				string stateName = String.Format("State{0:00}", i);
				KAState state = new KAState(m_roleStateMgr, this);
				if ( state.LoadFromFile(path, stateName) )
				{
					m_mapSubState.Add(state.type, state);
					ret++;
				}
			}

			return ret;
		}

		public KARoleStateMgr RoleStateMgr
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

		public KAState GetSubStateByType(int type)
		{
			KAState state = null;
			if ( m_mapSubState.TryGetValue(type, out state) )
			{
				return state;
			}
			return null;
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

		private bool StateCheck(KAState lastState)
		{
			if (m_info.check != null)
			{
				object[] ret = m_info.check.Call(lastState);
				if (ret.Length != 0 && ret[0].ToString() != "True" && ret[0].ToString() != "False")
				{
					Debug.LogError("The value returned by Check Function in invalid. Return:"+ret[0].ToString()+", File:" + m_path);
					return false;
				}
				return (bool)ret[0];
			}
			return true;
		}

		public virtual bool check(KAState lastState)
		{
			if (m_info.check != null)
			{
				return StateCheck(lastState);
			}
			return true;
		}

		private void StatePreAction()
		{
			if (m_info.PreAction != null)
			{
				m_info.PreAction.Call();
			}
		}

		public virtual void PreAction()
		{
			Clear();
			if (m_mapSubState.Values.Count != 0)
			{
				ChangeToSubState(0);
			}
			StatePreAction();
		}

		private void StateAfterAction()
		{
			if (m_info.AfterAction != null)
			{
				m_info.AfterAction.Call();
			}
		}

		public virtual void AfterAction()
		{
			StateAfterAction();
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

		public void Update()
		{
			UpdateSelf();
			UpdateNextState();
		}

		protected bool ChangeToSubState(int type)
		{
			KAState state = null;
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
					KAState state = null;
					if (m_parent == null)
					{
						state = m_roleStateMgr.getStateByType(type);
						if (state != null && state.check(this))
						{
							if (m_roleStateMgr.ChangeToState(type))
							{
								return;
							}

						}
					}
					else
					{
						if (type == SUBSTATE_END)
						{
							if (!m_roleStateMgr.role.AnmationMgr.isPlaying)
							{
								m_parent.AfterAction();
							}
						}
						else
						{
							state = m_parent.GetSubStateByType(type);
							if (state != null && state.check(this))
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

	}

}