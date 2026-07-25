namespace LongRoad.Core.Localization
{
    public enum Locale
    {
        en_US, ru_RU
    }

    public static class LocaleExtension
    {
        public static string GetLocaleCode(this Locale locale)
        {
            return locale switch
            {
                Locale.en_US => "en-US",
                Locale.ru_RU => "ru-RU",
                _ => throw new System.NotImplementedException()
            };
        }
    }
}
