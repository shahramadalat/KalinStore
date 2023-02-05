using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KalinWinApp.Database
{
    internal class SelectOne
    {
        public string Select(string query, string[,] parameters)
        {
            string result = "";
            SqlCommand command = new SqlCommand(query,Connection.conn);
            if (parameters != null)
            {
                for (int i = 0; i <= parameters.GetUpperBound(0); i++)
                {
                    command.Parameters.AddWithValue(parameters[i, 0], parameters[i, 1]);
                }
            }
            Connection.conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows )
            {
                result = reader[0].ToString();
            }
            Connection.conn.Close();
            return result;
        }
    }
}
