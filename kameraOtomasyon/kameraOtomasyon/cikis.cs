﻿using System;
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
    public partial class cikis : Form
    {
        static string conString = "server=localhost; Initial Catalog=dbName;User ID=id;Password=pass;";
        SqlConnection myConnection = new SqlConnection(conString);
        public cikis()
        {
            InitializeComponent();
        }
        private void cikis_Load(object sender, EventArgs e)
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
            dataGridView1.Columns[10].Width = 200;

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
            int girisKm = 0, cikisKm = 0;
            string sonucSaat = "", sonucTarih = "0";
            int sonucKm = 0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                try
                {
                    girisSaati = row.Cells[2].Value.ToString();
                    girisTarihi = row.Cells[3].Value.ToString();
                    girisKm = Convert.ToInt32(row.Cells[4].Value.ToString());
                    cikisSaati = row.Cells[6].Value.ToString();
                    cikisTarihi = row.Cells[7].Value.ToString();
                    cikisKm = Convert.ToInt32(row.Cells[8].Value.ToString());
                    if (girisTarihi.Equals(cikisTarihi))
                    {
                        sonucSaat = (Convert.ToDateTime(cikisSaati) - Convert.ToDateTime(girisSaati)).ToString();
                    }
                    else
                    {
                        sonucTarih = (Convert.ToDateTime(cikisTarihi) - Convert.ToDateTime(girisTarihi)).Days.ToString();
                        sonucSaat = (Convert.ToDateTime(cikisSaati) - Convert.ToDateTime(girisSaati)).ToString();
                    }
                    sonucKm = cikisKm - girisKm;

                    label2.Text = row.Cells[1].Value.ToString() + " plakalı araç " + "\nBekleme Süresi: " +
                        sonucTarih + " gün, " +
                        sonucSaat + " saat/dakika";
                    label3.Text = sonucKm + " km yol yapıldı";
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
                worksheet.Cells[1, 2] = "PLAKA";
                worksheet.Cells[1, 3] = "GİRİŞ SAATİ";
                worksheet.Cells[1, 4] = "GİRİŞ TARİHİ";
                worksheet.Cells[1, 5] = "GİRİŞ KM";
                worksheet.Cells[1, 6] = "GELDİĞİ YER";
                worksheet.Cells[1, 7] = "ÇIKIŞ SAATİ";
                worksheet.Cells[1, 8] = "ÇIKIŞ TARİHİ";
                worksheet.Cells[1, 9] = "ÇIKIŞ KM";
                worksheet.Cells[1, 10] = "GİTTİĞİ YER";
                worksheet.Cells[1, 11] = "ŞOFÖR ADI";

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if (j == 2)
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString().Substring(0, 5);
                        else if (j == 3)
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString().Substring(0, 10);
                        else if (j == 6)
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString().Substring(0, 5);
                        else if (j == 7)
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
                string kayit = "SELECT * from cikis";
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
            dataGridView1.Columns["cikisSaati"].HeaderText = "ÇIKIŞ SAATİ";
            dataGridView1.Columns["cikisTarihi"].HeaderText = "ÇIKIŞ TARİHİ";
            dataGridView1.Columns["cikisKm"].HeaderText = "ÇIKIŞ KM";
            dataGridView1.Columns["gittigiYer"].HeaderText = "GİTTİĞİ YER";
            dataGridView1.Columns["soforAdi"].HeaderText = "ŞOFÖR ADI";
        }

        public void ara()
        {
            try
            {
                myConnection.Open();
                string kayit = "SELECT * from cikis where plaka = '" + textBox1.Text + "'";
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
