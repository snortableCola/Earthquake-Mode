using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject bettingPanel;
    public GameObject selectionPanel; // Reference to the Selection panel
    public GameObject flipPanel;

    void Start()
    {
        ShowBettingPanel(); // Start with the betting panel
    }

    public void ShowBettingPanel()
    {
        bettingPanel.SetActive(true);
        selectionPanel.SetActive(false);
        flipPanel.SetActive(false);
    }

    public void ShowSelectionPanel()
    {
        bettingPanel.SetActive(false);
        selectionPanel.SetActive(true);
        flipPanel.SetActive(false);
    }

    public void ShowFlipPanel()
    {
        bettingPanel.SetActive(false);
        selectionPanel.SetActive(false);
        flipPanel.SetActive(true);
    }
    public void EndCG()
    { 
    flipPanel.SetActive(false);
    }
}
