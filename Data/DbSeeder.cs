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
            var email = $"{Slug(first)}.{Slug(last)}@example.com";

            people.Add(new Person { FirstName = first, LastName = last, Phone = phone, Email = email });
        }

        db.People.AddRange(people);
        db.SaveChanges();
    }

    // Převede jméno na podobu vhodnou do e-mailu: bez diakritiky, malými písmeny.
    private static string Slug(string value)
    {
        var normalized = value.Normalize(System.Text.NormalizationForm.FormD);
        var sb = new System.Text.StringBuilder();
        foreach (var ch in normalized)
        {
            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch)
                != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }
        return sb.ToString().Normalize(System.Text.NormalizationForm.FormC).ToLowerInvariant();
    }
}
