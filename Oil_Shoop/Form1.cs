using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Management;

namespace Oil_Shoop
{
    public partial class Form1 : Form
    {
        public static int tries = 0;
        MySqlConnection con = new MySqlConnection("server=localhost;database=oil_database;uid=root;pwd=rootroot");
        MySqlCommand com;
        public static bool isAdmin=true;
        public Form1()
        {
            InitializeComponent();

           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {

            MySqlDataAdapter ad = new MySqlDataAdapter("select * from login ", con);
            DataTable tb = new DataTable();
            ad.Fill(tb);
            DateTime t1, t2;

            if (tb.Rows.Count > 0)
            {
                t1 = DateTime.Parse(tb.Rows[0]["d"].ToString());
                //MessageBox.Show(t1.ToString("yyyy-MM-dd")+"");
                //MessageBox.Show(tb.Rows[0]["user"].ToString() + " " + tb.Rows[0]["password"].ToString());
                if (int.Parse(tb.Rows[0]["ds"].ToString()) == 30)
                {
                    tries = 1;
                    new security().Show();
                    this.Hide();
                }
                else if (int.Parse(tb.Rows[0]["ds"].ToString()) == 180)
                {
                    tries = 2;
                    new security().Show();
                    this.Hide();
                }
                else if (int.Parse(tb.Rows[0]["ds"].ToString()) == 365)
                {
                    tries = 3;
                    new security().Show();
                    this.Hide();
                }
                else if (int.Parse(tb.Rows[0]["ds"].ToString()) == 500)
                {
                    tries = 4;
                    new security().Show();
                    this.Hide();
                }
                else if (int.Parse(tb.Rows[0]["ds"].ToString()) == 650)
                {
                    tries = 5;
                    new security().Show();
                    this.Hide();
                }
                else if (int.Parse(tb.Rows[0]["ds"].ToString()) ==800)
                {
                    tries = 6;
                    new security().Show();
                    this.Hide();
                }
                else
                {

                    t2 = DateTime.Now;
                    con.Open();
                    if ((t2.ToString("yyyy-MM-dd")!=t1.ToString("yyyy-MM-dd")))
                    {
                        com = new MySqlCommand("update login set ds=ds+1 ,d='"+ t2.ToString("yyyy-MM-dd")+"'  where id=1", con);
                        com.ExecuteNonQuery();

                        DataTable copyTable = new DataTable();
                        MySqlDataAdapter adapter = new MySqlDataAdapter("select * from item", con);
                        adapter.Fill(copyTable);

                        int rowCount = copyTable.Rows.Count;
                        for(int i = 0; i < rowCount; i++)
                        {
                            string item_name = copyTable.Rows[i]["name"].ToString();
                            int item_count = int.Parse(copyTable.Rows[i]["count"].ToString());
                            
                            com = new MySqlCommand("insert into item_copy(name,count,date)values(@v1,@v2,@v3)",con);
                            com.Parameters.AddWithValue("@v1", item_name);
                            com.Parameters.AddWithValue("@v2", item_count);
                            com.Parameters.AddWithValue("@v3", t2.ToString("yyyy-MM-dd"));
                            com.ExecuteNonQuery();
                        }

                        string folderPath = "F:\\MOBILE\\المخزن";
                        //MessageBox.Show(folderPath + "\\oil_backup" + DateTime.Now.ToString("yyyy,mm,dd-HH,mm,ss") + ".sql");
                        con.Close();
                        using (MySqlConnection conn = new MySqlConnection("server=localhost;database=oil_database;uid=root;pwd=rootroot"))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                using (MySqlBackup mb = new MySqlBackup(cmd))
                                {
                                    cmd.Connection = conn;
                                    conn.Open();
                                    mb.ExportToFile(folderPath + "\\oil_backup" + DateTime.Now.ToString("yyyy,MM,dd-HH,mm,ss") + ".sql");
                                    conn.Close();
                                    MessageBox.Show("صباح الفل ربنا يوسع رزقك ..... تم انشاء نسخة احتياطية بنجاح الرجاء التأكد من الاتصال بالانترنت شكرا لك ");
                                }
                            }
                        }
                        con.Open();


                    }
                    com = new MySqlCommand("select admin from login where admin= @u and admin_password= @p", con);
                    MySqlDataReader dr;
                    DataTable dt = new DataTable();
                    try
                    {
                       
                        com.Parameters.AddWithValue("@u", txt_username.Text);
                        com.Parameters.AddWithValue("@p", txt_password.Text);
                        dr = com.ExecuteReader();
                        dt.Load(dr);
                        if (dt.Rows.Count > 0)
                        {
                            isAdmin = true;
                            new Home().Show();
                            this.Hide();
                        }
                        else
                        {

                            com = new MySqlCommand("select username from login where username= @u and password= @p", con);
                            dt = new DataTable();
                            try
                            {

                                com.Parameters.AddWithValue("@u", txt_username.Text);
                                com.Parameters.AddWithValue("@p", txt_password.Text);
                                dr = com.ExecuteReader();
                                dt.Load(dr);
                                if (dt.Rows.Count > 0)
                                {
                                    isAdmin = false;
                                    new Home().Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("تأكد من اسم المستخدم وكلمه المرور");

                                }
                            }
                            catch (Exception ee)
                            {
                                MessageBox.Show(ee.Message);
                            }
                            con.Close();
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message);
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string manuf = "", product = "", serial = "";
            manuf = systemInfo.Manufacturer;
            product = systemInfo.Product;
            serial = systemInfo.SerialNumber;
            //manuf =a= "Hewlett-Packard" && product == "3048h" && serial == "MXL0460C78")
            if ((manuf == "Hewlett-Packard" && product == "1850" && serial == "CZC339B5FW"))
            {

            }
            else
            {
                MessageBox.Show("لقد تم تغير الجهاز");
                Application.Exit();
            }
        }

        private void txt_username_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.SelectNextControl((Control)sender, false, true, true, true);
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.SelectNextControl((Control)sender, false, true, true, true);
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
