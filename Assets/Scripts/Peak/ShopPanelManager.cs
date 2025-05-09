using UnityEngine;

public class ShopPanelManager : MonoBehaviour
{
    public static ShopPanelManager Instance { get; private set; }

	[SerializeField] private ShopPanel _shopPanel;

	public ShopPanel ShopPanel => _shopPanel;

	private void Awake()
	{
		Instance = this;
	}
}
