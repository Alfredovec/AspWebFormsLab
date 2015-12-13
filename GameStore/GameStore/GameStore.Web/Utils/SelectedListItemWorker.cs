using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.Web.Models;

namespace GameStore.Web.Utils
{
    public static class SelectedListItemWorker
    {
        public static IEnumerable<SelectListItem> GenerateSelectListGenres(IEnumerable<GenreViewModel> genres)
        {
            return genres.Select(g => new SelectListItem {Text = g.Name, Value = g.Id.ToString()}).ToList();
        }

        public static IEnumerable<SelectListItem> GenerateSelectListPlatformTypes(IEnumerable<PlatformTypeViewModel> types)
        {
            return types.Select(g => new SelectListItem { Text = g.Type, Value = g.Id.ToString() }).ToList();
        }

        public static IEnumerable<SelectListItem> GenerateSelectListPublishers(IEnumerable<PublisherViewModel> publishers)
        {
            return publishers.Select(g => new SelectListItem { Text = g.CompanyName, Value = g.Id.ToString() }).ToList();
        }

        public static IEnumerable<SelectListItem> GenerateSelectListRoles(List<RoleViewModel> roles)
        {
            return roles.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToList();
        }
    }
}