using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName= "Brains/Rocky Road/Rainbow Warrior", order = 3)]
public class RainbowWarriorBrain : RockyRoadBrain
{
    // Use this for initialization
    public override void Initialize(SnackThinker snack)
    {
        base.InitializeBase(snack);
        eRockyRoadSkinState = PlayerBuild.E_ROCKYROAD_STATE.E_ROCKYROAD_STATE_RAINBOWWARRIOR;
        characterSkinnedRenderer.sharedMaterial = c_characterMaterial;
        //classname = "Rainbow Warrior";
        //eClassstate = PlayerBuild.E_CLASS_STATE.E_ROCKYROAD_STATE_RAINBOWWARRIOR;
    }
}
