using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BoardGames.Models
{
    public class Event
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole musi znajdować się w przedziale 2-50 znaków.")]
        [Display(Name = "Nazwa wydarzenia")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        public int HostPlayerID { get; set; }
        [Required]
        [Display(Name = "Miejsce")]
        public string Place { get; set; }
        [Display(Name = "Data startu")]
        [Required]
        [DataType(DataType.Date, ErrorMessage ="Podaj właściwą date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Organizator")]
        public virtual Player HostPlayer { get; set; }
        [Display(Name = "Gry")]
        public virtual List<BoardGame> BoardGames { get; set; }
        [Display(Name = "Uczestnicy")]
        public virtual List<Player> ParticipantPlayers { get; set; }
        
    }
}