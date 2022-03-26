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

        // Index
        public IActionResult Index(string teamName)
        {
            // If a key-value pair of "id" exists
            HttpContext.Session.Remove("id");

            // assign teamName in this ViewBag.TeamName
            ViewBag.TeamName = teamName ?? "Home";

            // get the record of bowlers of a certain team
            var record = bowler.Bowlers
                .Include(x => x.Team)
                .Where(x => x.Team.TeamName == teamName || teamName == null)
                .ToList();
            return View(record);
        }

        [HttpGet]
        public IActionResult Form()
        {
            // assign a list of teams in ViewBag.Teams
            ViewBag.Teams = bowler.Teams.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Form(Bowler b)
        {
            // get the max BowlerID
            int max = 0;

            foreach (var s in bowler.Bowlers)
            {
                if (max < s.BowlerID)
                {
                    max = s.BowlerID;
                }
            }

            // assign BowlerID which is max + 1 (to get a new BowlerID) 
            b.BowlerID = max + 1;

            // if model is validated
            if (ModelState.IsValid)
            {
                bowler.Add(b);
                bowler.SaveChanges();

                // redirect to Index
                return RedirectToAction("Index", new { teamName = "" });
            }
            else
            {
                return View();
            }
        }

        // Edit a bowler (GET)

        [HttpGet]
        public IActionResult Edit(int bowlerId)
        {
            // this indicates that I am editing
            ViewBag.New = false;

            // get a list of teams and assign them in ViewBag.Teams
            ViewBag.Teams = bowler.Teams.ToList();

            // Set a key-value pair of "id" with bowlerID (string type)
            HttpContext.Session.SetString("id", bowlerId.ToString());

            // get a record of a particular bowler
            var record = bowler.Bowlers.Single(x => x.BowlerID == bowlerId);

            return View("Form", record);
        }

        // Edit a bowler (POST)
        [HttpPost]
        public IActionResult Edit(Bowler b)
        {
            // get the value pair of "id"
            string id = HttpContext.Session.GetString("id");

            // make it a int type
            int int_id = int.Parse(id);

            // assign it with BowlerID in the passed model
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
            // if fails

            // indicates that I am still editing
            ViewBag.New = false;

            // assign a list of teams in ViewBag.Teams
            ViewBag.Teams = bowler.Teams.ToList();

            return View("Form", b);
        }

        // Delete
        public IActionResult Delete(int bowlerId)
        {
            // find a bowler from DB by its id
            var record = bowler.Bowlers.Single(x => x.BowlerID == bowlerId);
            // remove the record
            bowler.Bowlers.Remove(record);
            // save changes
            bowler.SaveChanges();

            return RedirectToAction("Index", new { teamName = "" });
        }
    }
}