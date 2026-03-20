using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace BusinessLayer
{
    public class CategoriaProduto
    {
        public CategoriaProduto()
        {
            Novo();
        }

        public CategoriaProduto(int categoriaId, string nomeCategoria) : this()
        {
            CategoriaId = categoriaId;
            NomeCategoria = nomeCategoria;
        }

        public int CategoriaId { get; set; }
        public string NomeCategoria { get; set; } = string.Empty;

        public void Novo()
        {
            CategoriaId = 0;
            NomeCategoria = string.Empty;
        }

        public bool Gravar(out string erro)
        {
            return DataLayer.CategoriaProduto.Gravar(CategoriaId, NomeCategoria, out erro);
        }

        public static bool Eliminar(int categoriaId, out string erro)
        {
            return DataLayer.CategoriaProduto.Eliminar(categoriaId, out erro);
        }

        public static CategoriaProduto? Obter(int categoriaId, out string erro)
        {
            string nomeCategoria = string.Empty;

            bool ok = DataLayer.CategoriaProduto.Obter(categoriaId, ref nomeCategoria, out erro);

            if (!ok)
                return null;

            return new CategoriaProduto(categoriaId, nomeCategoria);
        }

        public static DataTable Listar(out string erro)
        {
            return DataLayer.CategoriaProduto.Listar(out erro);
        }

        public static CategoriaProdutoCollection Listar()
        {
            string erro = string.Empty;
            DataTable dt = Listar(out erro);
            return new CategoriaProdutoCollection(dt);
        }
    }
}
