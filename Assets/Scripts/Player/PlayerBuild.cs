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
/// 
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
public interface IClass
{
    uint MAX_PLAYERS { get; set; } // Make 4

	PlayerBuild.E_BUILD_STATE eCurrentBuildState { get; set; } // what build are we? : Monopede
	PlayerController.E_CLASS_STATE eCurrentClassState { get; set; } // what class are we? : Rockyroad
    //GameObject c_player { get; set; } // which model we will be
	Rigidbody c_rb { get; set; }

	// Accounts for weak, neutral, boss meshes
	MeshFilter[] c_MeshFilter { get; set; }
	SkinnedMeshRenderer[] c_SkinnedMeshRenderer { get; set; }
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
}

// Properties every class (Monopede, Quadruped etc) will contain.
// The Interface inforces this rule.
public abstract class PlayerBuild : ScriptableObject, IClass
{
	public string aName = "New Class";
	public uint MAX_PLAYERS { get; set; } //
	public PlayerController.E_CLASS_STATE eCurrentClassState { get; set; }
	public Rigidbody c_rb { get; set; }
	public Animator c_Animator { get; set; }
	public MeshFilter[] c_MeshFilter { get; set; }
	public SkinnedMeshRenderer[] c_SkinnedMeshRenderer { get; set; }

	// Which material will they have?
	public Material[] c_MatA { get; set; } // Characters main texture
	public Material[] c_MatB { get; set; } // Characters alternate texture

	public ParticleSystem c_Splat { get; set; } // for the splat
	public GameObject[] c_ShotArray { get; set; } // Which blobs/ projectiles we'll fire

	public Image c_PlayerIcon { get; set; }

	// Need one:
	public PlayerController r_PC { get; set; }
	public PlayerAnims r_PA { get; set; }
	public BossBlobs r_BB { get; set; }

	public enum E_BUILD_STATE
	{
		E_BUILD_STATE_MONOPEDE, // 1 leg
		E_BUILD_STATE_BIPEDAL,	// 2 legs, humanoid
		E_BUILD_STATE_QUADRUPED,	// 4 legs

		E_BUILD_STATE_COUNT,
	};
	public abstract E_BUILD_STATE eCurrentBuildState { get; set; }

	//public void Create(){	}
	public abstract void Initialize(GameObject obj);

	void Start()
	{
		int iPLayerStates = 3;
		c_MeshFilter = new MeshFilter[iPLayerStates];
		c_SkinnedMeshRenderer = new SkinnedMeshRenderer[iPLayerStates];
		c_MatA = new Material[iPLayerStates];
		c_MatB = new Material[iPLayerStates];
	}
}
