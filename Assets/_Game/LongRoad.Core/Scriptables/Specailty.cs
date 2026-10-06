using LongRoad.Core.Scriptables.Abstractions;
using NaughtyAttributes;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "Specaility", menuName = "Entities/Specaility")]
    public class Specailty : LongRoadScriptable
    {
        [ShowAssetPreview]
        public Sprite Icon;
    }
}
