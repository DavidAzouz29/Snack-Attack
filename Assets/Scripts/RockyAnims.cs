using UnityEngine;
using System.Collections;

public class RockyAnims : MonoBehaviour {

    public Animator m_Anim;

    private PlayerController m_PC;

    private bool m_Idling, m_Walking, m_Attacking;

    private Vector3 m_PreviousPos;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_PC = FindObjectOfType<PlayerController>();
        m_Idling = true;
        m_PreviousPos = transform.position;
    }


    void FixedUpdate()
    {
        IdleToWalk();
        WalkToIdle();
        AttackToIdle();
        AttackToWalk();
        if (Input.GetButtonDown(m_PC.Melee))
        {
            Attack();
        }

        m_PreviousPos = transform.position;
    }

    void ChangeStates()
    {

    }

    void Attack()
    {
        m_Anim.SetTrigger("Attack");
        
    }

    void IdleToWalk()
    {
        if (Vector3.Distance(m_PreviousPos, transform.position) > 0.15 && !m_Walking)
        {
            m_Anim.SetBool("Walking", true);
            m_Anim.SetBool("Idling", false);
            m_Walking = true;
        }
    }

    void WalkToIdle()
    {
        if ( Vector3.Distance(m_PreviousPos, transform.position) == 0 && m_Walking)
        {
            m_Anim.SetBool("Idling", true);
            m_Anim.SetBool("Walking", false);
            m_Walking = false;
        }
    }

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
}
