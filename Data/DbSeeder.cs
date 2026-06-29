using Microsoft.EntityFrameworkCore;
using PersonApp.Models;

namespace PersonApp.Data;

public static class DbSeeder
{
    private static readonly string[] FirstNames =
        { "Jan", "Petr", "Eva", "Lucie", "Tomáš", "Martina", "Jakub", "Tereza", "Pavel", "Hana" };

    private static readonly string[] LastNames =
        { "Novák", "Svoboda", "Dvořák", "Černá", "Procházková", "Kučera", "Veselý", "Horáková", "Marek", "Pokorná" };

    public static void Seed(AppDbContext db)
    {
        // Zajistí, že databáze i tabulky existují (funguje pro SQLite i MySQL bez migrací).
        db.Database.EnsureCreated();

        if (db.People.Any())
        {
            return;
        }

        // Deterministický seed, ať jsou data při každém spuštění stejná.
        var random = new Random(42);
        var people = new List<Person>();

        for (int i = 0; i < 5; i++)
        {
            var first = FirstNames[random.Next(FirstNames.Length)];
            var last = LastNames[random.Next(LastNames.Length)];
            var phone = $"+420 {random.Next(600, 800)} {random.Next(100, 999)} {random.Next(100, 999)}";

            people.Add(new Person { FirstName = first, LastName = last, Phone = phone });
        }

        db.People.AddRange(people);
        db.SaveChanges();
    }
}
