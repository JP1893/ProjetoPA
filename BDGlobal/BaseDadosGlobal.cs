using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BDGlobal
{
    public class BaseDadosGlobal
    {
        public static SqlConnection AbrirBaseDados(string connectionString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public static DataTable ObterLista(string nomeStoreProcedure, string connectionString, out string erro)
        {
            DataTable dataTable = new DataTable();
            erro = string.Empty;

            try
            {
                using SqlConnection sqlConnection = AbrirBaseDados(connectionString);
                using SqlCommand sqlCommand = new SqlCommand(nomeStoreProcedure, sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                using SqlDataReader dataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleResult);
                dataTable.Load(dataReader);
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }

            return dataTable;
        }
    }
}
