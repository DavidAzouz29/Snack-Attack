///<summary>
/// Author: 		David Azouz
/// Date Created: 	27/07/16
/// Date Modified: 	27/07/16
/// --------------------------------------------------
/// Brief: A Player Properties class that provides common data accross all players.
/// viewed https://unity3d.com/learn/tutorials/topics/scripting/interfaces
/// 14. How to program in C# - INTERFACES - Tutorial https://youtu.be/IQpss9YAc4g 
/// ***EDIT***
/// - 	- David Azouz 11/04/16
/// -  - David Azouz 11/04/16
/// - Players have unique material - David Azouz 21/06/16
/// 
/// TODO:
/// - change remove const from MAX_PLAYERS
/// </summary>
using UnityEngine;
using System.Collections;

// Classes must "Implement" features of a Interface(s)
// which class our characters are a part of
//public interface IClass
//{
//    uint MAX_PLAYERS { get; set; } // Make 4

//    PlayerController.E_CLASS_STATE playerState { get; set; } // what class are we?
//    GameObject player { get; set; } // which model we will be
//    ParticleSystem splat { get; set; } // for the splat
//    GameObject[] shotArray { get; set; } // Which blobs/ projectiles we'll fire

//    // Which material will they have?
//    Material matA { get; set; }
//    Material matB { get; set; }

//    // Need one:
//    PlayerController r_PC { get; set; }

//    Animator c_Animator { get; set; }
//    PlayerAnims r_PA { get; set; }
//    BossBlobs r_BB { get; set; }
//}

/*public class PlayerProperties : ScriptableObject//, IClass
{
    
	
}*/
