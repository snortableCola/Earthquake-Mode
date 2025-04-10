using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ButtonUI
{
    /// <summary>
    /// This class is used to store a list of sprites.
    /// </summary>
    [CreateAssetMenu(fileName = "ListSprites", menuName = "ListSprites", order = 0)]

    [System.Serializable]
    public class ListSprites : ScriptableObject
    {
        public List<TMP_SpriteAsset> sprites;
    }
}
