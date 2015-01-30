using UnityEngine;
using System.Collections;

namespace KAct
{
	public class KAPlayerAnimationListener : MonoBehaviour
	{

		KARole m_role = null;

		// Use this for initialization
		void Start()
		{
			m_role = transform.parent.GetComponent<KARole>();
			if (m_role == null)
			{
				Debug.LogError("AnimationListener Init Error. The Role is invalid.");
			}
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void SetParam1(float param)
		{
			m_role.StateParam1 = param;
		}
		public void SetParam2(float param)
		{
			m_role.StateParam2 = param;
		}
		public void SetParam3(float param)
		{
			m_role.StateParam3 = param;
		}
		public void SetParam4(float param)
		{
			m_role.StateParam4 = param;
		}

	}
}