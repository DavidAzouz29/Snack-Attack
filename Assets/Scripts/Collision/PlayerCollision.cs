using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

    public int damage;
    public bool isActive;
    public bool isHeavyAttack;
    //Attack Timer
    private float m_Timer = 0;
    private bool m_TimerEnabled = false;
    void Start()
    {
        isActive = false;
    }

    void Update()
    {
        if (isActive)
        {
            m_TimerEnabled = true;
        }
        if(m_TimerEnabled)
        {
            m_Timer += Time.deltaTime;
        }
        if(m_Timer > 1)
        {
            isActive = false;
            m_TimerEnabled = false;
            m_Timer = 0.0f;
        }
    }
}
