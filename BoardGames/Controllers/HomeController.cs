using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoardGames.DAL;
using BoardGames.ViewModels;

namespace BoardGames.Controllers
{
    public class HomeController : Controller
    {
        private ServiceContext db = new ServiceContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var data = db.BoardGames
                .OrderByDescending(bg => bg.Players.Count)
                .Select(bg => new BoardGamePlayersGroup()
            {
                BoardGame = bg,
                Players = bg.Players.Count
            });
         
            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}