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
    public partial class ziyaretciGirisDuzenle : Form
    {
        static string conString = "server=localhost; Initial Catalog=dbName;User ID=id;Password=pass;";
        SqlConnection myConnection = new SqlConnection(conString);
        public ziyaretciGirisDuzenle()
        {
            InitializeComponent();
        }

        private void ziyaretciGirisDuzenle_Load(object sender, EventArgs e)
        {
            verileriGetir();
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[5].Width = 200;
            dataGridView1.Columns[6].Width = 200;
            dataGridView1.Columns[7].Width = 200;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            string id = "", adi = "", tcEhliyet = "", girisTarihi = "", girisSaati = "", ziyaretEdilen = "", ziyaretciKartNo = "", plaka = "";

            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    id = row.Cells[0].Value.ToString();
                    adi = row.Cells[1].Value.ToString();
                    tcEhliyet = row.Cells[2].Value.ToString();
                    girisTarihi = row.Cells[3].Value.ToString().Substring(6, 4) + "-" + row.Cells[3].Value.ToString().Substring(3, 2) + "-" + row.Cells[3].Value.ToString().Substring(0, 2);
                    girisSaati = row.Cells[4].Value.ToString().Substring(0, 5);
                    ziyaretEdilen = row.Cells[5].Value.ToString();
                    ziyaretciKartNo = row.Cells[6].Value.ToString();
                    plaka = row.Cells[7].Value.ToString();
                }

                textBox1.Text = id.TrimEnd();
                textBox2.Text = adi.TrimEnd();
                textBox3.Text = tcEhliyet.TrimEnd();
                textBox4.Text = girisTarihi;
                textBox5.Text = girisSaati;
                textBox6.Text = ziyaretEdilen.TrimEnd();
                textBox7.Text = ziyaretciKartNo.TrimEnd();
                textBox8.Text = plaka.TrimEnd();
            }
            catch (Exception ex)
            {
                Console.Write("Hata: " + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            verileriGuncelle(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text);  
        }

        public void verileriGetir()
        {
            try
            {
                myConnection.Open();
                string kayit = "SELECT * from ziyaretciGiris";
                SqlCommand komut = new SqlCommand(kayit, myConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;                
            }
            catch (Exception ex)
            {
                Console.Write("Hata: " + ex);
            }
            finally
            {
                myConnection.Close();
            }
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["adi"].HeaderText = "ZİYARETÇİ ADI VE SOYADI";
            dataGridView1.Columns["tcEhliyet"].HeaderText = "T.C./EHLİYET NO";
            dataGridView1.Columns["girisTarihi"].HeaderText = "GİRİŞ TARİHİ";
            dataGridView1.Columns["girisSaati"].HeaderText = "GİRİŞ SAATİ";
            dataGridView1.Columns["ziyaretEdilen"].HeaderText = "ZİYARET EDİLEN YER";
            dataGridView1.Columns["ziyaretciKartNo"].HeaderText = "ZİYARETCİ KART NO";
            dataGridView1.Columns["plaka"].HeaderText = "PLAKA";
        }

        public void verileriGuncelle(string id, string adi, string tcEhliyet, string girisTarihi, string girisSaati, string ziyaretEdilen, string ziyaretciKartNo, string plaka)
        {
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("Update ziyaretciGiris set adi='" + adi + "',tcEhliyet='" + tcEhliyet + "',girisTarihi='" + girisTarihi + "',girisSaati='" + girisSaati + "',ziyaretEdilen='" + ziyaretEdilen + "',ziyaretciKartNo='" + ziyaretciKartNo + "',plaka='" + plaka + "' where id =" + id + "", myConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }
            finally
            {
                myConnection.Close();
                verileriGetir();
            }            
        }
    }
}
