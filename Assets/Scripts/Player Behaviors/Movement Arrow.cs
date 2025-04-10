using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MovementArrow : MonoBehaviour
{
	public void Point(Space start, Space target)
    {
		Vector3 startPos = start.transform.position;
		Vector3 targetPos = target.transform.position;

		Vector3 direction = (targetPos - startPos).normalized;
		Vector3 midpoint = (startPos + targetPos) / 2f;
		midpoint.y += 0.3f;

		// Align the arrow's local Y-axis with the direction vector on the board
		float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

		transform.SetPositionAndRotation(midpoint, Quaternion.Euler(90f, angle, 90f));
	}
}
