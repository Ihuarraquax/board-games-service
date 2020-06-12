using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGames.Models
{
    public class Team
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Descripton { get; set; }
        public string ImageUrl { get; set; }
        public int OwnerId { get; set; }
        public HashSet<Player> Players { get; set; }
        public HashSet<BoardGame> BoardGames { get; set; }
        public HashSet<Player> Invited { get; set; }
        public HashSet<Player> JoinRequests { get; set; }


        public virtual Player Owner { get; set; }
    }
}