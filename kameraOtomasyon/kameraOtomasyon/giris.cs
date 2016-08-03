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
    public partial class giris : Form
    {
        static string conString = "server=localhost; Initial Catalog=dbName;User ID=id;Password=pass;";
        SqlConnection myConnection = new SqlConnection(conString);
        String plaka = "";
        public giris()
        {
            InitializeComponent();
        }
        private void giris_Load(object sender, EventArgs e)
        {
            verileriGetir();
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[5].Width = 200;
            dataGridView1.Columns[6].Width = 200;

            Timer timer1 = new Timer();
            timer1.Interval = 60000;//1 dakika
            timer1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.ToShortTimeString();
            textBox3.Text = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" || textBox2.Text != "" || textBox3.Text != "" || textBox6.Text != "" || textBox7.Text != "" || textBox8.Text != "")
                girisYap();
            else
                MessageBox.Show("Lütfen tüm alanları doldurunuz!");
            verileriGetir();
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                plaka = row.Cells[1].Value.ToString();
            }
            label5.Text = plaka;
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

                        if (textBox4.Text != "" || textBox5.Text != "")
                        {
                            cikisHesapla(row.Cells[0].Value.ToString(), row.Cells[2].Value.ToString(), girisTarihi, Convert.ToInt32(row.Cells[4].Value.ToString()), row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString());
                            cikisYap(row.Cells[0].Value.ToString());
                        }
                        else
                            MessageBox.Show("Lütfen tüm alanları doldurunuz!");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("Hata :" + ex);
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
            dataGridView1.Columns["soforAdi"].HeaderText = "ŞOFÖR ADI VE SOYADI";
        }

        public void girisYap()
        {
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("insert into giris (plaka,girisSaati,girisTarihi,girisKm,geldigiYer,soforAdi) values(@parameter1,@parameter2,@parameter3,@parameter4,@parameter5,@parameter6)", myConnection);
                cmd.Parameters.AddWithValue("@parameter1", textBox1.Text);
                cmd.Parameters.AddWithValue("@parameter2", textBox2.Text);
                cmd.Parameters.AddWithValue("@parameter3", textBox3.Text);
                cmd.Parameters.AddWithValue("@parameter4", Convert.ToInt32(textBox6.Text));
                cmd.Parameters.AddWithValue("@parameter5", textBox7.Text);
                cmd.Parameters.AddWithValue("@parameter6", textBox8.Text);
                cmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }
        }

        public void cikisHesapla(string id, string girisSaati, string girisTarihi, int girisKm, string geldigiYer, string soforAdi)
        {
            string cikisTarihi = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
            string cikisSaati = DateTime.Now.ToShortTimeString();
            int cikisKm = Convert.ToInt32(textBox4.Text);
            string gittigiYer = textBox5.Text;
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("insert into cikis (plaka,girisSaati,girisTarihi,girisKm,geldigiYer,cikisSaati,cikisTarihi,cikisKm,gittigiYer,soforAdi) values(@parameter1,@parameter2,@parameter3,@parameter4,@parameter5,@parameter6,@parameter7,@parameter8,@parameter9,@parameter10)", myConnection);
                cmd.Parameters.AddWithValue("@parameter1", label5.Text);
                cmd.Parameters.AddWithValue("@parameter2", girisSaati);
                cmd.Parameters.AddWithValue("@parameter3", girisTarihi);
                cmd.Parameters.AddWithValue("@parameter4", girisKm);
                cmd.Parameters.AddWithValue("@parameter5", geldigiYer);
                cmd.Parameters.AddWithValue("@parameter6", cikisSaati);
                cmd.Parameters.AddWithValue("@parameter7", cikisTarihi);
                cmd.Parameters.AddWithValue("@parameter8", cikisKm);
                cmd.Parameters.AddWithValue("@parameter9", gittigiYer);
                cmd.Parameters.AddWithValue("@parameter10", soforAdi);
                cmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }

            string sonucSaat = "", sonucTarih = "";
            int sonucKm = 0;
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
                sonucKm = cikisKm - girisKm;
                MessageBox.Show(label5.Text + " plakalı aracın çıkışı yapıldı. " + "\nBekleme Süresi: " +
                    sonucTarih + " gün, " +
                    sonucSaat + " saat/dakika, " +
                    sonucKm + " km yol yapıldı");
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
                SqlCommand kmt = new SqlCommand("DELETE giris where id=" + id, myConnection);
                kmt.ExecuteNonQuery();
                myConnection.Close();
                verileriGetir();
            }
            catch (Exception ex)
            {
                Console.Write("Hata :" + ex);
            }
        }
        bool butonTik = false;

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (butonTik == false)
            {
                butonTik = true;
                textBox2.Enabled = true;
            }
            else
            {
                butonTik = false;
                textBox2.Enabled = false;
            }
        }
        bool butonTik1 = false;
        private void btnEdit1_Click(object sender, EventArgs e)
        {
            if (butonTik1 == false)
            {
                butonTik1 = true;
                textBox3.Enabled = true;
            }
            else
            {
                butonTik1 = false;
                textBox3.Enabled = false;
            }
        }
    }
}
