using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
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

    public abstract void StartGame();

    public virtual void CompleteMinigame(int reward)
    {
        Player player = GameManager.Instance.CurrentPlayer;
        if (player != null)
        {
            player.Points += reward; // Update the player's points
            Debug.Log($"{player.name} completed the minigame and earned {reward} points. Total points: {player.Points}");
        }

        // Notify the MinigameManager that the minigame is completed
        MinigameManager.Instance.EndCurrentMinigame();

        //Show movement panels again at the end of minigames 
        MinigameManager.Instance.panelManager.ShowMovementUI(); 
    }
}