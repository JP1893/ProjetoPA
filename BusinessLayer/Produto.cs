using System;
using System.Data;

namespace BusinessLayer
{
    public class Produto
    {
        public Produto()
        {
            Novo();
        }

        public Produto(int produtoId, string nomeProduto, int categoriaId, decimal precoVenda,
            decimal precoCusto, int quantidade, bool ativo, DateTime dataCriacao,
            DateTime? dataVenda, int? clienteId) : this()
        {
            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            CategoriaId = categoriaId;
            PrecoVenda = precoVenda;
            PrecoCusto = precoCusto;
            Quantidade = quantidade;
            Ativo = ativo;
            DataCriacao = dataCriacao;
            DataVenda = dataVenda;
            ClienteId = clienteId;
        }
        public static Produto? Obter(int produtoId, out string erro)
        {
            string nomeProduto = string.Empty;
            int categoriaId = 0;
            decimal precoVenda = 0;
            decimal precoCusto = 0;
            int quantidade = 0;
            bool ativo = true;
            DateTime dataCriacao = DateTime.Today;
            DateTime? dataVenda = null;
            int? clienteId = null;

            bool ok = DataLayer.Produto.Obter(
                produtoId, ref nomeProduto, ref categoriaId, ref precoVenda, ref precoCusto,
                ref quantidade, ref ativo, ref dataCriacao, ref dataVenda, ref clienteId, out erro);

            if (!ok)
                return null;

            return new Produto(produtoId, nomeProduto, categoriaId, precoVenda, precoCusto,
                quantidade, ativo, dataCriacao, dataVenda, clienteId);
        }

        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal PrecoCusto { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataVenda { get; set; }
        public int? ClienteId { get; set; }

        public void Novo()
        {
            ProdutoId = 0;
            NomeProduto = string.Empty;
            CategoriaId = 0;
            PrecoVenda = 0;
            PrecoCusto = 0;
            Quantidade = 0;
            Ativo = true;
            DataCriacao = DateTime.Today;
            DataVenda = null;
            ClienteId = null;
        }

        public bool Gravar(out string erro)
        {
            return DataLayer.Produto.Gravar(
                ProdutoId, NomeProduto, CategoriaId, PrecoVenda, PrecoCusto,
                Quantidade, Ativo, DataCriacao, DataVenda, ClienteId, out erro);
        }

        public static bool Eliminar(int produtoId, out string erro)
        {
            return DataLayer.Produto.Eliminar(produtoId, out erro);
        }

        public static DataTable Listar(out string erro)
        {
            return DataLayer.Produto.Listar(out erro);
        }

        public static ProdutoCollection Listar()
        {
            string erro = string.Empty;
            DataTable dt = Listar(out erro);
            return new ProdutoCollection(dt);
        }

        public bool IsCategoriaId(int categoriaId)
        {
            bool isCategoriaId = true;
            if (categoriaId > 0 && this.CategoriaId != categoriaId)
            {
                isCategoriaId = false;
            }
            return isCategoriaId;
        }

        public bool IsPaisId(int paisId)
        {
            bool isPaisId = true;
            if (paisId > 0 && this.ClienteId != paisId)
            {
                isPaisId = false;
            }
            return isPaisId;
        }

        internal int? ObterAnoVenda()
        {
            if (this.FoiVendido())
            {
                return this.DataVenda.Value.Year;
            }
            return null;
        }

        internal bool FoiVendido()
        {
            if (this.DataVenda.HasValue)
            {
                return true;
            }
            return false;
        }

        internal float ObterLucro()
        {
            return (float)(this.PrecoVenda - this.PrecoCusto);
        }

        internal bool FiltraDados(int categoriaId, string produtoSelecionado)
        {
            bool valido = true;

            if (categoriaId > 0)
            {
                valido = (this.CategoriaId == categoriaId);
            }

            if (!string.IsNullOrEmpty(produtoSelecionado) && !string.Equals(produtoSelecionado, "All", StringComparison.OrdinalIgnoreCase))
            {
                valido = valido && (string.Equals(this.NomeProduto, produtoSelecionado, StringComparison.OrdinalIgnoreCase));
            }

            return valido;
        }
    }
}