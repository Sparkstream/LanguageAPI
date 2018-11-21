using LanguageAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageAPI.Helpers
{
    public static class ValidData
    {
        public static bool isValidDatabaseEntry(LanguageItem languageItem)
        {
            if (languageItem.languageCode == null)
            {
                return false;
            }
            if (languageItem.languageName == null)
            {
                return false;
            }
            if (languageItem.rank == 0)
            {
                return false;
            }
            return true;
        }

      
    }
}
