using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace BusinessLayer
{
    public class ClienteCollection : Collection<Cliente>
    {
        public ClienteCollection()
        {
        }

        public ClienteCollection(DataTable dataTable) : this()
        {
            CarregarLista(dataTable);
        }

        public void CarregarLista(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.AsEnumerable())
            {
                Cliente cliente = new Cliente(
                    row.Field<int>("ClienteId"),
                    row.Field<string>("Nome") ?? string.Empty,
                    row.Field<string>("Email") ?? string.Empty,
                    row.Field<string?>("Telefone"),
                    row.Field<int>("CidadeId"),
                    (EnumSegmentoCliente)row.Field<int>("SegmentoId"),
                    row.Field<DateTime>("DataRegisto"),
                    row.Field<bool>("Ativo")
                );

                Add(cliente);
            }
        }
    }
}
