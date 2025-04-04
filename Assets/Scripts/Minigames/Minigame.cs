using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    private Player _player;
    protected Player Player
    {
        get => _player;
        set
        {
            _player = value;
            Debug.Log($"Player changed to {value}");
        }
    }
    public abstract void StartGame();
    public virtual void SetPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogError($"Null Player passed to {name} {GetInstanceID()}!");
            return;
        }

        Player = player; // Assign the player reference
        Debug.Log($"Minigame {name} {GetInstanceID()} received player: {player.name}");
    }

    public virtual void Cleanup()
    {
        // Example: Reset any UI event listeners tied to this instance
        Debug.Log($"Cleaning up minigame: {name} {GetInstanceID()}");

        // Hide any related UI
        if (MinigameManager.Instance.panelManager != null)
        {
            MinigameManager.Instance.panelManager.HideAllPanels();
        }
    }

    public virtual void CompleteMinigame(int reward)
    {
        if (Player != null)
        {
            Player.AdjustPoints(reward); // Update the player's points
            Debug.Log($"{Player.name} completed the minigame and earned {reward} points. Total points: {Player.totalPoints}");
        }

        // Notify the MinigameManager that the minigame is completed
        MinigameManager.Instance.MinigameCompleted(reward);
    }
}