using EFCoreAvancado.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCoreAvancado
{
    public class Program
    {
        public static int _contador;
        public static void Main()
        {
            // GapDoEnsureCreated();

            // HealthCheckDatabase()

            // GerenciarEstadoDaConexao(gerenciarEstadoConexao: false);

            // _contador = 0;

            // GerenciarEstadoDaConexao(gerenciarEstadoConexao: true);

            // SqlInjection();

            // MigracaoesPendentes();

            // AplicarMigracaoEmTempoExecucao();

            // MigracoesAplicadas();

            // ScriptGeralDoBancoDeDados();

            // CarregamentoAdiantado_Eager();

            // CarregamentoExplicito_Explicitly();
            _contador = 0;
            CarregamentoLento_LazyLoad();
            // _contador = 0;
            // CarregamentoLento_LazyLoad(GereciarConexao: true);
        }

        public static void CarregamentoLento_LazyLoad(bool GereciarConexao = false)
        {
            using var db = new ApplicationDbContext();

            var connection = db.Database.GetDbConnection();

            SetUpTiposCarregamentos(db);

            if (GereciarConexao)
            {
                connection.Open();
            }

            // db.ChangeTracker.LazyLoadingEnabled = false;

            var dapartamentos = db
                .Departamentos.ToList();


            connection.StateChange += (_, __) => ++_contador;

            var time = System.Diagnostics.Stopwatch.StartNew();

            foreach (var departamento in dapartamentos)
            {

                Console.WriteLine($"-------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"-------------------------------------");
                        Console.WriteLine($"Funcionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum Funcionario");
                }
            }

            time.Stop();

            if (GereciarConexao)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"Tempo :{time.Elapsed.ToString()}, Contador: {_contador}");
            }
        }

        public static void CarregamentoExplicito_Explicitly()
        {
            using var db = new ApplicationDbContext();
            SetUpTiposCarregamentos(db);

            var dapartamentos = db
                .Departamentos.ToList();

            foreach (var departamento in dapartamentos)
            {
                if (departamento.Id == 2)
                {
                    // db.Entry(departamento).Collection(d => d.Funcionarios).Load();
                    db.Entry(departamento)
                         .Collection(d => d.Funcionarios)
                         .Query()
                         .Where(f => f.Id > 2)
                         .ToList();
                }

                Console.WriteLine($"-------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"-------------------------------------");
                        Console.WriteLine($"Funcionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum Funcionario");
                }
            }
        }
        public static void CarregamentoAdiantado_Eager()
        {
            using var db = new ApplicationDbContext();
            SetUpTiposCarregamentos(db);

            var dapartamentos = db
                .Departamentos
                .Include(deparamento => deparamento.Funcionarios);

            foreach (var departamento in dapartamentos)
            {
                Console.WriteLine($"-------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios.Any())
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"-------------------------------------");
                        Console.WriteLine($"Funcionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum Funcionario");
                }
            }
        }
        public static void SetUpTiposCarregamentos(ApplicationDbContext db)
        {
            if (!db.Departamentos.Any())
            {
                db.Departamentos.AddRange(
                    new Domain.Departamento()
                    {
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Domain.Funcionario>()
                        {
                            new Domain.Funcionario()
                            {
                                Nome = "Guilherme",
                                RG = "99384758",
                                CPF = "93847583902"
                            }
                        }
                    },
                    new Domain.Departamento()
                    {
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

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
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