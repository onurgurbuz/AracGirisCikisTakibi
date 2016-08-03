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
    public partial class ziyaretciCikisDuzenle : Form
    {
        static string conString = "server=localhost; Initial Catalog=dbName;User ID=id;Password=pass;";
        SqlConnection myConnection = new SqlConnection(conString);
        public ziyaretciCikisDuzenle()
        {
            InitializeComponent();
        }

        private void ziyaretciCikisDuzenle_Load(object sender, EventArgs e)
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
            dataGridView1.Columns[8].Width = 200;
            dataGridView1.Columns[9].Width = 200;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            string id = "", adi = "", tcEhliyet = "", girisTarihi = "", girisSaati = "", cikisTarihi = "", cikisSaati = "", ziyaretEdilen = "", ziyaretciKartNo = "", plaka = "";

            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    id = row.Cells[0].Value.ToString();
                    adi = row.Cells[1].Value.ToString();
                    tcEhliyet = row.Cells[2].Value.ToString();
                    girisTarihi = row.Cells[3].Value.ToString().Substring(row.Cells[3].Value.ToString().LastIndexOf(".") + 1, 4) + "-" + row.Cells[3].Value.ToString().Substring(row.Cells[3].Value.ToString().IndexOf(".") + 1, row.Cells[3].Value.ToString().LastIndexOf(".") - row.Cells[3].Value.ToString().IndexOf(".") - 1).Trim('.') + "-" + row.Cells[3].Value.ToString().Substring(0, row.Cells[3].Value.ToString().IndexOf(".") + 1).Trim('.');
                    girisSaati = row.Cells[4].Value.ToString().Substring(0, 5);
                    cikisTarihi = row.Cells[5].Value.ToString().Substring(row.Cells[5].Value.ToString().LastIndexOf(".") + 1, 4) + "-" + row.Cells[5].Value.ToString().Substring(row.Cells[5].Value.ToString().IndexOf(".") + 1, row.Cells[5].Value.ToString().LastIndexOf(".") - row.Cells[5].Value.ToString().IndexOf(".") - 1).Trim('.') + "-" + row.Cells[5].Value.ToString().Substring(0, row.Cells[5].Value.ToString().IndexOf(".") + 1).Trim('.');
                    cikisSaati = row.Cells[6].Value.ToString().Substring(0, 5);
                    ziyaretEdilen = row.Cells[7].Value.ToString();
                    ziyaretciKartNo = row.Cells[8].Value.ToString();
                    plaka = row.Cells[9].Value.ToString();
                }
                textBox1.Text = id.TrimEnd();
                textBox2.Text = adi.TrimEnd() ;
                textBox3.Text = tcEhliyet.TrimEnd();
                textBox4.Text = girisTarihi;
                textBox5.Text = girisSaati;
                textBox6.Text = cikisTarihi;
                textBox7.Text = cikisSaati;
                textBox8.Text = ziyaretEdilen.TrimEnd();
                textBox9.Text = ziyaretciKartNo.TrimEnd();
                textBox10.Text = plaka.TrimEnd();
            }
            catch (Exception ex)
            {
                Console.Write("Hata: " + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            verileriGuncelle(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text);
            verileriGetir();
        }
        public void verileriGetir()
        {
            try
            {
                myConnection.Open();
                string kayit = "SELECT * from ziyaretciCikis";
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
            dataGridView1.Columns["adi"].HeaderText = "ZİYARETÇİ ADI VE SOYADI";
            dataGridView1.Columns["tcEhliyet"].HeaderText = "T.C./EHLİYET NO";
            dataGridView1.Columns["girisTarihi"].HeaderText = "GİRİŞ TARİHİ";
            dataGridView1.Columns["girisSaati"].HeaderText = "GİRİŞ SAATİ";
            dataGridView1.Columns["cikisTarihi"].HeaderText = "ÇIKIŞ TARİHİ";
            dataGridView1.Columns["cikisSaati"].HeaderText = "ÇIKIŞ SAATİ";
            dataGridView1.Columns["ziyaretEdilen"].HeaderText = "ZİYARET EDİLEN YER";
            dataGridView1.Columns["ziyaretciKartNo"].HeaderText = "ZİYARETÇİ KART NO";
            dataGridView1.Columns["plaka"].HeaderText = "PLAKA";
        }

        public void verileriGuncelle(string id, string adi, string tcEhliyet, string girisTarihi, string girisSaati, string cikisTarihi, string cikisSaati, string ziyaretEdilen, string ziyaretciKartNo, string plaka)
        {
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("Update ziyaretciCikis set adi='" + adi + "',tcEhliyet='" + tcEhliyet + "',girisTarihi='" + girisTarihi + "',girisSaati='" + girisSaati + "',cikisTarihi='" + cikisTarihi + "',cikisSaati='" + cikisSaati + "' ,ziyaretciKartNo='" + ziyaretciKartNo + "',plaka='" + plaka + "' where id =" + id + "", myConnection);
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
