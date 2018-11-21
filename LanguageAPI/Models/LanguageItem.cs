using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageAPI.Models
{
    public class LanguageItem
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int rank { get; set; }
        public string languageName { get; set; }
        public string languageCode { get; set; }
        public string word { get; set; }
    }

}
