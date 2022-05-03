﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;


namespace Mokkivaraus
{
    public partial class frmAsiakastiedot : Form
    {
        private static MySqlConnection connection;
        private static MySqlCommand cmd = null;
        private static DataTable dt;
        private static MySqlDataAdapter sda;
        public string IP, Tietonimi, ID, Port;
        int i = 4;
        public frmAsiakastiedot()
        {
            using (StreamReader read = new StreamReader("C:\\Temp\\Asiakastiedot.txt"))
            {
                IP = read.ReadLine();
                Port = read.ReadLine();
                Tietonimi = read.ReadLine();
                ID = read.ReadLine();
            }
            InitializeComponent();
        }

        private void frmAsiakastiedot_Load(object sender, EventArgs e)
        {
            uint portparsed;
            portparsed = uint.Parse(Port);
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = IP;
                builder.Port = portparsed;
                builder.UserID = ID;
                builder.Password = "Ruutti";
                builder.Database = Tietonimi;
                builder.SslMode = MySqlSslMode.None;
                connection = new MySqlConnection(builder.ToString());
                //MessageBox.Show("Database connection successfull", "Connection", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("connection failed" + ex);
            }
            populateDGV();
        }
        private void poisto()
        {
            txtEtu.Clear();
            txtSuku.Clear();
            txtPostiO.Clear();
            cbPostiN.Text = "";
            txtPostiP.Clear();
            txtSahko.Clear();
            txtPuhelin.Clear();
        }
        private void btnVarauksiin_Click(object sender, EventArgs e)
        {
            populateDGV();
            frmMokkivalinta valinnat = new frmMokkivalinta(); // tähän täytyy tehdä postinumeron tarkistus saadaan vanhasta työstä jos numeroa ei löydy se lisätään niin myös henkilöön kuin postiin
            valinnat.Show();
        }
        public void ExecuteMyQuery(string query)
        {
            //tarkastetaan onko kysely mennyt läpi
            try
            {
                OpenConnection();
                cmd = new MySqlCommand(query, connection);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    //MessageBox.Show("Kysely suoritettu");
                }
                else
                {
                    MessageBox.Show("Kyselyä ei suoritettu");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public void populateDGV()
        {
            string query = "SELECT * FROM asiakas";
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
            adapter.Fill(table);
            dgvAsiakkaat.DataSource = table;
        }

        private void btnAsiakkaat_Click(object sender, EventArgs e)
        {
            frmTiedot asiakkaat = new frmTiedot();
            asiakkaat.Show();
        }

        private void chkYksityinen_CheckedChanged(object sender, EventArgs e)
        {
            if(chkYksityinen.Checked == true)
            {
                chkYritys.Checked = false;
            }
        }

        private void chkYritys_CheckedChanged(object sender, EventArgs e)
        {
            if (chkYritys.Checked == true)
            {
                chkYksityinen.Checked = false;
            }
        }

        private void frmAsiakastiedot_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult Result = MessageBox.Show("Haluatko poistua ilman tietojen tallennusta?", "Olet poistumassa tallentamatta!", MessageBoxButtons.YesNo);
            if (Result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (Result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnLissee_Click(object sender, EventArgs e)
        {
            string insertQuery2 = "INSERT INTO posti(postinro,toimipaikka) VALUES('" + cbPostiN.Text + "','" + txtPostiP.Text + "')";
            ExecuteMyQuery(insertQuery2);
            string insertQuery = "INSERT INTO asiakas(etunimi,sukunimi,lahiosoite,sahkoposti,puhelinnro,postinro) VALUES('" + txtEtu.Text + "','" + txtSuku.Text + "','" + txtPostiO.Text + "','" + txtSahko.Text + "','" + txtPuhelin.Text + "','" + cbPostiN.Text + "')";
            ExecuteMyQuery(insertQuery);
            populateDGV();
            poisto();
        }

        private void btnPoista_Click(object sender, EventArgs e)
        {

        }
    }
}
