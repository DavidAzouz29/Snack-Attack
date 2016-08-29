///<summary>
/// Author: 		David Azouz
/// Date Created: 	27/08/16
/// Date Modified: 	27/08/16
/// --------------------------------------------------
/// Brief: A Build shared among common players
/// e.g. RockyRoad, Princess Cake
/// viewed 
/// https://unity3d.com/learn/tutorials/topics/scripting/character-select-system-scriptable-objects 
/// 
/// ***EDIT***
/// - Functionality mostly added - David Azouz 27/08/16
/// - 	- David Azouz /08/16
/// 
/// TODO:
/// - 
/// </summary>
/// --------------------------------------------------


using UnityEngine;
using System.Collections;

[CreateAssetMenu (menuName = "PlayerBuilds/MonopedeBuild")]
public class MonopedeBuild : PlayerBuild
{
	public override E_BUILD_STATE eCurrentBuildState { get; set; }
	public override void Initialize(GameObject obj)
	{
        // Name
        switch(eCurrentClassState)
        {
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD:
                {
                    aName = "RockyRoad";
                    break;
                }
            case PlayerController.E_CLASS_STATE.E_CLASS_STATE_PATTYCAKE:
                {
                    aName = "PrincessCake";
                    break;
                }
        }

        c_rb = obj.GetComponent<Rigidbody>();
        c_Animator = obj.GetComponent<Animator>();

        r_PC = obj.GetComponent<PlayerController>();
        r_PA = obj.GetComponent<PlayerAnims>();
        r_BB = obj.GetComponent<BossBlobs>();
    }
}
