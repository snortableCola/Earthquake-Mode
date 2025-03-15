using UnityEngine;

public struct Adjacency
{
	public Vector2 Direction;
	public Space Space;
	public bool IsForwards;

	public Adjacency(Vector2 direction, Space space, bool isForwards)
	{
		Direction = direction;
		Space = space;
		IsForwards = isForwards;
	}
}