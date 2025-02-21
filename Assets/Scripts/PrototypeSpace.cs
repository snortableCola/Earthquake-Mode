using UnityEngine;

public class PrototypeSpace : MonoBehaviour
{
    public bool IsOnFire;
    public BoardRegion Region;
    public SpaceType Type;

    public PrototypeSpace[] AdjacentSpaces;

    public enum BoardRegion
    {
        None,
        Lowlands,
        Plains,
        Mountains
    }

    public enum SpaceType
    {
        None,
        Negative
    }
}
