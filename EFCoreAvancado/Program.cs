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
            CriarStoredProcedure();
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