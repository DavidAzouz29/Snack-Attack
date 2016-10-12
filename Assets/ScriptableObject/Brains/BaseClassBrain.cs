using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName= "Brains/Base Class Brain", order =0)]
public class BaseClassBrain : SnackBrain
{
    [SerializeField] protected PlayerBuild.E_BASE_CLASS_STATE _eBaseClassState;
    //[SerializeField] protected new PlayerBuild.E_ROCKYROAD_STATE eRockyRoadSkinState;
    // Animation
    [SerializeField] protected RuntimeAnimatorController _charAnimatorController;
    [SerializeField] protected Avatar _charAvatar;
    // Meshes
    [SerializeField] protected Mesh[] _charStateMeshes = new Mesh[(int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT];
    [SerializeField] protected Mesh _charBlobMesh;
    [SerializeField] public float _localScale = 109.1183f;
    [SerializeField] public Quaternion _rotation;

    public override void InitializeBase(SnackThinker snack)
    {
        eBaseClassState = _eBaseClassState;
        c_charAnimatorController = _charAnimatorController;
        c_charAvatar = _charAvatar;
        c_charStateMeshes = _charStateMeshes;
        c_charBlobMesh = _charBlobMesh;
    }

    public override void Initialize(SnackThinker snack) { }
    public override Material GetMaterial(int i) { return c_charStateMaterials[i]; }
    /*{
        InitializeBase(snack);
        c_charSkinnedRenderer.sharedMaterial = c_charStateMaterials[1];
        //classname = "Rocky Road";
        //eClassstate = PlayerBuild.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
        //charMaterial = charMaterials[0];
    } */

}
