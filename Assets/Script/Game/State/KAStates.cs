using UnityEngine;
using System.Collections;
using KAct;

namespace KActOld
{
	public class KAStateIdle : KAState
	{
		public override bool check(KAState lastState)
		{
			KAPlayer role = (KAPlayer)m_roleStateMgr.role; ;
			if (role.OnLand && !role.IsSkill && !KAKeyboardMgr.IsArrowKey())
			{
				return true;
			}

			return false;
		}

		public KAStateIdle(KARoleStateMgr StateMgr)
			: base(StateMgr)
		{
			m_info.type = (int)KAStateType.S_IDLE;
			m_info.hash = Animator.StringToHash("Idle");
			m_info.nextState = new int[] { (int)KAStateType.S_WALK, (int)KAStateType.S_RUN, (int)KAStateType.S_JUMP, (int)KAStateType.S_ATTACK };
		}

		public override void PreAction()
		{
			base.PreAction();
			KARole role = m_roleStateMgr.role;
			role.transform.rigidbody.velocity = Vector3.zero;
		}

	}

	public class KAStateWalk : KAState
	{
		public override bool check(KAState lastState)
		{
			KAPlayer role = (KAPlayer)m_roleStateMgr.role;
			if (role.OnLand && !role.IsSkill && KAKeyboardMgr.IsArrowKey() && !role.KeyboardMgr.IsRunKey())
			{
				return true;
			}

			return false;
		}

		public KAStateWalk(KARoleStateMgr StateMgr)
			: base(StateMgr)
		{
			m_info.type = (int)KAStateType.S_WALK;
			m_info.hash = Animator.StringToHash("Walk");
			m_info.nextState = new int[] { (int)KAStateType.S_IDLE, (int)KAStateType.S_RUN, (int)KAStateType.S_JUMP, (int)KAStateType.S_ATTACK };
		}

	}

	public class KAStateRun : KAState
	{
		public override bool check(KAState lastState)
		{
			KAPlayer role = (KAPlayer)m_roleStateMgr.role;
			if (role.OnLand && !role.IsSkill && KAKeyboardMgr.IsArrowKey() && role.KeyboardMgr.IsRunKey())
			{
				return true;
			}

			return false;
		}

		public KAStateRun(KARoleStateMgr StateMgr)
			: base(StateMgr)
		{
			m_info.type = (int)KAStateType.S_RUN;
			m_info.hash = Animator.StringToHash("Run");
			m_info.nextState = new int[] { /*(int)KAStateType.S_WALK,*/ (int)KAStateType.S_IDLE, (int)KAStateType.S_JUMP };
		}

	}

	public class KAStateJump : KAState
	{
		enum SubStateType
		{
			S_START = 0,
			S_UP,
			S_TOP,
			S_DOWN,
			S_END
		}

		public class KASubStateJumpStart : KASubState
		{
			public KASubStateJumpStart(KAState parent)
				: base(parent)
			{
				m_info.type = (int)SubStateType.S_START;
				m_info.hash = Animator.StringToHash("JumpStart");
				m_info.nextState = new int[] { (int)SubStateType.S_UP };
			}

			public override void PreAction()
			{
				base.PreAction();
				m_roleStateMgr.role.OnLand = false;
			}

		}

		public class KASubStateJumpUp : KASubState
		{
			public KASubStateJumpUp(KAState parent)
				: base(parent)
			{
				m_info.type = (int)SubStateType.S_UP;
				m_info.hash = Animator.StringToHash("JumpUp");
				m_info.nextState = new int[] { (int)SubStateType.S_TOP };
			}

			public override bool check(KAState lastState)
			{
				if (!m_roleStateMgr.role.AnmationMgr.isPlaying)
				{
					return true;
				}
				return false;
			}

		}

		public class KASubStateJumpTop : KASubState
		{
			public KASubStateJumpTop(KAState parent)
				: base(parent)
			{
				m_info.type = (int)SubStateType.S_TOP;
				m_info.hash = Animator.StringToHash("JumpTop");
				m_info.nextState = new int[] { (int)SubStateType.S_DOWN };
			}

			public override bool check(KAState lastState)
			{
				KAPlayer role = (KAPlayer)m_roleStateMgr.role;
				if (role.rigidbody.velocity.y <= 0)
				{
					return true;
				}
				return false;
			}
		}

		public class KASubStateJumpDown : KASubState
		{
			public KASubStateJumpDown(KAState parent)
				: base(parent)
			{
				m_info.type = (int)SubStateType.S_DOWN;
				m_info.hash = Animator.StringToHash("JumpDown");
				m_info.nextState = new int[] { (int)SubStateType.S_END };
			}

			public override bool check(KAState lastState)
			{
				KAPlayer role = (KAPlayer)m_roleStateMgr.role;
				if (!role.AnmationMgr.isPlaying)
				{
					return true;
				}
				return false;
			}
		}

		public class KASubStateJumpEnd : KASubState
		{
			public KASubStateJumpEnd(KAState parent)
				: base(parent)
			{
				m_info.type = (int)SubStateType.S_END;
				m_info.hash = Animator.StringToHash("JumpEnd");
				m_info.nextState = new int[] { SUBSTATE_END };
			}

			public override bool check(KAState lastState)
			{
				KAPlayer role = (KAPlayer)m_roleStateMgr.role;
				Transform transform = role.transform;
				if ((transform.position.y - (transform.localScale.y / 2)) < 0.5)
				{
					return true;
				}
				return false;
			}

