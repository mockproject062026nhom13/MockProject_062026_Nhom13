namespace CRUDAccountDemo.Data.Seeders;

using CRUDAccountDemo.Business.Entities;
using CRUDAccountDemo.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await context.Accounts.AnyAsync())
            return;

        var accounts = new List<Account>
        {
            // High balance — test large deposits/withdrawals
            new("Alice Johnson",      "alice.johnson@mail.com",      125_000.00m),
            new("Grace Wilson",       "grace.wilson@mail.com",       200_000.00m),
            new("Victoria Walker",    "victoria.walker@mail.com",    180_000.00m),
            new("Iris Gonzalez",      "iris.gonzalez@mail.com",      100_000.00m),
            new("Liam Jackson",       "liam.jackson@mail.com",        90_000.00m),
            new("Amy King",           "amy.king@mail.com",            65_000.00m),
            new("Quinn Robinson",     "quinn.robinson@mail.com",      55_000.00m),
            new("Emily Davis",        "emily.davis@mail.com",         75_000.00m),

            // Medium balance — normal operations
            new("Catherine Lopez",    "catherine.lopez@mail.com",     40_000.00m),
            new("Noah Martin",        "noah.martin@mail.com",         30_500.00m),
            new("Frederick Green",    "frederick.green@mail.com",     28_000.00m),
            new("Xena Allen",         "xena.allen@mail.com",          22_000.00m),
            new("Isabella Taylor",    "isabella.taylor@mail.com",     45_000.00m),
            new("James Anderson",     "james.anderson@mail.com",      18_300.00m),
            new("Tina Lewis",         "tina.lewis@mail.com",          15_000.00m),
            new("Olivia Garcia",      "olivia.garcia@mail.com",       10_000.00m),
            new("Zachary Hernandez",  "zachary.hernandez@mail.com",    9_100.00m),
            new("Frank Miller",       "frank.miller@mail.com",         8_200.00m),
            new("Rachel Clark",       "rachel.clark@mail.com",         7_800.00m),
            new("Gloria Adams",       "gloria.adams@mail.com",         5_500.00m),

            // Low balance — test withdrawal down to near zero
            new("Carol White",        "carol.white@mail.com",         12_500.00m),
            new("David Brown",        "david.brown@mail.com",          3_750.00m),
            new("William Hall",       "william.hall@mail.com",         4_600.00m),
            new("Samuel Rodriguez",   "samuel.rodriguez@mail.com",     2_400.00m),
            new("Brian Wright",       "brian.wright@mail.com",         1_750.00m),
            new("Mia Harris",         "mia.harris@mail.com",           1_000.00m),
            new("Ulysses Lee",        "ulysses.lee@mail.com",            750.00m),
            new("Bob Smith",          "bob.smith@mail.com",            50_000.00m),
            new("Yasmine Young",      "yasmine.young@mail.com",          350.00m),
            new("Karen Thomas",       "karen.thomas@mail.com",           250.00m),
            new("Patrick Martinez",   "patrick.martinez@mail.com",       100.00m),
            new("Henry Moore",        "henry.moore@mail.com",            500.00m),

            // Zero balance — test insufficient balance error on Withdraw
            new("Daniel Hill",        "daniel.hill@mail.com",             0.00m),
            new("Eleanor Scott",      "eleanor.scott@mail.com",           0.00m),
            new("Harold Baker",       "harold.baker@mail.com",            0.00m),
        };

        await context.Accounts.AddRangeAsync(accounts);
        await context.SaveChangesAsync();
    }
}
