using ExampleTest2.Models;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.Data;

public class DatabaseContext : DbContext
{
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<CharacterTitle> CharacterTitles { get; set; }
    public DbSet<Title> Titles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>().HasData(new List<Item>
        {
            new Item
            {
                Id = 1,
                Name = "Potion",
                Weight = 5
            },
            new Item
            {
                Id = 2,
                Name = "Sword",
                Weight = 20
            }
        });

        modelBuilder.Entity<Title>().HasData(new List<Title>
        {
            new Title
            {
                Id = 1,
                Name = "The Brave"
            },
            new Title
            {
                Id = 2,
                Name = "The Bold"
            }
        });

        modelBuilder.Entity<Character>().HasData(new List<Character>
        {
            new Character()
            {
                Id = 1,
                FirstName = "Matthew",
                LastName = "Patrick",
                CurrentWeight = 25,
                MaxWeight = 60
            },
            new Character()
            {
                Id = 2,
                FirstName = "Gregory",
                LastName = "Brown",
                CurrentWeight = 5,
                MaxWeight = 40
            },
            new Character()
            {
                Id = 3,
                FirstName = "George",
                LastName = "Fehloy'd",
                CurrentWeight = 10,
                MaxWeight = 70
            },
        });

        modelBuilder.Entity<CharacterTitle>().HasData(new List<CharacterTitle>
        {
            new CharacterTitle()
            {
                CharacterId = 1,
                TitleId = 1,
                AcquiredAt = new DateTime(2024, 05, 12)
            },
            new CharacterTitle()
            {
                CharacterId = 1,
                TitleId = 2,
                AcquiredAt = new DateTime(2024, 06, 12)
            },
            new CharacterTitle()
            {
                CharacterId = 2,
                TitleId = 2,
                AcquiredAt = new DateTime(2023, 06, 07)
            }
        });

        modelBuilder.Entity<Backpack>().HasData(new List<Backpack>
        {
            new Backpack()
            {
                CharacterId = 1,
                ItemId = 1,
                Amount = 1
            },
            new Backpack()
            {
                CharacterId = 1,
                ItemId = 2,
                Amount = 1
            },
            new Backpack()
            {
                CharacterId = 2,
                ItemId = 1,
                Amount = 5
            },
            new Backpack()
            {
                CharacterId = 3,
                ItemId = 1,
                Amount = 2
            }
        });
    }
}