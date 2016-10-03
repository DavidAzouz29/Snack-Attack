using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName= "Brains/Rocky Road/Rocky Road", order =0)]
public class RockyRoadBrain : SnackBrain
{
    [SerializeField] private PlayerBuild.E_BASE_CLASS_STATE eBaseClassState;
    public PlayerBuild.E_ROCKYROAD_STATE eRockyRoadSkinState;
    public RuntimeAnimatorController c_characterAnimatorController;
    public Avatar c_characterAvatar;
    public Mesh c_characterMesh;
    public Material c_characterMaterial;
    
    public override void InitializeBase(SnackThinker snack)
    {
        eBaseClassState = PlayerBuild.E_BASE_CLASS_STATE.E_BASE_CLASS_STATE_ROCKYROAD;
        characterAnimator.runtimeAnimatorController = c_characterAnimatorController;
        characterAnimator.avatar = c_characterAvatar;
        characterSkinnedRenderer.sharedMesh = c_characterMesh;
    }

    public override void Initialize(SnackThinker snack)
    {
        InitializeBase(snack);
        eRockyRoadSkinState = PlayerBuild.E_ROCKYROAD_STATE.E_ROCKYROAD_STATE_ROCKYROAD;
        characterSkinnedRenderer.sharedMaterial = c_characterMaterial;
        //classname = "Rocky Road";
        //eClassstate = PlayerBuild.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
        //characterMaterial = characterMaterials[0];
    }

}
