using UnityEngine;

public class TestGun : MonoBehaviour
{
    [SerializeField] private RectTransform chamber;

    public void TestSpin()
    {
        // rotate the chamber recttransform 360 degrees over 1 second
        LeanTween.rotateAround(chamber, Vector3.forward, 360f, 1f).setEase(LeanTweenType.easeInOutSine);
        Debug.Log("SpinChamber() initiated");
    }
}
