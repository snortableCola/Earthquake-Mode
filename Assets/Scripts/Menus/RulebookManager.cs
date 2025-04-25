using UnityEngine;

public class RulebookManager : MonoBehaviour
{
    public GameObject SetupPanel;
    public GameObject SpacesPanel;
    public GameObject HowToWinPanel;
    public GameObject DisastersPanel; 
  

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowSetupPanel()
    {
        HideAllPanels();
        SetupPanel.SetActive(true);
    
    }
    private void HideAllPanels()
    { 
    
    }
}
