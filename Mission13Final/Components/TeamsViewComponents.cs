using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mission13Final.Models;

namespace Mission13Final.Components
{
    public class TeamsViewComponents : ViewComponent
    {
        // get the context of Bowler
        private BowlerContext bowler { get; set; }

        // constructor
        public TeamsViewComponents(BowlerContext temp)
        {
            bowler = temp;
        }

        // invoke
        public IViewComponentResult Invoke()
        {
            // get the route data of teamName and assign that in ViewBag.SelectedTeam
            ViewBag.SelectedTeam = RouteData?.Values["teamName"] ?? "";

            // get team names from context
            var teams = bowler.Bowlers
                .Select(x => x.Team.TeamName)
                .Distinct()
                .OrderBy(x => x);

            return View(teams);
        }
    }
}
