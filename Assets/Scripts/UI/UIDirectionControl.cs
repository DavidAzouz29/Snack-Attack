using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    // This class is used to make sure world space UI
    // elements such as the health bar face the correct direction.

    public bool m_UseRelativeRotation = true;       // Use relative rotation should be used for this gameobject?

    private Transform m_relativeTransform;
    private Quaternion m_RelativeRotation;          // The local rotatation at the start of the scene.
    private Vector3 m_RelativePosition;          // The local Position at the start of the scene.


    private void Start ()
    {
        m_relativeTransform = transform.parent.transform;
        m_RelativeRotation = m_relativeTransform.localRotation;
        m_RelativePosition = m_relativeTransform.position;
    }


    private void Update ()
    {
        if (m_UseRelativeRotation)
        {
            transform.rotation = m_RelativeRotation;
            transform.position = m_RelativePosition;
        }
    }

    void OnCollisionEnter(Collision a_collision)
    {
        // allow the ring to follow the player when...
        // they climb up shelves but not when they jump.
        if (a_collision.transform.tag != "StaticObject")
        {
            //transform.localPosition = transform.parent.localPosition;
        }
    }
}