using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DataLayer
{
    public class Cidade
    {
        private static string ConnectionString =>
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ProjetoPAEmpresa;Data Source=MIGUELNOVO\SQLEXPRESS";

        public static bool Gravar(int cidadeId, string nomeCidade, int paisId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Gravar_Cidade", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@CidadeId", SqlDbType.Int) { Value = cidadeId });
                cmd.Parameters.Add(new SqlParameter("@NomeCidade", SqlDbType.NVarChar, 100) { Value = nomeCidade });
                cmd.Parameters.Add(new SqlParameter("@PaisId", SqlDbType.Int) { Value = paisId });

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return false;
            }
        }

        public static bool Eliminar(int cidadeId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Eliminar_Cidade", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CidadeId", SqlDbType.Int) { Value = cidadeId });

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

                using SqlCommand cmd = new SqlCommand("Listar_Cidade", con);
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

        public static bool Obter(int cidadeId, ref string nomeCidade, ref int paisId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Obter_Cidade", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CidadeId", SqlDbType.Int) { Value = cidadeId });

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nomeCidade = dr["NomeCidade"]?.ToString() ?? string.Empty;
                    paisId = Convert.ToInt32(dr["PaisId"]);
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
