using UnityEngine;
using System;
using System.Collections;
using LuaInterface;

namespace KAct
{
	public class KASkill : KAState {

		protected int m_id = 0;

		public KASkill(KARoleStateMgr StateMgr, KAState parent = null)
			: base(StateMgr, parent)
		{ }

		public bool LoadFromFile(string luaFile, string stateName = "State")
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

			/*
			if (state["type"] == null || state["type"].GetType().ToString() != "System.Double")
			{
				Debug.LogError("State Init Error. Can not find type in State Info. File:" + luaFile);
				return false;
			}
			int type = Convert.ToInt32((double)state["type"]);
			m_info.type = type;
			*/

			if (state["key"] == null || state["type"].GetType().ToString() != "System.Double")
			{
				Debug.LogError("State Init Error. Can not find type in State Info. File:" + luaFile);
				return false;
			}

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
				m_info.nextState[i] = Convert.ToInt32(nextState[i + 1]);
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

	}


}