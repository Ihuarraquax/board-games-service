using BoardGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoardGames.ViewModels
{
    public class BoardGamePlayersGroup
    {
        public BoardGame BoardGame { get; set; }
        public int Players { get; set; }
    }
}