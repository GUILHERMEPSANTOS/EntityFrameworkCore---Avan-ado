using EFCoreAvancado.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCoreAvancado
{
    public class Program
    {
        public static void Main()
        {
            // GapDoEnsureCreated();
            // HealthCheckDatabase();
            // _contador = 0;
            // GerenciarEstadoDaConexao(gerenciarEstadoConexao: false);
            // _contador = 0;
            // GerenciarEstadoDaConexao(gerenciarEstadoConexao: true);
        }

        static int _contador;



        public static void ExecuteSql()
        {
            using var db = new ApplicationDbContext();

            // Primeira opção
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }
            // segunda opcao - transforma os argumentos em dbParam
            var descricao = "Teste";
            db.Database.ExecuteSqlRaw("update departamentos set descricao={0} where id=1", descricao);

            //terceira opção 
            db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={descricao} where id=1");
        }

        // gerenciamos manualmente a conexão com banco de dados para garantir performance
        public static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao = false)
        {

            using var db = new ApplicationDbContext();
            var time = System.Diagnostics.Stopwatch.StartNew();

            var connection = db.Database.GetDbConnection();

            connection.StateChange += (_, __) => ++_contador;

            if (gerenciarEstadoConexao)
            {
                connection.Open();
            }

            for (int i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            time.Stop();

            string message = $"Tempo: {time.Elapsed.ToString()}, gerenciarEstadoConexao: {gerenciarEstadoConexao}, Contador: {_contador}";

            Console.WriteLine(message);
        }

        // verificamos se podemos conectar
        public static void HealthCheckDatabase()
        {
            using var db = new ApplicationDbContext();

            var canConnect = db.Database.CanConnect();

            if (canConnect)
            {
                Console.WriteLine("Pode conectar");
            }
            else
            {
                Console.WriteLine("Não pode conectar");
            }
        }

        // criar e deletar um banco de dados sem utilizar migrations
        public static void EnsureCreatedAndDeleted()
        {
            using var db = new ApplicationDbContext();

            // db.Database.EnsureCreated();
            // db.Database.EnsureDeleted();
        }

        // solução do problema de utilizar 2 contextos com EnsureCreated()
        public static void GapDoEnsureCreated()
        {
            using var db1 = new ApplicationDbContext();
            using var db2 = new ApplicationDbContextCidade();

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();


            // Forçamos a criação das tabelas do segundo contexto do banco de dados 
            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();


            databaseCreator.CreateTables();
        }
    }
}