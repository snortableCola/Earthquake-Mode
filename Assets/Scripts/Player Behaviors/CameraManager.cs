using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField] private Transform _overhead;

	public static CameraManager Instance { get; private set; }
	private Player _target;
	private Vector3 _offset;

	private void Awake()
	{
		Instance = this;
	}

	public void ReturnOverhead()
	{
		_target = null;
		transform.SetPositionAndRotation(_overhead.position, _overhead.rotation);
	}

	public void MoveToPlayer(Player player)
	{
		_target = player;
		Transform playerCameraAnchor = player.transform.GetChild(0);
		transform.SetPositionAndRotation(playerCameraAnchor.position, playerCameraAnchor.rotation);
		_offset = transform.position - player.transform.position;
	}

	private void LateUpdate()
	{
		if (_target == null) return;

		transform.position = new Vector3(_target.transform.position.x + _offset.x, transform.position.y, _target.transform.position.z + _offset.z);
	}
}
