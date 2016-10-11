using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour {

    public int damage;
    public bool weaponIsActive;
    public bool isHeavyAttack;
    public Transform m_ParentTransform;
    [SerializeField] private TrailRenderer c_TrailRenderer;
    //Attack Timer
    private float m_Timer = 0;
    private bool m_TimerEnabled = false;
    void Start()
    {
        weaponIsActive = false;
        c_TrailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (weaponIsActive)
        {
            m_TimerEnabled = true;
            PunchEffects(true);
        }
        if (m_TimerEnabled)
        {
            m_Timer += Time.deltaTime;
        }
        if(m_Timer > 1)
        {
            weaponIsActive = false;
            m_TimerEnabled = false;
            PunchEffects(false);
            m_Timer = 0.0f;
        }
    }

    void PunchEffects(bool isActive)
    {
        c_TrailRenderer.enabled = isActive;
    }
}
