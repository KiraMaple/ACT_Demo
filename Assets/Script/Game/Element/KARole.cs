using UnityEngine;
using System.Collections;

namespace KAct
{
	public class KARole : MonoBehaviour
	{

		protected bool m_bLand = true;
		protected bool m_bSkill = false;
		protected bool m_bLeft = false;
		protected float m_fWalkXSpeed = 10f;
		protected float m_fWalkZSpeed = 8f;
		protected float m_fRunXSpeed = 20f;
		protected float m_fRunZSpeed = 16f;
		protected float m_fAttackMoveSpeed = 2.0f;
		protected float m_fAttackMoveSpeedX = 4.0f;
		protected float m_fJumpBase = 28.0f;
		protected float m_fJumpForce = 0.0f;

		protected KAAnimationMgr m_animationMgr = null;
		protected KARoleStateMgr m_roleStateMgr = null;

		public KAAnimationMgr AnmationMgr { get { return m_animationMgr; } }

		public KARoleStateMgr RoleStateMgr { get { return m_roleStateMgr; } }

		public float JumpForce
		{
			get { return m_fJumpForce; }
			set { m_fJumpForce = value; }
		}

		public float JumpBase
		{
			get { return m_fJumpBase; }
		}

		public bool OnLand
		{
			get { return m_bLand; }
			set { m_bLand = value; }
		}

		public bool IsSkill
		{
			get { return m_bSkill; }
			set { m_bSkill = value; }
		}

		// Use this for initialization
		public virtual void Start()
		{
			m_animationMgr = new KAAnimationMgr(this);
			m_roleStateMgr = new KARoleStateMgr(this);
			JumpForce = JumpBase;
		}

		// Update is called once per frame
		public virtual void Update()
		{
			m_roleStateMgr.Update();
			UpdateDir();
			UpdateMove();
		}

		protected virtual void UpdateDir()
		{
		}

		protected virtual void UpdateMove()
		{
		}

		public void FaceTo(KACommon.Direction dir)
		{
			KAStateType type = (KAStateType)m_roleStateMgr.StateType;
			if (type == KAStateType.S_ATTACK || type == KAStateType.S_SKILL || type == KAStateType.S_ATTACKED)
			{
				return;
			}

			Vector3 scale = transform.localScale;
			if (dir == KACommon.Direction.D_LEFT)
			{
				m_bLeft = true;
				scale.x = -Mathf.Abs(scale.x);
			}
			else if (dir == KACommon.Direction.D_RIGHT)
			{
				m_bLeft = false;
				scale.x = Mathf.Abs(scale.x);
			}
			transform.localScale = scale;

		}

		public float StateParam1 { get { return m_roleStateMgr.state.Param1; } set { m_roleStateMgr.state.Param1 = value; } }
		public float StateParam2 { get { return m_roleStateMgr.state.Param2; } set { m_roleStateMgr.state.Param2 = value; } }
		public float StateParam3 { get { return m_roleStateMgr.state.Param3; } set { m_roleStateMgr.state.Param3 = value; } }
		public float StateParam4 { get { return m_roleStateMgr.state.Param4; } set { m_roleStateMgr.state.Param4 = value; } }

	}
}