using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    protected Player player; 
    public abstract void StartGame();
    public virtual void SetPlayer(Player player)
    {
        this.player = player;
        Debug.Log($"Minigame received player: {player.name}");
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
