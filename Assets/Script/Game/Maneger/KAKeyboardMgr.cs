using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class KACombKeyData
{
	public enum DataType
	{
		TYPE_RUN,
		TYPE_SKILL
	}

	private DataType m_type;
	private uint m_param1;
	private uint m_param2;

	public KACombKeyData(DataType type, uint param1, uint param2 = 0)
	{
		m_type = type;
		m_param1 = param1;
		m_param2 = param2;
	}

	public DataType type
	{
		get { return m_type; }
	}

	public uint param1
	{
		get { return m_param1; }
	}

	public uint param2
	{
		get { return m_param2; }
	}

}

public class KACombKey
{
	public const int MAX_KEY_COMB = 10;
	private KeyCode[] m_keyArray = null;
	private KACombKeyData m_customData;

	public KACombKey(KeyCode[] keyArray, KACombKeyData customData)
	{
		int ArrayLength = keyArray.Length;
		if (keyArray.Length > MAX_KEY_COMB)
		{
			ArrayLength = MAX_KEY_COMB;
		}
		m_keyArray = new KeyCode[ArrayLength];
		for (int i = 0; i < ArrayLength; i++)
		{
			m_keyArray[i] = keyArray[i];
		}
		m_customData = customData;
	}

	public KeyCode this[int index]
	{
		get { return m_keyArray[index]; }
		//set { m_keyArray[index] = value;  }
	}

	public KACombKeyData CustomData
	{
		get { return m_customData; }
	}

	public int Length
	{
		get { return m_keyArray.Length; }
	}

	public bool IsComb(KeyCode[] keyArray)
	{
		if (keyArray.Length < m_keyArray.Length)
		{
			return false;
		}

		int nStart = keyArray.Length - m_keyArray.Length;
		for ( int i = 0 ; i < m_keyArray.Length ; i++ )
		{
			if (m_keyArray[i] != keyArray[nStart + i])
			{
				return false;
			}
		}
		return true;
	}

	public bool IsComb(List<KeyCode> keyList)
	{
		KeyCode[] keyArray = keyList.ToArray();
		return IsComb(keyArray);
	}
}

public class KAKeyboardMgr
{
	private KARole m_role = null;
	private KARoleStateMgr m_stateMgr = null;

	// 奔跑组合键
	private KACombKey[] m_runKeyArray = new KACombKey[] {
		new KACombKey( new KeyCode[] { KeyCode.LeftArrow, KeyCode.LeftArrow },
			new KACombKeyData ( KACombKeyData.DataType.TYPE_RUN, 0u)),
		new KACombKey( new KeyCode[] { KeyCode.RightArrow, KeyCode.RightArrow },
			new KACombKeyData ( KACombKeyData.DataType.TYPE_RUN, 0u))
	};

	// 组合键列表
	private List<KACombKey> m_combKeyList = new List<KACombKey>();

	// 已经输入的键位列表
	private List<KeyCode> m_inputKeyList = new List<KeyCode>();

	// 监听的键列表
	private KeyCode[] m_listenedKeyList = new KeyCode[] {
		KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,	//上下左右
		KeyCode.X, KeyCode.Z, KeyCode.C	//攻击、技能、跳跃
	};

	/*
	// 监听按键位标志
	private int m_keyMask = 0;
	*/

	// 组合键，键与键之间的最大空隙
	private float m_maxDelteTime = 0.5f;
	private float m_deltaTime = 0.0f;

	public KAKeyboardMgr(KARole role)
	{
		m_role = role;
		m_stateMgr = role.RoleStateMgr;
		ReadCombKeyList();
	}

	public void ReadCombKeyList()
	{
		// TODO 随后可以更改为读取配置档加载

		m_combKeyList.Add(new KACombKey(
			new KeyCode[] { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.Z },
			new KACombKeyData(KACombKeyData.DataType.TYPE_SKILL, 1u)
				));

		m_combKeyList.Add(new KACombKey(
			new KeyCode[] { KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.Z },
			new KACombKeyData(KACombKeyData.DataType.TYPE_SKILL, 1u)
				));
	}

	public void UpdateKeyboadInput()
	{
		UpdateInputList();

		if (Input.anyKeyDown)
		{
			//DealKeyChange();
			DealCombKey();
		}
	}

	private void UpdateInputList()
	{
		m_deltaTime += Time.deltaTime;

		if (m_deltaTime > m_maxDelteTime)
		{
			ClearInputList();
		}

		if (Input.anyKeyDown)
		{
			m_deltaTime = 0;
			// TODO 测试同时按键
			foreach (KeyCode key in m_listenedKeyList)
			{
				if (Input.GetKeyDown(key))
				{
					if (m_inputKeyList.Count >= KACombKey.MAX_KEY_COMB)
					{
						m_inputKeyList.RemoveAt(0);
					}
					m_inputKeyList.Add(key);
				}
			}

		}
	}

	/*
	private void DealKeyChange()
	{
		for (int i = 0; i < m_listenedKeyList.Length; i++ )
		{
			KeyCode key = m_listenedKeyList[i];
			if (Input.GetKeyDown(key))
			{
				if (key == KeyCode.LeftArrow)
				{
					m_role.FaceTo(KACommon.Direction.D_LEFT);
				}

				if (key == KeyCode.RightArrow)
				{
					m_role.FaceTo(KACommon.Direction.D_RIGHT);
				}
				m_keyMask = m_keyMask | (1 << i);
			}

			if (Input.GetKeyUp(key))
			{
				m_keyMask = m_keyMask & ~(1 << i);
			}
		}
	}
	*/

	public bool IsRunKey()
	{
		if (Input.anyKeyDown)
		{
			foreach (KACombKey CombKey in m_runKeyArray)
			{
				if (CombKey.IsComb(m_inputKeyList))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void DealCombKey()
	{
		// 只响应最长组合键
		KACombKey CombKeyDeal = null;
		foreach (KACombKey CombKey in m_combKeyList)
		{
			if (CombKey.IsComb(m_inputKeyList))
			{
				if (CombKeyDeal == null)
				{
					CombKeyDeal = CombKey;
				}
				else
				{
					if (CombKeyDeal.Length < CombKey.Length)
					{
						if (m_stateMgr.IsCanDeal(CombKeyDeal.CustomData))
						{
							CombKeyDeal = CombKey;
						}
					}
				}
			}
		}

		if (CombKeyDeal != null)
		{
			KACombKeyData data = CombKeyDeal.CustomData;
			m_stateMgr.DealCombKeyData(data);
		}
	}

	private void ClearInputList()
	{
		if (m_inputKeyList.Count != 0)
		{
			m_inputKeyList.Clear();
		}
		m_deltaTime = 0;
	}

	public static bool IsArrowKey()
	{
		bool left = Input.GetKey(KeyCode.LeftArrow);
		bool right = Input.GetKey(KeyCode.RightArrow);
		bool up = Input.GetKey(KeyCode.UpArrow);
		bool down = Input.GetKey(KeyCode.DownArrow);
		if (((left && right) && (up && down)) ||
			((left && right) && (!up && !down)) ||
			((!left && !right) && (up && down)) ||
			((!left && !right) && (!up && !down)))
		{
			return false;
		}
		return true;
	}

}
