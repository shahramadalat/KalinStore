using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalinWinApp.Database
{
    public class SelectAll
    {

        public DataTable dataTable(string query, string[,] parameters)
        {
            SqlCommand command = new SqlCommand(query, Connection.conn);
            if (parameters != null)
            {
                for (int i = 0; i <= parameters.GetUpperBound(0); i++)
                {
                    command.Parameters.AddWithValue(parameters[i, 0], parameters[i,1]);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand= command;
            Connection.conn.Open();
            DataTable dt = new DataTable();
            da.Fill(dt);
            Connection.conn.Close();
            return dt;
        }

       
       
    }
}
