using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace BusinessLayer
{
    public class Cidade
    {
        public Cidade()
        {
            Novo();
        }

        public Cidade(int cidadeId, string nomeCidade, int paisId) : this()
        {
            CidadeId = cidadeId;
            NomeCidade = nomeCidade;
            PaisId = paisId;
        }

        public int CidadeId { get; set; }
        public string NomeCidade { get; set; } = string.Empty;
        public int PaisId { get; set; }

        public void Novo()
        {
            CidadeId = 0;
            NomeCidade = string.Empty;
            PaisId = 0;
        }

        public bool Gravar(out string erro)
        {
            return DataLayer.Cidade.Gravar(CidadeId, NomeCidade, PaisId, out erro);
        }

        public static bool Eliminar(int cidadeId, out string erro)
        {
            return DataLayer.Cidade.Eliminar(cidadeId, out erro);
        }

        public static Cidade? Obter(int cidadeId, out string erro)
        {
            string nomeCidade = string.Empty;
            int paisId = 0;

            bool ok = DataLayer.Cidade.Obter(cidadeId, ref nomeCidade, ref paisId, out erro);

            if (!ok)
                return null;

            return new Cidade(cidadeId, nomeCidade, paisId);
        }

        public static DataTable Listar(out string erro)
        {
            return DataLayer.Cidade.Listar(out erro);
        }

        public static CidadeCollection Listar()
        {
            string erro = string.Empty;
            DataTable dt = Listar(out erro);
            return new CidadeCollection(dt);
        }
    }
}
