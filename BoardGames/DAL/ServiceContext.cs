using BoardGames.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BoardGames.DAL
{
    public class ServiceContext : DbContext
    {
        public ServiceContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<BoardGame> BoardGames { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Event>().HasRequired<Player>(e => e.HostPlayer)
                .WithMany(p => p.HostedEvents).HasForeignKey(e => e.HostPlayerID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>().HasMany<Player>(e => e.ParticipantPlayers)
                .WithMany(p => p.ParticipatedEvents);

            modelBuilder.Entity<BoardGame>().HasOptional<Player>(b => b.CreatedByPlayer);

            modelBuilder.Entity<BoardGame>().HasMany<Player>(b => b.Players).WithMany(p => p.FavouriteGames);

            modelBuilder.Entity<Guild>().HasRequired<Player>(t => t.Owner).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Guild>().HasMany<Player>(t => t.Players).WithMany(p => p.Guilds);

            modelBuilder.Entity<Guild>().HasMany<Player>(t => t.Invited).WithMany(p=> p.GuildInvites);

            modelBuilder.Entity<Guild>().HasMany<Player>(t => t.JoinRequests).WithMany(p => p.GuildRequests);

            modelBuilder.Entity<Guild>().HasMany<ChatMessage>(t => t.Chat);

        }

    }
}