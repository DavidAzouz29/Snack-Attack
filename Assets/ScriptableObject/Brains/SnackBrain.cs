using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class SnackBrain : ScriptableObject
{
    protected string charName;
    protected Color Color;
    // Unique per base class
    protected PlayerBuild.E_BASE_CLASS_STATE eBaseClassState;
    //protected Animator c_charAnimator;
    protected RuntimeAnimatorController c_charAnimatorController;
    protected Avatar c_charAvatar;
    //protected SkinnedMeshRenderer c_charSkinnedRenderer;
    protected Mesh[] c_charStateMeshes = new Mesh[(int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT];
    protected Mesh c_charBlobMesh;

    // Unique per class
    protected Sprite c_charIcon;
    protected Material[] c_charStateMaterials;
    protected Material c_charBlobMaterial;
    protected PlayerBuild.E_ROCKYROAD_STATE eRockyRoadSkinState;
    protected PlayerBuild.E_PRINCESSCAKE_STATE ePrincessCakeSkinState;
    protected PlayerBuild.E_PIZZAPUNK_STATE ePizzaPunkSkinState;

    //public virtual void Initialize(SnackThinker snack) { } //base.Initialize(snack);
    public abstract void InitializeBase(SnackThinker snack);
    // Used for Sub Classes
    public abstract void Initialize(SnackThinker snack);
    // Getters
    public PlayerBuild.E_BASE_CLASS_STATE GetBaseState() { return eBaseClassState; }
    public abstract Material GetMaterial(int i);
    public Mesh[] GetStateMeshes() { return c_charStateMeshes; }
    public Mesh GetStateMesh(int i) { return c_charStateMeshes[i]; }
    public Mesh GetBlobMesh() { return c_charBlobMesh; }
    public RuntimeAnimatorController GetAnimatorController() { return c_charAnimatorController; }
    public Avatar GetAnimatorAvatar() { return c_charAvatar; }
    public Sprite GetIcon() { return c_charIcon; }
    //public abstract void Think(SnackThinker snack);
}
