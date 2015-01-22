using UnityEngine;
using System.Collections;

public class Saber : MonoBehaviour
{
	enum AnimType
	{
		BATTLE_IDLE,
		WALK,
		RUN
	};

	public class GroupKey
	{
		public const int m_nKeyMax = 10;
		private int m_nKeyCount = 0;
		private KeyCode[] m_aKeyCode;

		public GroupKey(KeyCode[] aKeyCode)
		{
			m_aKeyCode = new KeyCode[m_nKeyMax];
			for (int i = 0; i < m_nKeyCount && i < aKeyCode.Length; i++ )
			{
				m_aKeyCode[i] = aKeyCode[i];
			}
		}

		public int GetKeyCodeCount()
		{
			return m_nKeyCount;
		}

		public KeyCode GetKeyCode(int nPos)
		{
			if (nPos >= m_nKeyCount)
			{
				return KeyCode.None;
			}

			return m_aKeyCode[nPos];
		}
	}

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

	private float m_fXSpeed = 10f;
	private float m_fZSpeed = 8f;
	private const int m_nQueueCnt = 10;
	private Queue m_keycodeQueue;

	private string getAnimationName(AnimType index)
	{
		string szRet = string.Empty;
		switch (index)
		{
			case AnimType.BATTLE_IDLE:
				szRet = m_animName[0];
				break;
			case AnimType.WALK:
				szRet = m_animName[1];
				break;
			case AnimType.RUN:
				szRet = m_animName[2];
				break;
			default:
				szRet = m_animName[0];
				break;
		}
		return szRet;
	}

	// Use this for initialization
	void Start()
	{
		m_stAnimator = transform.Find("Animation").GetComponent<Animator>();
		if (m_stAnimator == null)
		{
			Application.Quit();
		}
		rigidbody.velocity = Vector3.zero;
		m_keycodeQueue = new Queue(m_nQueueCnt);

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
		bool bIsRun = CheckRunKey();

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
			m_stAnimator.Play(getAnimationName(AnimType.WALK));

			Vector3 vVelocity = rigidbody.velocity;

			vInput.x *= m_fXSpeed;
			vInput.z *= m_fZSpeed;
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
		foreach ( KeyCode key in System.Enum.GetValues( typeof(KeyCode) ) )
		{
			if ( Input.GetKeyDown(key) )
			{
				if (m_keycodeQueue.Count > m_nQueueCnt)
				{
					m_keycodeQueue.Dequeue();
				}
				m_keycodeQueue.Enqueue(key);
			}
		}
	}

	private void DealGroupInput()
	{
		foreach( GroupKey group in m_aGourpKey)
		{
			;
		}
	}

	private void CheckRunKey()
	{
		foreach (GroupKey group in m_aGourpKey)
		{
			int nCount = group.GetKeyCodeCount();
			for (int i = 0; i < nCount; i++)
			{
				int nGroupIndex = nCount - i - 1;
				int nQueueIndex = m_nQueueCnt - i - 1;
				m_keycodeQueue.Peek();
			}
		}
	}
}
