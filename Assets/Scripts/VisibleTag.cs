using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class VisibleTag : MonoBehaviour
{
	private bool _field;
	private Renderer _renderer;
	private Material _baseMaterial;

	[SerializeField] private Material _material;

	public bool State
	{
		get => _field;
		set
		{
			_field = value;
			if (_field)
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
