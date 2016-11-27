using UnityEngine;

public class TimerSelect : MonoBehaviour
{
    void Awake()
    {
        // The default is set to 3 minutes
        GameManager.Instance.m_ActiveGameSettings.iRoundTimerChoice = 1;
    }

    public void TimerSelection(int a_value)
    {
        GameManager.Instance.m_ActiveGameSettings.iRoundTimerChoice = a_value;
    }
}
