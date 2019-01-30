using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursoEntityF
{
    public  class Produto
    {
        public int Id { get; set; }
        public double PrecoUnitario { get; set; }
        public string Categoria { get; set; }
        public string Nome { get; set; }
        public string Unidade { get; set; }
        public IList<PromocaoProduto> Promocoes { get; set; }
        public IList<Compra> Compras { get; set; }

        public override string ToString()
        {
            return "Produto: " + this.Nome;
        }
    }
    
}
