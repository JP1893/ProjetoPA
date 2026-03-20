using System;
using System.Data;

namespace BusinessLayer
{
    public class Cliente
    {
        public Cliente()
        {
            Novo();
        }

        public Cliente(int clienteId, string nome, string email, string? telefone, int cidadeId,
            EnumSegmentoCliente segmentoId, DateTime dataRegisto, bool ativo) : this()
        {
            ClienteId = clienteId;
            Nome = nome;
            Email = email;
            Telefone = telefone;
            CidadeId = cidadeId;
            SegmentoId = segmentoId;
            DataRegisto = dataRegisto;
            Ativo = ativo;
        }

        public int ClienteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public int CidadeId { get; set; }
        public EnumSegmentoCliente SegmentoId { get; set; }
        public DateTime DataRegisto { get; set; }
        public bool Ativo { get; set; }

        public void Novo()
        {
            ClienteId = 0;
            Nome = string.Empty;
            Email = string.Empty;
            Telefone = string.Empty;
            CidadeId = 0;
            SegmentoId = EnumSegmentoCliente.Particular;
            DataRegisto = DateTime.Today;
            Ativo = true;
        }

        public bool Gravar(out string erro)
        {
            return DataLayer.Cliente.Gravar(
                ClienteId, Nome, Email, Telefone, CidadeId,
                (int)SegmentoId, DataRegisto, Ativo, out erro);
        }

        public static bool Eliminar(int clienteId, out string erro)
        {
            return DataLayer.Cliente.Eliminar(clienteId, out erro);
        }

        public static Cliente? Obter(int clienteId, out string erro)
        {
            string nome = string.Empty;
            string email = string.Empty;
            string? telefone = string.Empty;
            int cidadeId = 0;
            int segmentoId = 1;
            DateTime dataRegisto = DateTime.Today;
            bool ativo = true;

            bool ok = DataLayer.Cliente.Obter(
                clienteId, ref nome, ref email, ref telefone, ref cidadeId,
                ref segmentoId, ref dataRegisto, ref ativo, out erro);

            if (!ok)
                return null;

            return new Cliente(clienteId, nome, email, telefone, cidadeId,
                (EnumSegmentoCliente)segmentoId, dataRegisto, ativo);
        }

        public static DataTable Listar(out string erro)
        {
            return DataLayer.Cliente.Listar(out erro);
        }

        public static ClienteCollection Listar()
        {
            string erro = string.Empty;
            DataTable dt = Listar(out erro);
            return new ClienteCollection(dt);
        }
    }
}
