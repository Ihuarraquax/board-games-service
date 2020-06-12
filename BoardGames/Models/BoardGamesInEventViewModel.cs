using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoardGames.Models
{
    public class BoardGamesInEventViewModel
    {
        public int[] BoardGameId { get; set; }
        public MultiSelectList BoardGamesList { get; set; }
    }
}