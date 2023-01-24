using EFCoreAvancado.Data;
using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAvancado;
public class Program
{
    public static void Main()
    {
        Collations();
    }

    public static void Collations()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

}