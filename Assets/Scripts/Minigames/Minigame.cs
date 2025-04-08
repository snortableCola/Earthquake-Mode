using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
	public abstract string Instructions { get; }
	public abstract GameObject InitialPanel { get; }
	public abstract void StartGame();
}