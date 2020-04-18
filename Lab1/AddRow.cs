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
    
    public partial class AddRow : Form
    {
        Form1 form1;
        int count;
        public string str;
        string[] mass;
        DataTable dataTable;
        DataGridView dtg;
        public bool buttonClick;
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private void SearchColumn(int index)
        {
            if (dataTable.Columns[index].ColumnName != "")
                label1.Text = "Введите условие для столбца  <<" + dataTable.Columns[index].ColumnName + ">>:";


            if (mass[index] != null)
            {
                textBox1.Text = mass[index];
            }
            else
            {
                textBox1.Text = "";
            }
        }

        public AddRow(Form1 fr1)
        {
            InitializeComponent();

            form1 = fr1;

            count = 0;

            button2.Enabled = false;

            dtg = form1.tabControl1.SelectedTab.Controls[0] as DataGridView;
            DataTable data = new DataTable();
            dataTable = new DataTable();

            data = (DataTable)dtg.DataSource;

            dataTable = data.Copy();

            dataTable.Columns.Remove(dataTable.Columns[form1.PKey]);

            
            label1.Text = "Введите условие для столбца  <<" + dataTable.Columns[count].ColumnName + ">>:";
            mass = new string[dataTable.Columns.Count];
        }

        private void ОтменаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonClick = true;
            this.Close();

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

            str = "";
            str = textBox1.Text;
            mass[count] = str;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            count++;
            if (count == dataTable.Columns.Count - 1)
            {
                button3.Enabled = false;
                button2.Enabled = true;
            }
            else
            {
                button3.Enabled = true;
                button2.Enabled = true;
            }
            SearchColumn(count);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            count--;
            if (count > 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = true;
                button2.Enabled = false;
            }
            SearchColumn(count);
        }

        private void ПодтвердитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            str = "INSERT INTO [" + form1.tabControl1.SelectedTab.Text + "](";
            int k = -1;
            for (int i = 0; i < mass.Length; i++)
            {
                if (mass[i] != null && mass[i] != "")
                {
                    k = i;
                }
            }
            if (k != -1)
            {
                for (int i = 0; i < mass.Length; i++)
                {
                    if (mass[i] != null && mass[i] != "")
                    {
                        if (k == i)
                        {
                            str += "[" + dataTable.Columns[i].ColumnName + "]";
                        }
                        else
                        {
                            str += "[" + dataTable.Columns[i].ColumnName + "],";
                        }
                    }

                }
                str += ") VALUES(";
                for (int i = 0; i < mass.Length; i++)
                {
                    if (mass[i] != null && mass[i] != "")
                    {
                        if (k == i)
                        {
                            str += "@param" + i;
                        }
                        else
                        {
                            str += "@param" + i + ", ";
                        }
                    }
                }
                str += ")";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = str;
                        for (int i = 0; i < mass.Length; i++)
                        {
                            if (mass[i] != "" && mass[i] != null)
                            {
                                cmd.Parameters.AddWithValue("@param" + i, mass[i]);
                            }
                        }
                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }

                    }
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Необходимо заполнить хотя бы одно поле.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Заполните хотя бы одно поле и нажмите <<Подтвердить>>, чтобы добавить строку.", "Справка", MessageBoxButtons.OK);
        }
    }
}
