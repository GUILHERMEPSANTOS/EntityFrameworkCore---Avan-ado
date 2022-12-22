using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCoreAvancado.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        // public virtual IEnumerable<Funcionario> Funcionarios { get; set; }
        private Action<object, string> _lazyLoader { get; set; }
        public Departamento()
        {

        }
        private Departamento(Action<object, string> lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private IEnumerable<Funcionario> _funcionarios;

        public IEnumerable<Funcionario> Funcionarios
        {
            get {
                _lazyLoader?.Invoke(this, nameof(Funcionarios));

                return _funcionarios;
            }

            set => _funcionarios = value;
        }


        /*  private ILazyLoader _lazyLoader { get; set; }
         private IEnumerable<Funcionario> _funcionarios;

         public IEnumerable<Funcionario> Funcionarios
         {
             get => _lazyLoader.Load(this, ref _funcionarios);
             set => _funcionarios = value;
         }
         private Departamento(ILazyLoader lazyLoader)
         {
             _lazyLoader = lazyLoader;
         } */
    }
}