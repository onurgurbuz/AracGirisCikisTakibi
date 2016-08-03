using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kameraOtomasyon
{
    public partial class anaSayfa : Form
    {
        public anaSayfa()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            giris form1 = new giris();
            yavruform(form1);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            cikis form2 = new cikis();
            yavruform(form2);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            girisDuzenle form3 = new girisDuzenle();
            yavruform(form3);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            cikisDuzenle form4 = new cikisDuzenle();
            yavruform(form4);
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ziyaretciGiris form5 = new ziyaretciGiris();
            yavruform(form5);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ziyaretciCikis form6 = new ziyaretciCikis();
            yavruform(form6);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            ziyaretciGirisDuzenle form7 = new ziyaretciGirisDuzenle();
            yavruform(form7);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            ziyaretciCikisDuzenle form8 = new ziyaretciCikisDuzenle();
            yavruform(form8);
        }
        void yavruform(Form yavru)
        {
            bool durum = false;
            foreach (Form eleman in this.MdiChildren)
            {
                if (eleman.Name == yavru.Name)
                {
                    durum = true;
                    eleman.Activate();
                }
                else
                {
                    eleman.Close();
                }
            }
            if (durum == false)
            {

                yavru.MdiParent = this;
                yavru.Show();
                yavru.FormBorderStyle = FormBorderStyle.None;
                yavru.ShowIcon = false;
                yavru.Text = "";
                yavru.Dock = DockStyle.Fill;
            }
        }
    }
}
