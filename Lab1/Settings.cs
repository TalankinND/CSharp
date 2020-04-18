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
    public partial class Settings : Form
    {
        public bool check;
        public Form1 frr1;
        public Settings(Form1 fr1)
        {
            InitializeComponent();
            frr1 = fr1;
            check = checkBox1.Checked;
            
            if (check != frr1.check)
                checkBox1.Checked = frr1.check;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            check = checkBox1.Checked;
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В данном окне отображены настройки приложения", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
