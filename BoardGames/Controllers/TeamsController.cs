using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BoardGames.DAL;
using BoardGames.Models;

namespace BoardGames.Controllers
{
    public class TeamsController : Controller
    {
        private ServiceContext db = new ServiceContext();

        // GET: Teams
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Players).Include(t => t.JoinRequests).Include(t=> t.Invited);
            return View(teams.ToList());
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Descripton")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Where(t => t.ID == id).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();
            if (team == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> items = new List<SelectListItem>();
            
            List<SelectListItem> selectItems = db.Players.Select(p => p).ToList().Where(p => 
            p.ID != team.Owner.ID 
            && !team.Players.Any(pl => pl.ID == p.ID) 
            && !team.Invited.Any(pl => pl.ID == p.ID)
            && !team.JoinRequests.Any(pl => pl.ID == p.ID))
                .Select(p => new SelectListItem { Text = p.Name, Value = p.ID.ToString() }).ToList();

            selectItems.ForEach(sli => items.Add(sli));

            ViewBag.PlayerToInvite = items;
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Descripton")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //invites and more

        public ActionResult JoinRequest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Where(t => t.ID == id).Include(t => t.JoinRequests).SingleOrDefault();
            if (team == null)
            {
                return HttpNotFound();
            }
            
            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            team.JoinRequests.Add(player);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult CancelJoinRequest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Where(t => t.ID == id).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();
            if (team == null)
            {
                return HttpNotFound();
            }

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            team.JoinRequests.Remove(player);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult InvitePlayer(int teamId, int PlayerToInvite)
        {
            Team team = db.Teams.Where(t => t.ID == teamId).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == PlayerToInvite).FirstOrDefault();

            team.Invited.Add(player);

            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult CancelInvite(int ID, int playerID)
        {
            Team team = db.Teams.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            team.Invited.Remove(player);

            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemovePlayer(int ID, int playerID)
        {
            Team team = db.Teams.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            team.Players.Remove(player);

            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult MakeOwner(int ID, int playerID)
        {
            Team team = db.Teams.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            team.Players.Remove(player);
            team.Players.Add(team.Owner);
            team.Owner = player;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AcceptJoinRequest(int ID, int playerID)
        {
            Team team = db.Teams.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            team.JoinRequests.Remove(player);
            team.Players.Add(player);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult DeclineJoinRequest(int ID, int playerID)
        {
            Team team = db.Teams.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            team.JoinRequests.Remove(player);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Accept(int ID)
        {
            Team team = db.Teams.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            team.Invited.Remove(player);
            team.Players.Add(player);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Decline(int ID)
        {
            Team team = db.Teams.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            team.Invited.Remove(player);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Quit(int? ID)
        {
            Team team = db.Teams.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            team.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
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
