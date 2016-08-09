/// ----------------------------------
/// <summary>
/// Name: GameObjectExtensions.cs
/// Author: David Azouz
/// Date Created: 9/08/2016
/// Date Modified: 8/2016
/// ----------------------------------
/// Brief: GameObjectExtensions class that "Lists" GameObjects (U.I.) elements.
/// viewed: 
/// http://forum.unity3d.com/threads/ui-image-how-to-change-render-order-between-parent-and-child.268340/
/// *Edit*
/// -  - David Azouz 9/08/2016
/// -  - David Azouz 9/08/2016
/// TODO:
/// -  - /8/2016
/// - 
/// </summary>
/// ----------------------------------

using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    // public static void SortChildren(this GameObject gameObject)
    // {
    //     var children = gameObject.GetComponentsInChildren<Transform>(true);
    //     var sorted = from child in children
    //                  orderby child.gameObject.activeInHierarchy descending, child.localPosition.z descending
    //                  where child != gameObject.transform
    //                  select child;
    //     for (int i = 0; i < sorted.Count(); i++)
    //         sorted.ElementAt(i).SetSiblingIndex(i);
    // }
    public static void SortChildren(this GameObject o)
    {
        var children = o.GetComponentsInChildren<Transform>(true).ToList();
        children.Remove(o.transform);
        children.Sort(Compare);
        for (int i = 0; i < children.Count; i++)
            children[i].SetSiblingIndex(i);
    }

    private static int Compare(Transform lhs, Transform rhs)
    {
        if (lhs == rhs) return 0;
        var test = rhs.gameObject.activeInHierarchy.CompareTo(lhs.gameObject.activeInHierarchy);
        if (test != 0) return test;
        if (lhs.localPosition.z < rhs.localPosition.z) return 1;
        if (lhs.localPosition.z > rhs.localPosition.z) return -1;
        return 0;
    }
}