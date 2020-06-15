using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGames.Models
{
    public class Player
    {
        public int ID { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage ="Proszę podać właściwy format email")]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole musi znajdować się w przedziale 2-50 znaków.")]
        [Display(Name = "Imie")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole musi znajdować się w przedziale 2-50 znaków.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Ulubione gry")]

        public string Avatar { get; set; }

        public virtual List<BoardGame> FavouriteGames { get; set; }
        [Display(Name = "Organizowane wydarzenia")]
        public virtual List<Event> HostedEvents { get; set; }
        [Display(Name = "Uczestnik wydarzeń")]
        public virtual List<Event> ParticipatedEvents { get; set; }
        [Display(Name = "Drużyny")]
        public virtual List<Guild>  Guilds { get; set; }
        public virtual List<Guild> GuildRequests { get; set; }
        public virtual List<Guild> GuildInvites { get; set; }


        public String NameAndEmail { get { return ToString(); } }
        public String Name { get { return FirstName+" "+ LastName; } }
        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Email})";
        }
    }
}