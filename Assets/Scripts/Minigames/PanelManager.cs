using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; } // Singleton instance

	[SerializeField] private GameObject _movementUI;
	[SerializeField] private GameObject _instructionPanel;
	[SerializeField] private TMP_Text _instructionText;
	[SerializeField] private TMP_Text _gameNameText;
	[SerializeField] private Button _startMinigameButton;

	void Awake()
	{
		Instance = this;

        _startMinigameButton.onClick.AddListener(OnStartMinigameButtonClicked);
    }

    public void ShowInstructionPanel(Minigame minigame)
    {
        _movementUI.SetActive(false);
        HideAllMinigamePanels(minigame);

		_instructionText.text = minigame.Instructions;
		_gameNameText.text = minigame.name; // Set the minigame name in the TMP_Text component
		_instructionPanel.SetActive(true);
	}

    public void OnStartMinigameButtonClicked()
    {
        _instructionPanel.SetActive(false); // Hide the instruction panel
		MinigameManager.Instance.StartMinigame(); // Start the minigame
	}

    public void ShowPanel(Minigame minigame, int panelIndex)
    {
		if (panelIndex < minigame.MinigamePanels.Length)
		{
			HideAllMinigamePanels(minigame);
			minigame.MinigamePanels[panelIndex].SetActive(true);
        }
    }

    public void HideAllMinigamePanels(Minigame minigame)
    {
		foreach (var panel in minigame.MinigamePanels)
		{
			panel.SetActive(false);
		}
	}

	public void ShowMovementUI() => _movementUI.SetActive(true);
}
