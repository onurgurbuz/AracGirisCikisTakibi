using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kameraOtomasyon
{
    public partial class girisDuzenle : Form
    {
        static string conString = "server=localhost; Initial Catalog=dbName;User ID=id;Password=pass;";
        SqlConnection myConnection = new SqlConnection(conString);
        public girisDuzenle()
        {
            InitializeComponent();
        }

        private void girisDuzenle_Load(object sender, EventArgs e)
        {
            verileriGetir();
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[5].Width = 200;
            dataGridView1.Columns[6].Width = 200;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            string id = "", plaka = "", girisSaati = "", girisTarihi = "", geldigiYer = "", soforAdi = "";
            int girisKm = 0;

            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    id = row.Cells[0].Value.ToString();
                    plaka = row.Cells[1].Value.ToString();
                    girisSaati = row.Cells[2].Value.ToString().Substring(0, 5);
                    girisTarihi = row.Cells[3].Value.ToString().Substring(row.Cells[3].Value.ToString().LastIndexOf(".") + 1, 4) + "-" + row.Cells[3].Value.ToString().Substring(row.Cells[3].Value.ToString().IndexOf(".") + 1, row.Cells[3].Value.ToString().LastIndexOf(".") - row.Cells[3].Value.ToString().IndexOf(".") - 1).Trim('.') + "-" + row.Cells[3].Value.ToString().Substring(0, row.Cells[3].Value.ToString().IndexOf(".") + 1).Trim('.');
                    girisKm = Convert.ToInt32(row.Cells[4].Value.ToString());
                    geldigiYer = row.Cells[5].Value.ToString();
                    soforAdi = row.Cells[6].Value.ToString();
                }

                textBox1.Text = id;
                textBox2.Text = plaka.TrimEnd();
                textBox3.Text = girisSaati;
                textBox4.Text = girisTarihi;
                textBox5.Text = girisKm.ToString();
                textBox6.Text = geldigiYer.TrimEnd();
                textBox7.Text = soforAdi.TrimEnd();
            }
            catch (Exception ex)
            {
                Console.Write("Hata: " + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            verileriGuncelle(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, Convert.ToInt32(textBox5.Text), textBox6.Text, textBox7.Text);
        }
        public void verileriGetir()
        {
            try
            {
                myConnection.Open();
                string kayit = "SELECT * from giris";
                SqlCommand komut = new SqlCommand(kayit, myConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Hata: " + ex);
            }
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["plaka"].HeaderText = "PLAKA";
            dataGridView1.Columns["girisSaati"].HeaderText = "GİRİŞ SAATİ";
            dataGridView1.Columns["girisTarihi"].HeaderText = "GİRİŞ TARİHİ";
            dataGridView1.Columns["girisKm"].HeaderText = "GİRİŞ KM";
            dataGridView1.Columns["geldigiYer"].HeaderText = "GELDİĞİ YER";
            dataGridView1.Columns["soforAdi"].HeaderText = "ŞOFÖR ADI";
        }

        public void verileriGuncelle(string id, string plaka, string girisSaati, string girisTarihi, int girisKm, string geldigiYer, string soforAdi)
        {
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("Update giris set plaka='" + plaka + "',girisSaati='" + girisSaati + "',girisTarihi='" + girisTarihi + "',girisKm=" + girisKm + " ,geldigiYer='" + geldigiYer + "',soforAdi='" + soforAdi + "' where id =" + id + "", myConnection);
                cmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }
            verileriGetir();
        }
    }
}
