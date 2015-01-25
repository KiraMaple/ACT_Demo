using UnityEngine;
using System.Collections;

public class KAAnimationMgr {

	private KARole m_role = null;
	private Animator m_animator = null;

	public KAAnimationMgr(KARole role)
	{
		m_role = role;
		m_animator = m_role.transform.Find("Animation").GetComponent<Animator>();
		if (m_animator == null)
		{
			Debug.Log("Role Animator is not found. Role:" + m_role.transform.name);
		}
	}

	public void Play(string name)
	{
		m_animator.Play(name);
	}

	public void Play(int hash)
	{
		m_animator.Play(hash);
	}

}
