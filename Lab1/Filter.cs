using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    
    public partial class Filter : Form
    {

        Form1 form1;
        int count;
        public string str;
        string[] mass;
        DataGridView dtg;
        public bool buttonClick;

        public Filter(Form1 fr1)
        {
            InitializeComponent();

            form1 = fr1;

            count = 0;

            button2.Enabled = false;

            dtg = form1.tabControl1.SelectedTab.Controls[0] as DataGridView;

            label1.Text = "Введите условие для столбца  <<" + dtg.Columns[0].Name + ">>:";

            mass = new string[dtg.Columns.Count];
        }

        private void SearchColumn(int index)
        {
            if (dtg.Columns[index].Name != "")
                label1.Text = "Введите условие для столбца  <<" + dtg.Columns[index].Name + ">>:";


            if (mass[index] != null)
            {
                textBox1.Text = mass[index];
            }
            else
            {
                textBox1.Text = "";
            }
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
            if (count == dtg.Columns.Count - 1)
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
            int k = -1;
            str = "SELECT * FROM [" + form1.tabControl1.SelectedTab.Text + "]";
            for (int i = 0; i < mass.Length; i++)
            {
                if (mass[i] != null && mass[i] != "")
                {
                    k = i;
                }
            }
            if (k != -1)
            {
                str += " WHERE ";
                for (int i = 0; i < mass.Length; i++)
                {
                    if (mass[i] != null && mass[i] != "")
                    {
                        if (i == k)
                        {
                            str += "[" + dtg.Columns[i].Name + "]" + " = '" + mass[i] + "'";
                        }
                        else
                        {
                            str += "[" + dtg.Columns[i].Name + "]" + " = '" + mass[i] + "' AND ";
                        }
                    }
                }
                this.Close();
            }
            
        }

        private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1) Можно переходить по условиям с помощью кнопок на форме.\n2) После заполнения информации необходимо нажать на кнопку <<Подтвердить>>.", "Справка", MessageBoxButtons.OK);
        }
    }
}
