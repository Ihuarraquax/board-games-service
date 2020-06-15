using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BoardGames.DAL;
using BoardGames.Models;

namespace BoardGames.Controllers
{
    public class EventsController : Controller
    {
        private ServiceContext db = new ServiceContext();

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.Include(a => a.HostPlayer);
            //var events = db.Events.Include("HostPlayer");
            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            ViewBag.Player = player;
            return View(events.OrderByDescending(e => e.Date).ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParticipantPlayers = new SelectList(db.Players, "ID", "NameAndEmail");
            ViewBag.Guilds = new SelectList(db.Guilds, "ID", "Name");
            return View(@event);
        }
        [Authorize]
        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.HostPlayerID = new SelectList(db.Players, "ID", "Email");
            var boardGamesOptions = db.BoardGames.Select(bg => new SelectListItem
            {
                Value = bg.ID.ToString(),
                Text = bg.Name,
                Selected = false
            }).ToList();
            ViewBag.BoardGamesList = new MultiSelectList(boardGamesOptions, "Value", "Text");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Place,Date")] Event @event, int[] BoardGames)
        {
            if (ModelState.IsValid)
            {
                @event.HostPlayer = db.Players.Single(p => p.Email == User.Identity.Name);
                @event.BoardGames = db.BoardGames.Where(bg => BoardGames.Contains(bg.ID)).ToList();
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HostPlayerID = new SelectList(db.Players, "ID", "Email", @event.HostPlayerID);
            return View(@event);
        }
        [Authorize]
        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.HostPlayerID = new SelectList(db.Players, "ID", "Email", @event.HostPlayerID);
            ViewBag.HostPlayerID = new SelectList(db.Guilds, "ID", "Email", @event.HostPlayerID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,HostPlayerID,Place,Date")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HostPlayerID = new SelectList(db.Players, "ID", "Email", @event.HostPlayerID);
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            @event.ParticipantPlayers = null;
            @event.HostPlayer = null;
            @event.BoardGames = null;
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Cancel(int id)
        {
            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            db.Events.Find(id).ParticipantPlayers.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Join(int id)
        {
            Player player = db.Players.Where(p => p.Email == User.Identity.Name).FirstOrDefault();
            db.Events.Find(id).ParticipantPlayers.Add(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendInvite(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            string playerId = Request["ParticipantPlayers"];
            string messageText = Request["message"];
            Player player = db.Players.Find(int.Parse(playerId));
            
            var message = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["sender"], player.Email)
            {
                Subject = "Zaproszenie do gry",
                Body = messageText
            };

            var smtpClient = new System.Net.Mail.SmtpClient
            {
                Host = ConfigurationManager.AppSettings["smtpHost"],
                Credentials = new System.Net.NetworkCredential(
                    ConfigurationManager.AppSettings["sender"],
                    ConfigurationManager.AppSettings["passwd"]),
                EnableSsl = true
            };

            smtpClient.Send(message);
            return RedirectToAction("Index");
        }

        public ActionResult SendInviteToGuild()
        {
            Event @event = db.Events.Find(int.Parse(Request["id"]));
            if (@event == null)
            {
                return HttpNotFound();
            }

            int guildId = int.Parse(Request["Guilds"].ToString());
            string domainName = Request.Url.GetLeftPart(UriPartial.Authority);
            string messageText = $"{@event.HostPlayer.NameAndEmail} zaprasza cię na wydarzenie <b>{@event.Name}</b>!\n\n" +
                $"{domainName}/Event/Details/{@event.ID}";

            foreach (var p in db.Guilds.Where(g=> g.ID == guildId).SelectMany(g => g.Players).ToList())
            {
                var message = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["sender"], p.Email)
                {
                    Subject = "Zaproszenie do Wydarzenia ",
                    Body = messageText
                };

                var smtpClient = new System.Net.Mail.SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["smtpHost"],
                    Credentials = new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["sender"],
                        ConfigurationManager.AppSettings["passwd"]),
                    EnableSsl = true
                };

                smtpClient.Send(message);
            }
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
