using UnityEngine;
using System.Collections;

namespace KAct
{
	public class KAPlayer : KARole
	{

		protected KAKeyboardMgr m_keyboardMgr = null;

		public KAKeyboardMgr KeyboardMgr { get { return m_keyboardMgr; } }

		// Use this for initialization
		public override void Start()
		{
			base.Start();
			m_keyboardMgr = new KAKeyboardMgr(this);

			///*
			KAState state = new KAState(m_roleStateMgr);
			if (state.LoadFromFile("Lua/State/PlayerIdleState.lua"))
			{
				m_roleStateMgr.AddState(state);
				m_roleStateMgr.ChangeToState(state.type);
			}

			state = new KAState(m_roleStateMgr);
			if (state.LoadFromFile("Lua/State/PlayerWalkState.lua"))
			{
				m_roleStateMgr.AddState(state);
			}

			state = new KAState(m_roleStateMgr);
			if (state.LoadFromFile("Lua/State/PlayerRunState.lua"))
			{
				m_roleStateMgr.AddState(state);
			}

			state = new KAState(m_roleStateMgr);
			if (state.LoadFromFile("Lua/State/PlayerJumpState.lua"))
			{
				m_roleStateMgr.AddState(state);
			}

			state = new KAState(m_roleStateMgr);
			if (state.LoadFromFile("Lua/State/PlayerAttackState.lua"))
			{
				m_roleStateMgr.AddState(state);
			}
			//*/ 

			/*
			KAState IdleState = new KAStateIdle(m_roleStateMgr);
			m_roleStateMgr.AddState(IdleState);

			KAState WalkState = new KAStateWalk(m_roleStateMgr);
			m_roleStateMgr.AddState(WalkState);

			KAState RunState = new KAStateRun(m_roleStateMgr);
			m_roleStateMgr.AddState(RunState);

			KAState JumpState = new KAStateJump(m_roleStateMgr);
			m_roleStateMgr.AddState(JumpState);

			KAState AttackState = new KAStateAttack(m_roleStateMgr);
			m_roleStateMgr.AddState(AttackState);

			m_roleStateMgr.ChangeToState(IdleState.type);
			*/


		}

		// Update is called once per frame
		public override void Update()
		{
			m_keyboardMgr.UpdateKeyboadInput();
			base.Update();
			UpdateDir();
			UpdateMove();

		}
		protected override void UpdateDir()
		{
			bool Left = Input.GetKey(KeyCode.LeftArrow);
			bool Right = Input.GetKey(KeyCode.RightArrow);
			if (!(Left && Right))
			{
				if (Left && !m_bLeft)
				{
					FaceTo(KACommon.Direction.D_LEFT);
				}

				if (Right && m_bLeft)
				{
					FaceTo(KACommon.Direction.D_RIGHT);
				}
			}
		}

		protected override void UpdateMove()
		{
			if (m_roleStateMgr.IsCanMove() == false)
			{
				return;
			}

			Vector3 velcity = transform.rigidbody.velocity;
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

			int state = m_roleStateMgr.StateType;
			int lastState = m_roleStateMgr.LastStateType;
			float fXSpeed = 0, fZSpeed = 0;
			if (state == (int)KAStateType.S_RUN || (state == (int)KAStateType.S_JUMP && lastState == (int)KAStateType.S_RUN))
			{
				fXSpeed = m_fRunXSpeed;
				fZSpeed = m_fRunZSpeed;
			}
			else if (state == (int)KAStateType.S_WALK || (state == (int)KAStateType.S_JUMP && lastState != (int)KAStateType.S_RUN))
			{
				fXSpeed = m_fWalkXSpeed;
				fZSpeed = m_fWalkZSpeed;
			}
			else if (state == (int)KAStateType.S_ATTACK)
			{
				if (m_roleStateMgr.state.Param1 == 0)
				{
					if (((vInput.x > 0) && !m_bLeft)
						|| ((vInput.x < 0) && m_bLeft))
					{
						fXSpeed = m_fAttackMoveSpeedX;
					}
					else if (vInput.x == 0)
					{
						vInput.x = m_bLeft ? -1 : 1;
						fXSpeed = m_fAttackMoveSpeed;
					}
				}
			}

			velcity.x = vInput.x * fXSpeed;
			velcity.z = vInput.z * fZSpeed;

			transform.rigidbody.velocity = velcity;
		}

	}
}