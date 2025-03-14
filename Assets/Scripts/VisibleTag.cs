using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class VisibleTag : MonoBehaviour
{
	private bool _state;
	private Renderer _renderer;
	private Material _baseMaterial;

	[SerializeField] private Material _material;

	public bool State
	{
		get => _state;
		set
		{
			_state = value;
			if (_state)
			{
				_renderer.materials = new Material[] { _baseMaterial, _material };
			}
			else
			{
				_renderer.materials = new Material[] { _baseMaterial };
			}
		}
	}

	private void Awake()
	{
		_renderer = GetComponent<Renderer>();
		_baseMaterial = _renderer.material;
	}
}
