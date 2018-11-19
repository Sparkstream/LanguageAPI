using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageAPI.Model
{
    public class LanguageItem
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string sourceLanguage { get; set; }
        public string targetLanguage { get; set; }
        public string sourceWord { get; set; }
        public string targetWord { get; set; }

    }
}
