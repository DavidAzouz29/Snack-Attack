using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName= "Brains/Rocky Road/Mint Chop Chip", order = 1)]
public class MintChopChipBrain : RockyRoadBrain
{
    // Use this for initialization
    public override void Initialize(SnackThinker snack)
    {
        base.InitializeBase(snack);
        eRockyRoadSkinState = PlayerBuild.E_ROCKYROAD_STATE.E_ROCKYROAD_STATE_MINTCHOPCHIP;
        characterSkinnedRenderer.sharedMaterial = c_characterMaterial;
        //classname = "Mint Chop Chip";
        //eClassstate = PlayerBuild.E_CLASS_STATE.E_CLASS_STATE_MINTCHOPCHIP;
    }
}
