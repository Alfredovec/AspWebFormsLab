using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Entities;

namespace GameStore.Models.Services
{
    public interface ITranslateService
    {
        string GetGameDescription(string language, IEnumerable<GameTranslation> translations);

        string GetGenreName(string language, IEnumerable<GenreTranslation> translations);

        string GetPublisherDescrition(string language, IEnumerable<PublisherTranslation> translations);
    }
}
