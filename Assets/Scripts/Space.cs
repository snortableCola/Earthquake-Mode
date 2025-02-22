using UnityEngine;

public class Space : MonoBehaviour
{
    public bool IsOnFire;
    public BoardRegion Region;
    public SpaceType Type;

    public Space[] AdjacentSpaces;
    public Space NextSpace;

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
