using EFCoreAvancado.Data;
using EFCoreAvancado.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAvancado;

public class Program
{

    public static void Main()
    {
        // FiltroGlobal();
        // IgnoreFiltroGlobal();
        // ConsultaProjetada();
        // ConsultaParametrizada();
        // ConsultaInterpolada();
        // ConsultaComTag();
        // Consulta1N();
        // ConsultaN1();
        // DivisaoDeConsultas();
    }

    public static void DivisaoDeConsultas()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        var departamentos = db.Set<Departamento>()
            .Where(departamento => !departamento.Excluido)
            .Include(departamento => departamento.Funcionarios)
            //.AsSplitQuery()
            .AsSingleQuery()
            .ToList();

        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Departamento descrição: {departamento.Descricao}");

            foreach (var funcionario in departamento.Funcionarios)
            {
                Console.WriteLine($"\t Funcionario Nome: {funcionario.Nome}, Departamento: {funcionario.Departamento}");
            }
        }
    }

    public static void ConsultaN1()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        var funcionarios = db.Funcionarios
            .Include(departamento => departamento.Departamento)
            .ToList();

        foreach (var funcionario in funcionarios)
        {
            Console.WriteLine($"\t Funcionario Nome: {funcionario.Nome}, Departamento: {funcionario.Departamento.Descricao}");
        }
    }

    public static void Consulta1N()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        var departamentos = db.Set<Departamento>()
            .Where(departamento => !departamento.Excluido)
            .Include(departamento => departamento.Funcionarios)
            .ToList();

        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Departamento descrição: {departamento.Descricao}");

            foreach (var funcionario in departamento.Funcionarios)
            {
                Console.WriteLine($"\t Funcionario Nome: {funcionario.Nome}, Departamento: {funcionario.Departamento}");
            }
        }
    }

    public static void ConsultaComTag()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        var departamentos = db.Departamentos
                .TagWith(@"Estou enviando um comentario
                    /)_)/
                ")
                .ToList();


        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Departamento descrição: {departamento.Descricao}");
        }
    }

    public static void ConsultaInterpolada()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        var id = 0;

        var departamentos = db.Departamentos
                .FromSqlInterpolated($"SELECT * FROM Departamentos WITH(NOLOCK) WHERE ID > {id}")
                .Where(departamento => !departamento.Excluido)
                .ToList();


        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Departamento descrição: {departamento.Descricao}");
        }
    }

    public static void ConsultaParametrizada()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        // var id = 0;

        var id = new SqlParameter
        {
            Value = 0,
            SqlDbType = System.Data.SqlDbType.Int
        };

        var departamentos = db.Departamentos
                .FromSqlRaw("SELECT * FROM Departamentos WITH(NOLOCK) WHERE ID > {0}", id)
                .Where(departamento => !departamento.Excluido)
                .ToList();

        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Departamento descrição: {departamento.Descricao}");
        }
    }

    public static void ConsultaProjetada()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        var departamentos = db.Departamentos
            .Where(departamento => departamento.Id > 0)
            .Select(departamento =>
                 new
                 {
                     departamento.Descricao,
                     Funcionario = departamento.Funcionarios.Select(funcionario => funcionario.Nome)
                 }).ToList();


        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Departamento descrição: {departamento.Descricao}");

            foreach (var funcionario in departamento.Funcionario)
            {
                Console.WriteLine($"\t Funcionario Nome: {funcionario}");
            }
        }
    }

    public static void IgnoreFiltroGlobal()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        var departamentos = db.Departamentos.IgnoreQueryFilters().Where(departamento => departamento.Id > 0).ToList();


        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Departamento descrição: {departamento.Descricao} \t Excluido {departamento.Excluido}");
        }
    }
    public static void FiltroGlobal()
    {
        using var db = new ApplicationDbContext();

        SetUp();

        var departamentos = db.Departamentos.Where(departamento => departamento.Id > 0).ToList();


        foreach (var departamento in departamentos)
        {
            Console.WriteLine($"Departamento descrição: {departamento.Descricao} \t Excluido {departamento.Excluido}");
        }
    }
    public static void SetUp()
    {
        using var db = new ApplicationDbContext();

        if (db.Database.EnsureCreated())
        {
            db.Departamentos.AddRange(
                new Domain.Departamento()
                {
                    Ativo = true,
                    Descricao = "Departamento 01",
                    Funcionarios = new List<Domain.Funcionario>()
                    {
                            new Domain.Funcionario()
                            {
                                Nome = "Guilherme",
                                RG = "99384758",
                                CPF = "93847583902"
                            }
                    },
                    Excluido = true
                },
                new Domain.Departamento()
                {
                    Ativo = true,
                    Descricao = "Departamento 02",
                    Funcionarios = new List<Domain.Funcionario>()
                    {
                            new Domain.Funcionario()
                            {
                                Nome = "Santos",
                                RG = "90987654",
                                CPF = "23425643421"
                            },
                            new Domain.Funcionario()
                            {
                                Nome = "Pereira",
                                RG = "50487654",
                                CPF = "55525643421"
                            }
                    }
                }
            );
        }

        db.SaveChanges();
    }
}