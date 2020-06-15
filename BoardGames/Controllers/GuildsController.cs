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
    public class GuildsController : Controller
    {
        private ServiceContext db = new ServiceContext();

        // GET: guilds
        public ActionResult Index()
        {
            var guilds = db.Guilds.Include(t => t.Players).Include(t => t.JoinRequests).Include(t=> t.Invited);
            return View(guilds.ToList());
        }

        // GET: guilds/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guild guild = db.Guilds.Where(t => t.ID == id).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).Include(t=> t.Chat).SingleOrDefault();
            if (guild == null)
            {
                return HttpNotFound();
            }
            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            if(guild.Owner.ID==player.ID || guild.Players.Any(p => p.ID == player.ID))
            {
                return View(guild);

            }
            return RedirectToAction("Index");
        }

        // GET: guilds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: guilds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Descripton,ImageUrl")] Guild guild)
        {
            if (ModelState.IsValid)
            {
                Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
                guild.OwnerId = player.ID;
                guild.Owner = player;
                db.Guilds.Add(guild);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guild);
        }

        // GET: guilds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guild guild = db.Guilds.Where(t => t.ID == id).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();
            if (guild == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> items = new List<SelectListItem>();
            
            List<SelectListItem> selectItems = db.Players.Select(p => p).ToList().Where(p => 
            p.ID != guild.Owner.ID 
            && !guild.Players.Any(pl => pl.ID == p.ID) 
            && !guild.Invited.Any(pl => pl.ID == p.ID)
            && !guild.JoinRequests.Any(pl => pl.ID == p.ID))
                .Select(p => new SelectListItem { Text = p.Name, Value = p.ID.ToString() }).ToList();

            selectItems.ForEach(sli => items.Add(sli));

            ViewBag.PlayerToInvite = items;
            return View(guild);
        }

        // POST: guilds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Descripton,ImageUrl")] Guild guild)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guild).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guild);
        }

        // GET: guilds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guild guild = db.Guilds.Find(id);
            if (guild == null)
            {
                return HttpNotFound();
            }
            return View(guild);
        }

        // POST: guilds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Guild guild = db.Guilds.Find(id);
            db.Guilds.Remove(guild);
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
            Guild guild = db.Guilds.Where(t => t.ID == id).Include(t => t.JoinRequests).SingleOrDefault();
            if (guild == null)
            {
                return HttpNotFound();
            }
            
            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            guild.JoinRequests.Add(player);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult CancelJoinRequest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guild guild = db.Guilds.Where(t => t.ID == id).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();
            if (guild == null)
            {
                return HttpNotFound();
            }

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            guild.JoinRequests.Remove(player);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult InvitePlayer(int guildId, int PlayerToInvite)
        {
            Guild guild = db.Guilds.Where(t => t.ID == guildId).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == PlayerToInvite).FirstOrDefault();

            guild.Invited.Add(player);

            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult CancelInvite(int ID, int playerID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            guild.Invited.Remove(player);

            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemovePlayer(int ID, int playerID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            guild.Players.Remove(player);

            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult MakeOwner(int ID, int playerID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            guild.Players.Remove(player);
            guild.Players.Add(guild.Owner);
            guild.Owner = player;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AcceptJoinRequest(int ID, int playerID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            guild.JoinRequests.Remove(player);
            guild.Players.Add(player);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult DeclineJoinRequest(int ID, int playerID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.ID == playerID).FirstOrDefault();

            guild.JoinRequests.Remove(player);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Accept(int ID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            guild.Invited.Remove(player);
            guild.Players.Add(player);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Decline(int ID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            guild.Invited.Remove(player);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Quit(int? ID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).SingleOrDefault();

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            guild.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddComment(int ID)
        {
            Guild guild = db.Guilds.Where(t => t.ID == ID).Include(t => t.Players).Include(t => t.JoinRequests).Include(t => t.Invited).Include(g=> g.Chat).SingleOrDefault();

            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

            ChatMessage chatMessage = new ChatMessage { Guild = guild, Author = player, Content = Request["content"], Date = DateTime.Now };
            guild.Chat.Add(chatMessage);
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
