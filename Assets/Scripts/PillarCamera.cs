using UnityEngine;
using System.Collections;

public class PillarCamera : MonoBehaviour
{
    public Transform target;
    public Transform pillar;
    public float fDistance;
    public float speed;
	
	// Update is called once per frame
	void LateUpdate ()
    {
        Vector3 pillar_pos = pillar.position;
        pillar_pos.y = target.position.y;

        Vector3 to_player = target.position - pillar_pos;
        to_player.Normalize();

        Vector3 cam_pos = pillar_pos + to_player * fDistance;

        transform.position = cam_pos;
        transform.rotation = Quaternion.LookRotation(-to_player, Vector3.up);

	}
}
