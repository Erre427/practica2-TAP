using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Practica1
{
    public partial class Form1 : Form
    {
        private int velocidadX = 5;
        private int velocidadY = 5;
        private List<Button> botones = new List<Button>();
        private Dictionary<Button, Point> velocidadesBotones = new Dictionary<Button, Point>();
        private static int ventanaContador = 1;
        private Random rnd = new Random();
        private static List<Form2> ventanasClonadas = new List<Form2>();
        private Timer botonTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            Text = "Practica1";
            BackColor = Color.Gray;

            botonTimer.Interval = 50; // Menor intervalo para movimiento más fluido
            botonTimer.Tick += MoverBotones;
            botonTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Mover el PictureBox
            pictureBox1.Left += velocidadX;
            pictureBox1.Top += velocidadY;

            // Verificar colisión con los bordes del formulario
            if (pictureBox1.Left <= 0) // Pared izquierda
            {
                velocidadX = -velocidadX;
                BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                pictureBox1.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                ventanaContador = Math.Max(1, ventanaContador - 1);
                CerrarVentanasClonadas();
            }
            if (pictureBox1.Right >= this.ClientSize.Width) // Pared derecha
            {
                velocidadX = -velocidadX;
                AbrirNuevaVentana();
            }
            if (pictureBox1.Top <= 0) // Pared superior
            {
                velocidadY = -velocidadY;
                // Eliminar un botón si hay alguno
                if (botones.Count > 0)
                {
                    Button btn = botones[botones.Count - 1];
                    this.Controls.Remove(btn);
                    botones.RemoveAt(botones.Count - 1);
                    velocidadesBotones.Remove(btn);
                }
            }
            if (pictureBox1.Bottom >= this.ClientSize.Height) // Pared inferior
            {
                velocidadY = -velocidadY;
                AgregarBoton();
            }
            // Esquina superior izquierda
            if (pictureBox1.Left <= 30 && pictureBox1.Top <= 30)
            {
                this.Size = new Size(this.Width + 10, this.Height + 10);
                foreach (Button btn in botones)
                {
                    this.Controls.Remove(btn);
                }
                botones.Clear();
                velocidadesBotones.Clear();
                pictureBox1.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
            // Esquina inferior izquierda
            if (pictureBox1.Left <= 30 && pictureBox1.Bottom >= this.ClientSize.Height - 30)
            {
                this.Size = new Size(this.Width - 10, this.Height - 10);
                velocidadX = Math.Max(1, velocidadX - 1);
                velocidadY = Math.Max(1, velocidadY - 1);
            }
            // Esquina inferior derecha
            if (pictureBox1.Right >= this.ClientSize.Width - 30 && pictureBox1.Bottom >= this.ClientSize.Height - 30)
            {
                pictureBox1.Size = new Size(Math.Max(10, pictureBox1.Width - 5), Math.Max(10, pictureBox1.Height - 5));
                CerrarVentanasClonadas();
            }

            foreach (Button btn in botones)
            {
                if (CheckCollision(pictureBox1, btn))
                {
                    // Acción cuando hay colisión
                    pictureBox1.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    velocidadX = -velocidadX;
                    velocidadY = -velocidadY;
                }
            }
        }

        private bool CheckCollision(Control a, Control b)
        {
            return a.Bounds.IntersectsWith(b.Bounds);
        }

        private void AgregarBoton()
        {
            Button btn = new Button();
            btn.Text = "Btn";
            btn.Size = new Size(50, 30);
            btn.Location = new Point(rnd.Next(ClientSize.Width - 50), rnd.Next(ClientSize.Height - 30));
            botones.Add(btn);
            this.Controls.Add(btn);

            // Asignar velocidad inicial aleatoria al botón
            velocidadesBotones[btn] = new Point(rnd.Next(3, 7) * (rnd.Next(2) == 0 ? 1 : -1),
                                                 rnd.Next(3, 7) * (rnd.Next(2) == 0 ? 1 : -1));
        }

        private void MoverBotones(object sender, EventArgs e)
        {
            for (int i = 0; i < botones.Count; i++)
            {
                Button btn = botones[i];
                Point velocidad = velocidadesBotones[btn];

                int newX = btn.Left + velocidad.X;
                int newY = btn.Top + velocidad.Y;

                // Rebotar en los bordes
                if (newX <= 0 || newX + btn.Width >= ClientSize.Width)
                    velocidadesBotones[btn] = new Point(-velocidad.X, velocidad.Y);
                if (newY <= 0 || newY + btn.Height >= ClientSize.Height)
                    velocidadesBotones[btn] = new Point(velocidad.X, -velocidad.Y);

                // Mover botón con velocidad ajustada
                btn.Left += velocidadesBotones[btn].X;
                btn.Top += velocidadesBotones[btn].Y;
            }
        }

        private void AbrirNuevaVentana()
        {
            if (ventanaContador < 10) // Limitar cantidad de ventanas
            {
                ventanaContador++;
                Form2 nuevaVentana = new Form2(); // Crear una nueva ventana de Form2
                nuevaVentana.Size = this.Size;
                nuevaVentana.Text = "Nueva Ventana";
                nuevaVentana.Show();
                ventanasClonadas.Add(nuevaVentana);
            }
        }

        private void CerrarVentanasClonadas()
        {
            foreach (var ventana in ventanasClonadas.ToList())
            {
                ventana.Close();
            }
            ventanasClonadas.Clear();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
