using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName= "Brains/Sub Class Brain", order =1)]
public class SubClassBrain : BaseClassBrain
{
    public BaseClassBrain _baseClassBrain;
    [SerializeField] protected string _charName;
    [SerializeField] protected Color _Color;
    [SerializeField] protected Sprite _charIcon;
    [SerializeField] protected Material[] _charStateMaterials = new Material[(int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT];
    [SerializeField] protected Material _charBlobMaterial;

    void OnEnable()
    {
        Initialize(null);
    }

    public override void Initialize(SnackThinker snack)
    {
        //_baseClassBrain.InitializeBase(snack);
        eBaseClassState = _baseClassBrain.GetBaseState();
        c_charAnimatorController = _baseClassBrain.GetAnimatorController();
        c_charAvatar = _baseClassBrain.GetAnimatorAvatar();
        c_charStateMeshes = _baseClassBrain.GetStateMeshes();
        c_charBlobMesh = _baseClassBrain.GetBlobMesh();
        _localScale = _baseClassBrain._localScale;
        _rotation = _baseClassBrain._rotation;

        //eRockyRoadSkinState = PlayerBuild.E_ROCKYROAD_STATE.E_ROCKYROAD_STATE_ROCKYROAD;
        charName = _charName;
        Color = _Color;
        c_charIcon = _charIcon;
        c_charBlobMaterial = _charBlobMaterial;
    }

    public override Material GetMaterial(int i)
    {
        return _charStateMaterials[i];
    }

}
