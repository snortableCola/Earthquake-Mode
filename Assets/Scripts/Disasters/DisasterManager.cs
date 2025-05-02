using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisasterManager : MonoBehaviour
{
    private static readonly Dictionary<Biome, string[]> s_warnings = new()
    {
        {
            Biome.Shore, new string[] { "Gale Advisory", "Flood Watch", "Flood Warning", "Tsunami!" }
        },
        {
            Biome.Plains, new string[] { "Wind Advisory", "Tornado Watch", "Tornado Warning", "Tornado!" }
        },
        {
            Biome.Mountains, new string[] { "Fire Advisory", "Fire Watch", "Fire Warning", "Wildfire!" }
        }
    };
    public static DisasterManager Instance { get; private set; }
    

	[SerializeField] private int _disasterThreshold;

	[SerializeField] private Tsunami _tsunami;
	[SerializeField] private Tornado _tornado;
	[SerializeField] private Wildfire _wildfire;
	[SerializeField] private Earthquake _earthquake;

    public Wildfire Wildfire => _wildfire;

	[SerializeField] private TextMeshProUGUI disaster_info;


	private Player _currentPlayer; //ref to current player 
    private void Awake()
	{
		Instance = this;
		_disasterTracker = new()
		{
			{_wildfire, 0},
			{_tsunami, 0},
			{_tornado, 0},
			{_earthquake, 0}
		};
        Debug.Log("Disaster levels initialized to 0.");
       
    }
    /// <summary>
    /// Sets the current active player and updates the disaster information display.
    /// </summary>
    /// <param name="player">The player whose turn it is.</param>
    public void SetCurrentPlayer(Player player)
    {
        _currentPlayer = player;
        UpdateDisasterInfo();

    }

    // Update the disaster text for the current player
    public void UpdateDisasterInfo()
    {
        if (_currentPlayer == null) return;

        Biome biome = _currentPlayer.CurrentBiome;
        Disaster disaster = GetDisasterForBiome(biome);

        if (disaster == null)
        {
            disaster_info.text = $"{biome}: No disasters.";
            Debug.Log($"Biome {biome} has no associated disaster.");

            return;
        }
        Debug.Log("Current _disasterTracker contents:");
        foreach (var entry in _disasterTracker)
        {
            Debug.Log($"Disaster: {entry.Key.name}, Level: {entry.Value}");
        }
        int level = _disasterTracker[disaster];
        Debug.Log($"Current player is in biome {biome} with disaster {disaster.name} at level {level}");
        disaster_info.text = GetDisasterMessage(biome, level);
    }

   
    private Disaster GetDisasterForBiome(Biome biome)
    {
        return biome switch
        {
            Biome.Shore => _tsunami,
            Biome.Plains => _tornado,
            Biome.Mountains => _wildfire,
            _ => null
        };
    }

	// Method to initialize messages for each biome and disaster level
	private string GetDisasterMessage(Biome biome, int level) => $"{biome}: {(level > 0 && s_warnings.TryGetValue(biome, out string[] warnings) && level <= warnings.Length ? warnings[level - 1] : "Safe from disasters")}";


	private Dictionary<Disaster, int> _disasterTracker;

	/// <summary>
	/// Increments the disaster level of whatever is associated with the specified biome.
	/// </summary>
	/// <param name="biome">The biome whose corresponding disaster should be incremented.</param>
	/// <param name="player">The player responsible for the disaster level's incrementation.</param>
	public IEnumerator IncrementBiomeDisaster(Biome biome, Player player)
	{
        Disaster disaster = GetDisasterForBiome(biome);
        if (disaster == null)
        {
            Debug.LogWarning($"No disaster associated with biome {biome}.");
            yield break;
        }

        // Increment the disaster level
        int currentLevel = _disasterTracker[disaster];
        if (currentLevel >= _disasterThreshold)
        {
            Debug.LogWarning($"Disaster {disaster.name} already at or above threshold: {currentLevel}");
            //DisasterParticleManager.GetInstance().PlayEffect(disaster.DisasterType);
            yield break;
        }

        _disasterTracker[disaster]++; // Increment the level
        int disasterLevel = _disasterTracker[disaster];
        Debug.Log($"Incremented {disaster.name} in {biome} to level {disasterLevel}");

        // Update the disaster information display
        UpdateDisasterInfo();

        // Check if the disaster level reached the threshold
        if (disasterLevel == _disasterThreshold)
        {
            yield return IncrementDisaster(disaster, player); // Only handle threshold logic here
        }
    }

	public IEnumerator IncrementEarthquake() => IncrementDisaster(_earthquake, null);

	/// <summary>
	/// Increments the disaster level for a specified disaster, triggering it if it reaches the disaster threshold.
	/// </summary>
	/// <param name="disaster">The disaster to increment the disaster level of, and to potentially trigger.</param>
	/// <param name="incitingPlayer"></param>
	private IEnumerator IncrementDisaster(Disaster disaster, Player incitingPlayer)
    {
        if (!disaster.IsPossible)
        {
            Debug.LogWarning($"Disaster {disaster.name} is not possible at this time.");
            yield break;
        }
       

        int disasterLevel = _disasterTracker[disaster];
        Debug.Log($"Checking disaster {disaster.name} at level {disasterLevel}");

        // Disaster is triggered when the level reaches the threshold
        if (disasterLevel != _disasterThreshold)
        {
			DisasterParticleManager.Instance.HandleEffect(disaster.name,true, 5f);
        }

        if (disaster == _earthquake && disasterLevel != _disasterThreshold)
        {
            DisasterParticleManager.Instance.EarthquakeShake();
        }

        Debug.Log($"Triggering disaster {disaster.name}!");
        yield return disaster.StartDisaster(incitingPlayer);

        // Reset the disaster level
        _disasterTracker[disaster] = 0;
        UpdateDisasterInfo();
    }

	public void RefreshDisasters()
	{
		foreach(Disaster disaster in _disasterTracker.Keys)
		{
			disaster.Refresh();
		}
	}
	
}
