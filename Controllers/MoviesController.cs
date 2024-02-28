using MoviesDBManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesDBManager.Controllers
{
    public class MoviesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Movies(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Movies.HasChanged)
            {
                return PartialView(DB.Movies.ToList().OrderBy(c => c.Name));
            }
            return null;
        }
        public ActionResult Create()
        {
            return View(new Movie());
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                DB.Movies.Add(movie);
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Details(int id)
        {
            Movie movie = DB.Movies.Get(id);
            if (movie != null)
            {
                return View(movie);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            Movie movie = DB.Movies.Get(id);
            if (movie != null)
            {
                return View(movie);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Movie movie, List<int> SelectedActors)
        {
            if (ModelState.IsValid)
            {
                DB.Movies.Update(movie, SelectedActors);
                return RedirectToAction("Details", new { id = movie.Id });
            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            DB.Movies.Delete(id);
            return RedirectToAction("Index");
        }
    }
}