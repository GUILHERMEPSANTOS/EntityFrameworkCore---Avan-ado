using EFCoreAvancado.Data;
using EFCoreAvancado.Domain;

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