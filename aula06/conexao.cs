using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace aula06
{
    class conexao
    {

        public static SqlConnection conecta = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=AgendaSimples;Integrated Security=SSPI");

        public static SqlCommand comando;
        public static SqlDataReader leitor;



        public static void abreConexao()
        {
            try
            {
                conecta.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show("Nao foi possivel se conectar ao banco");
            }
            

        }

        public static void fechaConexao()
        {
            try
            {
                conecta.Close();
            }
            catch (Exception)
            {
               
            }


        }

    }
}
