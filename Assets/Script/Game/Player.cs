using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    enum ActionType{
        IDLE,
        WALK,
        ATTACK
    };

    float fSpeedX = 20.0f;
    float fSpeedZ = 10.0f;

    Rigidbody stRigid = null;

    private Animator m_stAnimator = null;
    private SpriteRenderer m_stSpriteRender = null;
    private Transform m_stTransform = null;

    private ActionType bAction = ActionType.IDLE;

	// Use this for initialization
	void Start () {
        m_stAnimator = transform.Find("Animation").GetComponent<Animator>();
        m_stSpriteRender = m_stAnimator.GetComponent<SpriteRenderer>();
        m_stTransform = (Transform)gameObject.GetComponent<Transform>();

        m_stAnimator.Play("idle");
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 v = Vector3.zero;
	    if (Input.GetKey(KeyCode.W))
        {
            v.z += fSpeedZ * Time.deltaTime;
            //transform.position.Set(v.x, v.y, v.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            v.z -= fSpeedZ * Time.deltaTime;
            //transform.position.Set(v.x, v.y, v.z);
        }

        if (Input.GetKey(KeyCode.A))
        {
            v.x -= fSpeedX * Time.deltaTime;
            //transform.position.Set(v.x, v.y, v.z);
        }

        if (Input.GetKey(KeyCode.D))
        {
            v.x += fSpeedX * Time.deltaTime;
            //transform.position.Set(v.x, v.y, v.z);
        }

        ///*
        if (v != Vector3.zero)
        {
            m_stAnimator.Play("move");
            if (v.x < 0)
            {
                Vector3 vScale = transform.localScale;
                vScale.x = -Mathf.Abs(vScale.x);
                transform.localScale = vScale;
            }
            else if (v.x > 0)
            {
                Vector3 vScale = transform.localScale;
                vScale.x = Mathf.Abs(vScale.x);
                transform.localScale = vScale;
            }
            m_stTransform.Translate(v, Space.Self);
        }
        else
        {
            m_stAnimator.Play("idle");
        }
        //*/
	}

    /*
    void Attack()
    {
        AnimatorStateInfo stateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(IdleState))
        {
		    // 在待命状态下，按下攻击键，进入攻击1状态，并记录连击数为1
            this.animator.SetInteger(ActionCMD, 1);
            this.curComboCount = 1;
        }
        else if (stateInfo.IsName(AtkSliceState))
        {
		    // 在攻击1状态下，按下攻击键，记录连击数为2（切换状态在Update()中）
            this.curComboCount = 2;
        }
        else if (stateInfo.IsName(AtkStabState))
        {
		    // 在攻击2状态下，按下攻击键，记录连击数为3（切换状态在Update()中）
            this.curComboCount = 3;
        }
    }
    */
}
