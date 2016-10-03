using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName= "Brains/Princess Cake/Princess Cake", order =0)]
public class PrincessCakeBrain : SnackBrain
{
    [SerializeField] private PlayerBuild.E_BASE_CLASS_STATE eBaseClassState;
    public PlayerBuild.E_PRINCESSCAKE_STATE ePrincessCakeSkinState;
    public RuntimeAnimatorController c_characterAnimatorController;
    public Avatar c_characterAvatar;
    public Mesh c_characterMesh;
    public Material c_characterMaterial;
    
    public override void InitializeBase(SnackThinker snack)
    {
        eBaseClassState = PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_PRINCESSCAKE;
        characterAnimator.runtimeAnimatorController = c_characterAnimatorController;
        characterAnimator.avatar = c_characterAvatar;
        characterSkinnedRenderer.sharedMesh = c_characterMesh;
    }

    public override void Initialize(SnackThinker snack)
    {
        InitializeBase(snack);
        ePrincessCakeSkinState = PlayerBuild.E_PRINCESSCAKE_STATE.E_PRINCESSCAKE_STATE_PRINCESSCAKE;
        characterSkinnedRenderer.sharedMaterial = c_characterMaterial;
        //classname = "Rocky Road";
        //eClassstate = PlayerBuild.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
        //characterMaterial = characterMaterials[0];
    }

}
