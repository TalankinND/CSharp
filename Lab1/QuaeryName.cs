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
    
    public partial class QuaeryName : Form
    {
        SQL sql;
        public string str;
        public QuaeryName(SQL sq)
        {
            InitializeComponent();
            sql = sq;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            str = "";
            str = textBox1.Text;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (sql.tabControl1.TabCount > 0)
            {
                for (int i = 0; i < sql.tabControl1.TabCount; i++)
                {
                    if (str == sql.tabControl1.TabPages[i].Text)
                    {
                        MessageBox.Show("Такое название уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (str != "")
                        {
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Название не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                if (str != "")
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Название не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void QuaeryName_FormClosed(object sender, FormClosedEventArgs e)
        {
            sql.Show();
        }
    }
}
