///<summary>
/// Author: 		David Azouz
/// Date Created: 	27/07/16
/// Date Modified: 	27/07/16
/// --------------------------------------------------
/// Brief: A Player Build class that provides common data accross all players.
/// viewed https://unity3d.com/learn/tutorials/topics/scripting/interfaces
/// 14. How to program in C# - INTERFACES - Tutorial https://youtu.be/IQpss9YAc4g 
/// https://unity3d.com/learn/tutorials/topics/scripting/character-select-system-scriptable-objects 
/// ***EDIT***
/// - 	- David Azouz 11/04/16
/// - Players have unique material - David Azouz 21/06/16
/// - Functionality mostly added - David Azouz 27/08/16 2 hours
/// - Change from Mesh and Skinned... to different GameObjects we're switching between.
/// TODO:
/// - change remove const from MAX_PLAYERS
/// - removed contents from Player Controller and add here
/// </summary>
/// --------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// Classes must "Implement" features of a Interface(s)
/// which class our characters are a part of.
/// Think of interfaces as "Tags" - e.g. Inventory

// Every Class will have
/*public interface IClass
{
    uint MAX_PLAYERS { get; set; } // Make 4

	PlayerBuild.E_BUILD_STATE eCurrentBuildState { get; set; } // what build are we? : Monopede
	PlayerController.E_CLASS_STATE eCurrentClassState { get; set; } // what class are we? : Rockyroad
    //GameObject c_player { get; set; } // which model we will be
	Rigidbody c_rb { get; set; }

	// Accounts for weak, neutral, boss meshes
	//MeshFilter[] c_MeshFilter { get; set; }
	//SkinnedMeshRenderer[] c_SkinnedMeshRenderer { get; set; }
	// Which material/ skin will they have?
	Material[] c_MatA { get; set; } // Characters main texture
	Material[] c_MatB { get; set; } // Characters alternate texture

	Animator c_Animator { get; set; }
    ParticleSystem c_Splat { get; set; } // for the splat
    GameObject[] c_ShotArray { get; set; } // Which blobs/ projectiles we'll fire

	Image c_PlayerIcon { get; set; }

    // Need one:
    PlayerController r_PC { get; set; }
    PlayerAnims r_PA { get; set; }
    BossBlobs r_BB { get; set; }

	//void Create();
} */

// Properties every class (Monopede, Quadruped etc) will contain.
// The Interface inforces this rule.
public abstract class PlayerBuild : ScriptableObject
{
    public string aName = "New Class";
    //public uint MAX_PLAYERS { get; set; } //
    public GameObject[] c_States { get; set; } // boss, neut, weak Game Objects we're switching between
    public Rigidbody c_rb { get; set; }
    public Animator c_Animator { get; set; }
    //public MeshFilter[] c_MeshFilter { get; set; }
    //public SkinnedMeshRenderer[] c_SkinnedMeshRenderer { get; set; }

    // Which material will they have?
    public Material[] c_MatA { get; set; } // Characters main texture
    public Material[] c_MatB { get; set; } // Characters alternate texture

    public GameObject[] c_ShotArray { get; set; } // Which blobs/ projectiles we'll fire
    public GameObject[] c_BlobArray { get; set; } // Which blobs we'll drop
    //-------------------------------
    // Particle systems
    //-------------------------------
    public ParticleSystem c_Splat { get; set; } // for the splat

    public Image c_PlayerIcon { get; set; }
    public Image[] c_PlayerIcons { get; set; }
    // This is used for the (ring) U.I. and punch emission colour
    public Color c_PlayerColour { get; set; }

    // Need one each:
    public PlayerController r_PC { get; set; }
    public PlayerAnims r_PA { get; set; }
    public BossBlobs r_BB { get; set; }
    public UIBossLevel r_UIBL { get; set; }

    //-------------------------------
    // Player variables
    // based on Boss State
    //-------------------------------
    public float[] fMovementSpeed { get; set; }
    //public float[] fJumpForce { get; set; }
    // Combat
    // Damage values
    public float[] fAttackLight { get; set; }
    public float[] fAttackHeavy { get; set; }
    // Block Damage modifier
    public float[] fAttackBlock { get; set; }
    // amount of boss blobs we will drop to
    public float[] fBossBlobs { get; set; }

    //-------------------------------
    // Current values
    //-------------------------------
    public float currMovementSpeed { get; set; }
    public float currAttackLight { get; set; }
    public float currAttackHeavy { get; set; }
    public float currAttackBlock { get; set; }
    public float currBossBlobs { get; set; }

    //-------------------------------
    // Player "States"
    //-------------------------------
    #region Build State
    public enum E_BUILD_STATE
    {
        E_BUILD_STATE_MONOPEDE,  // 1 leg
        E_BUILD_STATE_BIPEDAL,   // 2 legs, humanoid
        E_BUILD_STATE_QUADRUPED, // 4 legs

        E_BUILD_STATE_COUNT,
    };
    public abstract E_BUILD_STATE eCurrentBuildState { get; set; }
    #endregion

    #region Class State
    public enum E_ROCKYROAD_STATE
    {
        // Rocky Road // Monopede
        E_ROCKYROAD_STATE_ROCKYROAD,
        E_ROCKYROAD_STATE_MINTCHOPCHIP, //=10,
        E_ROCKYROAD_STATE_COOKIECRUNCH,
        E_ROCKYROAD_STATE_RAINBOWWARRIOR,

        E_ROCKYROAD_BASE_ROCKYROAD_COUNT,
    }

