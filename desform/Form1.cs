using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography; // Şifreleme işlemleri bu namespace kullanılarak yapılıyor.
using System.IO;

namespace desform
{
    public partial class Anaform : Form
    {
        static String anahtar1;
        static String orjinalmetin1;
        static String anahtar2;
        static String orjinalmetin2;
        //byte[] bytes = ASCIIEncoding.ASCII.GetBytes("TurgutKurt");
        public Anaform()
        {
            InitializeComponent();
        }
        private void BtnSifrele_Click(object sender, EventArgs e)
        {
            if(Txtanahtar1.TextLength < 8  || Txtmetin1.TextLength < 8)
            {
                MessageBox.Show("Key alanına 8 karakter girilmelidir", "Hata Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                anahtar1 = Txtanahtar1.Text;
                orjinalmetin1 = Txtmetin1.Text;
                Txtsonuc.Text = Sifrele(orjinalmetin1);
            }
        }
        private void BtnSifrecoz_Click(object sender, EventArgs e)
        {
            if (Txtanahtar2.TextLength < 8)
            {
                MessageBox.Show("Key alanına 8 karakter girilmelidir", "Hata Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                anahtar2 = Txtanahtar2.Text;
                orjinalmetin2 = Txtmetin2.Text;
                Txtsonuc2.Text = Sifrecoz(orjinalmetin2);
            }
        }
        private void Anaform_Load(object sender, EventArgs e)
        {
        }
        public static string Sifrele(string orjinalmetin)
        {
            byte[] anahtarbyte = ASCIIEncoding.ASCII.GetBytes(anahtar1);
            if (String.IsNullOrEmpty(orjinalmetin))
            {
                MessageBox.Show("Metin Giriş Alanı Boş Bırakılamaz", "Hata Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
            DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, dcsp.CreateEncryptor(anahtarbyte, anahtarbyte), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(orjinalmetin);
            writer.Flush(); 
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        public static string Sifrecoz(string sifrelimetin)
        {
            MemoryStream memoryStream;
            byte[] anahtarbyte = ASCIIEncoding.ASCII.GetBytes(anahtar2);
            if (String.IsNullOrEmpty(sifrelimetin))
            {
                MessageBox.Show("Metin Giriş Alanı Boş Bırakılamaz", "Hata Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
            DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider();
            try
            {
                memoryStream = new MemoryStream(Convert.FromBase64String(sifrelimetin));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dcsp.CreateDecryptor(anahtarbyte, anahtarbyte), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);
                try
                {
                    return reader.ReadToEnd();
                }
                catch (Exception)
                {
                    MessageBox.Show("Lütfen Daha Önce Kullandığınız Anahtarı Kullanınız ", "Hata Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Şifrelenmiş Metini Yanlış Girdiniz", "Hata Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return "";
        }
        private void Txtanahtar1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Txtanahtar1.TextLength == 8)
            {
                e.Handled = true;
            }
        }

        private void Txtanahtar2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Txtanahtar2.TextLength == 8)
            {
                e.Handled = true;
            }
        }

        private void Txtmetin1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Txtmetin1.TextLength == 8)
            {
                e.Handled = true;
            }
        }
    }
}
