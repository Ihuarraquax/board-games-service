using BoardGames.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace BoardGames.DAL
{
    public class ServiceInit : DropCreateDatabaseAlways<ServiceContext>
    {
        protected override void Seed(ServiceContext context)
        {
            var categories = new List<Category>
            {
            new Category { Name = "Strategiczna" },
            new Category { Name = "Euro" },
            new Category { Name = "Dedukcyjna" },
            new Category { Name = "Rodzinna" }
            };

            categories.ForEach(c => context.Categories.Add(c));

            var boardGames = new List<BoardGame>
            {
                new BoardGame {Name = "Scythe",
                    Categories = new List<Category>{categories[1], categories[0] },
                    Description="Mechy i Europa",
                    MinPlayers=1,
                    MaxPlayers=5,
                    Website="https://www.rebel.pl/product.php/1,303/103303/Scythe-edycja-polska.html"
                },
                new BoardGame {Name = "Avalon",
                    Categories = new List<Category>{categories[2]},
                    Description="Gra dedukcyjna w swiecie legend arturiańskich",
                    MinPlayers=5,
                    MaxPlayers=10,
                    Website="https://www.rebel.pl/product.php/1,1203/26217/Avalon-Rycerze-Krola-Artura.html"
                },
                new BoardGame {Name = "Neuroshima Hex",
                    Categories = new List<Category>{categories[0]},
                    Description="Bitewniak",
                    MinPlayers=2,
                    MaxPlayers=6,
                    Website="https://www.rebel.pl/product.php/1,302/27758/Neuroshima-HEX-edycja-3.0.html"
                },
                new BoardGame {Name = "Splendor",
                    Categories = new List<Category>{categories[3]},
                    Description="Kolekcjonowanie nigdy nie było tak uzależniające!",
                    MinPlayers=2,
                    MaxPlayers=4,
                    Website="https://www.rebel.pl/product.php/1,303/28615/Splendor.html"
                },
                new BoardGame {Name = "Azul",
                    Categories = new List<Category>{categories[3]},
                    Description="Francuska Gra Roku 2018",
                    MinPlayers=2,
                    MaxPlayers=4,
                    Website="https://www.rebel.pl/product.php/1,606/107324/Azul-edycja-polska.html"
                },
                new BoardGame {Name = "Terraformacja Marsa",
                    Categories = new List<Category>{categories[0]},
                    Description="Zmień Marsa w planetę zdatną do życia!",
                    MinPlayers=2,
                    MaxPlayers=6,
                    Website="https://www.rebel.pl/product.php/1,302/27758/Neuroshima-HEX-edycja-3.0.html"
                },
                new BoardGame {Name = "7 Cudów Świata",
                    Categories = new List<Category>{categories[1]},
                    Description="Pokaż, jakie cuda może zbudować Twoja cywilizacja!",
                    MinPlayers=2,
                    MaxPlayers=7,
                    Website="https://www.rebel.pl/product.php/1,302/19537/7-Cudow-Swiata.html"
                },
                new BoardGame {Name = "Colt Express",
                    Categories = new List<Category>{categories[1]},
                    Description="Zabawna i ekscytująca gra w napad na pociąg!",
                    MinPlayers=2,
                    MaxPlayers=6,
                    Website="https://www.rebel.pl/product.php/1,302/27758/Neuroshima-HEX-edycja-3.0.html"
                },
                new BoardGame {Name = "7 Cudów Świata: Pojedynek",
                    Categories = new List<Category>{categories[0]},
                    Description="Świetna, dwuosobowa wersja kultowej gry",
                    MinPlayers=2,
                    MaxPlayers=2,
                    Website="https://www.rebel.pl/product.php/1,302/27758/Neuroshima-HEX-edycja-3.0.html"
                },
                new BoardGame {Name = "Resistance",
                    Categories = new List<Category>{categories[2]},
                    Description="Czy zdołasz domyślić się, kto z zebranych jest Agentem?",
                    MinPlayers=5,
                    MaxPlayers=10,
                    Website="https://www.rebel.pl/product.php/1,1203/97141/The-Resistance-edycja-polska.html"}
            };
            boardGames.ForEach(bg => context.BoardGames.Add(bg));
            context.SaveChanges();

            var players = new List<Player>
            {
                new Player {Email = "zablo432432@o2.pl", 
                    FirstName = "Hubert", 
                    LastName = "Zabłocki",
                    FavouriteGames = new List<BoardGame> { boardGames[1], boardGames[2] }},
                new Player {Email = "Borys@o2.pl",
                    FirstName = "Borys",
                    LastName = "Nowak",
                    FavouriteGames = new List<BoardGame> { boardGames[0], boardGames[2] }},
                new Player {Email = "Boromir@o2.pl",
                    FirstName = "Boromir",
                    LastName = "Kowalski",
                    FavouriteGames = new List<BoardGame>{ boardGames[1], boardGames[3] }}
            };

            players.ForEach(p => context.Players.Add(p));
            context.SaveChanges();


            var events = new List<Event> {
                new Event {Name = "Turniej molocha",
                    BoardGames = new List<BoardGame>{ boardGames[3] },
                    HostPlayerID = players[0].ID,
                    Place="131 UPH",
                    Date= new DateTime(2020,5,25,18,0,0),
                    ParticipantPlayers = new List<Player> {players[0],players[2] },
                },
                new Event {Name = "Luźne granie",
                    BoardGames = new List<BoardGame>{ boardGames[1],boardGames[2] },
                    HostPlayerID = players[2].ID,
                    Place="Białki",
                    Date= new DateTime(2020,5,26,19,0,0),
                    ParticipantPlayers = new List<Player> {players[1],players[0] }}
            };

            events.ForEach(e => context.Events.Add(e));
            context.SaveChanges();


            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("User"));

            var users = players.Select(p => new ApplicationUser { UserName = p.Email }).ToList();

            users.ForEach(u => { 
                userManager.Create(u, "Complex.Password.123");
                userManager.AddToRole(u.Id,"User");    
            });

            DbSet<BoardGame> boardGames1 = context.BoardGames;
        }
    }
}