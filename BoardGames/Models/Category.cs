using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoardGames.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Pole musi znajdować się w przedziale 2-50 znaków.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set;  }

        public virtual List<BoardGame> BoardGames { get; set; }

    }
}