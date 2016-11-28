///<summary>
///
/// 
/// **Edit**
/// - Trail Renderers - David Azouz 11/10/16
/// - Emission maps - David Azouz 16/10/16
/// - 
/// viewed: https://docs.unity3d.com/Manual/MaterialsAccessingViaScript.html
/// http://answers.unity3d.com/questions/914923/standard-shader-emission-control-via-script.html
/// Notes: Shader.PropertyToID()
/// emissiveMap = GetComponentInParent<SkinnedMeshRenderer>().sharedMaterial; //TODO: test
/// 
/// </summary>

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour {
    const string sTip = "Use 'x' for Light Attacks, and 'y' for Heavy.";
    [Header("Damage: " + sTip)]
    [Tooltip(sTip)]
    public Vector2 v2WeakDamage = new Vector2(8.0f, 12.0f);  //(15.0f, 25.0f);
    public Vector2 v2NeutDamage = new Vector2(12.0f, 17.0f); //(25.0f, 35.0f);
    public Vector2 v2BossDamage = new Vector2(17.0f, 22.0f); //(35.0f, 45.0f);
    public float damage; // will be cast as an int (Line 794 in Boss Blobs)

    public bool weaponIsActive;
    public bool isHeavyAttack;
    public Transform m_ParentTransform;
    public SkinnedMeshRenderer c_SMR;
    [SerializeField] private TrailRenderer c_TrailRenderer;
    private int iPlayerID;
    private GameSettings.PlayerInfo players;
    private BossBlobs r_BossBlobs;
    //Attack Timer
    private float m_Timer = 0;
    private bool m_TimerEnabled = false;
    [SerializeField]
    private Material emissiveMap;

    void Start()
    {
        // Return to Menu more than Splash
        if (SceneManager.GetActiveScene().buildIndex != Scene.Menu)
        {
            // Not the splash screen
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                weaponIsActive = false;
                c_TrailRenderer = GetComponent<TrailRenderer>();
                iPlayerID = GetComponentInParent<PlayerController>().GetPlayerID();
                players = GameSettings.Instance.players[iPlayerID];
                r_BossBlobs = GetComponentInParent<BossBlobs>();
            }
        }
    }

    void Update()
    {
        if (weaponIsActive)
        {
            m_TimerEnabled = true;
            PunchEffects(true);
            // Emissive map //[(int)eTransitionState]
            c_SMR.sharedMaterial.SetTexture("_EmissionMap", players.Brain.GetEmissionMaps()[(int)r_BossBlobs.m_TransitionState]);
            c_SMR.sharedMaterial.SetColor("_EmissionColor", players.Color);

            // Light Attack Damage
            switch (r_BossBlobs.m_TransitionState)
            {
                case BossBlobs.TransitionState.WEAK:
                    {
                        damage = v2WeakDamage.x;
                        break;
                    }
                case BossBlobs.TransitionState.NEUT:
                    {
                        damage = v2NeutDamage.x;
                        break;
                    }
                case BossBlobs.TransitionState.BOSS:
                    {
                        damage = v2BossDamage.x;
                        break;
                    }
            }
        }
        else if(isHeavyAttack)
        {
            m_TimerEnabled = true; //TODO: tweak? Boss state and heavy attack isn't working?
            PunchEffects(true);
            c_SMR.sharedMaterial.SetTexture("_EmissionMap", players.Brain.GetEmissionMaps()[(int)r_BossBlobs.m_TransitionState]);
            c_SMR.sharedMaterial.SetColor("_EmissionColor", players.Color);

            // Heavy Attack Damage
            switch (r_BossBlobs.m_TransitionState)
            {
                case BossBlobs.TransitionState.WEAK:
                    {
                        damage = v2WeakDamage.y;
                        break;
                    }
                case BossBlobs.TransitionState.NEUT:
                    {
                        damage = v2NeutDamage.y;
                        break;
                    }
                case BossBlobs.TransitionState.BOSS:
                    {
                        damage = v2BossDamage.y;
                        break;
                    }
            }
        }

        if (m_TimerEnabled)
        {
            m_Timer += Time.deltaTime;
        }
        // Turn off
        if (m_Timer > 1)
        {
            weaponIsActive = false;
            m_TimerEnabled = false;
            PunchEffects(false);
            // Emissive map //[(int)eTransitionState]
            c_SMR.sharedMaterial.SetTexture("_EmissionMap", null);
            c_SMR.sharedMaterial.SetColor("_EmissionColor", Color.black);
            m_Timer = 0.0f;
        }
    }

    void PunchEffects(bool isActive)
    {
        c_TrailRenderer.enabled = isActive;
    }
}
