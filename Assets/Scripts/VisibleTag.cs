using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class VisibleTag : MonoBehaviour
{
	private bool _state;
	private Renderer _renderer;

	[SerializeField] private Material _material;

	public bool State
	{
		get => _state;
		set
		{
			if (_state == value) return;

			_state = value;
			if (_state)
			{
				_renderer.sharedMaterials = _renderer.sharedMaterials.Append(_material).ToArray();
			}
			else
			{
				_renderer.sharedMaterials = _renderer.sharedMaterials.Where(m => m != _material).ToArray();
			}
		}
	}

	private void Awake()
	{
		_renderer = GetComponent<Renderer>();
	}
}
