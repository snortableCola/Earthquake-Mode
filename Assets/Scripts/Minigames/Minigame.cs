using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    protected Player player; 
    public abstract void StartGame();
    public virtual void SetPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogError($"Null Player passed to {name} {GetInstanceID()}!");
            return;
        }
        
        this.player = player; // Assign the player reference
        Debug.Log($"Minigame {name} {GetInstanceID()} received player: {player.name}");

    }
    public virtual void Cleanup()
    {
        // Example: Reset any UI event listeners tied to this instance
        Debug.Log($"Cleaning up minigame: {name}");


        // Hide any related UI
        if (MinigameManager.Instance.panelManager != null)
        {
            MinigameManager.Instance.panelManager.HideAllPanels();
        }
    }

    public virtual void CompleteMinigame(int reward)
    {
        if (player != null)
        {
            player.AdjustPoints(reward); // Update the player's points
            Debug.Log($"{player.name} completed the minigame and earned {reward} points. Total points: {player.totalPoints}");
        }

        // Notify the MinigameManager that the minigame is completed
        MinigameManager.Instance.MinigameCompleted(reward);
    }
}
