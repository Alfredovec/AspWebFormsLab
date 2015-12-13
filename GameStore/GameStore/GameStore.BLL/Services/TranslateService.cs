using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Entities;
using GameStore.Models.Services;

namespace GameStore.BLL.Services
{
    class TranslateService : ITranslateService
    {
        private Language ToLanguageEnum(string language)
        {
            var sb = new StringBuilder(language);
            sb[0] = char.ToUpper(sb[0]);
            return (Language)Enum.Parse(typeof (Language), sb.ToString());
        }

        public string GetGameDescription(string language, IEnumerable<GameTranslation> translations)
        {
            var lang = ToLanguageEnum(language);
            var neededTranslation =
                translations.FirstOrDefault(t => t.Language == lang);
            if (neededTranslation != null)
            {
                return neededTranslation.Description;
            }
            var translation = translations.FirstOrDefault(t => t.Language == GameTranslation.DefaultLanguage);
            return translation == null ? "" : translation.Description;
        }

        public string GetGenreName(string language, IEnumerable<GenreTranslation> translations)
        {
            var lang = ToLanguageEnum(language);
            var neededTranslation =
                translations.FirstOrDefault(t => t.Language == lang);
            if (neededTranslation != null)
            {
                return neededTranslation.Name;
            }
            var translation = translations.FirstOrDefault(t => t.Language == GameTranslation.DefaultLanguage);
            return translation == null ? "" : translation.Name;
        }


        public string GetPublisherDescrition(string language, IEnumerable<PublisherTranslation> translations)
        {
            var lang = ToLanguageEnum(language);
            var neededTranslation =
                translations.FirstOrDefault(t => t.Language == lang);
            if (neededTranslation != null)
            {
                return neededTranslation.Description;
            }
            var translation = translations.FirstOrDefault(t => t.Language == GameTranslation.DefaultLanguage);
            return translation == null ? "" : translation.Description;
        }
    }
}
