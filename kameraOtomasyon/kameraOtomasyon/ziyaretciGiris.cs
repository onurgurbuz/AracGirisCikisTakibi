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
    public partial class ziyaretciGiris : Form
    {
        static string conString = "server=localhost; Initial Catalog=dbName;User ID=id;Password=pass;";
        SqlConnection myConnection = new SqlConnection(conString);
        String cikisiYapilacakZiyaretci = "";
        public ziyaretciGiris()
        {
            InitializeComponent();
        }

        private void ziyaretciGiris_Load(object sender, EventArgs e)
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


            Timer timer1 = new Timer();
            timer1.Interval = 60000;//1 dakika
            timer1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            textBox4.Text = DateTime.Now.ToShortTimeString();
            textBox3.Text = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" || textBox2.Text != "" || textBox3.Text != "" || textBox4.Text != "" || textBox5.Text != "" || textBox6.Text != "" || textBox7.Text != "")
                girisYap();
            else
                MessageBox.Show("Lütfen tüm alanları doldurunuz!");
            verileriGetir();
        }
        bool butonTik = false;
        private void btnEdit1_Click(object sender, EventArgs e)
        {
            if (butonTik == false)
            {
                butonTik = true;
                textBox3.Enabled = true;
            }
            else
            {
                butonTik = false;
                textBox3.Enabled = false;
            }
        }
        bool butonTik1 = false;
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (butonTik1 == false)
            {
                butonTik1 = true;
                textBox4.Enabled = true;
            }
            else
            {
                butonTik1 = false;
                textBox4.Enabled = false;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                cikisiYapilacakZiyaretci = row.Cells[1].Value.ToString();
            }
            label9.Text = cikisiYapilacakZiyaretci;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string girisTarihi = "";
            DialogResult cikis = new DialogResult();
            cikis = MessageBox.Show("Devam etmek istiyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo);
            if (cikis == DialogResult.Yes)
            {
                try
                {
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        girisTarihi = row.Cells[3].Value.ToString().Substring(row.Cells[3].Value.ToString().LastIndexOf(".") + 1, 4) + "-" + row.Cells[3].Value.ToString().Substring(row.Cells[3].Value.ToString().IndexOf(".") + 1, row.Cells[3].Value.ToString().LastIndexOf(".") - row.Cells[3].Value.ToString().IndexOf(".") - 1).Trim('.') + "-" + row.Cells[3].Value.ToString().Substring(0, row.Cells[3].Value.ToString().IndexOf(".") + 1).Trim('.');
                        cikisHesapla(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), girisTarihi, row.Cells[4].Value.ToString(), row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString(), row.Cells[7].Value.ToString());
                        cikisYap(row.Cells[0].Value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("Hata: " + ex);
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            verileriGetir();
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
            dataGridView1.Columns["ziyaretEdilen"].HeaderText = "ZİYARET EDİLEN YER";
            dataGridView1.Columns["ziyaretciKartNo"].HeaderText = "ZİYARETCİ KART NO";
            dataGridView1.Columns["plaka"].HeaderText = "PLAKA";
        }

        public void girisYap()
        {
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("insert into ziyaretciGiris (adi,tcEhliyet,girisTarihi,girisSaati,ziyaretEdilen,ziyaretciKartNo,plaka) values(@parameter1,@parameter2,@parameter3,@parameter4,@parameter5,@parameter6,@parameter7)", myConnection);
                cmd.Parameters.AddWithValue("@parameter1", (textBox1.Text).TrimEnd());
                cmd.Parameters.AddWithValue("@parameter2", textBox2.Text.TrimEnd());
                cmd.Parameters.AddWithValue("@parameter3", textBox3.Text);
                cmd.Parameters.AddWithValue("@parameter4", textBox4.Text);
                cmd.Parameters.AddWithValue("@parameter5", textBox5.Text);
                cmd.Parameters.AddWithValue("@parameter6", textBox6.Text);
                cmd.Parameters.AddWithValue("@parameter7", textBox7.Text);
                cmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }
        }

        public void cikisHesapla(string id, string adi, string tcEhliyet, string girisTarihi, string girisSaati, string ziyaretEdilen, string ziyaretciKartNo, string plaka)
        {
            string cikisTarihi = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
            string cikisSaati = DateTime.Now.ToShortTimeString();

            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("insert into ziyaretciCikis (adi,tcEhliyet,girisTarihi,girisSaati,cikisTarihi,cikisSaati,ziyaretEdilen,ziyaretciKartNo,plaka) values(@parameter1,@parameter2,@parameter3,@parameter4,@parameter5,@parameter6,@parameter7,@parameter8,@parameter9)", myConnection);
                cmd.Parameters.AddWithValue("@parameter1", adi.TrimEnd());
                cmd.Parameters.AddWithValue("@parameter2", tcEhliyet.TrimEnd());
                cmd.Parameters.AddWithValue("@parameter3", girisTarihi);
                cmd.Parameters.AddWithValue("@parameter4", girisSaati);
                cmd.Parameters.AddWithValue("@parameter5", cikisTarihi);
                cmd.Parameters.AddWithValue("@parameter6", cikisSaati);
                cmd.Parameters.AddWithValue("@parameter7", ziyaretEdilen.TrimEnd());
                cmd.Parameters.AddWithValue("@parameter8", ziyaretciKartNo.TrimEnd());
                cmd.Parameters.AddWithValue("@parameter9", plaka.TrimEnd());
                cmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }
            string sonucSaat = "", sonucTarih = "";
            try
            {
                if (girisTarihi.Equals(cikisTarihi))
                {
                    sonucSaat = (Convert.ToDateTime(cikisSaati) - Convert.ToDateTime(girisSaati)).ToString();

                }
                else
                {
                    sonucTarih = (Convert.ToDateTime(cikisTarihi) - Convert.ToDateTime(girisTarihi)).ToString();
                    sonucSaat = (Convert.ToDateTime(cikisSaati) - Convert.ToDateTime(girisSaati)).ToString();

                }
                MessageBox.Show(label9.Text + " adlı ziyaretçinin çıkışı yapıldı. " + "\nBekleme Süresi: " +
                    sonucTarih + " gün, " +
                    sonucSaat + " saat/dakika, ");
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }
        }

        public void cikisYap(string id)
        {
            try
            {
                myConnection.Open();
                SqlCommand kmt = new SqlCommand("DELETE ziyaretciGiris where id=" + id, myConnection);
                kmt.ExecuteNonQuery();
                myConnection.Close();
                verileriGetir();
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }
        }
    }
}
