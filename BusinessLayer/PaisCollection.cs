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
    public class PaisCollection : Collection<Pais>
    {
        public PaisCollection()
        {
        }

        public PaisCollection(DataTable dataTable) : this()
        {
            CarregarLista(dataTable);
        }

        public void CarregarLista(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.AsEnumerable())
            {
                Pais pais = new Pais(
                    row.Field<int>("PaisId"),
                    row.Field<string>("NomePais") ?? string.Empty
                );

                Add(pais);
            }
        }
    }
}
