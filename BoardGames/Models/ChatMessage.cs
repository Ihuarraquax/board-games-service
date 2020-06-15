using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGames.Models
{
    public class ChatMessage
    {
        public int ID { get; set; }
        public Player Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public virtual Guild Guild { get; set; }
        
    }
}