using EFCoreAvancado.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAvancado
{
    public class Program
    {
        public static void Main()
        {
            // CriarStoredProcedure();
            // InserirDadosViaProcedure();
            // CriarStoredProcedureDeConsulta();
            ConsultaViaProcedure();
        }

        public static void ConsultaViaProcedure()
        {
            {

                var db = new ApplicationDbContext();

                var dep = new SqlParameter("@dep", "Departamento");

                var departamentos = db.Departamentos
                    .FromSqlRaw("EXEC GetDepartamento @dep", dep)
                    .ToList();

                foreach (var departamento in departamentos)
                {
                    Console.WriteLine($"Deparatamento Descrição: {departamento.Descricao}");
                }
            }
        }
        public static void CriarStoredProcedureDeConsulta()
        {

            var db = new ApplicationDbContext();

            var scritpStoredProcedure = @"
                CREATE OR ALTER PROCEDURE GetDepartamento @Descricao VARCHAR(50)
                AS
                BEGIN
                    SELECT *
                    FROM Departamentos 
                    WHERE Descricao Like @Descricao + '%'
                END
            ";

            db.Database.ExecuteSqlRaw(scritpStoredProcedure);
        }

        public static void InserirDadosViaProcedure()
        {
            var db = new ApplicationDbContext();

            db.Database.ExecuteSqlRaw("EXEC CriarDepartamento @p0, @p1", "Departamento Via Proc", true);
        }

        public static void CriarStoredProcedure()
        {

            var db = new ApplicationDbContext();

            var scritpStoredProcedure = @"
                CREATE OR ALTER PROCEDURE CriarDepartamento @Descricao VARCHAR(50)
                                                           ,@Ativo     BIT
                AS
                BEGIN
                    INSERT INTO
                         Departamentos(Descricao, Ativo, Excluido)
                    VALUES (@Descricao,@Ativo, 0);
                END
            ";

            db.Database.ExecuteSqlRaw(scritpStoredProcedure);

        }
    }
}