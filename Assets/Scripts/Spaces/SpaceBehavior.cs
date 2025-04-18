using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class SpaceBehavior : MonoBehaviour
{
	public virtual bool HasPassingBehavior { get; } = false;
    public virtual IEnumerator RespondToPlayerPassing(Player player)
    {
        yield break;
    }

	public abstract IEnumerator RespondToPlayerEnd(Player player);
    
    public virtual IEnumerator WaitForHudCompletion()
    {
        yield break;
    }
}
