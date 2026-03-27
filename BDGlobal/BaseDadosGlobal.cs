using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BDGlobal
{
    public static class BaseDadosGlobal
    {
        public static string ConnectionString =>
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ProjetoPAEmpresa;TrustServerCertificate=True;Data Source=MIGUELNOVO\SQLEXPRESS";

        public static SqlConnection AbrirBaseDados()
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public static DataTable ObterLista(string nomeStoreProcedure, out string erro)
        {
            DataTable dataTable = new DataTable();
            erro = string.Empty;

            try
            {
                using SqlConnection sqlConnection = AbrirBaseDados();
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