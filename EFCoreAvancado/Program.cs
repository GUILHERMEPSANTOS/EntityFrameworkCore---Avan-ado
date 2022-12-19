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
            // HealthCheckDatabase()
            // _contador = 0;
            // GerenciarEstadoDaConexao(gerenciarEstadoConexao: false);
            // _contador = 0;
            // GerenciarEstadoDaConexao(gerenciarEstadoConexao: true);
            // SqlInjection();
            // MigracaoesPendentes();
            // AplicarMigracaoEmTempoExecucao();
            // MigracoesAplicadas();
            ScriptGeralDoBancoDeDados();
        }
        static int _contador;

        public static void ScriptGeralDoBancoDeDados()
        {

            using var db = new ApplicationDbContext();

            var script = db.Database.GenerateCreateScript();

            Console.WriteLine(script);

        }
        public static void MigracoesAplicadas()
        {
            using var db = new ApplicationDbContext();

            var migracoes = db.Database.GetAppliedMigrations();

            Console.WriteLine($"Migrações = {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migracao: {migracao}");
            }
        }

        public static void TodasMigracoes()
        {
            using var db = new ApplicationDbContext();

            var migracoes = db.Database.GetMigrations();

            Console.WriteLine($"Migrações = {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migracao: {migracao}");
            }
        }

        public static void AplicarMigracaoEmTempoExecucao()
        {
            using var db = new ApplicationDbContext();

            db.Database.Migrate();
        }

        public static void MigracaoesPendentes()
        {
            using var db = new ApplicationDbContext();

            IEnumerable<string> migracaoesPendentes = db.Database.GetPendingMigrations();

            Console.WriteLine($"Migrações = {migracaoesPendentes.Count()}");

            foreach (var migracao in migracaoesPendentes)
            {
                Console.WriteLine($"Migracao: {migracao}");
            }

        }

        // nunca aceite concatenação sempre argumentos no metodo somente
        public static void SqlInjection()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Domain.Departamento
                {
                    Descricao = "Departamento  2"
                },
                new Domain.Departamento
                {
                    Descricao = "Departamento 3"
                }
            );

            db.SaveChanges();


            var descricao = "Teste' or 1='1";                                                                         // 'Teste' or 1='1'
            db.Database.ExecuteSqlRaw($"update Departamentos set descricao='Depaartamento Alterado' where descricao = '{descricao}'");

            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                Console.WriteLine($"Id: {departamento.Id}, Descricao: {departamento.Descricao}");
            }
        }

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