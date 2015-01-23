using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Saber : MonoBehaviour
{
	/*
	private string[] m_animName = {
									  "BattleIdle",
									  "Walk",
									  "Run"
								  };

	private GroupKey[] m_aGourpKey = new GroupKey[]{
		new GroupKey(new KeyCode[]{KeyCode.LeftArrow, KeyCode.LeftArrow}),
		new GroupKey(new KeyCode[]{KeyCode.RightArrow, KeyCode.RightArrow})
	};

	private Animator m_stAnimator;
	private Rigidbody m_stRigidbody;

	private bool m_bIsRun = false;
	private	float m_nSaveTime = 0;
	private float m_fWalkXSpeed = 10f;
	private float m_fWalkZSpeed = 10f;
	private float m_fRunXSpeed = 20f;
	private float m_fRunZSpeed = 10f;
	private const int m_nQueueCnt = 10;
	private List<KeyCode> m_keycodeList;

	// Use this for initialization
	void Start()
	{
		m_stAnimator = transform.Find("Animation").GetComponent<Animator>();
		if (m_stAnimator == null)
		{
			Application.Quit();
		}
		rigidbody.velocity = Vector3.zero;
		m_keycodeList = new List<KeyCode>();

		m_stAnimator.Play(getAnimationName(AnimType.BATTLE_IDLE));
	}

	// Update is called once per frame
	void Update()
	{
		DealKeyInput();
	}

	private bool DealKeyInput()
	{
		SaveKeyInput();
		DealGroupInput();
		if (m_bIsRun == false)
		{
			m_bIsRun = CheckRunKey();
		}

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

		if (vInput != Vector3.zero)
		{
			if (vInput.x > 0)
			{
				faceTo(true);
			}
			else if (vInput.x < 0)
			{
				faceTo(false);
			}

			//TODO 反向对isRun的操作

			if (m_bIsRun)
			{
				m_stAnimator.Play(getAnimationName(AnimType.RUN));
				
				vInput.x *= m_fRunXSpeed;
				vInput.z *= m_fRunZSpeed;
			}
			else
			{
				m_stAnimator.Play(getAnimationName(AnimType.WALK));

				vInput.x *= m_fWalkXSpeed;
				vInput.z *= m_fWalkZSpeed;
			}
		}
		else
		{
			m_stAnimator.Play(getAnimationName(AnimType.BATTLE_IDLE));
		}

		rigidbody.velocity = vInput;

		return true;
	}

	private void faceTo(bool isRight)
	{
		Vector3 vScale = transform.localScale;
		if (isRight)
		{
			vScale.x = Mathf.Abs(vScale.x);
		}
		else
		{
			vScale.x = -Mathf.Abs(vScale.x);
		}
		transform.localScale = vScale;
	}

	private void SaveKeyInput()
	{
		m_nSaveTime += Time.deltaTime;

		if (m_nSaveTime > 0.5f)
		{
			ClearKeyList();
			m_nSaveTime = 0;
		}

		bool isSave = false;

		if (Input.anyKey)
		{
			;
		}

		foreach ( KeyCode key in System.Enum.GetValues( typeof(KeyCode) ) )
		{
			if ( Input.GetKeyDown(key) )
			{
				if (m_keycodeList.Count > m_nQueueCnt)
				{
					m_keycodeList.RemoveAt(0);
				}
				m_keycodeList.Add(key);
				isSave = true;
			}
		}

		if (isSave)
		{
			m_nSaveTime = 0;
		}
	}

	private void ClearKeyList()
	{
		m_keycodeList.Clear();
	}

	private void DealGroupInput()
	{
		foreach( GroupKey group in m_aGourpKey)
		{
			;
		}
	}

	private bool CheckRunKey()
	{
		bool bFound = false;
		foreach (GroupKey group in m_aGourpKey)
		{

			int nCount = group.GetKeyCodeCount();
			if (m_keycodeList.Count < nCount)
			{
				return false;
			}
			for (int i = 0; i < nCount; i++)
			{
				int nGroupIndex = nCount - i - 1;
				int nListIndex = m_keycodeList.Count - i - 1;
				KeyCode keyInList = m_keycodeList[nListIndex];
				KeyCode keyInGroup = group.GetKeyCode(nGroupIndex);
				if (keyInList != keyInGroup)
				{
					bFound = false;
				}
				else
				{
					bFound = true;
				}
			}

			if (bFound == true)
			{
				return true;
			}
		}
		return false;
	}
	*/
}
