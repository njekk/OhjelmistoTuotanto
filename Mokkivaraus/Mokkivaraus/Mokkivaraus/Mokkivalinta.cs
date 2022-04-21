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

namespace Mokkivaraus
{
    public partial class frmMokkivalinta : Form
    {
        private static MySqlConnection connection;
        private static MySqlCommand cmd = null;
        private static DataTable dt;
        private static MySqlDataAdapter sda;
        public frmMokkivalinta()
        {
            InitializeComponent();
        }
        public void populateDGV()
        {
            string query = "SELECT * FROM mokki";
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
            adapter.Fill(table);
            dgwMokkivalinta.DataSource = table;
        }
        private void btnVaraaM_Click(object sender, EventArgs e)
        {
            string insertquery = "INSERT INTO posti(postinro, toimipaikka) VALUES('70780','Kuopio')";
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(insertquery, connection);
            adapter.Fill(table);
            dgwMokkivalinta.DataSource = table;
            populateDGV();
        }

        private void frmMokkivalinta_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "127.0.0.1";
                builder.Port = 3306;
                builder.UserID = "root";
                builder.Password = "Ruutti";
                builder.Database = "Mokkivaraus";
                builder.SslMode = MySqlSslMode.None;
                connection = new MySqlConnection(builder.ToString());
                MessageBox.Show("Database connection successfull", "Connection", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("connection failed" + ex);
            }
            populateDGV();
        }

        private void btnAsiakkaisiin_Click(object sender, EventArgs e)
        {
            frmTiedot at = new frmTiedot();
            at.Show();
        }

        private void chkPaikanP_CheckedChanged(object sender, EventArgs e)
        {
            if(chkPaikanP.Checked == true)
            {
                chkLasku.Checked = false;
            }
        }

        private void chkLasku_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLasku.Checked == true)
            {
                chkPaikanP.Checked = false;
            }
        }
    }
}
