using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds some general information of a space.
/// </summary>
[RequireComponent(typeof(NextSpaceProvider), typeof(MeshRenderer))]
public class Space : MonoBehaviour
{
    /// <summary>
    /// Represents whether or not the space is on fire.
    /// </summary>
    private bool _isOnFire;

    private Renderer _renderer;
    private Material _baseMaterial;
    [SerializeField] private Material _fireMaterial;

	public void Awake()
	{
		_renderer = GetComponent<MeshRenderer>();
        _baseMaterial = _renderer.material;
	}

    public bool IsOnFire
    {
        get => _isOnFire;
        set
        {
            _isOnFire = value;
            if (_isOnFire)
			{
                _renderer.materials = new Material[] { _baseMaterial, _fireMaterial };
            }
            else
			{
				_renderer.materials = new Material[] { _baseMaterial };
			}
        }
    }

    /// <summary>
    /// Represents the space's biome.
    /// </summary>
    public BoardBiome Biome;

    /// <summary>
    /// Represents the space's typical behavior when landed on by a player.
    /// </summary>
    public SpaceType Type;

    /// <summary>
    /// An enum containing all potential biomes a space could occupy.
    /// </summary>
    public enum BoardBiome
    {
        None,
        Coast,
        Plains,
        Mountains
    }

    /// <summary>
    /// An enum containing descriptors for each way a space could behave when landed on by a player.
    /// </summary>
    public enum SpaceType
    {
        None,
        Oil,
		Resource,
		Negative,
        Transport,
        Bonus,
        Shop,
        Sabotage
    }
}
