using UnityEngine;

public class DisasterMeter : MonoBehaviour
{
    [SerializeField] public GameObject MeterUI;
    public void ToggleObject()
    {
        if (MeterUI != null)
        {
            MeterUI.SetActive(!MeterUI.activeSelf);
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
