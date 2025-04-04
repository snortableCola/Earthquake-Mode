using ButtonUI;
using TMPro;
using UnityEngine;

namespace ButtonPrompts
{

    [RequireComponent(typeof(TMP_Text))]
    public class SetTextToTextBox : MonoBehaviour
    {
        [TextArea(2, 3)]
        [SerializeField] private string message = "Press BUTTONPROMPT to interact.";

        [Header("Set Up For Sprites")]
        [SerializeField] private ListSprites sprites;
        [SerializeField] private DeviceType deviceType;

        private Playerinput playerInput;
        private TMP_Text textBox;

        private void Awake()
        {
            playerInput = new Playerinput();
            textBox = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            SetText();
        }

        [ContextMenu("Set Text")]

        private void SetText()
        {
            
        }
    } 

}
