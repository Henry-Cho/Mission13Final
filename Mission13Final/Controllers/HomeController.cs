using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13Final.Models;

namespace Mission13Final.Controllers
{
    public class HomeController : Controller
    {
        private BowlerContext bowler { get; set; }

        public HomeController(BowlerContext _b)
        {
            bowler = _b;
        }

        public IActionResult Index(string teamName)
        {
            HttpContext.Session.Remove("id");
            ViewBag.TeamName = teamName ?? "Home";
            var record = bowler.Bowlers
                .Include(x => x.Team)
                .Where(x => x.Team.TeamName == teamName || teamName == null)
                .ToList();
            return View(record);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Form()
        {
            ViewBag.Teams = bowler.Teams.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Form(Bowler b)
        {
            int max = 0;

            foreach (var s in bowler.Bowlers)
            {
                if (max < s.BowlerID)
                {
                    max = s.BowlerID;
                }
            }

            b.BowlerID = max + 1;

            if (ModelState.IsValid)
            {
                bowler.Add(b);
                bowler.SaveChanges();

                return RedirectToAction("Index", new { teamName = "" });
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int bowlerId)
        {
            ViewBag.New = false;

            ViewBag.Teams = bowler.Teams.ToList();

            HttpContext.Session.SetString("id", bowlerId.ToString());

            var record = bowler.Bowlers.Single(x => x.BowlerID == bowlerId);

            return View("Form", record);
        }

        // Edit a moive (POST)
        [HttpPost]
        public IActionResult Edit(Bowler b)
        {
            string id = HttpContext.Session.GetString("id");

            int int_id = int.Parse(id);

            b.BowlerID = int_id;

            // model validation
            if (ModelState.IsValid)
            {
                // edit a specific model
                bowler.Update(b);
                bowler.SaveChanges();
                HttpContext.Session.Remove("id");
                // show them in the index page
                return RedirectToAction("Index", new { teamName = "" });
            }
            ViewBag.New = false;

            ViewBag.Teams = bowler.Teams.ToList();

            return View("Form", b);
        }

        // Delete
        public IActionResult Delete(int bowlerId)
        {
            // find a movie from DB by its id
            var record = bowler.Bowlers.Single(x => x.BowlerID == bowlerId);
            bowler.Bowlers.Remove(record);
            bowler.SaveChanges();

            return RedirectToAction("Index", new { teamName = "" });
        }
    }
}