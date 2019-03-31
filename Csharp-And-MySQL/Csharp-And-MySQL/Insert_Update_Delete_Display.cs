using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace Csharp_And_MySQL
{
    public partial class Insert_Update_Delete_Display : Form
    {
        public Insert_Update_Delete_Display()
        {
            InitializeComponent();
        }

        //string connString = "Server=162.241.224.137;Port=3306;Database=lesagess_db;Uid=lesagess_db;password=lesagess_db@db";
        // MySqlConnection connection = new MySqlConnection("datasource=162.241.224.137;port=3306;Initial Catalog='lesagess_db';username=lesagess_db;password=lesagess_db@db");
        //MySqlConnection connection = new MySqlConnection("datasource=127.0.0.1;port=3306;Initial Catalog='db';username=root;password=");
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog='db';username=root;password=");


        private void Insert_Update_Delete_Display_Load(object sender, EventArgs e)
        {
            populateDGV();
        }

        public void populateDGV()
        {
            //pupulate the datagridview
            string selectQuery = "SELECT * FROM users";
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection);
            adapter.Fill(table);
            dataGridView_USERS.DataSource = table;
        }

        private void dataGridView_USERS_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxID.Text = dataGridView_USERS.CurrentRow.Cells[0].Value.ToString();
            textBoxFName.Text = dataGridView_USERS.CurrentRow.Cells[1].Value.ToString();
            textBoxLName.Text = dataGridView_USERS.CurrentRow.Cells[2].Value.ToString();
            textBoxAge.Text = dataGridView_USERS.CurrentRow.Cells[3].Value.ToString();
        }

        public void openConnection()
        {
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public void executeMyQuery(string query)
        {
            try
            {
                openConnection();
                MySqlCommand command = new MySqlCommand(query, connection);

                if(command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Query Execute");
                }
                else
                {
                    MessageBox.Show("Query Not Execute");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                closeConnection();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string insertQuery = "INSERT INTO users(fname,lname,age) VALUES('" + textBoxFName.Text + "','" + textBoxLName.Text + "','" + textBoxAge.Text + "')";
            executeMyQuery(insertQuery);
            populateDGV();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string updateQuery = "UPDATE users SET fname='"+ textBoxFName.Text + "',lname='" + textBoxLName.Text + "',age=" + textBoxAge.Text + " WHERE id = "+int.Parse(textBoxID.Text);
            executeMyQuery(updateQuery);
            populateDGV();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string deleteQuery = "DELETE FROM users  WHERE id = " + int.Parse(textBoxID.Text);
            executeMyQuery(deleteQuery);
            populateDGV();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            MySqlDataReader mdr;
            string select = "SELECT * FROM users WHERE id = " + int.Parse(textBoxID.Text);
            MySqlCommand command = new MySqlCommand(select, connection);
            openConnection();
            mdr = command.ExecuteReader();

            if(mdr.Read())
            {
                textBoxFName.Text = mdr.GetString("fname");
                textBoxLName.Text = mdr.GetString("lname");
                textBoxAge.Text = mdr.GetString("age");

            }
            else
            {
                MessageBox.Show("User Not Found");
            }
            closeConnection();
        }
    }
}
