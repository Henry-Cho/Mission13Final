using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mission13Final.Models;

namespace Mission13Final.Components
{
    public class TeamsViewComponents : ViewComponent
    {
        private BowlerContext bowler { get; set; }

        public TeamsViewComponents(BowlerContext temp)
        {
            bowler = temp;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedTeam = RouteData?.Values["teamName"] ?? "";

            var teams = bowler.Bowlers
                .Select(x => x.Team.TeamName)
                .Distinct()
                .OrderBy(x => x);

            return View(teams);
        }
    }
}
