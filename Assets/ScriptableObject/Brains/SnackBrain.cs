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
    protected PlayerController.E_CLASS_STATE eClassState;
    //protected Animator c_charAnimator;
    protected RuntimeAnimatorController c_charAnimatorController;
    protected Avatar c_charAvatar;
    //protected SkinnedMeshRenderer c_charSkinnedRenderer;
    protected Mesh[] c_charStateMeshes = new Mesh[(int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT];
    protected Mesh c_charBlobMesh;
    protected Texture[] c_charEmissionMaps = new Texture[(int)PlayerBuild.E_BOSS_STATE.E_BOSS_STATE_MAIN_COUNT];

    // Unique per class
    protected Sprite c_charIcon;
    protected Material[] c_charStateMaterials;
    protected Material c_charBlobMaterial;
    protected PlayerBuild.E_ROCKYROAD_STATE eRockyRoadSkinState;
    protected PlayerBuild.E_PRINCESSCAKE_STATE ePrincessCakeSkinState;
    protected PlayerBuild.E_PIZZAPUNK_STATE ePizzaPunkSkinState;

    //public virtual void Initialize(SnackThinker snack) { } //base.Initialize(snack);
    public abstract void InitializeBase();
    //public abstract void InitializeBase(SnackThinker snack);
    // Used for Sub Classes
    public abstract void Initialize(SnackThinker snack);
    //public abstract void Initialize(SnackThinker snack);
    // Getters
    public abstract string GetClassName();
    public abstract PlayerBuild.E_BASE_CLASS_STATE GetBaseState();
    public abstract PlayerController.E_CLASS_STATE GetClassState();
    public abstract Material GetStateMaterial(int i);
    public abstract Mesh[] GetStateMeshes();
    public abstract Mesh GetStateMesh(int i);
    public abstract Material GetBlobMaterial();
    public abstract Mesh GetBlobMesh();
    public abstract Texture[] GetEmissionMaps();
    public abstract RuntimeAnimatorController GetAnimatorController();
    public abstract Avatar GetAnimatorAvatar();
    public abstract Sprite GetIcon();
    //public abstract void Think(SnackThinker snack);
}
