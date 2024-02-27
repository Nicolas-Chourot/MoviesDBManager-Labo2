using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoviesDBManager.Models;

namespace MoviesDBManager.Controllers
{
    public class ActorsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Actors(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Actors.HasChanged)
            {
                return PartialView(DB.Actors.ToList().OrderBy(c => c.Name));
            }
            return null;
        }
        public ActionResult Create()
        {
            ViewBag.Castings = null;
            ViewBag.Movies = SelectListUtilities<Movie>.Convert(DB.Movies.ToList());
            return View(new Actor());
        }

        [HttpPost]
        public ActionResult Create(Actor actor, List<int> SelectedMovies)
        {
            if (ModelState.IsValid)
            {
                DB.Actors.Add(actor, SelectedMovies);
                return RedirectToAction("Index");
            }
            ViewBag.Castings = SelectListUtilities<Movie>.Convert(actor.Movies);
            ViewBag.Movies = SelectListUtilities<Movie>.Convert(DB.Movies.ToList());
            return View();
        }

        public ActionResult Details(int id)
        {
            Actor actor = DB.Actors.Get(id);
            if (actor != null)
            {
                return View(actor);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Actor actor = DB.Actors.Get(id);
            if (actor != null)
            {
                ViewBag.Castings = SelectListUtilities<Movie>.Convert(actor.Movies);
                ViewBag.Movies = SelectListUtilities<Movie>.Convert(DB.Movies.ToList());
                return View(actor);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Actor actor, List<int> SelectedMovies)
        {
            if (ModelState.IsValid)
            {
                DB.Actors.Update(actor, SelectedMovies);
                return RedirectToAction("Details", new {id = actor.Id });
            }
            ViewBag.Movies = SelectListUtilities<Movie>.Convert(actor.Movies);
            ViewBag.Movies = SelectListUtilities<Movie>.Convert(DB.Movies.ToList());
            return View(actor);
        }

        public ActionResult Delete(int id)
        {
            DB.Actors.Delete(id);
            return RedirectToAction("Index");
        }
    }
}