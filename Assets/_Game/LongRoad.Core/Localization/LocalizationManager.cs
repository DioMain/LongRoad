using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace LongRoad.Core.Localization
{
    public class LocalizationManager : LongRoadBehaviourCore
    {
        [SerializeField]
        private TableReference MainTable;
        [SerializeField]
        private TableReference EntitiesTable;

        public void ChangeLanguage(Locale locale)
        {
            StartCoroutine(SetLocale(locale.GetLocaleCode()));
        }

        public string GetMainString(string tag)
        {
            var lstr = new LocalizedString(MainTable, tag);

            return lstr.GetLocalizedString();
        }

        public string GetEntityString(string tag)
        {
            var lstr = new LocalizedString(EntitiesTable, tag);

            return lstr.GetLocalizedString();
        }

        private IEnumerator SetLocale(string localeCode)
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales
                .Locales.FirstOrDefault(i => i.LocaleName == localeCode);
        }
    }
}
