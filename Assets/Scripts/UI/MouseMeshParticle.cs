/// <summary>
/// Author: David Azouz
/// Brief: Script that sets this objects position to the Mouse, 
/// Allows a mesh particle effect to follow the mouse.
/// viewed: https://youtu.be/70JQ6MbHuvQ 
/// </summary>

using UnityEngine;
using System.Collections;

public class MouseMeshParticle : MonoBehaviour 
{
	public Vector3 c_MousePos;
	public float distance = 12.0f;
	
	// Update is called once per frame
	void Update () 
	{
		c_MousePos = Input.mousePosition;
		c_MousePos.z = distance;
		transform.position = Camera.main.ScreenToWorldPoint (c_MousePos);
	}
}
