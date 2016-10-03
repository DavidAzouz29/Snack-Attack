using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName= "Brains/Rocky Road/Cookie Crunch Brain", order = 2)]
public class CookieCrunchBrain : RockyRoadBrain
{
    // Use this for initialization
    public override void Initialize(SnackThinker snack)
    {
        base.InitializeBase(snack);
        eRockyRoadSkinState = PlayerBuild.E_ROCKYROAD_STATE.E_ROCKYROAD_STATE_COOKIECRUNCH;
        characterSkinnedRenderer.sharedMaterial = c_characterMaterial;
        //classname = "Mint Chop Chip";
        //eClassstate = PlayerBuild.E_CLASS_STATE.E_CLASS_STATE_MINTCHOPCHIP;
        //characterMaterial = characterMaterials[1];
    }
}
