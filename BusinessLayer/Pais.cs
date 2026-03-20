using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace BusinessLayer
{
    public class Pais
    {
        public Pais()
        {
            Novo();
        }

        public Pais(int paisId, string nomePais) : this()
        {
            PaisId = paisId;
            NomePais = nomePais;
        }

        public int PaisId { get; set; }
        public string NomePais { get; set; } = string.Empty;

        public void Novo()
        {
            PaisId = 0;
            NomePais = string.Empty;
        }

        public bool Gravar(out string erro)
        {
            return DataLayer.Pais.Gravar(PaisId, NomePais, out erro);
        }

        public static bool Eliminar(int paisId, out string erro)
        {
            return DataLayer.Pais.Eliminar(paisId, out erro);
        }

        public static Pais? Obter(int paisId, out string erro)
        {
            string nomePais = string.Empty;

            bool ok = DataLayer.Pais.Obter(paisId, ref nomePais, out erro);

            if (!ok)
                return null;

            return new Pais(paisId, nomePais);
        }

        public static DataTable Listar(out string erro)
        {
            return DataLayer.Pais.Listar(out erro);
        }

        public static PaisCollection Listar()
        {
            string erro = string.Empty;
            DataTable dt = Listar(out erro);
            return new PaisCollection(dt);
        }
    }
}
