using System.Collections.Generic;
using UnityEngine;

public class MovementArrowManager : MonoBehaviour
{
    public static MovementArrowManager Instance { get; private set; }

    [SerializeField] private Color _forwardsColor;
	[SerializeField] private Color _backwardsColor;

	[SerializeField] private MovementArrow _baseArrow;
    private List<MovementArrow> _allArrows;
    private int _nextArrowIndex;

	private void Awake()
	{
        Instance = this;
        _allArrows = new List<MovementArrow>() { _baseArrow };
	}

	public void EraseArrows()
    {
        for (int i = 0; i < _nextArrowIndex; i++)
        {
            _allArrows[i].gameObject.SetActive(false);
        }
        _nextArrowIndex = 0;
    }

    public void DrawArrow(Space start, Space target, bool isForwards)
    {
        GameObject arrowObject;
        MovementArrow arrow;
        if (_nextArrowIndex < _allArrows.Count)
        {
            arrow = _allArrows[_nextArrowIndex];
            arrowObject = arrow.gameObject;
        }
        else
        {
            arrowObject = Instantiate(_baseArrow.gameObject, transform);
			arrow = arrowObject.GetComponent<MovementArrow>();
            _allArrows.Add(arrow);
		}
        arrowObject.SetActive(true);
        arrowObject.GetComponent<SpriteRenderer>().color = isForwards ? _forwardsColor : _backwardsColor;
		arrow.Point(start, target);
        _nextArrowIndex++;
	}
}
