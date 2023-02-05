using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KalinWinApp.Database
{
    public class Excute
    {
        public void Command(string query, string[,] parameters)
        {
            SqlCommand command = new SqlCommand(query, Connection.conn);
            if (parameters != null)
            {
                if (parameters != null)
                {
                    for (int i = 0; i <= parameters.GetUpperBound(0); i++)
                    {
                        command.Parameters.AddWithValue(parameters[i, 0], parameters[i, 1]);
                    }
                }
            }
            Connection.conn.Open();
            command.ExecuteNonQuery();
            Connection.conn.Close();
        }

        public void Procedure(string procedure, Dictionary<string,object> parameters)
        {
            SqlCommand command = new SqlCommand(procedure, Connection.conn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
          
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
             
            }
            
            Connection.conn.Open();
            command.ExecuteNonQuery();
            Connection.conn.Close();
        }

    }
}
