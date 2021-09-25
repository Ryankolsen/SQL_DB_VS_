using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace EmployeeManagement
{
    class Db
    {

        public static SqlConnection con = new SqlConnection($"Data Source=DESKTOP-5QEG18K;Initial Catalog=EmployeeDatabase;User ID=xxxx;Password=xxxx");

        public static SqlCommand cmd = new SqlCommand(" ", con);
        public static DataSet ds;
        public static SqlDataAdapter da;

        public static BindingSource bs;
        public static string sql;

        public static void OpenConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    //MessageBox.Show("Connection Status: " + con.State.ToString());
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Open Connection Error: " + ex.Message);
            }
        }

        public static void CloseConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    //MessageBox.Show("Connection Status: " + con.State.ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Disconnection Error: " + ex.Message);
            }
        }


    }
}
