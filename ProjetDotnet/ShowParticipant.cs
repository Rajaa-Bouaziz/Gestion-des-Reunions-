﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetDotnet
{
    public partial class ShowParticipant : Form
    {
        //SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Ensak\C#\Projet\v3\ProjetDotnetProblem\ProjetDotnet\ProjetDotnet\DatabaseGestionReunion.mdf;Integrated Security=True");
        string startupPath1 = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "ProjetDotnet");

        SqlConnection connection;
        private int idUser = 2000;
        private int idReunion = -1;
        //private string type ="";
        public ShowParticipant()
        {
            InitializeComponent();
            this.connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + startupPath1 + "\\ProjetDotnet\\DatabaseGestionReunion.mdf;Integrated Security=True");

        }
        public ShowParticipant(int idUser, int idReunion, string type = "chef")
        {
            InitializeComponent();
            this.connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + startupPath1 + "\\ProjetDotnet\\DatabaseGestionReunion.mdf;Integrated Security=True");

            this.idUser = idUser;
            this.idReunion = idReunion;
            //this.type = type;
            /*if (type == "member")
            {
                Entete.Hide();
            }*/
        }

        private void ShowMember_Load(object sender, EventArgs e)
        {
            fillGrid();
        }





        private void fillGrid()
        {
            connection.Open();


            SqlCommand cmd1 = connection.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            //cmd1.CommandText = "select M.N_M,M.Nom,M.Prenom,M.mail,M.tel,M.photo from Equipe,Membre as M,Participant as P WHERE " +
            //    "Equipe.Id_equipe=M.id_equipe and P.N_M=M.N_M and Equipe.N_chef='" + idUser + "'";
            cmd1.CommandText = "select P.message ,P.etat, M.N_M,M.Nom,M.Prenom,M.mail,M.tel,M.photo from Membre as M,Participant as P WHERE " +
                " P.N_M=M.N_M and P.id_R='" + idReunion + "'";
            cmd1.ExecuteNonQuery();

            DataTable table = new DataTable();

            using (SqlDataReader reader = cmd1.ExecuteReader())
            {
                table.Columns.Add("Numero Membre");
                table.Columns.Add("Nom");
                table.Columns.Add("Prenom");

                table.Columns.Add("Mail");
                table.Columns.Add("Tel");
             




                //table.image.I

                //table.Columns.Add("Photo");

                DataGridViewImageColumn img = new DataGridViewImageColumn();
                metroGrid1.Columns.Add(img);

                //string nameImage = reader["photo"].ToString();
                //ProjetDotnet.images.
                string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "ProjetDotnet");

                img.ImageLayout = DataGridViewImageCellLayout.Zoom;

                List<string> nameImages = new List<string>();





                Image image = null;



                while (reader.Read())
                {



                    DataRow row = table.NewRow();
                    row["Numero Membre"] = reader["N_M"].ToString();
                    row["Nom"] = reader["Nom"];
                    //textBox1.Text= reader["devisN"].ToString();
                    row["Prenom"] = reader["Prenom"];

                    row["Mail"] = reader["mail"];

                    row["Tel"] = reader["tel"];
                  

                    // Read the file as one string. 
                    //string text = System.IO.File.ReadAllText(startupPath);
                    //Image image = Image.FromFile( nameImage);
                    //string fullPath=text+"images\"+nam
                    string nameImage = reader["photo"].ToString();
                    nameImages.Add(nameImage);



                    table.Rows.Add(row);


                }
                img.HeaderText = "Photo";
                img.Name = "Photo";


                metroGrid1.DataSource = table;
                int i;
                for (i = 0; i < nameImages.Count; i++)
                {
                    image = Image.FromFile(@"" + startupPath + "\\ImagesMembres\\" + nameImages[i]);
                    metroGrid1.Rows[i].Cells["Photo"].Value = image;
                }
                metroGrid1.Rows[i].Cells["Photo"].Value = null;
                /*image = Image.FromFile(@"" + startupPath + "\\images\\" + nameImages[0]);
                metroGrid1.Rows[0].Cells["Photo"].Value = image;
                image = Image.FromFile(@"" + startupPath + "\\images\\" + nameImages[1]);
                metroGrid1.Rows[1].Cells["Photo"].Value = image;*/

            }
            connection.Close();
        }
        private Form activeForm = null;
        public void openchildForm(Form childForm)
        {
            if (activeForm != null)

                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            corps.Controls.Add(childForm);
            corps.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

    

        private void corps_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
