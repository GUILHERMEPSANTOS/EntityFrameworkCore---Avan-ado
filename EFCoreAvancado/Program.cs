using EFCoreAvancado.Data;
using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCoreAvancado;
public class Program
{

    public static void Main()
    {
        ConsultarDepartamentos();
    }

    static void ConsultarDepartamentos()
    {
        using var db = new ApplicationDbContext();


        var departamentos = db.Set<Departamento>().Where(departamento => departamento.Id >0).ToArray();
    }

}