using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursoEntityF
{
    class ProdutoDAO : IProdutoDAO, IDisposable
    {
        private LojaContext banco;

        public ProdutoDAO()
        {
            this.banco = new LojaContext();    
        }

        public void Adicionar(Produto p)
        {
            banco.Produtos.Add(p);
        }

        public void Atualizar(Produto p)
        {
            banco.Produtos.Update(p);
        }


        public IList<Produto> ListarTodos()
        {
            return banco.Produtos.ToList();
        }

        public void remover(Produto p)
        {
            banco.Produtos.Remove(p);
        }

        public void GravarAlteracoes()
        {
            banco.SaveChanges();
        }

        public void LogSQLConsole()
        {
            banco.LogSQLToConsole();
        }

        public void Dispose()
        {
            this.banco.Dispose();
        }
    }
}
