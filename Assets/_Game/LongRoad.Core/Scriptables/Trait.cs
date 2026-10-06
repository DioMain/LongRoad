using LongRoad.Core.Scriptables.Abstractions;
using NaughtyAttributes;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "Trait", menuName = "Entities/Trait")]
    public class Trait : LongRoadScriptable
    {
        [ShowAssetPreview]
        public Sprite Icon;
    }
}