			public override void AfterAction()
			{
				base.AfterAction();
				KAPlayer role = (KAPlayer)m_roleStateMgr.role; ;
				role.OnLand = true;
			}
		}

		public override bool check(KAState lastState)
		{
			KAPlayer role = (KAPlayer)m_roleStateMgr.role;
			if (role.OnLand && !role.IsSkill && Input.GetKeyDown(KeyCode.C))
			{
				return true;
			}

			return false;
		}

		public override void PreAction()
		{
			base.PreAction();
			KAPlayer role = (KAPlayer)m_roleStateMgr.role;
			Vector3 gravity = Physics.gravity;
			Vector3 force = (-gravity) * role.rigidbody.mass * role.JumpForce;
			role.rigidbody.AddForce(force);
			ChangeToSubState((int)SubStateType.S_START);
		}

		public KAStateJump(KARoleStateMgr StateMgr)
			: base(StateMgr)
		{
			m_info.type = (int)KAStateType.S_JUMP;
			m_info.hash = 0;
			m_info.nextState = new int[] { (int)KAStateType.S_WALK, (int)KAStateType.S_IDLE };

			KASubState state = new KASubStateJumpStart(this);
			m_mapSubState.Add(state.type, state);
			state = new KASubStateJumpUp(this);
			m_mapSubState.Add(state.type, state);
			state = new KASubStateJumpTop(this);
			m_mapSubState.Add(state.type, state);
			state = new KASubStateJumpDown(this);
			m_mapSubState.Add(state.type, state);
			state = new KASubStateJumpEnd(this);
			m_mapSubState.Add(state.type, state);
		}

	}

	public class KAStateAttack : KAState
	{
		enum SubStateType
		{
			S_ATTACK1 = 1,
			S_ATTACK2,
			S_ATTACK3
		}

		public class KASubStateAttack1 : KASubState
		{
			public KASubStateAttack1(KAState parent)
				: base(parent)
			{
				m_info.type = (int)SubStateType.S_ATTACK1;
				m_info.hash = Animator.StringToHash("Attack1");
				m_info.nextState = new int[] { (int)SubStateType.S_ATTACK2, SUBSTATE_END };
			}

			public override void PreAction()
			{
				base.PreAction();
				KAPlayer role = (KAPlayer)m_roleStateMgr.role;
				role.IsSkill = true;
			}

			public override void AfterAction()
			{
				base.AfterAction();
				KAPlayer role = (KAPlayer)m_roleStateMgr.role;
				role.IsSkill = false;
			}
		}

		public class KASubStateAttack2 : KASubState
		{
			public KASubStateAttack2(KAState parent)
				: base(parent)
			{
				m_info.type = (int)SubStateType.S_ATTACK2;
				m_info.hash = Animator.StringToHash("Attack2");
				m_info.nextState = new int[] { (int)SubStateType.S_ATTACK3, SUBSTATE_END };
			}

			public override bool check(KAState lastState)
			{
				if (lastState.Param1 != 0 && Input.GetKeyDown(KeyCode.X))
				{
					return true;
				}
				return false;
			}

			public override void PreAction()
			{
				base.PreAction();
				KAPlayer role = (KAPlayer)m_roleStateMgr.role;
				role.IsSkill = true;
			}

			public override void AfterAction()
			{
				base.AfterAction();
				KAPlayer role = (KAPlayer)m_roleStateMgr.role;
				role.IsSkill = false;
			}
		}

		public class KASubStateAttack3 : KASubState
		{
			public KASubStateAttack3(KAState parent)
				: base(parent)
			{
				m_info.type = (int)SubStateType.S_ATTACK3;
				m_info.hash = Animator.StringToHash("Attack3");
				m_info.nextState = new int[] { SUBSTATE_END };
			}

			public override bool check(KAState lastState)
			{
				if (lastState.Param1 != 0 && Input.GetKeyDown(KeyCode.X))
				{
					return true;
				}
				return false;
			}

			public override void PreAction()
			{
				base.PreAction();
				KAPlayer role = (KAPlayer)m_roleStateMgr.role; ;
				role.IsSkill = true;
			}

			public override void AfterAction()
			{
				base.AfterAction();
				KAPlayer role = (KAPlayer)m_roleStateMgr.role; ;
				role.IsSkill = false;
			}
		}

		public override bool check(KAState lastState)
		{
			KAPlayer role = (KAPlayer)m_roleStateMgr.role; ;
			if (role.OnLand && !role.IsSkill && Input.GetKeyDown(KeyCode.X))
			{
				return true;
			}

			return false;
		}

		public override void PreAction()
		{
			base.PreAction();
			KAPlayer role = (KAPlayer)m_roleStateMgr.role; ;
			role.IsSkill = true;
			ChangeToSubState((int)SubStateType.S_ATTACK1);
		}

		public KAStateAttack(KARoleStateMgr StateMgr)
			: base(StateMgr)
		{
			m_info.type = (int)KAStateType.S_ATTACK;
			m_info.hash = 0;
			m_info.nextState = new int[] { (int)KAStateType.S_WALK, (int)KAStateType.S_IDLE, (int)KAStateType.S_ATTACK };

			KASubState state = new KASubStateAttack1(this);
			m_mapSubState.Add(state.type, state);
			state = new KASubStateAttack2(this);
			m_mapSubState.Add(state.type, state);
			state = new KASubStateAttack3(this);
			m_mapSubState.Add(state.type, state);
		}

	}
}