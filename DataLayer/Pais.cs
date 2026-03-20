using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DataLayer
{
    public class Pais
    {
        private static string ConnectionString =>
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ProjetoPAEmpresa;Data Source=MIGUELNOVO\SQLEXPRESS";

        public static bool Gravar(int paisId, string nomePais, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Gravar_Pais", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PaisId", SqlDbType.Int) { Value = paisId });
                cmd.Parameters.Add(new SqlParameter("@NomePais", SqlDbType.NVarChar, 100) { Value = nomePais });

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return false;
            }
        }

        public static bool Eliminar(int paisId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Eliminar_Pais", con);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public static DataTable Listar(out string erro)
        {
            erro = string.Empty;
            DataTable dt = new DataTable();

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Listar_Pais", con);
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

        public static bool Obter(int paisId, ref string nomePais, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Obter_Pais", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@PaisId", SqlDbType.Int) { Value = paisId });

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nomePais = dr["NomePais"]?.ToString() ?? string.Empty;
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
