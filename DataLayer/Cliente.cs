using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DataLayer
{
    public class Cliente
    {
        private static string ConnectionString =>
            @"Server=.\SQLEXPRESS;Database=ProjetoPAEmpresa;Trusted_Connection=True;TrustServerCertificate=True";

        public static bool Gravar(
            int clienteId, string nome, string email, string? telefone, int cidadeId,
            int segmentoId, DateTime dataRegisto, bool ativo, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Gravar_Cliente", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ClienteId", SqlDbType.Int) { Value = clienteId });
                cmd.Parameters.Add(new SqlParameter("@Nome", SqlDbType.NVarChar, 150) { Value = nome });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 150) { Value = email });
                cmd.Parameters.Add(new SqlParameter("@Telefone", SqlDbType.NVarChar, 30)
                {
                    Value = string.IsNullOrWhiteSpace(telefone) ? DBNull.Value : telefone
                });
                cmd.Parameters.Add(new SqlParameter("@CidadeId", SqlDbType.Int) { Value = cidadeId });
                cmd.Parameters.Add(new SqlParameter("@SegmentoId", SqlDbType.Int) { Value = segmentoId });
                cmd.Parameters.Add(new SqlParameter("@DataRegisto", SqlDbType.Date) { Value = dataRegisto });
                cmd.Parameters.Add(new SqlParameter("@Ativo", SqlDbType.Bit) { Value = ativo });

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                return false;
            }
        }

        public static bool Eliminar(int clienteId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Eliminar_Cliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ClienteId", SqlDbType.Int) { Value = clienteId });

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

                using SqlCommand cmd = new SqlCommand("Listar_Cliente", con);
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

        public static bool Obter(
            int clienteId, ref string nome, ref string email, ref string? telefone,
            ref int cidadeId, ref int segmentoId, ref DateTime dataRegisto,
            ref bool ativo, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                using SqlCommand cmd = new SqlCommand("Obter_Cliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ClienteId", SqlDbType.Int) { Value = clienteId });

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nome = dr["Nome"]?.ToString() ?? string.Empty;
                    email = dr["Email"]?.ToString() ?? string.Empty;
                    telefone = dr["Telefone"] == DBNull.Value ? null : dr["Telefone"].ToString();
                    cidadeId = Convert.ToInt32(dr["CidadeId"]);
                    segmentoId = Convert.ToInt32(dr["SegmentoId"]);
                    dataRegisto = Convert.ToDateTime(dr["DataRegisto"]);
                    ativo = Convert.ToBoolean(dr["Ativo"]);
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
