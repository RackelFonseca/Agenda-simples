using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aula06
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // VARIAVEL GLOBAL
        int acao;


        // 0 = NOVO
        // 1 = ALTERAR

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // QUANDO A TELA CARREGA
        private void Form1_Load(object sender, EventArgs e)
        {
            statusInicio();
            carregaGrid();

        }

        // STATUS INICIAL DO FORM
        private  void statusInicio()
        {
            gbDados.Enabled = false;
            btnAlterar.Enabled = false;
            btnGravar.Enabled = false;
            btnExcluir.Enabled = false;

            btnNovo.Enabled = true;
            btnSair.Enabled = true;

        }


        // STATUS DE ALTERACAO
        private void statusAltera()
        {

            gbDados.Enabled = true;
            btnAlterar.Enabled = false;
            btnGravar.Enabled = true;

            btnNovo.Enabled = false;
            btnSair.Enabled = true;

            txtNome.Focus();

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            
            statusAltera();

            // NOVO REGISTRO
            acao = 0;

        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            statusAltera();
            // ALTERACAO DE REGISTRO
            acao = 1;
        }

        // GRAVAR
        private void btnGravar_Click(object sender, EventArgs e)
        {
            if (acao == 0)
            {
                insereRegistro();

            }
            else if (acao == 1)
            {

                atualizaRegitro();

            }

            carregaGrid();
            statusInicio();

        }

        private void atualizaRegitro()
        {
            try
            {
                conexao.fechaConexao();
                string sql = "UPDATE agenda SET " +
                                             "nome = @nome, " +
                                             "telefone = @telefone, " +
                                             "email = @email " +

                                             "WHERE id_agenda = @id";

                conexao.abreConexao();

                conexao.comando = new System.Data.SqlClient.SqlCommand(sql, conexao.conecta);

                // PARAMETROS
                conexao.comando.Parameters.AddWithValue("@nome", txtNome.Text);
                conexao.comando.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                conexao.comando.Parameters.AddWithValue("@email", txtEmail.Text);

                conexao.comando.Parameters.AddWithValue("@id", Convert.ToInt32(txCodigo.Text));

                conexao.comando.ExecuteNonQuery();

                MessageBox.Show("Dados alterados com sucesso.");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);                
            }
   

        }



        // CARREGO O GRID

        private void carregaGrid()
        {
            conexao.fechaConexao();
            string sql = "SELECT * FROM agenda ORDER BY id_agenda";

            conexao.abreConexao();
            conexao.comando = new System.Data.SqlClient.SqlCommand(sql, conexao.conecta);

            conexao.leitor = conexao.comando.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("Codigo", typeof(string));
            dt.Columns.Add("Nome", typeof(string));
            dt.Columns.Add("Telefone", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            while (conexao.leitor.Read())
            {
                DataRow dr = dt.NewRow();

                dr["Codigo"] = conexao.leitor["id_agenda"].ToString();
                dr["Nome"] = conexao.leitor["nome"].ToString();
                dr["Telefone"] = conexao.leitor["telefone"].ToString();
                dr["Email"] = conexao.leitor["email"].ToString();

                dt.Rows.Add(dr);

            }

            grdAgenda.DataSource = dt;
            grdAgenda.Update();

        }


        // INSERT
        private  void insereRegistro()
        {
            try
            {
                conexao.fechaConexao();

                string sql = "INSERT INTO agenda (" +
                                    " nome," +
                                    " telefone," +
                                    " email" +
                                    " )" +
                                    " VALUES" +
                                    " (" +
                                    " @nome," +
                                    " @telefone," +
                                    " @email" +
                                    " )";

                conexao.abreConexao();
                conexao.comando = new System.Data.SqlClient.SqlCommand(sql, conexao.conecta);


                // PARAMETROS
                conexao.comando.Parameters.AddWithValue("@nome", txtNome.Text);
                conexao.comando.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                conexao.comando.Parameters.AddWithValue("@email", txtEmail.Text);

                conexao.comando.ExecuteNonQuery();

                MessageBox.Show("Dados gravados com sucesso.");


            }
            catch (Exception)
            {

                throw;
            }
        }

        // UPDATE

        private void atualizaRegistro()
        {


        }

        // DOIS CLIQUES
        private void grdAgenda_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txCodigo.Text = grdAgenda.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = grdAgenda.CurrentRow.Cells[1].Value.ToString();
            txtTelefone.Text = grdAgenda.CurrentRow.Cells[2].Value.ToString();
            txtEmail.Text = grdAgenda.CurrentRow.Cells[3].Value.ToString();

       
            btnAlterar.Enabled = true;
            btnExcluir.Enabled = true;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                conexao.fechaConexao();
                string sql = "DELETE FROM agenda WHERE id_agenda = @id";
                conexao.abreConexao();

                conexao.comando = new System.Data.SqlClient.SqlCommand(sql, conexao.conecta);

                // PARAMETROS
               conexao.comando.Parameters.AddWithValue("@id", Convert.ToInt32(txCodigo.Text));

                conexao.comando.ExecuteNonQuery();

                MessageBox.Show("Registro excluido.");
                carregaGrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao exlcuir registro.");
            }

        }
    }
}
