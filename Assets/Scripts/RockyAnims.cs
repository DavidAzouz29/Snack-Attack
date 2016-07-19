using UnityEngine;
using System.Collections;

public class RockyAnims : MonoBehaviour {

    public Animator m_Anim;

    private PlayerController m_PC;

    private bool m_Idiling, m_Walking, m_Attacking;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_PC = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate()
    {
        if (m_PC.m_Moving && !m_Walking)
            BeginWalk();
        else if(!m_PC.m_Moving && m_Walking)
        {
            StopWalk();

        }

        if (Input.GetButtonDown("Fire2"))
        {
            Attack();
        }
    }

    void BeginWalk()
    {
        m_Anim.SetTrigger("BeginWalk");
        m_Walking = true;
    }

    void StopWalk()
    {
        m_Anim.SetTrigger("StopWalk");
        m_Walking = false;
    }

    void Attack()
    {
        m_Anim.SetTrigger("Attack");
    }
}
