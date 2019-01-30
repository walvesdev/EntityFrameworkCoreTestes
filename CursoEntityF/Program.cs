using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CursoEntityF
{
    class Program
    {
        static void Main(string[] args)
        {
            JoinUmParaMuitos();
        }

        private static void JoinUmParaMuitos()
        {
            using (var banco = new LojaContext())
            {
                banco.LogSQLToConsole();

                var cliente = banco.Clientes.Include(c => c.EnderecoDeEntrega).FirstOrDefault();

                Console.WriteLine($"Endereço de entrega: {cliente.EnderecoDeEntrega.Logradouro}");

                //carregamento Join com Include com objeto relacionado
                var produto = banco
                    .Produtos
                    //.Include(c => c.Compras)
                    .FirstOrDefault();

                //carregamento Join explicito com objeto relacionado 
                banco
                    .Entry(produto)
                    .Collection(c => c.Compras)
                    .Query()
                    .Where(c => c.Preco < 10)
                    .Load();

                Console.WriteLine($"Mostrando as compras do produto {produto.Nome}");
                foreach (var item in produto.Compras)
                {

                    Console.WriteLine(item);

                }

            }
        }

        private static void JoinMuitosParaMuitos()
        {
            using (var banco = new LojaContext())
            {
                banco.LogSQLToConsole();

                var promocao = banco
                    .Promocoes
                    .Include(p => p.Produtos)
                    .ThenInclude(pp => pp.Produto)
                    .FirstOrDefault();


                Console.WriteLine("\nMostrando os produtos da prmoção!");

                foreach (var item in promocao.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }
            }
        }

        private static void IncluirPromoicao()
        {
            using (var banco = new LojaContext())
            {
                //banco.LogSQLToConsole();

                var promocao = new Promocao()
                {
                    Descricacao = "Queima Total Janeiro 2018",
                    DataInicio = new DateTime(2018, 1, 1),
                    DataTermino = new DateTime(2018, 1, 31)
                };
                var produtos = banco
                   .Produtos
                   .Where(p => p.Categoria == "Bebidas")
                   .ToList();

                foreach (var item in produtos)
                {
                    promocao.IncluirProduto(item);
                }


                banco.Promocoes.Add(promocao);
                ExibeEntries(banco.ChangeTracker.Entries());
                banco.SaveChanges();
            }
        }

        private static void UmParaUM()
        {
            var cliente = new Cliente();
            cliente.Nome = "Willian";
            cliente.EnderecoDeEntrega = new Endereco()
            {
                Numero = 12,
                Logradouro = "RUA 38",
                Complemento = "Sobrado",
                Bairro = "Marilia",
                Cidade = "Barretos"
            };

            using (var banco = new LojaContext())
            {
                banco.LogSQLToConsole();
                banco.Clientes.Add(cliente);
                banco.SaveChanges();
            }
        }

        private static void UmParaMuitos()
        {
            //compra 6 pão fraces
            var paoFrances = new Produto
            {
                Nome = "pao frances",
                PrecoUnitario = 0.40,
                Unidade = "UN",
                Categoria = "Padaria"
            };

            var compra = new Compra();
            compra.Quantidade = 6;
            compra.Produto = paoFrances;
            compra.Preco = paoFrances.PrecoUnitario * compra.Quantidade;


            using (var banco = new LojaContext())
            {
                banco.LogSQLToConsole();
                banco.Compras.Add(compra);
                banco.SaveChanges();
            }
        }

        private static void MuitosParaMuitos()
        {
            var p1 = new Produto() { Nome = "Suco Laranja", Categoria = "Bebidas", PrecoUnitario = 8.56, Unidade = "Litros" };
            var p2 = new Produto() { Nome = "café", Categoria = "Bebidas", PrecoUnitario = 9.99, Unidade = "Gramas" };
            var p3 = new Produto() { Nome = "Leite", Categoria = " Bebidas", PrecoUnitario = 3.79, Unidade = "Litros" };

            var promocaoPascoa = new Promocao();
            promocaoPascoa.Descricacao = "Pascoa Feliz";
            promocaoPascoa.DataInicio = DateTime.Now;
            promocaoPascoa.DataTermino = DateTime.Now.AddMonths(3);
            promocaoPascoa.IncluirProduto(p1);
            promocaoPascoa.IncluirProduto(p2);
            promocaoPascoa.IncluirProduto(p3);

            using (var banco = new LojaContext())
            {
                banco.LogSQLToConsole();
                banco.Promocoes.Add(promocaoPascoa);
                banco.SaveChanges();
            }
        }

        private static void SelecionarTodos()
        {
            using (var banco = new ProdutoDAO())
            {
                banco.LogSQLConsole();

                IList<Produto> produtos = banco.ListarTodos();
                foreach (var p in produtos)
                {
                    Console.WriteLine(p);
                }
            }
        }

        private static void GravarUsandoEntity()
        {
            Produto p = new Produto();
            p.Nome = "Senhor dos Aneis III";
            p.Categoria = "Livros";
            p.PrecoUnitario = 20.89;

            using (var banco = new ProdutoDAO())
            {
                banco.LogSQLConsole();
                banco.Adicionar(p);
                banco.GravarAlteracoes();
            }
        }

        //ExibeEntries(banco.ChangeTracker.Entries());
        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var e in entries)
            {
                Console.WriteLine($"{e.Entity.ToString()} - {e.State}");
            }
        }


    }

}

