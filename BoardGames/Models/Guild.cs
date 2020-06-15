using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGames.Models
{
    public class Guild
    {
        public int ID { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        public string Descripton { get; set; }
        [Display(Name = "Link do grafiki")]
        public string ImageUrl { get; set; }
        public int OwnerId { get; set; }
        public HashSet<Player> Players { get; set; }
        public HashSet<BoardGame> BoardGames { get; set; }
        public HashSet<Player> Invited { get; set; }
        public HashSet<Player> JoinRequests { get; set; }
        public List<ChatMessage> Chat { get; set; }


        public virtual Player Owner { get; set; }
    }
}