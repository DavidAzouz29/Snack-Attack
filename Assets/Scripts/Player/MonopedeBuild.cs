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
/// - replace PATTYCAKE with PRINCESSCAKE
/// </summary>
/// --------------------------------------------------


using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "PlayerBuilds/MonopedeBuild")]
public class MonopedeBuild : PlayerBuild
{
    //
    public override E_BUILD_STATE eCurrentBuildState { get; set; }
    public override E_CLASS_STATE eCurrentClassState { get; set; }
    public override E_BOSS_STATE eCurrentBossState { get; set; }

    //
    public override void Initialize(GameObject obj)
    {
        // We list only the Monopede build character names
        switch (eCurrentClassState)
        {
            case E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD:
                {
                    aName = "RockyRoad";
                    c_PlayerIcon = c_PlayerIcons[(int)E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD];
                    break;
                }
            case E_CLASS_STATE.E_CLASS_STATE_PRINCESSCAKE:
                {
                    aName = "PrincessCake";
                    c_PlayerIcon = c_PlayerIcons[(int)E_CLASS_STATE.E_CLASS_STATE_PRINCESSCAKE];
                    break;
                }
        }

        eCurrentBossState = E_BOSS_STATE.E_BOSS_STATE_NEUTRAL;
        // TODO: if check
        c_States[(int)E_BOSS_STATE.E_BOSS_STATE_NEUTRAL].SetActive(true);
        c_rb = obj.GetComponent<Rigidbody>();
        c_Animator = obj.GetComponent<Animator>();

        r_PC = obj.GetComponent<PlayerController>();
        r_PA = obj.GetComponent<PlayerAnims>();
        r_BB = obj.GetComponent<BossBlobs>();
        r_UIBL = obj.GetComponent<UIBossLevel>();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

}
