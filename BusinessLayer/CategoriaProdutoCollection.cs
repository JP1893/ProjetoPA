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
    public class CategoriaProdutoCollection : Collection<CategoriaProduto>
    {
        public CategoriaProdutoCollection()
        {
        }

        public CategoriaProdutoCollection(DataTable dataTable) : this()
        {
            CarregarLista(dataTable);
        }

        public void CarregarLista(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.AsEnumerable())
            {
                CategoriaProduto categoria = new CategoriaProduto(
                    row.Field<int>("CategoriaId"),
                    row.Field<string>("NomeCategoria") ?? string.Empty
                );

                Add(categoria);
            }
        }
    }
}
