using BDGlobal;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DataLayer
{
    public class Cidade
    {
        public static bool Gravar(int cidadeId, string nomeCidade, int paisId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = BaseDadosGlobal.AbrirBaseDados();

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
                using SqlConnection con = BaseDadosGlobal.AbrirBaseDados();

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
            return BaseDadosGlobal.ObterLista("Listar_Cidade", out erro);
        }

        public static bool Obter(int cidadeId, ref string nomeCidade, ref int paisId, out string erro)
        {
            erro = string.Empty;

            try
            {
                using SqlConnection con = BaseDadosGlobal.AbrirBaseDados();

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