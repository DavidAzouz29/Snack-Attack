/// <summary>
/// Name: PowerUpEffect.cs
/// Author: David Azouz
/// Date Modified: 14/07/16
/// --------------------------------------------------
/// Brief: Power up effect.
/// viewed: Delegate object example 27:21m https://youtu.be/VBA1QCoEAX4 
/// https://bitbucket.org/richardfine/scriptableobjectdemo 
/// --------------------------------------------------
/// Edits:
/// -  - David Azouz 14/07/2016
/// - 
/// 
/// TODO:
/// 
/// </summary>

using UnityEngine;
using System.Collections;

abstract public class PowerUpEffect : ScriptableObject
{
    // Applies an effect onto the other game object
    public abstract void ApplyTo(GameObject go);
}

// TODO: move to separate script
// Adds an amount to the total value of our Boss 
class BossBooster : PowerUpEffect
{
    public int Amount;
    // Apply an amount to a game object
    public override void ApplyTo(GameObject go)
    {
        ///go.GetComponent<Health>().currentValue += Amount;
        //go.GetComponent<Boss>().currentValue += Amount;
    }
}

// TODO: move to separate script
class Powerup : MonoBehaviour
{
    public PowerUpEffect effect;
    public void OnTriggerEnter(Collider other)
    {
        effect.ApplyTo(other.gameObject);
    }
}