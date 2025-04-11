using UnityEngine;

public class OilManager : MonoBehaviour
{
	public static OilManager Instance { get; private set; }

	[SerializeField] private int ActiveSpaceCount = 2;

	private OilSpace[] _oilSpaces;

	private void Awake()
	{
		Instance = this;
		_oilSpaces = FindObjectsByType<OilSpace>(FindObjectsSortMode.None);
		SelectRandomOilSpaces();
	}

	[ContextMenu("Randomize Oil")]
	public void SelectRandomOilSpaces()
	{
		for (int i = _oilSpaces.Length - 1; i > 0; i--)
		{
			int rnd = Random.Range(0, i + 1);
			(_oilSpaces[i], _oilSpaces[rnd]) = (_oilSpaces[rnd], _oilSpaces[i]);
		}
		for (int i = 0; i < _oilSpaces.Length; i++)
		{
			_oilSpaces[i].IsActive = i < ActiveSpaceCount;
		}
	}
}
