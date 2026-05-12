using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace BusinessLayer
{
    public class ProdutoCollection : Collection<Produto>
    {
        public ProdutoCollection()
        {
        }

        public ProdutoCollection(DataTable dataTable) : this()
        {
            CarregarLista(dataTable);
        }

        public void CarregarLista(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.AsEnumerable())
            {
                Produto produto = new Produto(
                    row.Field<int>("ProdutoId"),
                    row.Field<string>("NomeProduto") ?? string.Empty,
                    row.Field<int>("CategoriaId"),
                    row.Field<decimal>("PrecoVenda"),
                    row.Field<decimal>("PrecoCusto"),
                    row.Field<int>("Quantidade"),
                    row.Field<bool>("Ativo"),
                    row.Field<DateTime>("DataCriacao"),
                    row.IsNull("DataVenda") ? null : row.Field<DateTime?>("DataVenda"),
                    row.IsNull("ClienteId") ? null : row.Field<int?>("ClienteId")
                );

                Add(produto);
            }
        }

        public double ObterTotalVendas(int categoriaID, int paisID)
        {
            double totalVendas = (double)(from element in this
                                          where element.IsCategoriaId(categoriaID) && element.IsPaisId(paisID)
                                          select element.PrecoVenda).Sum();

            return totalVendas;
        }

        public double ObterLucro(int categoriaID)
        {
            return (double)this
                .Where(p => (categoriaID == 0 || p.CategoriaId == categoriaID)
                            && p.DataVenda.HasValue)
                .Sum(p => p.ObterLucro());
        }

        public int ObterProdutosVendidos(int categoriaID, int ano)
        {
            return this
                .Where(p => (categoriaID == 0 || p.CategoriaId == categoriaID)
                            && p.DataVenda.HasValue
                            && p.DataVenda.Value.Year == ano)
                .Count();
        }

        public int ObterStockDisponivel(int categoriaID)
        {
            return this
                .Where(p => (categoriaID == 0 || p.CategoriaId == categoriaID)
                            && p.Ativo
                            && !p.DataVenda.HasValue)
                .Sum(p => p.Quantidade);
        }

        public LucroPorAnoCollection ObterLucroPorAno(int categoriaId)
        {
            LucroPorAnoCollection resultado = new LucroPorAnoCollection();

            var dados = this
                .Where(p => p.DataVenda.HasValue
                            && (categoriaId == 0 || p.CategoriaId == categoriaId))
                .GroupBy(p => p.DataVenda.Value.Year)
                .Select(g => new
                {
                    Ano = g.Key,
                    Lucro = g.Sum(p => p.ObterLucro())
                })
                .OrderBy(x => x.Ano);

            foreach (var item in dados)
            {
                resultado.Adicionar(item.Ano, item.Lucro);
            }

            return resultado;
        }




    }
}
