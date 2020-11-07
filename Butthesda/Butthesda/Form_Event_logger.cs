using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Butthesda
{
    public partial class Form_Event_logger : Form
    {
        public static Form_Event_logger form;

        public Form_Event_logger()
        {
            InitializeComponent();
            form = this;

        }

        internal void Write(string text)
        {
            Invoke((MethodInvoker)(() => richTextBox1.Text = text));
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
     
        }
    }
}
