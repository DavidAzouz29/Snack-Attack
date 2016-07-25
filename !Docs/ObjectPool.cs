/// <summary>
/// Name: ObjectPool.cs
/// Author: David Azouz
/// Date Created: 17/07/2016
/// Date Modified: 17/07/2016
/// ----------------------------------
/// Brief: An Object Pool class that assists with optimization.
/// GameObjects are reused instead of constantly calling Instatiate
/// viewed: http://gameprogrammingpatterns.com/object-pool.html
/// http://aieportal.aie.edu.au/pluginfile.php/49980/mod_resource/content/0/Optimisations%20-%20Tutorial.pdf
/// *Edit*
/// - Object Pool Instance - 17/07/2016
/// TODO:
/// - Make an Object Pool Manager
/// - 
/// </summary>

using UnityEngine;
//using System.Collections;

public class ObjectPool : MonoBehaviour
{
    //--------------------
    // Public variables
    //--------------------
    public static ObjectPool current;
    // The Prefab to pool
    public GameObject prefab;
    // how many to create to start with
    public int pre_generated_count;

    //is the pre-generated count a hard limit on how many to make?
    public bool hard_limit;
    //if we don't have a hard limit, how many objects do we grow by when we need to
    public int grow_rate;

    //--------------------
    // Private variables
    //--------------------
    // the array of actual generated gameobjects
    public GameObject[] generated;
    //how many have been spawned from the generated list
    private int used_count;
    //private int m_iFirstDeadIndex = -1; // First dead thing

    void Awake()
    {
        current = this;
    }

    // Use this for initialization
    void Start()
    {
        generated = new GameObject[pre_generated_count];
        for (int i = 0; i < generated.Length; ++i)
        {
            generated[i] = (GameObject)Instantiate(prefab);
            generated[i].SetActive(false);
        }
        used_count = 0;
        //m_iFirstDeadIndex = 0;
    }

    public GameObject Create(Vector3 a_position, Quaternion a_rotation)
    {
        GameObject result = null;
        //------------------------------------------------------------
        #region Expanding the array
        //------------------------------------------------------------
        // if we've filled up the array and we don't have a hard limit
        if (used_count == generated.Length && !hard_limit)
        {
            //creates a new array of objects
            GameObject[] new_array = new GameObject[generated.Length + grow_rate];

            for(int i = 0; i < new_array.Length; ++i)
            {
                // copy the old array into the new one
                if(i < generated.Length)
                {
                    new_array[i] = generated[i];
                }
                else
                {
                    // and create new gameobjects to fill in the empty space
                    new_array[i] = (GameObject)Instantiate(prefab);
                    new_array[i].SetActive(false);
                }
            }
            // swap the arrays
            generated = new_array;
        }
        #endregion
        //------------------------------------------------------------

        // check if we have space left
        if (used_count < generated.Length)
        {
            //get the next free gameobject
            result = generated[used_count];
            //turn the object on
            result.SetActive(true);
            //Send awake and start messages to all of its compontents
            result.SendMessage("Awake");
            result.SendMessage("Start");

            result.transform.position = a_position;
            result.transform.rotation = a_rotation;

            used_count++;
        }

        return result;
    }

    /* // Emits more GameObjects if we need to
    void Emit()
    {
        // Check that there is room for more particles.
        if (m_iFirstDeadIndex >= used_count) return;

        GameObject& go = generated[m_uiFirstDeadIndex++];
    }

    // Update is called once per frame
    void Update()
    {
        Emit();
        // sorts all the active objects to the left,
        // and all deactivated ones to after the first one that's "dead"/ to the right
        // TODO: we i++ so we can still access the "0th" element of the array
        for (int i = 0; i < m_iFirstDeadIndex; i++)
        {
            GameObject* go = &generated[i];

            // if we're deallocated/ not active
            if (!go.isActive())
            {
                *go = generated[m_iFirstDeadIndex - 1];
                m_iFirstDeadIndex--;
                continue;
            }
        }
    } */
	
	public void Free(GameObject a_go)
    {
        for(int i = 0; i < used_count; ++i)
        {
            if(generated[i] == a_go)
            {
                --used_count;

                a_go.SendMessage("OnDestroy");
                a_go.SetActive(false);
                GameObject tmp = generated[i];
                generated[i] = generated[used_count];
                generated[used_count] = tmp;

                return;
            }
        }
        Debug.Log("OP: Freed");
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < generated.Length; i++)
        {
            if (!generated[i].activeInHierarchy)
            {
                return generated[i];
            }
        }

        /*if (hard_limit)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            generated.Add(obj);
            return obj;
        } */

        return null;
    }
}
