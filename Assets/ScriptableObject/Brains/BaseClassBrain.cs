using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName= "Brains/Base Class Brain", order =0)]
public class BaseClassBrain : SnackBrain
{
    [Header("Base class properties")]
    [SerializeField] protected PlayerBuild.E_BASE_CLASS_STATE _eBaseClassState;
    //[SerializeField] protected new PlayerBuild.E_ROCKYROAD_STATE eRockyRoadSkinState;
    // Animation
    [SerializeField] protected RuntimeAnimatorController _charAnimatorController;
    [SerializeField] protected Avatar _charAvatar;
    // Meshes
    [SerializeField] protected Mesh[] _charStateMeshes = new Mesh[(int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT];
    [SerializeField] protected Mesh _charBlobMesh;
    [SerializeField] protected Texture[] _charEmissionMaps = new Texture[(int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT];
    [SerializeField] protected AudioClip[] _charTaunts = new AudioClip[4];
    [SerializeField] public float _localScale = 109.1183f;
    [SerializeField] public Quaternion _rotation;

    /*void OnEnable()
    {
        InitializeBase();
    } */

    public override void InitializeBase()
    {
        eBaseClassState = _eBaseClassState;
        c_charAnimatorController = _charAnimatorController;
        c_charAvatar = _charAvatar;
        c_charStateMeshes = _charStateMeshes;
        c_charBlobMesh = _charBlobMesh;
        c_charEmissionMaps = _charEmissionMaps;
        c_charTaunts = _charTaunts;
    }

    public override void Initialize(SnackThinker snack) { }
    public override string GetClassName() { return "None"; }
    public override PlayerBuild.E_BASE_CLASS_STATE GetBaseState() { return _eBaseClassState; }
    public override PlayerController.E_CLASS_STATE GetClassState() { return PlayerController.E_CLASS_STATE.E_PLAYER_STATE_COUNT; }
    public override Material GetStateMaterial(int i) { return null; }
    public override Mesh[] GetStateMeshes() { return _charStateMeshes; }
    public override Mesh GetStateMesh(int i) { return _charStateMeshes[i]; }
    public override Material GetBlobMaterial() { return null; }
    public override Mesh GetBlobMesh() { return _charBlobMesh; }
    public override Texture[] GetEmissionMaps() { return _charEmissionMaps; }
    public override AudioClip[] GetAudioTaunts() { return _charTaunts; }
    public override AudioClip GetAudioTaunt(int i) { return _charTaunts[i]; }
    public override RuntimeAnimatorController GetAnimatorController() { return _charAnimatorController; }
    public override Avatar GetAnimatorAvatar() { return _charAvatar; }
    public override int GetAnimID() { return -1; }
    public override Sprite GetIcon(int i) { return null; }


    /*{
        InitializeBase(snack);
        c_charSkinnedRenderer.sharedMaterial = c_charStateMaterials[1];
        //classname = "Rocky Road";
        //eClassstate = PlayerBuild.E_CLASS_STATE.E_CLASS_STATE_ROCKYROAD;
        //charMaterial = charMaterials[0];
    } */

}
