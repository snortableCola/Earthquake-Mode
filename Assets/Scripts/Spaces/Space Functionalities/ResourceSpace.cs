using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Space))]
public class ResourceSpace : SpaceBehavior
{
  
    private Space _space;
    private Image _blueHUD;
    public float hudMessageDuration = 2f;

    private void Awake()
    {
        _space = GetComponent<Space>();
        // Dynamically find the image by name or tag
        _blueHUD = Resources.FindObjectsOfTypeAll<Image>()
                       .FirstOrDefault(img => img.gameObject.CompareTag("BLUEHUD"));

        if (_blueHUD == null)
        {
            Debug.LogError("HUD Image not found! Make sure the tag is correctly set and object exists.");
        }

    }

   
    public override IEnumerator RespondToPlayerEnd(Player player) => GeneralResourceSpace.Instance.RespondToPlayerEnd(player);

    public override IEnumerator WaitForHudCompletion() => GeneralResourceSpace.Instance.WaitForHudCompletion();
}