    public enum E_PRINCESSCAKE_STATE
    {
        // Princess Cake // MonopedeS
        E_PRINCESSCAKE_STATE_PRINCESSCAKE,
        E_PRINCESSCAKE_STATE_DUCHESSCAKE,
        E_PRINCESSCAKE_STATE_POUNDCAKE, //E_PRINCESSCAKE_STATE_PRINCEGATEAUX,
        E_PRINCESSCAKE_STATE_ANGELCAKE, //E_PRINCESSCAKE_STATE_PRINCETORTE,
        
        E_PRINCESSCAKE_BASE_PRINCESSCAKE_COUNT,
    }

    public enum E_PIZZAPUNK_STATE
    {
        // Cheesy Pizza // Monopede
        E_PIZZAPUNK_STATE_PIZZAPUNK,
        //E_PIZZAPUNK_STATE_GREASYPIZZA,
        //E_PIZZAPUNK_STATE_CHEESYPASTA,
        //E_PIZZAPUNK_STATE_GREASYPASTA,

        E_PIZZAPUNK_BASE_PIZZAPUNK_COUNT,
    }

    public enum E_BASE_CLASS_STATE
    {
        /// <summary>
        /// Classes must have an even amount of skins per base class. i.e. 2, 4, 6 etc.
        /// Base classes: Rocky Road, Princess Cake, Cheesy Pizza
        /// </summary>
        E_BASE_CLASS_STATE_ROCKYROAD,
        E_BASE_CLASS_STATE_PRINCESSCAKE,
        E_BASE_CLASS_STATE_PIZZAPUNK,

        // Counts
        E_BASE_CLASS_STATE_BASE_COUNT,
        E_BASE_CLASS_SKIN_COUNT = 4,
        //E_CLASS_BASE_ROCKYROAD_COUNT = E_ROCKYROAD_STATE.E_ROCKYROAD_BASE_ROCKYROAD_COUNT,
        E_BASE_CLASS_BASE_PRINCESSCAKE_COUNT = E_ROCKYROAD_STATE.E_ROCKYROAD_BASE_ROCKYROAD_COUNT + E_PRINCESSCAKE_STATE.E_PRINCESSCAKE_BASE_PRINCESSCAKE_COUNT,
        E_BASE_CLASS_BASE_PIZZAPUNK_COUNT = E_BASE_CLASS_BASE_PRINCESSCAKE_COUNT + E_PIZZAPUNK_STATE.E_PIZZAPUNK_BASE_PIZZAPUNK_COUNT,
        E_BASE_CLASS_STATE_TOTAL_COUNT = E_ROCKYROAD_STATE.E_ROCKYROAD_BASE_ROCKYROAD_COUNT + E_PRINCESSCAKE_STATE.E_PRINCESSCAKE_BASE_PRINCESSCAKE_COUNT,
        
    };
    public abstract E_BASE_CLASS_STATE eCurrentBaseClassState { get; set; }
    //public PlayerController.E_CLASS_STATE eCurrentClassState { get; set; }
    #endregion

    #region Boss State
    // 
    public enum E_BOSS_STATE
    {
        E_BOSS_STATE_WEAK_E,    // (empty) when we spawn in-game/ runtime
        E_BOSS_STATE_WEAK,      // When we spawn for the first time
        E_BOSS_STATE_NEUTRAL,   // Spawn in as (start of game)
        E_BOSS_STATE_BOSS,      // They are the biggest form
        E_BOSS_STATE_BOSS_F,    // (full) can not absorb any more boss blobs

        E_BOSS_STATE_COUNT,
        E_BOSS_STATE_MAIN_COUNT = 3,
    };
    public abstract E_BOSS_STATE eCurrentBossState { get; set; }
    #endregion

    //-------------------------------
    // Functions for subclasses
    //-------------------------------
    //public void Create(){	}
    public abstract void Initialize(GameObject obj);
    //
    public abstract void Update();

    //-------------------------------
    // Protected Variables
    //-------------------------------
    protected const uint MAX_PLAYERS = 4;
    protected const uint PLAYER_STATES = 5;

    //-------------------------------
    // Private Variables
    //-------------------------------
    float maxBossBlobs = 200.0f;

    void Start()
    {
        //c_MeshFilter = new MeshFilter[iPLayerStates];
        //c_SkinnedMeshRenderer = new SkinnedMeshRenderer[iPLayerStates];
        c_MatA = new Material[PLAYER_STATES];
        c_MatB = new Material[PLAYER_STATES];

        eCurrentBossState = E_BOSS_STATE.E_BOSS_STATE_NEUTRAL;
        // invalidating the data
        eCurrentBuildState = E_BUILD_STATE.E_BUILD_STATE_COUNT;
        eCurrentBaseClassState = E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_BASE_COUNT;

        fMovementSpeed = new float[PLAYER_STATES];
        fAttackLight = new float[PLAYER_STATES];
        fAttackHeavy = new float[PLAYER_STATES];
        fAttackBlock = new float[PLAYER_STATES];
        fBossBlobs = new float[PLAYER_STATES];

        currMovementSpeed = fMovementSpeed[(int)E_BOSS_STATE.E_BOSS_STATE_NEUTRAL];
        currAttackLight = fAttackLight[(int)E_BOSS_STATE.E_BOSS_STATE_NEUTRAL];
        currAttackHeavy = fAttackHeavy[(int)E_BOSS_STATE.E_BOSS_STATE_NEUTRAL];
        currAttackBlock = fAttackBlock[(int)E_BOSS_STATE.E_BOSS_STATE_NEUTRAL];
        currBossBlobs = fBossBlobs[(int)E_BOSS_STATE.E_BOSS_STATE_NEUTRAL];
    }
}
