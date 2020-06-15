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
                    Website="https://www.rebel.pl/product.php/1,303/103303/Scythe-edycja-polska.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/5/0/501d5ed7526903d1bfe5251236205906.651450.800x0.jpg"
                },
                new BoardGame {Name = "Avalon",
                    Categories = new List<Category>{categories[2]},
                    Description="Gra dedukcyjna w swiecie legend arturiańskich",
                    MinPlayers=5,
                    MaxPlayers=10,
                    Website="https://www.rebel.pl/product.php/1,1203/26217/Avalon-Rycerze-Krola-Artura.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/a/v/avalonnn.392016.800x0.png"
                },
                new BoardGame {Name = "Neuroshima Hex",
                    Categories = new List<Category>{categories[0]},
                    Description="Bitewniak",
                    MinPlayers=2,
                    MaxPlayers=6,
                    Website="https://www.rebel.pl/product.php/1,302/27758/Neuroshima-HEX-edycja-3.0.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/n/s/nsh3plbox3d.438159.800x0.jpg"
                },
                new BoardGame {Name = "Splendor",
                    Categories = new List<Category>{categories[3]},
                    Description="Kolekcjonowanie nigdy nie było tak uzależniające!",
                    MinPlayers=2,
                    MaxPlayers=4,
                    Website="https://www.rebel.pl/product.php/1,303/28615/Splendor.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/s/p/splendor_new_3d.202732.800x0.jpg"
                },
                new BoardGame {Name = "Azul",
                    Categories = new List<Category>{categories[3]},
                    Description="Francuska Gra Roku 2018",
                    MinPlayers=2,
                    MaxPlayers=4,
                    Website="https://www.rebel.pl/product.php/1,606/107324/Azul-edycja-polska.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/a/z/azul-pudelko-3d.615464.800x0.jpg"
                },
                new BoardGame {Name = "Terraformacja Marsa",
                    Categories = new List<Category>{categories[0]},
                    Description="Zmień Marsa w planetę zdatną do życia!",
                    MinPlayers=2,
                    MaxPlayers=6,
                    Website="https://www.rebel.pl/product.php/1,302/27758/Neuroshima-HEX-edycja-3.0.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/b/o/box_3d_TerraformacjaMarsa_podstawka.3011438.800x0.jpg"
                },
                new BoardGame {Name = "7 Cudów Świata",
                    Categories = new List<Category>{categories[1]},
                    Description="Pokaż, jakie cuda może zbudować Twoja cywilizacja!",
                    MinPlayers=2,
                    MaxPlayers=7,
                    Website="https://www.rebel.pl/product.php/1,302/19537/7-Cudow-Swiata.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/7/_/7_cudow_swiata_cover.324035.800x0.jpg"
                },
                new BoardGame {Name = "Colt Express",
                    Categories = new List<Category>{categories[1]},
                    Description="Zabawna i ekscytująca gra w napad na pociąg!",
                    MinPlayers=2,
                    MaxPlayers=6,
                    Website="https://www.rebel.pl/product.php/1,302/27758/Neuroshima-HEX-edycja-3.0.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/c/o/colt_3d.415327.800x0.jpg"
                },
                new BoardGame {Name = "7 Cudów Świata: Pojedynek",
                    Categories = new List<Category>{categories[0]},
                    Description="Świetna, dwuosobowa wersja kultowej gry",
                    MinPlayers=2,
                    MaxPlayers=2,
                    Website="https://www.rebel.pl/product.php/1,302/27758/Neuroshima-HEX-edycja-3.0.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/p/o/pojedynek_3d.365162.800x0.jpg"
                },
                new BoardGame {Name = "Resistance",
                    Categories = new List<Category>{categories[2]},
                    Description="Czy zdołasz domyślić się, kto z zebranych jest Agentem?",
                    MinPlayers=5,
                    MaxPlayers=10,
                    Website="https://www.rebel.pl/product.php/1,1203/97141/The-Resistance-edycja-polska.html",
                    ImageUrl="https://www.rebel.pl/repository/thumbnails/3/d/3d-resistance.570087.800x0.png"}
            };
            boardGames.ForEach(bg => context.BoardGames.Add(bg));
            context.SaveChanges();

            Player adminPlayer = new Player
            {
                Email = "hzablocki97@gmail.com",
                FirstName = "Hubert",
                LastName = "Zabłocki"
            };
            Player userPlayer = new Player
            {
                Email = "zablo432432@o2.pl",
                FirstName = "HubiUser",
                LastName = "ZabiUser"
            };
            context.Players.Add(adminPlayer);
            context.Players.Add(userPlayer);
            var players = new List<Player>
            {
                new Player {Email = "zablo432432@gmail.pl", 
                    FirstName = "Józef", 
                    LastName = "Bytner",
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

            var guilds = new List<Guild>
            {
                new Guild
                {
                    Name="Molochy",
                    Descripton="Grupa znajomych ze studiów, gramy sobie w tygodniu w molocha na naszym uniwersytecie",
                    Players = new HashSet<Player> { players[0], players[1]},
                    ImageUrl = "https://i1.wp.com/www.boardgameresource.com/wp-content/uploads/2016/05/gaston.png",
                    Owner = adminPlayer,
                    JoinRequests = new HashSet<Player>(){ userPlayer}
                },
                new Guild
                {
                    Name="Posterunki",
                    Descripton="Gramy sobie w różne gry, od Euro po Strategiczne. Zbieramy się w soboty w sali 27 na UPH",
                    Players = new HashSet<Player> { players[2], adminPlayer},
                    ImageUrl = "https://s.hdnux.com/photos/54/41/33/11671278/5/920x920.jpg",
                    Owner = players[0]
                },
                new Guild
                {
                    Name="Plebskie granie",
                    Descripton="Tylko casual gierki nic trudnego, max 10 min na gre",
                    Players = new HashSet<Player> { players[1] },
                    ImageUrl = "https://pyxis.nymag.com/v1/imgs/2c7/0cc/7e0a0ec0b953d33edbc370f56494231f4d-07-four-player-board-game-lede.rsquare.w700.jpg",
                    Owner = players[0]
                }
            };
            guilds.ForEach(g => context.Guilds.Add(g));
            context.SaveChanges();

            var chatMessages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Author = players[0], Content="elo elo testowy komentarz giedy gramy w lomocha", Guild = guilds[0], Date = DateTime.Now.AddMinutes(-30)
                },
                new ChatMessage
                {
                    Author = adminPlayer, Content="Nowy komentarz admina", Guild = guilds[0], Date = DateTime.Now.AddMinutes(-80)
                },
            };
            chatMessages.ForEach(cm => context.ChatMessages.Add(cm));
            context.SaveChanges();
            var events = new List<Event> {
                new Event {Name = "Turniej molocha",
                    BoardGames = new List<BoardGame>{ boardGames[3] },
                    Description = "Międzygalaktyczny turniej molocha, będą mistrzowie nie tylko z całego świata ale również z Polski!",
                    HostPlayerID = adminPlayer.ID,
                    Place="131 UPH",
                    Date= DateTime.Now.AddDays(7),
                    ParticipantPlayers = new List<Player> {players[0],players[2] },
                },
                new Event {Name = "Luźne granie",
                    BoardGames = boardGames.Select(bg => bg).ToList(),
                    HostPlayerID = players[2].ID,
                    Description = "Mamy wszystkie gry jakie są i jest super, każdy znajdzie coś dla siebie",
                    Place="Białki",
                    Date= DateTime.Now.AddDays(-7),
                    ParticipantPlayers = new List<Player> {players[1],players[0] }},
                new Event {Name = "Wielkie granie",
                    BoardGames = boardGames.Select(bg => bg).ToList(),
                    HostPlayerID = players[1].ID,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis pulvinar, purus in sodales consequat, urna quam maximus quam, at condimentum metus erat ornare dolor. Maecenas a quam dictum, laoreet felis vitae, tempor risus. Nullam ex nisi, tincidunt non augue vitae, dapibus eleifend augue. Fusce venenatis odio in leo egestas maximus. Nullam consequat ante quis interdum ultricies. Nullam sed odio urna. Nunc ornare urna eget ante vulputate condimentum. Etiam dignissim fermentum elit a blandit. Phasellus id augue lorem. Mauris maximus metus sed aliquet dignissim. Maecenas vehicula ante at tincidunt malesuada. Pellentesque et hendrerit neque. Donec libero turpis, dictum eu finibus vitae, euismod sed quam. Fusce consequat erat non pellentesque lobortis. Vestibulum eget justo mollis, sagittis neque nec, suscipit urna.",
                    Place="UPH",
                    Date= DateTime.Now.AddDays(3),
                    ParticipantPlayers = players.Select(p=>p).ToList()}
            };

            events.ForEach(e => context.Events.Add(e));
            context.SaveChanges();


            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Player"));
            roleManager.Create(new IdentityRole("Admin"));

            var users = players.Select(p => new ApplicationUser { UserName = p.Email }).ToList();

            users.ForEach(u => { 
                userManager.Create(u, "Complex.Password.123");
                userManager.AddToRole(u.Id,"Player");    
            });

            var admin = new ApplicationUser { UserName = adminPlayer.Email };
            var user = new ApplicationUser { UserName = userPlayer.Email };

            userManager.Create(admin, "Admin@123");
            userManager.AddToRole(admin.Id, "Admin");
            userManager.AddToRole(admin.Id, "Player");

            userManager.Create(user, "User@123");
            userManager.AddToRole(user.Id, "Player");
            

        }
    }
}