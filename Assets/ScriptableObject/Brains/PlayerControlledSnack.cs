using UnityEngine;
using System.Collections;

//[CreateAssetMenu(menuName="Brains/Player Controlled")]
public class PlayerControlledSnack //: SnackBrain
{

	/*public int PlayerNumber;
	private string m_MovementAxisName;
	private string m_TurnAxisName;
	private string m_FireButton;


	public void OnEnable()
	{
		m_MovementAxisName = "Vertical" + PlayerNumber;
		m_TurnAxisName = "Horizontal" + PlayerNumber;
		m_FireButton = "Fire" + PlayerNumber;
	}

	public override void Think(SnackThinker Snack)
	{
		var movement = Snack.GetComponent<SnackMovement>();

		movement.Steer(Input.GetAxis(m_MovementAxisName), Input.GetAxis(m_TurnAxisName));

		var shooting = Snack.GetComponent<SnackShooting>();

		if (Input.GetButton(m_FireButton))
			shooting.BeginChargingShot();
		else
			shooting.FireChargedShot(); 
	}*/
}
