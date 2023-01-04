using EFCoreAvancado.Data;
using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAvancado;
public class Program
{

    static int _contador;
    public static void Main()
    {

        // ConsultarDepartamentos();
        // HabilitandoBatchSize();
        TempoComandoGeral();
    }

    static void TempoComandoGeral()
    {
        using var db = new ApplicationDbContext();

        db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'; SELECT 1");

    }

    static void HabilitandoBatchSize()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Database.GetDbConnection().StateChange += (_, __) => ++_contador;

        for (int i = 0; i < 50; i++)
        {
            db.Departamentos.Add(
                new Departamento
                {
                    Descricao = "Departamneto"
                }
            );
        }

        db.SaveChanges();

        Console.WriteLine($"Contador {_contador}");
    }
    static void ConsultarDepartamentos()
    {
        using var db = new ApplicationDbContext();

        var departamentos = db.Set<Departamento>().Where(departamento => departamento.Id > 0).ToArray();

    }
}