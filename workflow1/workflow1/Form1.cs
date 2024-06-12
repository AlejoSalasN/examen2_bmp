using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workflow1
{
    public partial class Form1 : Form
    {
        int rojo = 255;
        int verde = 255;
        int azul = 255;
        Bitmap originalImage;
        bool sw = true;

        int rojoOp = 0;
        int verdeOp = 0;
        int azulOp = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mostrar();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            sw = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            rojo = 255;
            verde = 255;
            azul = 255;
            openFileDialog1.Filter = "Archivos de imagen|*.png;*.jpg;*.jpeg";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                Bitmap bmp = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = bmp;
                originalImage = new Bitmap(bmp);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c;
            int sR;
            int sG;
            int sB;

            int tolerance = 10; // Define una tolerancia para la comparación de colores

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    sR = c.R;
                    sG = c.G;
                    sB = c.B;

                    // Comparar colores dentro de una tolerancia
                    if (Math.Abs(sR - rojo) <= tolerance && Math.Abs(sG - verde) <= tolerance && Math.Abs(sB - azul) <= tolerance)
                    {
                        rojoOp = Math.Abs(sR - 255);
                        verdeOp = Math.Abs(sG - 255);
                        azulOp = Math.Abs(sB - 255);
                        Color reemplazo = Color.FromArgb(255, rojoOp, verdeOp, azulOp);
                        bmp2.SetPixel(i, j, reemplazo);
                    }
                    else
                    {
                        bmp2.SetPixel(i, j, c);
                    }
                }
            }


            pictureBox1.Image = bmp2;
            
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (sw)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                Color color = new Color();
                int sR = 0;
                int sG = 0;
                int sB = 0;
                //int pixelCount = 0; // Variable para contar los píxeles válidos

                // Mapear las coordenadas del clic a las coordenadas de la imagen original
                float scaleX = (float)bmp.Width / pictureBox1.ClientSize.Width;
                float scaleY = (float)bmp.Height / pictureBox1.ClientSize.Height;

                int mappedX = (int)(e.X * scaleX);
                int mappedY = (int)(e.Y * scaleY);

                color = bmp.GetPixel(mappedX, mappedY);
                Console.WriteLine(bmp.Width + " " + bmp.Height);
                Console.WriteLine(pictureBox1.ClientSize.Width + " " + pictureBox1.ClientSize.Height);
                Console.WriteLine(e.X + " " + e.Y);
                Console.WriteLine(mappedX + " " + mappedY);
                Console.WriteLine("");
                sR = color.R;
                sG = color.G;
                sB = color.B;

                textBox1.Text = sR.ToString();
                textBox2.Text = sG.ToString();
                textBox3.Text = sB.ToString();
                textBox6.Text = Math.Abs(sR - 255).ToString();
                textBox7.Text = Math.Abs(sG - 255).ToString();
                textBox8.Text = Math.Abs(sB - 255).ToString();
                textBox10.Text = $"#{Math.Abs(sR-255):X2}{Math.Abs(sG-255):X2}{Math.Abs(sB-255):X2}";
                textBox9.Text = $"#{sR:X2}{sG:X2}{sB:X2}";
                rojo = int.Parse(sR.ToString());
                verde = int.Parse(sG.ToString());
                azul = int.Parse(sB.ToString());
            } else
            {
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                Color color = new Color();
                int sR = 0;
                int sG = 0;
                int sB = 0;
                float scaleX = (float)bmp.Width / pictureBox1.ClientSize.Width;
                float scaleY = (float)bmp.Height / pictureBox1.ClientSize.Height;

                int mappedX = (int)(e.X * scaleX);
                int mappedY = (int)(e.Y * scaleY);

                color = bmp.GetPixel(mappedX, mappedY);
                sR = color.R;
                sG = color.G;
                sB = color.B;

                textBox1.Text = sR.ToString();
                textBox2.Text = sG.ToString();
                textBox3.Text = sB.ToString();
                textBox6.Text = Math.Abs(sR - 255).ToString();
                textBox7.Text = Math.Abs(sG - 255).ToString();
                textBox8.Text = Math.Abs(sB - 255).ToString();
                textBox10.Text = $"#{Math.Abs(sR - 255):X2}{Math.Abs(sG - 255):X2}{Math.Abs(sB - 255):X2}";
                textBox9.Text = $"#{sR:X2}{sG:X2}{sB:X2}";
                rojo = int.Parse(sR.ToString());
                verde = int.Parse(sG.ToString());
                azul = int.Parse(sB.ToString());

                string query = $"SELECT color, colorN FROM texturas WHERE rojo BETWEEN {int.Parse(sR.ToString())-10} AND {int.Parse(sR.ToString()) + 10} AND verde BETWEEN {int.Parse(sG.ToString()) - 10} AND {int.Parse(sG.ToString()) + 10} AND azul BETWEEN {int.Parse(sB.ToString()) - 10} AND {int.Parse(sB.ToString()) + 10}";

                try
                {
                    using (OdbcConnection con = new OdbcConnection("DSN=examen2"))
                    {
                        // Usar el DSN configurado
                        con.Open();

                        using (OdbcCommand cmd = new OdbcCommand(query, con))
                        {

                            using (OdbcDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string colors = reader["color"].ToString();
                                    string description = reader["colorN"].ToString();
                                    textBox4.Text = colors;
                                    textBox5.Text = description;
                                }
                                else
                                {
                                    textBox4.Text = "no hay datos";
                                    textBox5.Text = "no hay datos";
                                }
                            }
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    MessageBox.Show("Ocurrió un error en la base de datos: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error inesperado: " + ex.Message);
                }
            }
            
        }



        private void button3_Click(object sender, EventArgs e)
        {
            rojo = 255;
            verde = 255;
            azul = 255;
            if (sw)
            {
                if (originalImage != null)
                {
                    pictureBox1.Image = new Bitmap(originalImage); // Restaurar la imagen original
                }
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rojo = 255;
            verde = 255;
            azul = 255;

            if (sw)
            {
                
                try
                {
                    using (OdbcConnection con = new OdbcConnection("DSN=examen2"))
                    {

                        string sR = textBox1.Text;
                        string sG = textBox2.Text;
                        string sB = textBox3.Text;

                        // Verificar si los datos ya existen con tolerancia de 10
                        int intSR = int.Parse(sR);
                        int intSG = int.Parse(sG);
                        int intSB = int.Parse(sB);
                        con.Open();

                        string checkQuery = $"SELECT COUNT(*) FROM texturas WHERE rojo BETWEEN {intSR - 10} AND {intSR + 10} AND verde BETWEEN {intSG - 10} AND {intSG + 10} AND azul BETWEEN {intSB - 10} AND {intSB + 10}";
                        using (OdbcCommand checkCmd = new OdbcCommand(checkQuery, con))
                        {
                            using (OdbcDataReader reader = checkCmd.ExecuteReader())
                            {
                                if (reader.Read() && (reader.GetInt32(0) > 0))
                                {
                                    MessageBox.Show("Los datos de color ya existen en la base de datos.");
                                    return;
                                } else
                                {
                                    string insertQuery = $"INSERT INTO texturas (hexadecimal, rojo, verde, azul, color, hexadecimalN, rojoN, verdeN, azulN, colorN) VALUES ('{textBox9.Text}', '{sR}', '{sG}', '{sB}', '{textBox4.Text}', '{textBox10.Text}', '{Math.Abs(intSR-255)}', '{Math.Abs(intSG - 255)}', '{Math.Abs(intSB - 255)}', '{textBox5.Text}')";

                                    using (OdbcCommand insertCmd = new OdbcCommand(insertQuery, con))
                                    {
                                        insertCmd.ExecuteNonQuery();
                                        MessageBox.Show("Datos insertados correctamente.");
                                        mostrar(); // Actualizar la vista
                                    }
                                }
                            }
                        }

                        //string insertQuery = $"INSERT INTO texturas (descripcion, cR, cG, cB, color) VALUES ({textBox4.Text}, {sR}, {sG}, {sB}, {textBox5.Text})";

                        con.Close();
                    }
                }
                catch (OdbcException ex)
                {
                    MessageBox.Show("Ocurrió un error en la base de datos: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error inesperado: " + ex.Message);
                }
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
            } else
            {
                Console.WriteLine("juasjuas");
            }
        }

        public void mostrar()
        {
            try
            {
                using (OdbcConnection con = new OdbcConnection())
                {
                    // Usar el DSN configurado
                    con.ConnectionString = "DSN=examen2";

                    using (OdbcDataAdapter ada = new OdbcDataAdapter())
                    {
                        ada.SelectCommand = new OdbcCommand();
                        ada.SelectCommand.Connection = con;
                        ada.SelectCommand.CommandText = "SELECT hexadecimal, rojo, verde, azul, color, hexadecimalN, rojoN, verdeN, azulN, colorN FROM texturas";
                        DataSet ds = new DataSet();
                        ada.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                    }
                }
            }
            catch (OdbcException ex)
            {
                MessageBox.Show("Ocurrió un error en la base de datos: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sw = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            rojo = 255;
            verde = 255;
            azul = 255;
            openFileDialog1.Filter = "Archivos de imagen|*.png;*.jpg;*.jpeg";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                Bitmap bmp = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = bmp;
                originalImage = new Bitmap(bmp);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
