using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        int timerleft;
        public bool check = true;
        public string PKey = "";
        DataTable ds = new DataTable();
        DataTable dataTable;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public object keywordindex = "2";

        private string SearchPK(string tableName)
        {
            DataTable table = new DataTable();
            string PK = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("PrimaryKey", connection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@tableName", tableName);
                    adapter = new SqlDataAdapter(sqlCommand);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Fill(table);
                }
                foreach (DataRow row in table.Rows)
                {
                    foreach (var Key in row.ItemArray)
                    {
                        PK = Key.ToString();
                    }
                }
                return PK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void SelectFromDB(string tableName, string connString, DataTable data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("sp_User", connection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@TABLE_NAME", tableName);
                    adapter = new SqlDataAdapter(sqlCommand);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Fill(data);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateTable(string TableName, DataTable dataTable)
        {
            DataGridView dataGridView = new DataGridView();
            TabPage tabPage = new TabPage(TableName);
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            try
            {
                dataTable.Columns[SearchPK(TableName)].ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dataGridView.DataSource = dataTable;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            tabPage.Controls.Add(dataGridView);
            
            this.tabControl1.Controls.Add(tabPage);
            
        }

        public Form1()
        {
            InitializeComponent();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand("SelectAllTable", connection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    adapter = new SqlDataAdapter(sqlCommand);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Fill(ds);
                }

                foreach (DataRow r in ds.Rows)
                {
                    DataTable dt = new DataTable();

                    foreach (var cell in r.ItemArray)
                    {
                        SelectFromDB(cell.ToString(), connectionString, dt);
                        
                        CreateTable(cell.ToString(),dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            timerleft = 120;
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Start();
        }

        //обработки кнопки выход
        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //определения названия таблицы
        
        // кнопка добавления
        private void addButton_Click(object sender, EventArgs e)
        {

            PKey = SearchPK(this.tabControl1.SelectedTab.Text);

            AddRow addRow = new AddRow(this);
            addRow.ShowDialog();

            ОбновитьДанныеToolStripMenuItem_Click(sender, e);
        }
        // кнопка удаления
        private void deleteButton_Click(object sender, EventArgs e)
        {
            // удаляем выделенные строки из dataGridView1
            DataGridView dgv = this.tabControl1.SelectedTab.Controls[0] as DataGridView;
            DataTable dataTable = new DataTable();
            string str = "";

            foreach (DataGridViewRow row in dgv.SelectedRows)
            {
                try
                {
                    str = "DELETE FROM [" + this.tabControl1.SelectedTab.Text + "] WHERE ";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        str += "[" + dgv.Columns[SearchPK(this.tabControl1.SelectedTab.Text)].Name + "] = " + row.Cells[SearchPK(this.tabControl1.SelectedTab.Text)].Value;
                        
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = str;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error Message");
                }
            }

            SelectFromDB(this.tabControl1.SelectedTab.Text, connectionString, dataTable);
            dgv.DataSource = dataTable;
            ОбновитьДанныеToolStripMenuItem_Click(sender, e);
        }

        private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, @"C:\ProjectHtmlHelp\ProjectHelpTalankin.chm", HelpNavigator.KeywordIndex, keywordindex);   
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message,"Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void СохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.tabControl1.TabPages)
            {
                try
                {
                    DataGridView dgv = control.Controls[0] as DataGridView;
                    DataTable dt = (DataTable)dgv.DataSource;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand sqlCommand = new SqlCommand("sp_User", connection);
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@TABLE_NAME", control.Text);
                        adapter = new SqlDataAdapter(sqlCommand);
                        commandBuilder = new SqlCommandBuilder(adapter);
                        adapter.Update(dt);
                    }

                    dt.Clear();
                    SelectFromDB(control.Text, connectionString, dt);
                    dgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void РаботаСSQLЗапросамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQL sql = new SQL();
            sql.Show();
            this.Hide();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ФильтрацияToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            DataTable dt = new DataTable();
            Filter filter = new Filter(this);
            filter.ShowDialog();

            if (filter.buttonClick == false)
            {

                this.очиститьФильтрыToolStripMenuItem.Enabled = true;
                DataGridView dgv = this.tabControl1.SelectedTab.Controls[0] as DataGridView;
                dgv.ReadOnly = true;
                this.добавитьСтрокуToolStripMenuItem.Enabled = false;
                this.удалитьСтрокуToolStripMenuItem.Enabled = false;
                dataTable = new DataTable();
                dataTable = (DataTable)dgv.DataSource;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        adapter = new SqlDataAdapter(filter.str, connection);
                        commandBuilder = new SqlCommandBuilder(adapter);
                        adapter.Fill(dt);
                        dgv.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ОчиститьФильтрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.очиститьФильтрыToolStripMenuItem.Enabled = false;

            DataGridView dgv = this.tabControl1.SelectedTab.Controls[0] as DataGridView;

            dgv.DataSource = dataTable;
            dgv.ReadOnly = false;
            dgv.AllowUserToAddRows = true;
            dgv.AllowUserToDeleteRows = true;

            this.добавитьСтрокуToolStripMenuItem.Enabled = true;
            this.удалитьСтрокуToolStripMenuItem.Enabled = true;
        }

        private void ОбновитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.tabControl1.TabPages)
            {
                DataGridView dgv =  control.Controls[0] as DataGridView;
                DataTable dataTable = new DataTable();
                SelectFromDB(control.Text, connectionString, dataTable);
                dgv.DataSource = dataTable;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timerleft > 0)
            {
                timerleft = timerleft - 1;
            }
            else
            {
                СохранитьToolStripMenuItem_Click(sender, e);
                timerleft = 120;
            }
        }

        private void настройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings(this);
            settings.ShowDialog();

            if (!settings.checkBox1.Checked)
            {
                check = false;
                timer1.Enabled = false;
                timer1.Stop();
                timerleft = 120;
            }
            else
            {
                check = true;
                timer1.Enabled = true;
                timer1.Start();
                timerleft = 120;
            }
        }
    }
}
