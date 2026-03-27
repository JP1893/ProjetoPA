using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DataLayer
{
    public class Produto
    {
        private static string ConnectionString =>
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ProjetoPAEmpresa;TrustServerCertificate = true;Data Source=MIGUELNOVO\SQLEXPRESS";

        public static bool Gravar(
            int produtoId, string nomeProduto, int categoriaId, decimal precoVenda,
            decimal precoCusto, int quantidade, bool ativo, DateTime dataCriacao,
            DateTime? dataVenda, int? clienteId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Gravar_Produto", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdutoId", SqlDbType.Int) { Value = produtoId });
                cmd.Parameters.Add(new SqlParameter("@NomeProduto", SqlDbType.NVarChar, 150) { Value = nomeProduto });
                cmd.Parameters.Add(new SqlParameter("@CategoriaId", SqlDbType.Int) { Value = categoriaId });
                cmd.Parameters.Add(new SqlParameter("@PrecoVenda", SqlDbType.Decimal) { Value = precoVenda });
                cmd.Parameters.Add(new SqlParameter("@PrecoCusto", SqlDbType.Decimal) { Value = precoCusto });
                cmd.Parameters.Add(new SqlParameter("@Quantidade", SqlDbType.Int) { Value = quantidade });
                cmd.Parameters.Add(new SqlParameter("@Ativo", SqlDbType.Bit) { Value = ativo });
                cmd.Parameters.Add(new SqlParameter("@DataCriacao", SqlDbType.Date) { Value = dataCriacao });
                cmd.Parameters.Add(new SqlParameter("@DataVenda", SqlDbType.Date)
                {
                    Value = dataVenda.HasValue ? dataVenda.Value : DBNull.Value
                });
                cmd.Parameters.Add(new SqlParameter("@ClienteId", SqlDbType.Int)
                {
                    Value = clienteId.HasValue ? clienteId.Value : DBNull.Value
                });

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return false;
            }
        }
        public static bool Obter(
            int produtoId, ref string nomeProduto, ref int categoriaId, ref decimal precoVenda,
            ref decimal precoCusto, ref int quantidade, ref bool ativo, ref DateTime dataCriacao,
            ref DateTime? dataVenda, ref int? clienteId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Obter_Produto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProdutoId", SqlDbType.Int) { Value = produtoId });

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nomeProduto = dr["NomeProduto"]?.ToString() ?? string.Empty;
                    categoriaId = Convert.ToInt32(dr["CategoriaId"]);
                    precoVenda = Convert.ToDecimal(dr["PrecoVenda"]);
                    precoCusto = Convert.ToDecimal(dr["PrecoCusto"]);
                    quantidade = Convert.ToInt32(dr["Quantidade"]);
                    ativo = Convert.ToBoolean(dr["Ativo"]);
                    dataCriacao = Convert.ToDateTime(dr["DataCriacao"]);
                    dataVenda = dr["DataVenda"] == DBNull.Value ? null : Convert.ToDateTime(dr["DataVenda"]);
                    clienteId = dr["ClienteId"] == DBNull.Value ? null : Convert.ToInt32(dr["ClienteId"]);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return false;
            }
        }

        public static bool Eliminar(int produtoId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Eliminar_Produto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ProdutoId", SqlDbType.Int) { Value = produtoId });

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return false;
            }
        }

        public static DataTable Listar(out string erro)
        {
            erro = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Listar_Produto", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }

            return dt;
        }
    }
}
