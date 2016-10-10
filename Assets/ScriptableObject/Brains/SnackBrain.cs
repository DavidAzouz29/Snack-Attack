using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName= "Brains/Snack Brain", order =0)]
public abstract class SnackBrain : ScriptableObject
{
    protected Color Color;
    // Unique per base class
    protected Animator characterAnimator;
    protected SkinnedMeshRenderer characterSkinnedRenderer;

    // 

    //public virtual void Initialize(SnackThinker snack) { } //base.Initialize(snack);
    public abstract void InitializeBase(SnackThinker snack);
    // Used for Sub Classes
    public abstract void Initialize(SnackThinker snack);
    //public abstract void Think(SnackThinker snack);
}
