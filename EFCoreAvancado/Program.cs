using EFCoreAvancado.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAvancado;
public class Program
{
    public static void Main()
    {
        Esquema();
    }

    public static void Collations()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
    public static void PropagacaoDados()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();


        var script = db.Database.GenerateCreateScript();

        System.Console.WriteLine(script);
    }
    public static void Esquema()
    {
        using var db = new ApplicationDbContext();

        var script = db.Database.GenerateCreateScript();

        System.Console.WriteLine(script);
    }

}