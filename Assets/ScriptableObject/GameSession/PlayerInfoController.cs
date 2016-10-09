using UnityEngine;
using System.Collections;

public class PlayerInfoController : MonoBehaviour
{
	public int PlayerIndex;
	public UnityEngine.UI.Button PlayerColor;
	public UnityEngine.UI.Text c_PlayerBrain;
    [SerializeField] private GameSettings m_ActiveGameSettings;

    //private MainMenuController _mainMenu;
	private GameSettings.PlayerInfo _player;
	public void Awake()
	{
		//_mainMenu = GetComponentInParent<MainMenuController>();
        m_ActiveGameSettings = GetComponentInParent<MainMenuController>().GameSettingsTemplate;
    }

	public void Refresh()
	{
		_player = GameSettings.Instance.players[PlayerIndex];

		var colorBlock = PlayerColor.colors;
		colorBlock.normalColor = _player.Color;
		colorBlock.highlightedColor = _player.Color;
		PlayerColor.colors = colorBlock;

		c_PlayerBrain.text = (_player.Brain != null)
			? _player.Brain.name.TrimEnd(" Brain".ToCharArray())
			: "None";

    }

	/*public void OnCycleColor()
	{
		_player.Color = _mainMenu.GetNextColor(_player.Color);
		Refresh();
	} */

    // Left
    public void OnPreviousBrain()
    {
        //_player.Brain = _mainMenu.CycleNextSelection(_player.Brain, false);
        _player.Brain = m_ActiveGameSettings.CycleNextSelection(_player.Brain, false);
        Refresh();
    }

    // Right
    public void OnNextBrain()
	{
        //_player.Brain = _mainMenu.CycleNextSelection(_player.Brain, true);
        _player.Brain = m_ActiveGameSettings.CycleNextSelection(_player.Brain, true);
        Refresh();
	}
}
