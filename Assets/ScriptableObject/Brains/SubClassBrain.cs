using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName= "Brains/Sub Class Brain", order =1)]
public class SubClassBrain : BaseClassBrain
{
    [Header("Unique to each sub class")]
    public BaseClassBrain _baseClassBrain;
    public int _iBrainID; // # to Order by
    [SerializeField] protected string _charName;
    [SerializeField] protected Color _Color;
    [SerializeField] protected PlayerController.E_CLASS_STATE _eClassState;
    [SerializeField] protected Sprite[] _charIcons = new Sprite[2];
    [SerializeField] protected Material[] _charStateMaterials = new Material[(int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT];
    [SerializeField] protected Material _charBlobMaterial;

    void OnEnable()
    {
        Initialize(null);
    }

    public override void Initialize(SnackThinker snack)
    {
        //_baseClassBrain.InitializeBase();
        eBaseClassState = _baseClassBrain.GetBaseState();
        c_charAnimatorController = _baseClassBrain.GetAnimatorController();
        c_charAvatar = _baseClassBrain.GetAnimatorAvatar();
        c_charStateMeshes = _baseClassBrain.GetStateMeshes();
        c_charBlobMesh = _baseClassBrain.GetBlobMesh();
        c_charEmissionMaps = _baseClassBrain.GetEmissionMaps();
        c_charTaunts = _baseClassBrain.GetAudioTaunts();
        _localScale = _baseClassBrain._localScale;
        _rotation = _baseClassBrain._rotation;
        //eRockyRoadSkinState = PlayerBuild.E_ROCKYROAD_STATE.E_ROCKYROAD_STATE_ROCKYROAD;
        charName = _charName;
        Color = _Color;
        eClassState = _eClassState;
        c_charIcons = _charIcons; //TODO: set to neut through state
        c_charBlobMaterial = _charBlobMaterial;
    }

    public override string GetClassName() { return _charName; }
    public override PlayerBuild.E_BASE_CLASS_STATE GetBaseState() { return eBaseClassState; }
    public override PlayerController.E_CLASS_STATE GetClassState() { return eClassState; }
    public override Material GetStateMaterial(int i) { return _charStateMaterials[i]; }
    public override Mesh[] GetStateMeshes() { return c_charStateMeshes; }
    public override Mesh GetStateMesh(int i) { return c_charStateMeshes[i]; }
    public override Material GetBlobMaterial() { return _charBlobMaterial; }
    public override Mesh GetBlobMesh() { return c_charBlobMesh; }
    public override Texture[] GetEmissionMaps() { return c_charEmissionMaps; }
    public override AudioClip[] GetAudioTaunts() { return c_charTaunts; }
    public override AudioClip GetAudioTaunt(int i) { return c_charTaunts[i]; }
    public override RuntimeAnimatorController GetAnimatorController() { return c_charAnimatorController; }
    public override Avatar GetAnimatorAvatar() { return c_charAvatar; }
    public override Sprite GetIcon(int i) { return c_charIcons[i]; }

}
