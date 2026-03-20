using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DataLayer
{
    public class CategoriaProduto
    {
        private static string ConnectionString =>
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ProjetoPAEmpresa;Data Source=MIGUELNOVO\SQLEXPRESS";

        public static bool Gravar(int categoriaId, string nomeCategoria, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Gravar_CategoriaProduto", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@CategoriaId", SqlDbType.Int) { Value = categoriaId });
                cmd.Parameters.Add(new SqlParameter("@NomeCategoria", SqlDbType.NVarChar, 100) { Value = nomeCategoria });

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return false;
            }
        }

        public static bool Eliminar(int categoriaId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Eliminar_CategoriaProduto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CategoriaId", SqlDbType.Int) { Value = categoriaId });

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

                using SqlCommand cmd = new SqlCommand("Listar_CategoriaProduto", con);
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

        public static bool Obter(int categoriaId, ref string nomeCategoria, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Obter_CategoriaProduto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CategoriaId", SqlDbType.Int) { Value = categoriaId });

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nomeCategoria = dr["NomeCategoria"]?.ToString() ?? string.Empty;
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
    }
}
