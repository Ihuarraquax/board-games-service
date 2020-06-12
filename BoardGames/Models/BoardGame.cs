using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGames.Models
{
    public class BoardGame
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole musi znajdować się w przedziale 2-50 znaków.")]
        [Display(Name = "Tytuł")]
        public string Name { get; set; }
        [Range(1, 100)]
        [Display(Name = "Minimalna liczba graczy")]
        public int MinPlayers { get; set; }
        [Range(1, 100)]
        [Display(Name = "Maksymalna liczba graczy")]
        public int MaxPlayers { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Strona")]
        public string Website { get; set; }
        [Display(Name = "Grafika")]
        public string ImageUrl { get; set; }


        [Display(Name = "Dodane przez gracza")]
        public virtual Player CreatedByPlayer { get; set; }
        public virtual List<Event> Events { get; set; }
        [Display(Name = "Kategorie")]
        public virtual List<Category> Categories { get; set; }

        public virtual List<Player> Players { get; set; }
    }
}