using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
	[Serializable]
	public class PlayerInfo
	{
        // Unique per class varient
		public string ClassName; // name to display e.g. "Rocky Road"
        public Color Color;
        public PlayerBuild.E_BASE_CLASS_STATE eBaseClassState;
        public PlayerController.E_CLASS_STATE eClassState;
		
        // Serializing an object reference directly to JSON doesn't do what we want - we just get an InstanceID
		// which is not stable between sessions. So instead we serialize the string name of the object, and
		// look it back up again after deserialization
		private SnackBrain _cachedBrain;
		public SnackBrain Brain
		{
			get
			{
                if (_cachedBrain)
                {
                    return _cachedBrain;
                }                    
				if (!_cachedBrain && !String.IsNullOrEmpty(BrainName))
				{
                    // TODO: get rid of hack
					_cachedBrain = Instance.availableBrains.FirstOrDefault(b => b.name == BrainName);
				}
				return _cachedBrain;
			}
			set
			{
				_cachedBrain = value;
				BrainName = value ? value.name : String.Empty;
                ClassName = _cachedBrain.GetClassName();
                eBaseClassState = _cachedBrain.GetBaseState();
                eClassState = _cachedBrain.GetClassState();
            }
		}

		[SerializeField] private string BrainName;

		public string GetColoredName()
		{
			return "<color=#" + ColorUtility.ToHtmlStringRGBA(Color) + ">" + ClassName + "</color>";
		}
	}

	public List<PlayerInfo> players;

    public SubClassBrain[] availableBrains; //SnackBrain[]

    private static GameSettings _instance;
	public static GameSettings Instance
	{
		get
		{
			if (!_instance)
				_instance = Resources.FindObjectsOfTypeAll<GameSettings>().FirstOrDefault();
#if UNITY_EDITOR
			if (!_instance)
				InitializeFromDefault(UnityEditor.AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/Default game settings.asset"));
#endif
			return _instance;
		}
	}

    // Public due to SO
    public int NumberOfRounds;
	public int iRoundTimerChoice;

    public void OnEnable()
    {
#if UNITY_EDITOR
        // When working in the Editor and launching the game directly from the play scenes, rather than the
        // main menu, the brains may not be loaded and so Resources.FindObjectsOfTypeAll will not find them.
        // Instead, use the AssetDatabase to find them. At runtime, all available brains get loaded by the
        // MainMenuController so it's not a problem outside the editor.

        //availableBrains = UnityEditor.AssetDatabase.FindAssets("t:SubClassBrain")
        //                .Select(guid => UnityEditor.AssetDatabase.GUIDToAssetPath(guid))
        //                .Select(path => UnityEditor.AssetDatabase.LoadAssetAtPath<SubClassBrain>(path))
        //                .Where(b => b).ToArray();
#else
					availableBrains = Resources.FindObjectsOfTypeAll<SnackBrain>();
#endif

    }

    public SnackBrain CycleNextSelection(SnackBrain brain, bool isRight)
    {
        if (brain == null)
            return availableBrains[0];

        // Where you are now
        int index = Array.FindIndex(availableBrains, b => b == brain);
        if (isRight)
        {
            index++;
            return (index <= (availableBrains.Length - 1)) ? availableBrains[index] : availableBrains[0];
        }
        // Player chose left
        else
        {
            index--;
            if(index < 0) //TODO: correct?
            {
                index = availableBrains.Length - 1;
            }
            return (index >= 0) ? availableBrains[index] : null;
        }
    }

    public void SetRoundTimer(int a_time)
    {
        iRoundTimerChoice = a_time;
    }

	public static void LoadFromJSON(string path)
	{
		if (!_instance) DestroyImmediate(_instance);
		_instance = ScriptableObject.CreateInstance<GameSettings>();
		JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), _instance);
		_instance.hideFlags = HideFlags.HideAndDontSave;
	}

	public void SaveToJSON(string path)
	{
		Debug.LogFormat("Saving game settings to {0}", path);
		System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
	}

	public static void InitializeFromDefault(GameSettings settings)
	{
		if (_instance) DestroyImmediate(_instance);
		_instance = Instantiate(settings);
		_instance.hideFlags = HideFlags.HideAndDontSave;
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Window/Game Settings")]
	public static void ShowGameSettings()
	{
		UnityEditor.Selection.activeObject = Instance;
	}
#endif

	public bool ShouldFinishGame()
	{
		return GameState.Instance.RoundNumber >= NumberOfRounds;
    }

    public void OnBeginRound()
	{
		++GameState.Instance.RoundNumber;
	}

	public SnackThinker OnEndRound()
	{
		// Return the winner of the round, if there is one
		var winner = GameState.Instance.players.FirstOrDefault(t => t.IsAlive);

		if (winner != null)
			winner.TotalWins++;

		return winner != null ? winner.Snack : null;
	}

	public bool ShouldFinishRound()
	{
        //TODO: set another condition for winning
		return GameState.Instance.players.Count(p => p.IsAlive) <= 1;
	}

	public GameState.PlayerState GetGameWinner()
	{
		return GameState.Instance.GetPlayerWithMostWins();
	}
}
