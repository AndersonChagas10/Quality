using DTO;
using DTO.Helpers;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Linq;

public static class GetResources
{
    public static DictionaryEntry getResource(string value)
    {
        System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

        if (GlobalConfig.LanguageBrasil)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Guard.LANGUAGE_PT_BR);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Guard.LANGUAGE_PT_BR);
        }
        else
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
        }

        var list = resourceManager.GetResourceSet(
            Thread.CurrentThread.CurrentUICulture, true, false);


        var listRes = list.Cast<DictionaryEntry>();

        foreach (var r in listRes)
        {
            if (r.Key.ToString() == value)
                return r;
        }

        return new DictionaryEntry();
    }
}

