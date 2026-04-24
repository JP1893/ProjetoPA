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
                .Where(p => p.IsCategoriaId(categoriaID))
                .Sum(p => p.ObterLucro());
        }

        public LucroPorAnoCollection ObterLucroPorAno(int categoriaId)
        {
            LucroPorAnoCollection lucroPorAnos = new LucroPorAnoCollection();

            foreach (Produto produto in this)
            {
                if (produto.IsCategoriaId(categoriaId) &&
                    produto.FoiVendido())
                {
                    int? ano = produto.ObterAnoVenda();
                    if (ano.HasValue)
                    {
                        float lucro = produto.ObterLucro();

                        lucroPorAnos.Adicionar(ano.Value, lucro);
                    }
                }
            }

            return lucroPorAnos;
        }
    }
}
