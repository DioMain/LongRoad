using LongRoad.Core.Localization;
using NaughtyAttributes;
using UnityEngine;

namespace LongRoad.Core.Scriptables.Abstractions
{
    public abstract class LongRoadScriptable : ScriptableObject
    {
        [InfoBox("Need locales:" +
            "\ntag_name - for name" +
            "\ntag_desc - for description")]
        public string Tag;

        public string GetName(LocalizationManager localization)
        {
            return localization.GetEntityString($"{Tag}_name");
        }

        public string GetDescription(LocalizationManager localization)
        {
            return localization.GetEntityString($"{Tag}_desc");
        }
    }
}
