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
    public partial class ziyaretciCikis : Form
    {
        static string conString = "server=localhost; Initial Catalog=dbName;User ID=id;Password=pass;";
        SqlConnection myConnection = new SqlConnection(conString);
        public ziyaretciCikis()
        {
            InitializeComponent();
        }

        private void ziyaretciCikis_Load(object sender, EventArgs e)
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

            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 60000;//1 dakika
            timer1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ara();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            string girisSaati = "", girisTarihi = "", cikisSaati = "", cikisTarihi = "";
            string sonucSaat = "", sonucTarih = "0";

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                try
                {
                    girisSaati = row.Cells[4].Value.ToString();
                    girisTarihi = row.Cells[3].Value.ToString();
                    cikisSaati = row.Cells[6].Value.ToString();
                    cikisTarihi = row.Cells[5].Value.ToString();
                    
                    if (girisTarihi.Equals(cikisTarihi))
                    {
                        sonucSaat = (Convert.ToDateTime(cikisSaati) - Convert.ToDateTime(girisSaati)).ToString();
                    }
                    else
                    {
                        sonucTarih = (Convert.ToDateTime(cikisTarihi) - Convert.ToDateTime(girisTarihi)).Days.ToString();
                        sonucSaat = (Convert.ToDateTime(cikisSaati) - Convert.ToDateTime(girisSaati)).ToString();
                    }

                    label2.Text = row.Cells[1].Value.ToString().TrimEnd() + " adlı ziyaretçinin " + "\nBekleme Süresi: " +
                        sonucTarih + " gün, " +
                        sonucSaat + " saat/dakika";
                }
                catch (Exception ex)
                {
                    Console.Write("Hata: " + ex);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        private void ExportToExcel()
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "AISIN";

                int cellRowIndex = 2;
                int cellColumnIndex = 1;

                worksheet.Cells[1, 1] = "ID";
                worksheet.Cells[1, 2] = "ZİYARETÇİ ADI VE SOYADI";
                worksheet.Cells[1, 3] = "T.C./EHLİYET NO";
                worksheet.Cells[1, 4] = "GİRİŞ TARİHİ";
                worksheet.Cells[1, 5] = "GİRİŞ SAATİ";
                worksheet.Cells[1, 6] = "ÇIKIŞ TARİHİ";
                worksheet.Cells[1, 7] = "ÇIKIŞ SAATİ";
                worksheet.Cells[1, 8] = "ZİYARET EDİLEN YER";
                worksheet.Cells[1, 9] = "ZİYARETÇİ KART NO";
                worksheet.Cells[1, 10] = "PLAKA";

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if (j == 4)
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString().Substring(0, 5);
                        else if (j == 3)
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString().Substring(0, 10);
                        else if (j == 6)
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString().Substring(0, 5);
                        else if (j == 5)
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString().Substring(0, 10);
                        else
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel dosyaları (*.xlsx)|*.xlsx|Tüm Dosyalar (*.*)|*.*";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Veriler başarıyla Excele aktarıldı.");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
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

        public void ara()
        {
            try
            {
                myConnection.Open();
                string kayit = "SELECT * from ziyaretciCikis where adi = '" + textBox1.Text + "'";
                SqlCommand komut = new SqlCommand(kayit, myConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Console.Write("Hata: " + ex);
            }
        }
    }
}
