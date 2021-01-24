using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies/Random 
        public Movie movie;
        public ActionResult Random()
        {

            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
            new Customer {Id = 0, Name = "William"},
            new Customer {Id = 1, Name = "Breanna"}
            };

            var viewModel = new RandomMovieNewModel
            {
                Movie = movie,
                Customers = customers

            };
            return View(viewModel);

        }
        
        [Route("movies/released/{year}/{month:regex(\\d{4}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year +"/"+ month);
        }


    }
}