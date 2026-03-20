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
    public class CidadeCollection : Collection<Cidade>
    {
        public CidadeCollection()
        {
        }

        public CidadeCollection(DataTable dataTable) : this()
        {
            CarregarLista(dataTable);
        }

        public void CarregarLista(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.AsEnumerable())
            {
                Cidade cidade = new Cidade(
                    row.Field<int>("CidadeId"),
                    row.Field<string>("NomeCidade") ?? string.Empty,
                    row.Field<int>("PaisId")
                );

                Add(cidade);
            }
        }
    }
}
