/// <summary>
/// Author: 		David Azouz
/// Date Created: 	3/11/16
/// Date Modified: 	/16
/// --------------------------------------------------
/// Brief: Manages Effects to be displayed.
/// viewed: 
/// 
/// ***EDIT***
/// - added boss heavy dust and shield	- David Azouz 3/11/16
/// 
/// TODO:
/// - fix "Animator has not been initialized" warning
/// </summary>

using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    public Transform m_leftFist;
    public Transform m_rightFist;

    public GameObject heavyAttackSmokeEffect;
    public GameObject hitEffect;
    public GameObject shieldEffect;

    // used for getting the components from game objects
    private ParticleSystem c_HeavyAttackSmokeEffect;
    private ParticleSystem c_HitEffect;
    //private ParticleSystem c_ShieldEffect;

    // Used for instanciating/ destroying
    //private ParticleSystem localHeavyAttackSmokeEffect;
    //private ParticleSystem localHitEffect;
    //private ParticleSystem localShieldEffect;

    private int iChild = 3;
    //private string sCurrAnim = "aeaa";
    private Animator c_CurrAnim;
    private BossBlobs r_BossBlobs;
    private PlayerController r_PlayerController;

    // Heavy Attack
    [SerializeField] private bool m_isHeavyAttackToBeSpawned = false;
    private float m_AttackTimer = 0.0f;
    private bool m_AttackTimerEnabled = false;
    private float m_AttackThresholdSpawn = 0.4f; // time before RockyRoad's heavy hits the ground
    private float m_AttackThresholdDestroy = 0.4f; // we add the m_AttackThresholdSpawn in awake
    private Quaternion qHeavyRot = Quaternion.identity;
    private float fHeavySpawnHeight = 2.0f;

    // Hit


    // Blocking
    [SerializeField] private bool m_isBlocking = false;


    void Awake()
    {
        r_BossBlobs = GetComponent<BossBlobs>();
        r_PlayerController = GetComponent<PlayerController>();
        c_CurrAnim = transform.FindChild("Neut").GetComponent<Animator>();
        m_AttackThresholdDestroy += m_AttackThresholdSpawn;
        qHeavyRot.eulerAngles = new Vector3(90, 0);
        // turn the shields off
        transform.FindChild("Weak").GetChild(0).gameObject.SetActive(false); 
        transform.FindChild("Neut").GetChild(0).gameObject.SetActive(false); 
        transform.FindChild("Boss").GetChild(0).gameObject.SetActive(false); 
    }

    // Use this for initialization
    void Start()
    {
        c_HeavyAttackSmokeEffect = heavyAttackSmokeEffect.GetComponent<ParticleSystem>();
        c_HitEffect = hitEffect.GetComponent<ParticleSystem>();
        //c_ShieldEffect = shieldEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Anim Selection
        switch (r_BossBlobs.m_TransitionState)
        {
            case BossBlobs.TransitionState.WEAK:
                {
                    //if (c_CurrAnim.name != sCurrAnim)
                    {
                        c_CurrAnim = transform.FindChild("Weak").GetComponent<Animator>();
                        iChild = 2;
                        //sCurrAnim = "Weak";
                    }
                    break;
                }
            case BossBlobs.TransitionState.NEUT:
                {
                    //if (c_CurrAnim.name != sCurrAnim)
                    {
                        c_CurrAnim = transform.FindChild("Neut").GetComponent<Animator>();
                        iChild = 3;
                        //sCurrAnim = "Neut";
                    }
                    break;
                }
            case BossBlobs.TransitionState.BOSS:
                {
                    //if (c_CurrAnim.name != sCurrAnim)
                    {
                        c_CurrAnim = transform.FindChild("Boss").GetComponent<Animator>();
                        iChild = 4;
                        //sCurrAnim = "Boss";
                    }
                    break;
                }
        }
        #endregion

        // Blocking // will work for any evolution state that's active: weak, neut, boss //TODO: get "Blocking.cs" working
        if (Input.GetButton(r_PlayerController.Block))// && !c_CurrAnim.GetBool("Blocking"))
        {
            m_isBlocking = true;
        }
        else// if (Input.GetButtonUp(r_PlayerController.Block) && c_CurrAnim.GetBool("Blocking"))
        {
            m_isBlocking = false;
        }
        // Shield 
        transform.GetChild(iChild).GetChild(0).gameObject.SetActive(m_isBlocking);

        // Boss
        if (r_BossBlobs.m_TransitionState == BossBlobs.TransitionState.BOSS)
        {
            // Heavy Attack
            if (c_CurrAnim.GetBool("Attack2"))
            {
                m_isHeavyAttackToBeSpawned = true;
                m_AttackTimerEnabled = true;
            }

            //
            if (m_AttackTimerEnabled)
            {
                m_AttackTimer += Time.deltaTime;
            }

            // Heavy Attack
            if (m_isHeavyAttackToBeSpawned && m_AttackTimer > m_AttackThresholdSpawn && m_AttackTimer < m_AttackThresholdDestroy)
            {
                // Create a vector that's halfway between the two fists.
                Vector3 v3HeavyEffectSpawnEffect = Vector3.Lerp(m_leftFist.position, m_rightFist.position, 0.5f);
                v3HeavyEffectSpawnEffect = new Vector3(v3HeavyEffectSpawnEffect.x,
                    v3HeavyEffectSpawnEffect.y - fHeavySpawnHeight, v3HeavyEffectSpawnEffect.z);

                //localHeavyAttackSmokeEffect =
                Instantiate(c_HeavyAttackSmokeEffect, v3HeavyEffectSpawnEffect, qHeavyRot);// as ParticleSystem;// true);
                m_isHeavyAttackToBeSpawned = false;
                //Destroy(localHeavyAttackSmokeEffect, m_AttackThresholdDestroy);// localHeavyAttackSmokeEffect.duration);
                m_AttackTimerEnabled = false;
                m_AttackTimer = 0f;

            }
        }

        // If we change state and the particle still exists
        /*if (localHeavyAttackSmokeEffect && m_AttackTimer >= m_AttackThresholdDestroy)
        {
            Destroy(localHeavyAttackSmokeEffect, localHeavyAttackSmokeEffect.duration);
        } */
    }

    public void HitEffect(Vector3 a_otherPlayerPosition)
    {
        Instantiate(c_HitEffect, a_otherPlayerPosition, Quaternion.identity);
    }
}
