using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Oil_Shoop
{
    public partial class Form2 : Form
    {
        MySqlConnection con = new MySqlConnection("server=localhost;database=oil_database;uid=root;pwd=rootroot");
        MySqlCommand com;
        public Form2()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if (Home.admin_account)
            {
                string old_Pass = txt_oldPass.Text;
                string use = txt_username.Text;
                string pass = txt_password.Text;
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from login ", con);
                DataTable tb = new DataTable();
                ad.Fill(tb);


                if (tb.Rows.Count > 0)
                {
                    if (old_Pass == tb.Rows[0]["admin_password"].ToString())
                    {
                        try
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand("update login set admin = '" + use + "' , admin_password = '" + pass + "' where id=1", con);
                            //  cmd.Parameters.AddWithValue("@s1", user);
                            //cmd.Parameters.AddWithValue("@s2", pass);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("تم تعديل البيانات بنجاح");
                            new Form1().Show();
                            this.Hide();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("الرقم السرى القديم غير صحيح");
                    }
                }
            }
            else
            {
                string old_Pass = txt_oldPass.Text;
                string use = txt_username.Text;
                string pass = txt_password.Text;
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from login ", con);
                DataTable tb = new DataTable();
                ad.Fill(tb);


                if (tb.Rows.Count > 0)
                {
                    if (old_Pass == tb.Rows[0]["password"].ToString())
                    {
                        try
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand("update login set username = '" + use + "' , password = '" + pass + "' where id=1", con);
                            //  cmd.Parameters.AddWithValue("@s1", user);
                            //cmd.Parameters.AddWithValue("@s2", pass);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("تم تعديل البيانات بنجاح");
                            new Form1().Show();
                            this.Hide();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("الرقم السرى القديم غير صحيح");
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
