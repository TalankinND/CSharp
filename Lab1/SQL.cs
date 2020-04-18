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
    public partial class SQL : Form
    {
        DataTable ds = new DataTable();
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public object keyWordIndex = "3";
        public SQL()
        {
            InitializeComponent();
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text != "" && this.richTextBox1.Text != null)
            {
                выполнитьЗапросToolStripMenuItem.Enabled = true;
            }
            else
            {
                выполнитьЗапросToolStripMenuItem.Enabled = false;
            }
        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ОчиститьПолеВводаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }

        private void ВыполнитьЗапросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuaeryName qn = new QuaeryName(this);
            if (this.richTextBox1.Text != "")
            {
                
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        adapter = new SqlDataAdapter(this.richTextBox1.Text, connection);
                        commandBuilder = new SqlCommandBuilder(adapter);
                        adapter.Fill(ds);
                    }
                        if (this.richTextBox1.Text.ToLower().Contains("select") && (!this.richTextBox1.Text.ToLower().Contains("create") || this.richTextBox1.Text.ToLower().Contains("insert") || this.richTextBox1.Text.ToLower().Contains("delete") || this.richTextBox1.Text.ToLower().Contains("alter") || this.richTextBox1.Text.ToLower().Contains("update")))
                        {
                            qn.ShowDialog();
                            if (qn.str != "" && ds.Rows.Count > 0)
                            {
                                DataGridView dataGridView = new DataGridView();
                                TabPage tabPage = new TabPage(qn.str);
                                dataGridView.Dock = DockStyle.Fill;
                                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                                dataGridView.DataSource = ds;
                                dataGridView.AllowUserToDeleteRows = false;
                                dataGridView.AllowUserToAddRows = false;
                                dataGridView.ReadOnly = true;
                                tabPage.Controls.Add(dataGridView);
                                this.tabControl1.Controls.Add(tabPage);
                                ds = new DataTable();
                            }
                            this.tabControl1.ContextMenuStrip = this.contextMenuStrip1;
                        }
                    
                    MessageBox.Show("Запрос выполнен успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите запрос в поле ввода","Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SQL_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void УдалитьВкладкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

        private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, @"C:\ProjectHtmlHelp\ProjectHelpTalankin.chm", HelpNavigator.KeywordIndex, keyWordIndex);
        }
    }
}
