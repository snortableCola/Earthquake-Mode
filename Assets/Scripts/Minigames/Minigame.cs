using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
	public GameObject InitialPanel;
	public abstract string Instructions { get; }
	public abstract void StartGame();
}