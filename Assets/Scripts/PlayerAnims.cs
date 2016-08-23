/// ----------------------------------
/// <summary>
/// Name: PlayerController.cs
/// Author: Nick Dallafiore and David Azouz
/// Date Created: 27/07/2016
/// Date Modified: 7/2016
/// ----------------------------------
/// Brief: Animations Manager
/// viewed: http://stackoverflow.com/questions/30310847/gameobject-findobjectoftype-vs-getcomponent
/// *Edit*
/// - Set up - Nick Dallafiore /07/2016
/// - Jump animation added - David Azouz 27/07/2016
/// TODO:
/// - Set Triggers for Jump?
/// </summary>
/// ----------------------------------
using UnityEngine;
using System.Collections;

public class PlayerAnims : MonoBehaviour {

    public Animator m_Anim;
    public BossBlobs.TransitionState m_TransitionState; //TODO ADD BOSS ATTACKS

    private PlayerController m_PC;
    private bool m_Idling, m_Walking, m_Attacking, m_isJumping;
    private Vector3 m_PreviousPos;

    void Start()
    {
        // Player Controller script is attached to our game object anyway.
        m_PC = GetComponent<PlayerController>();
        m_Idling = true;
        m_isJumping = false;
        m_PreviousPos = transform.position;
        switch (m_TransitionState)
        {
            case BossBlobs.TransitionState.BOSS:
                m_Anim = gameObject.transform.FindChild("Boss").GetComponent<Animator>();
                break;
            case BossBlobs.TransitionState.NEUT:
                m_Anim = gameObject.transform.FindChild("Neut").GetComponent<Animator>();
                break;
            case BossBlobs.TransitionState.WEAK:
                m_Anim = gameObject.transform.FindChild("Weak").GetComponent<Animator>();
                break;
        }
        

    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown(m_PC.Attack1) && !m_Anim.GetBool("AttackTrigger"))
        {
            AttackAnim1();
        }
        if (Input.GetButtonDown(m_PC.Attack2) && !m_Anim.GetBool("AttackTrigger"))
        {
            AttackAnim2();
        }
        if (Input.GetButtonDown(m_PC.Block) && !m_Anim.GetBool("Blocking"))
        {
            m_Anim.SetTrigger("BlockStart");
            m_Anim.SetBool("Blocking", true);
        }
        if (Input.GetButtonUp(m_PC.Block) && m_Anim.GetBool("Blocking"))
        {
            m_Anim.SetTrigger("BlockEnd");
            m_Anim.SetBool("Blocking", false);
        }
        if (!gameObject.GetComponent<PlayerController>().isOnGround)
        {
            m_isJumping = true;
        }
        else
        {
            m_isJumping = false;
            m_Anim.SetBool("Jumping", false);
        }
        IdleToWalk();
        WalkToIdle();
        AttackToIdle();
        AttackToWalk();

        IdleToJump();
        WalkToJump();
        m_PreviousPos = transform.position;
    }

    //---------------------------
    // Idle
    //---------------------------
    void IdleToWalk()
    {
        // extra check is for our jump animation
        float fDis = Vector2.Distance(new Vector2(m_PreviousPos.x, m_PreviousPos.z), new Vector2(transform.position.x, transform.position.z));
        if (fDis > 0.15 && !m_Walking 
            && (m_PreviousPos.y - transform.position.y) < 0.5f)
        {
            m_Anim.SetBool("Walking", true);
            m_Anim.SetBool("Idling", false);
            m_Walking = true;
        }
    }
    void IdleToJump()
    {
        if (m_isJumping)
        {
            m_Anim.SetBool("Jumping", true);
            m_Anim.SetBool("Idling", false);
            m_Walking = false; // this is so the animation doesn't play
        }
    }
    //---------------------------
    // Walk
    //---------------------------
    void WalkToIdle()
    {
        float fDis = Vector2.Distance(new Vector2(m_PreviousPos.x, m_PreviousPos.z), new Vector2(transform.position.x, transform.position.z));
        if (fDis == 0 && m_Walking)
        {
            m_Anim.SetBool("Idling", true);
            m_Anim.SetBool("Walking", false);
            m_Walking = false;
        }
    }
    void WalkToJump()
    {
        if (m_isJumping)
        {
            m_Anim.SetBool("Jumping", true);
            m_Anim.SetBool("Walking", false);
            m_Walking = false; // this is so the animation doesn't play
        }
    }
    //---------------------------
    // Attack
    //---------------------------
    void AttackToIdle()
    {
        if (!m_Walking)
        {
            m_Anim.SetBool("Idling", true);
            m_Anim.SetBool("Walking", false);
            m_Anim.SetTrigger("AttackToIdle");
        }
    }
    void AttackToWalk()
    {
        if (m_Walking)
        {
            m_Anim.SetBool("Walking", true);
            m_Anim.SetBool("Idling", false);
            m_Anim.SetTrigger("AttackToWalk");
        }
    }
    void AttackToJump()
    {
        m_Anim.SetBool("Jumping", true);
        m_Anim.SetTrigger("AttackToJump");
        m_isJumping = true;
    }
    void AttackAnim1()
    {
        m_Anim.SetBool("Attack1", true);
        m_Anim.SetBool("Attacking1", true);
        m_Anim.SetBool("AttackTrigger", true);       
    }
    void AttackAnim2()
    {
        m_Anim.SetBool("Attack2", true);
        m_Anim.SetBool("Attacking2", true);
        m_Anim.SetBool("AttackTrigger", true);
    }
    //---------------------------
    // Jump To
    //---------------------------
    void JumpToIdle()
    {
        m_Anim.SetBool("Jumping", false);
        m_Anim.SetBool("Idling", true);
        m_isJumping = false;
    }
    void JumpToWalk()
    {
        m_Anim.SetBool("Jumping", false);
        m_Anim.SetBool("Walking", true);
        m_isJumping = false;
    }
    void JumpToAttack()
    {
        m_Anim.SetBool("Jumping", false);
        m_Anim.SetBool("Walking", true);
        m_isJumping = false;
    } 
}
