﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BoardGames.DAL;
using BoardGames.Models;
using PagedList;

namespace BoardGames.Controllers
{
    
    public class BoardGamesController : Controller
    {
        private ServiceContext db = new ServiceContext();

        // GET: BoardGames
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PlayersSortParm = sortOrder == "MinPlayers" ? "minPlayers_desc" : "MinPlayers";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var boardGames = from s in db.BoardGames
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                boardGames = boardGames.Where(bg => bg.Name.Contains(searchString)
                                       || bg.Description.Contains(searchString)
                                       || bg.Categories.Any(c => c.Name.Contains(searchString)));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    boardGames = boardGames.OrderByDescending(s => s.Name);
                    break;
                case "MinPlayers":
                    boardGames = boardGames.OrderBy(s => s.MinPlayers);
                    break;
                case "minPlayers_desc":
                    boardGames = boardGames.OrderByDescending(s => s.MinPlayers);
                    break;
                default:
                    boardGames = boardGames.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(boardGames.ToPagedList(pageNumber, pageSize));
        }

        // GET: BoardGames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardGame boardGame = db.BoardGames.Find(id);
            if (boardGame == null)
            {
                return HttpNotFound();
            }
            return View(boardGame);
        }

        // GET: BoardGames/Create
        [Authorize]
        public ActionResult Create()
        {
            var categoriesOptions = db.Categories.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Name,
                Selected = false
            }).ToList();
            ViewBag.CategoriesList = new MultiSelectList(categoriesOptions, "Value", "Text");
            return View();
        }

        // POST: BoardGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,Name,MinPlayers,MaxPlayers,Description,Website,ImageUrl")] BoardGame boardGame, int[] Categories)
        {
            if (ModelState.IsValid)
            {
                Player player = db.Players.Single(p => p.Email == User.Identity.Name);
                boardGame.CreatedByPlayer = player;
                //check if link is image, if not use placeholder
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadData(boardGame.ImageUrl);

                }
                catch
                {
                    boardGame.ImageUrl = "/Obrazki/boardgame-placeholder.jpg";
                }

                boardGame.Categories = db.Categories.Where(c => Categories.Contains(c.ID)).ToList();

                db.BoardGames.Add(boardGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(boardGame);
        }

        // GET: BoardGames/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardGame boardGame = db.BoardGames.Find(id);
            if (boardGame == null)
            {
                return HttpNotFound();
            }
            return View(boardGame);
        }

        // POST: BoardGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,Name,MinPlayers,MaxPlayers,Description,Website")] BoardGame boardGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boardGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(boardGame);
        }

        // GET: BoardGames/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BoardGame boardGame = db.BoardGames.Find(id);
            if (boardGame == null)
            {
                return HttpNotFound();
            }
            return View(boardGame);
        }

        // POST: BoardGames/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BoardGame boardGame = db.BoardGames.Find(id);
            db.BoardGames.Remove(boardGame);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult AddToFavourites(int id)
        {
            BoardGame boardGame = db.BoardGames.Find(id);
            Player player = db.Players.Single(p => p.Email == User.Identity.Name);
            player.FavouriteGames.Add(boardGame);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        [Authorize]
        public ActionResult RemoveFromFavourites(int id)
        {
            BoardGame boardGame = db.BoardGames.Find(id);
            Player player = db.Players.Single(p => p.Email == User.Identity.Name);
            player.FavouriteGames.Remove(boardGame);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
