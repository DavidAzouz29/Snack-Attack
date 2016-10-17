using UnityEngine;
using System.Collections;

public class AttackEnter : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private GameManager m_GameManager;
    float fPitchMin = 0.9f;
    float fPitchMax = 1.3f;

    void OnEnable()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCollision[] temp;
        AudioSource audioSourceSlot = null;
        temp = animator.gameObject.GetComponentsInChildren<PlayerCollision>();
        if (animator.GetBool("Boss") && animator.GetBool("Attacking2"))
        {
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].weaponIsActive = true;
                audioSourceSlot = m_GameManager.transform.GetChild(0).GetChild(1).GetComponent<AudioSource>();
                audioSourceSlot.pitch = Random.Range(fPitchMin, fPitchMax); //audioSourceSlot.loop = true;
                audioSourceSlot.Play();
                //TODO: m_GameManager.r_PlayerManager.GetPlayer(0).GetComponent<PlayerController>().enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < temp.Length; i++)
            {
                // Light Attack
                if (temp[i].gameObject.tag == "Weapon1" && animator.GetBool("Attacking1"))
                {
                    temp[i].weaponIsActive = true;
                    // Cheat to get the first sound (light attack)
                    audioSourceSlot = m_GameManager.transform.GetChild(0).GetComponentInChildren<AudioSource>();
                    // ScriptableObject so no "WaitForSeconds"
                    audioSourceSlot.pitch = Random.Range(fPitchMin, fPitchMax); //audioSourceSlot.loop = true;
                    audioSourceSlot.Play(); //audioSourceSlot.loop = true;
                    //audioSourceSlot.PlayDelayed(audioSourceSlot.clip.length); // For second hit etc.
                }
                // Heavy Attack
                if (temp[i].gameObject.tag == "Weapon2" && animator.GetBool("Attacking2"))
                {
                    temp[i].weaponIsActive = true;
                    temp[i].isHeavyAttack = true;
                    audioSourceSlot = m_GameManager.transform.GetChild(0).GetChild(1).GetComponent<AudioSource>();
                    audioSourceSlot.pitch = Random.Range(fPitchMin, fPitchMax); //audioSourceSlot.loop = true;
                    audioSourceSlot.Play();
                }
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
