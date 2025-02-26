using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practica1
{

    public partial class Form2 : Form
    {
        private int velocidadX = 5;
        private int velocidadY = 5;
        public Form2()
        {
            InitializeComponent();
            BackColor = Color.Gray;
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Mover el PictureBox
            pictureBox1.Left += velocidadX;
            pictureBox1.Top += velocidadY;

            // Verificar colisión con los bordes del formulario
            if (pictureBox1.Left <= 0 || pictureBox1.Right >= this.ClientSize.Width)
            {
                velocidadX = -velocidadX; // Invierte dirección en X
            }
            if (pictureBox1.Top <= 0 || pictureBox1.Bottom >= this.ClientSize.Height)
            {

                velocidadY = -velocidadY; // Invierte dirección en Y
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
