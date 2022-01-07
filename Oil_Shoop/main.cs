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
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing;

namespace Oil_Shoop
{

    public partial class Home : Form
    {
        MySqlConnection connection;
        MySqlCommand command;
        MySqlDataAdapter adapter;
        float total_price;
        string connstr = "server=localhost;database=oil_database;uid=root;pwd=rootroot";
        int sell_item_id = 0;
        public static bool admin_account;
        int back_item_id = 0;
        public Home()
        {
            InitializeComponent();
            this.MaximumSize = new System.Drawing.Size(1300, 700);

            connection = new MySqlConnection(connstr);
            connection.Open();
            preload();
            loadData(1);
        }

        private void preload()
        {

            if (Form1.isAdmin)
            {

            }
            else
            {
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
                tabControl1.TabPages.Remove(tabPage6);
                tabControl1.TabPages.Remove(tabPage9);
                tabControl1.TabPages.Remove(tabPage2);
            }
        }

        void loadData(int x)
        {
            try
            {

                sell_combo_inOut.SelectedIndex = 0;
                combo_inOrOut.SelectedIndex = 0;
                c_combo_inOut.SelectedIndex = 0;
                radio_priceA.Checked = true;
                bill_sellOrback.SelectedIndex = 0;

                //load catogaries to combobox1 
                adapter = new MySqlDataAdapter("Select * from cat", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                if (x == 1)
                {
                    comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;
                    comboBox1.ValueMember = "id";
                    comboBox1.DisplayMember = "name";
                    comboBox1.DataSource = table;
                    comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
                }

                //load catogaries to update combobox
                adapter = new MySqlDataAdapter("Select * from cat", connection);
                table = new DataTable();
                adapter.Fill(table);

                update.SelectedIndexChanged -= update_SelectedIndexChanged;
                update.ValueMember = "id";
                update.DisplayMember = "name";
                update.DataSource = table;
                update.SelectedIndexChanged += update_SelectedIndexChanged;

                adapter = new MySqlDataAdapter("Select * from item", connection);
                table = new DataTable();
                adapter.Fill(table);

                del_list_item.SelectedIndexChanged -= del_list_item_SelectedIndexChanged;
                del_list_item.ValueMember = "id";
                del_list_item.DisplayMember = "name";
                del_list_item.DataSource = table;
                del_list_item.SelectedIndexChanged += del_list_item_SelectedIndexChanged;

                adapter = new MySqlDataAdapter("Select * from cat", connection);
                table = new DataTable();
                adapter.Fill(table);

                if (x == 1)
                {
                    add_comboBox.SelectedIndexChanged -= add_comboBox_SelectedIndexChanged;
                    add_comboBox.ValueMember = "id";
                    add_comboBox.DisplayMember = "name";
                    add_comboBox.DataSource = table;
                    add_comboBox.SelectedIndexChanged += add_comboBox_SelectedIndexChanged;
                }

                del_listBox_cat.SelectedIndexChanged -= del_listBox_cat_SelectedIndexChanged;
                del_listBox_cat.ValueMember = "id";
                del_listBox_cat.DisplayMember = "name";
                del_listBox_cat.DataSource = table;
                del_listBox_cat.SelectedIndexChanged += del_listBox_cat_SelectedIndexChanged;

                import_combo.SelectedIndexChanged -= comboBox3_SelectedIndexChanged;
                import_combo.ValueMember = "id";
                import_combo.DisplayMember = "name";
                import_combo.DataSource = table;
                import_combo.SelectedIndexChanged += comboBox3_SelectedIndexChanged;


                export_combo.SelectedIndexChanged -= export_combo_SelectedIndexChanged;
                export_combo.ValueMember = "id";
                export_combo.DisplayMember = "name";
                export_combo.DataSource = table;
                export_combo.SelectedIndexChanged += export_combo_SelectedIndexChanged;


                adapter = new MySqlDataAdapter("Select * from userdata where type='بائع'", connection);
                table = new DataTable();
                adapter.Fill(table);

                import_company.SelectedIndexChanged -= import_companyName_SelectedIndexChanged;
                import_company.ValueMember = "id";
                import_company.DisplayMember = "name";
                import_company.DataSource = table;
                import_company.SelectedIndexChanged += import_companyName_SelectedIndexChanged;

                adapter = new MySqlDataAdapter("Select * from userdata where type='مشترى'", connection);
                table = new DataTable();
                adapter.Fill(table);

                export_company.SelectedIndexChanged -= export_company_SelectedIndexChanged;
                export_company.ValueMember = "id";
                export_company.DisplayMember = "name";
                export_company.DataSource = table;
                export_company.SelectedIndexChanged += export_company_SelectedIndexChanged;


                adapter = new MySqlDataAdapter("Select * from cat ", connection);
                table = new DataTable();
                adapter.Fill(table);

                comboBox3.ValueMember = "id";
                comboBox3.DisplayMember = "name";
                comboBox3.DataSource = table;

                adapter = new MySqlDataAdapter("Select * from item ", connection);
                table = new DataTable();
                adapter.Fill(table);

                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "name";
                comboBox2.DataSource = table;

                table = new DataTable();
                adapter = new MySqlDataAdapter("select * from userdata ", connection);
                adapter.Fill(table);

                float total_out = 0, total_in = 0;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["type"].ToString() == "بائع")
                    {
                        total_out += float.Parse(table.Rows[i]["remained"].ToString());
                    }
                    else
                    {
                        total_in += float.Parse(table.Rows[i]["remained"].ToString());
                    }
                }

                r_text_totalExport.Text = total_out + "";
                r_textTotalImport.Text = total_in + "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        int number_of_cat()
        {
            int x = 28;
            command = new MySqlCommand("selec id from cat where name=@ii", connection);
            MySqlDataReader dr;
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@ii", add_comboBox.SelectedValue);
                MessageBox.Show(add_comboBox.SelectedItem.ToString());
                dr = command.ExecuteReader();
                dt.Load(dr);
                x = (int)dt.Rows[0]["id"];
                connection.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

            return x;
        }
        private void add_btn_addItem_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            MySqlDataReader dr;
            string catname = add_comboBox.Text.ToString();
            if (catname != "" && add_txt_itemCount.Text != "" && add_txt_itemName.Text != "" && add_txt_itemPriceA.Text != "" && add_txt_itemPriceB.Text != "" && add_txt_itemPriceC.Text != "")
            {
                string id = add_comboBox.SelectedValue.ToString();
                command = new MySqlCommand("select * from item where name='" + add_txt_itemName.Text + "' and cat_id=" + int.Parse(id), connection);
                dr = command.ExecuteReader();
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("تم اضافه هذا المنتج من قبل يمكنك التعديل");

                }
                else
                {
                    try
                    {
                        command = new MySqlCommand("insert into item (name,count,priceA,priceB,priceC,cat_id,barcode,countinStore) values(@name,@count,@priceA,@priceB,@priceC,@id,@barcode,@countinStore)", connection);
                        command.Parameters.AddWithValue("@name", add_txt_itemName.Text);
                        command.Parameters.AddWithValue("@count", int.Parse(add_txt_itemCount.Text));
                        command.Parameters.AddWithValue("@priceA", Convert.ToDouble(add_txt_itemPriceA.Text));
                        command.Parameters.AddWithValue("@priceB", Convert.ToDouble(add_txt_itemPriceB.Text));
                        command.Parameters.AddWithValue("@priceC", Convert.ToDouble(add_txt_itemPriceC.Text));
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@barcode", txt_add_barcode.Text);
                        command.Parameters.AddWithValue("@countinStore", add_txt_itemCountStore.Text);
                        int n = command.ExecuteNonQuery();
                        if (n > 0)
                            MessageBox.Show("تم الأضافه بنجاح");
                        // add_txt_itemCount.Clear();
                        add_txt_itemName.Clear();
                        //add_txt_itemPriceA.Clear();
                        //add_txt_itemPriceB.Clear();
                        //add_txt_itemCountStore.Clear();
                        loadData(2);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("الرجاء التأكد من اداخال القيم بشكل صحيح");
                        MessageBox.Show(ex.Message);
                    }


                }

            }
            else
                MessageBox.Show("يجب التأكد من ادخال جميع البيانات قبل الحفظ");
        }

        private void add_btn_addCat_Click(object sender, EventArgs e)
        {
            if (add_txt_catName.Text != "")
            {
                // check if the name is exists
                command = new MySqlCommand("select * from cat where name=@n", connection);
                command.Parameters.AddWithValue("@n", add_txt_catName.Text);
                MySqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                if (dt.Rows.Count == 0)
                {
                    // if the name not exists we will insert new cat
                    command = new MySqlCommand("insert into cat (name) values(@name)", connection);
                    command.Parameters.AddWithValue("@name", add_txt_catName.Text);
                    int n = command.ExecuteNonQuery();
                    if (n > 0)
                        MessageBox.Show("تمت اضافة الفئه بنجاح");

                }
                else
                {
                    MessageBox.Show("تم اضافه هذه الفئه من قبل");
                }

                loadData(1);

            }
            else
                MessageBox.Show("من فضلك أدخل اسم الفئه أولآ ");

        }

        private void del_btn_cat_Click(object sender, EventArgs e)
        {
            if (del_listBox_cat.Text.ToString() == "")
            {
                MessageBox.Show("يجب اولا اختيار الفئه التى تريد حذفها ");
            }
            else
            {
                command = new MySqlCommand("delete from cat where id=@nn", connection);
                try
                {
                    command.Parameters.AddWithValue("@nn", int.Parse(del_listBox_cat.SelectedValue.ToString()));
                    command.ExecuteNonQuery();
                    MessageBox.Show("تم الحذف بنجاح");

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                //load_data_cat();
                loadData(1);
                del_txtCatSearch.Text = "";
            }
        }

        private void del_listBox_cat_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void del_btn_item_Click(object sender, EventArgs e)
        {
            if (del_list_item.Text.ToString() == "" || del_list_item.Text.ToString() == " ")
            {
                MessageBox.Show("يجب اختيار المنتج اولا ");
            }
            else
            {
                command = new MySqlCommand("delete from item where id=@nn", connection);
                try
                {
                    command.Parameters.AddWithValue("@nn", int.Parse(del_list_item.SelectedValue.ToString()));
                    command.ExecuteNonQuery();
                    MessageBox.Show(" تم الحذف بنجاح");
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                loadData(1);
            }


        }

        private void del_list_item_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void add_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void del_txtCatSearch_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void del_txtCatSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                adapter = new MySqlDataAdapter("select * from cat where name like '%" + del_txtCatSearch.Text + "%'", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                del_listBox_cat.SelectedIndexChanged -= del_listBox_cat_SelectedIndexChanged;
                del_listBox_cat.ValueMember = "id";
                del_listBox_cat.DisplayMember = "name";
                del_listBox_cat.DataSource = table;
                del_listBox_cat.SelectedIndexChanged += del_listBox_cat_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void del_txtItemSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                adapter = new MySqlDataAdapter("select * from item where name like '%" + del_txtItemSearch.Text + "%'", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                del_list_item.SelectedIndexChanged -= del_list_item_SelectedIndexChanged;
                del_list_item.ValueMember = "id";
                del_list_item.DisplayMember = "name";
                del_list_item.DataSource = table;
                del_list_item.SelectedIndexChanged += del_list_item_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string oldCat_name = update.Text;
            string id = update.SelectedValue.ToString();
            // MessageBox.Show(id);
            if (textBox6.Text != "")
            {
                try
                {
                    command = new MySqlCommand("update  cat set name=@name where id=" + id, connection);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    command.Parameters.AddWithValue("@name", textBox6.Text);
                    int n = command.ExecuteNonQuery();

                    if (n > 0)
                    {
                        MessageBox.Show("تم حفظ التعديل");
                        loadData(1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("الرجاء ادخال القيمه الجديده قبل حفظ التعديل");
            }
        }

        private void update_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load item in selected catogry in list box
            MySqlDataAdapter adapter;
            string id = comboBox1.SelectedValue.ToString();
            try
            {
                adapter = new MySqlDataAdapter("Select * from item where cat_id=" + int.Parse(id), connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged;
                listBox1.ValueMember = "id";
                listBox1.DisplayMember = "name";
                listBox1.DataSource = table;
                listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlDataAdapter adapter;
            if (comboBox1.Text.ToString() != "")
            {
                string cat_id = comboBox1.SelectedValue.ToString();
                if (listBox1.Text.ToString() != "")
                {
                    string item_id = listBox1.SelectedValue.ToString();
                    try
                    {
                        adapter = new MySqlDataAdapter("select * from item where id=" + int.Parse(item_id) + " and cat_id= " + int.Parse(cat_id), connection);
                        DataTable dt = new DataTable();
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            textBox5.Text = dt.Rows[0][1].ToString();
                            textBox2.Text = dt.Rows[0][2].ToString();
                            textBox3.Text = dt.Rows[0][3].ToString();
                            textBox4.Text = dt.Rows[0][4].ToString();
                            textBox1.Text = dt.Rows[0][5].ToString();
                            textBox9.Text = dt.Rows[0][8].ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text.ToString() != "")
            {
                string cat_id = comboBox1.SelectedValue.ToString();
                //MessageBox.Show(cat_id);
                if (listBox1.Text.ToString() != "")
                {
                    string item_id = listBox1.SelectedValue.ToString();
                    // MessageBox.Show(item_id);

                    command = new MySqlCommand("update item set name=@name , count=@count , priceA=@priceA , priceB=@priceB , priceC=@priceC , countinStore=@countinStore where id= " + int.Parse(item_id) + " and cat_id=" + int.Parse(cat_id), connection);

                    if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@name", textBox5.Text);
                            command.Parameters.AddWithValue("@count", int.Parse(textBox2.Text));
                            command.Parameters.AddWithValue("@priceA", float.Parse(textBox3.Text));
                            command.Parameters.AddWithValue("@priceB", float.Parse(textBox4.Text));
                            command.Parameters.AddWithValue("@priceC", float.Parse(textBox1.Text));
                            command.Parameters.AddWithValue("@countinStore", int.Parse(textBox9.Text));

                            int n = command.ExecuteNonQuery();
                            if (n > 0)
                            {
                                MessageBox.Show("تم حفظ التعديل");
                                loadData(2);
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("برجاء التأكد من ادخال اليانات بشكل صحيح");

                        }
                    }
                    else
                        MessageBox.Show("برجاء التأكد من ادخال جميع القيم قبل حفظ التعديل");

                }
                else
                    MessageBox.Show("برجاء اختيار المنتج الذي تريد تعديله ");

            }
            else
                MessageBox.Show("برجاء اختيار الفئه أولا");

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                command = new MySqlCommand("select * from item where cat_id=@aa", connection);
                DataTable dt = new DataTable();
                command.Parameters.AddWithValue("@aa", import_combo.SelectedValue);
                MySqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);
                import_list.ValueMember = "id";
                import_list.DisplayMember = "name";
                import_list.DataSource = dt;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (import_date.Value.ToString() != "" && import_combo.Text != "" && import_list.Text != "" && import_company.Text != "" && import_number.Text != "" && import_price.Text != "" && import_totalPrice.Text != "")
            {
                int item_id = int.Parse(import_list.SelectedValue.ToString());
                bool found = false;
                int index = 0;
                try
                {
                    for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    {
                        if (int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()) == item_id)
                        {
                            found = true;
                            index = i;
                            break;
                        }
                    }
                    if (found)
                    {
                        dataGridView1.Rows[index].Cells[4].Value = int.Parse(dataGridView1.Rows[index].Cells[4].Value.ToString()) + int.Parse(import_number.Text);
                        dataGridView1.Rows[index].Cells[5].Value = int.Parse(dataGridView1.Rows[index].Cells[4].Value.ToString()) * int.Parse(dataGridView1.Rows[index].Cells[3].Value.ToString());
                    }
                    else
                    {
                        dataGridView1.Rows.Add(import_combo.Text, import_list.SelectedValue.ToString(), import_list.Text, import_price.Text, import_number.Text, import_totalPrice.Text);

                    }
                    total_price = total_price + float.Parse(import_totalPrice.Text);
                    import_total.Text = total_price.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("يرجي ادخال البيانات كامله");
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                int index = this.dataGridView1.SelectedRows[0].Index;
                DataGridViewRow row = dataGridView1.Rows[index];
                total_price = total_price - float.Parse(row.Cells["total"].Value.ToString());
                import_total.Text = total_price.ToString();
                dataGridView1.Rows.RemoveAt(index);
                this.dataGridView1.Refresh();
            }
            else
            {
                MessageBox.Show("يجب اختيار المنتج المراد حذفه");
            }
        }

        private void tabPage8_Click(object sender, EventArgs e)
        {

        }

        private void client_btnSave_Click(object sender, EventArgs e)
        {
            bool isfound = false;
            if (client_txtPhone.Text != "")
                isfound = true;
            if (client_txtName.Text != "" && client_txtRemainMoney.Text != "" && comboBox5.Text.ToString() != "")
            {
                try
                {
                    command = new MySqlCommand("insert into userdata (name,phone,address,notes,type,remained,cash) values(@name,@phone,@address,@notes,@type,@remained,0)", connection);
                    command.Parameters.AddWithValue("@name", client_txtName.Text);
                    if (isfound)
                        command.Parameters.AddWithValue("@phone", float.Parse(client_txtPhone.Text));
                    else
                        command.Parameters.AddWithValue("@phone", client_txtPhone.Text);

                    command.Parameters.AddWithValue("@address", client_txtAddress.Text);
                    command.Parameters.AddWithValue("@notes", client_txtNote.Text);
                    command.Parameters.AddWithValue("@type", comboBox5.Text.ToString());
                    command.Parameters.AddWithValue("@remained", float.Parse(client_txtRemainMoney.Text));
                    //command.Parameters.AddWithValue("@numbers", int.Parse(client_txtNumber.Text));

                    int n = command.ExecuteNonQuery();
                    if (n > 0)
                    {
                        MessageBox.Show("تم الحفظ");
                        loadData(1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("برجاء التأكد من صحة القيم المدخله");
                }
            }

            else
            {
                MessageBox.Show("برجاء التأكد من ادخال  الاسم والمتبقي وتحديد بائع ام مشترى أولآ");
            }
        }

        private void import_companyName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void import_number_TextChanged(object sender, EventArgs e)
        {
            if (import_number.Text != "" && import_price.Text != "")
            {
                import_totalPrice.Text = (float.Parse(import_number.Text) * float.Parse(import_price.Text)) + "";
            }
        }

        private void import_price_TextChanged(object sender, EventArgs e)
        {
            if (import_number.Text != "" && import_price.Text != "")
            {
                import_totalPrice.Text = (float.Parse(import_number.Text) * float.Parse(import_price.Text)).ToString();
            }
        }

        private void import_total_TextChanged(object sender, EventArgs e)
        {
            if (import_total.Text != "" && import_paid.Text != "")
            {
                import_remain.Text = (float.Parse(import_total.Text) - float.Parse(import_paid.Text)).ToString();
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (import_total.Text != "" && import_paid.Text != "")
            {
                import_remain.Text = (float.Parse(import_total.Text) - float.Parse(import_paid.Text)).ToString();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (dataGridView1.RowCount > 0 && import_paid.Text != "" && import_total.Text != "")
            {
                //update userData information 

                try
                {
                    command = new MySqlCommand("update userdata set remained =remained+" + float.Parse(import_remain.Text) + " , cash=cash+" + float.Parse(import_total.Text) + " where id = " + int.Parse(import_company.SelectedValue.ToString()) + "", connection);
                    command.ExecuteNonQuery();

                    // MessageBox.Show(import_date.Value.ToString("yyyy-MM-dd"));
                    MySqlCommand command1 = new MySqlCommand("insert into bill (type,client,date,total,paid,remain,outOrin) values (@t,@c,@d,@total,@p,@r,@oi)", connection);
                    command1.Parameters.AddWithValue("@t", "استيراد");
                    command1.Parameters.AddWithValue("@c", import_company.Text);
                    command1.Parameters.AddWithValue("@d", import_date.Value.ToString("yyyy-MM-dd"));
                    command1.Parameters.AddWithValue("@total", float.Parse(import_total.Text));
                    command1.Parameters.AddWithValue("@p", float.Parse(import_paid.Text));
                    command1.Parameters.AddWithValue("@r", float.Parse(import_remain.Text));
                    command1.Parameters.AddWithValue("@oi", (sell_combo_inOut.Text));
                    command1.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("برجاء ادخال اسم الشركه أولآ");
                }
                MySqlCommand command2 = new MySqlCommand("select id from bill order by id desc", connection);
                int id = int.Parse(command2.ExecuteScalar().ToString());

                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    MySqlCommand command = new MySqlCommand("insert into orders (item_id,number,price,total_price,item_name,cat_name,bill_id) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7)", connection);
                    command.Parameters.AddWithValue("@v1", int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                    command.Parameters.AddWithValue("@v2", int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()));
                    command.Parameters.AddWithValue("@v3", float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()));
                    command.Parameters.AddWithValue("@v4", float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()));
                    command.Parameters.AddWithValue("@v5", dataGridView1.Rows[i].Cells[2].Value.ToString());
                    command.Parameters.AddWithValue("@v6", dataGridView1.Rows[i].Cells[0].Value.ToString());
                    command.Parameters.AddWithValue("@v7", id);

                    command.ExecuteNonQuery();

                    int c = int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    if (import_company.Text == "المخزن")
                        command = new MySqlCommand("update item set count=count+" + c + ",countinStore=countinStore-" + c + " where id =" + int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()) + "", connection);
                    else
                        command = new MySqlCommand("update item set count=count+" + c + "  where id = " + int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()) + "", connection);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("تم حفظ الفاتوره بنجاح");
                dataGridView1.Rows.Clear();
                total_price = 0;
                import_total.Text = "";
                import_remain.Text = "";
                import_paid.Text = "";
            }
            else
            {
                MessageBox.Show("يجب التأكد من وجود فاتوره وادخال المبلغ المدفوع منها قبل الحفظ");
            }
        }

        private void export_combo_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {

                command = new MySqlCommand("select * from item where cat_id=@aa", connection);
                DataTable dt = new DataTable();
                command.Parameters.AddWithValue("@aa", export_combo.SelectedValue);
                MySqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);
                export_list.ValueMember = "id";
                export_list.DisplayMember = "name";
                export_list.DataSource = dt;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        private void export_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            back_item_id = int.Parse(export_list.SelectedValue.ToString());
            bool found = false;
            int index = 0;
            DataTable table = new DataTable();
            adapter = new MySqlDataAdapter("select * from item where id = " + int.Parse(export_list.SelectedValue.ToString()) + "", connection);
            adapter.Fill(table);

            if (radio_priceA.Checked)
            {
                export_price.Text = table.Rows[0]["priceA"].ToString();
            }
            else
            {
                export_price.Text = table.Rows[0]["priceB"].ToString();
            }
            for (int i = 0; i < dataGridView2.RowCount - 1; i++)
            {
                if (int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) == back_item_id)
                {
                    found = true;
                    index = i;
                    break;
                }
            }
            if (found)
            {
                if (bill_sellOrback.SelectedIndex == 0)
                {
                    int count = int.Parse(table.Rows[0]["count"].ToString()) - int.Parse(dataGridView2.Rows[index].Cells[4].Value.ToString());
                }
                else
                {
                    int count = int.Parse(table.Rows[0]["count"].ToString()) + int.Parse(dataGridView2.Rows[index].Cells[4].Value.ToString());

                }
                export_label_count.Text = count.ToString();
            }
            else
            {
                export_label_count.Text = table.Rows[0]["count"].ToString();
            }
            sell_label_catName.Text = export_combo.Text;
            sell_label_itemName.Text = export_list.Text;
        }

        private void export_company_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (export_clientold.Text == "0" || export_clientold.Text == "")
            {
                DataTable table = new DataTable();
                adapter = new MySqlDataAdapter("select * from userdata where id = " + int.Parse(export_company.SelectedValue.ToString()) + "", connection);
                adapter.Fill(table);
                export_clientold.Text = table.Rows[0]["remained"].ToString();
                //export_clientNumber.Text = table.Rows[0]["numbers"].ToString();
            }
        }



        private void export_number_TextChanged(object sender, EventArgs e)
        {
            if (export_number.Text != "" && export_price.Text != "")
            {
                export_totalPrice.Text = (float.Parse(export_number.Text) * float.Parse(export_price.Text)) + "";
            }
        }

        private void export_price_TextChanged(object sender, EventArgs e)
        {
            if (export_number.Text != "" && export_price.Text != "")
            {
                export_totalPrice.Text = (float.Parse(export_number.Text) * float.Parse(export_price.Text)) + "";
            }
        }
        float total_price2 = 0;
        private void button12_Click(object sender, EventArgs e)
        {
            if (export_date.Value.ToString() != "" && export_combo.Text != "" && (export_list.Text != "" || sell_barcode.Text != "") && export_company.Text != "" && export_number.Text != "" && export_price.Text != "" && export_totalPrice.Text != "")
            {
                try
                {

                    //int item_id = int.Parse(export_list.SelectedValue.ToString());
                    int bill_index = bill_sellOrback.SelectedIndex;
                    bool found = false;
                    int index = 0;
                    float count_pay = float.Parse(export_number.Text);
                    float count_inStore = float.Parse(export_label_count.Text);

                    if (bill_index == 0)
                    {
                        if (count_pay > count_inStore)
                        {

                            MessageBox.Show(" الكميه المطلوبه غير متاحه حاليا" + "\n" + "الكميه المتاحه حاليآ من هذا المنتج " + "\n" + export_label_count.Text);
                        }
                        else if (count_pay < 1)
                            MessageBox.Show("برجاء التأكد من ان العدد المطلوب أكبر من الصفر");
                        else
                        {

                            for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                            {
                                if (int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) == back_item_id)
                                {
                                    found = true;
                                    index = i;
                                    break;
                                }
                            }
                            if (found)
                            {
                                dataGridView2.Rows[index].Cells[4].Value = float.Parse(dataGridView2.Rows[index].Cells[4].Value.ToString()) + float.Parse(export_number.Text);
                                dataGridView2.Rows[index].Cells[5].Value = float.Parse(dataGridView2.Rows[index].Cells[4].Value.ToString()) * float.Parse(dataGridView2.Rows[index].Cells[3].Value.ToString());
                            }
                            else
                            {
                                dataGridView2.Rows.Add(sell_label_catName.Text, back_item_id.ToString(), sell_label_itemName.Text, export_price.Text, export_number.Text, export_totalPrice.Text);

                                // command=new MySqlCommand("update item set count="+count_inStore+" where id ="+int.Parse(export_list.SelectedValue.ToString())+"",connection);
                                // command.ExecuteNonQuery();
                            }
                            total_price2 = total_price2 + float.Parse(export_totalPrice.Text);
                            export_total.Text = total_price2.ToString();
                            count_inStore -= count_pay;
                            export_label_count.Text = count_inStore.ToString();
                        }
                    }

                    else
                    {

                        if (count_pay < 1)
                            MessageBox.Show("برجاء التأكد من ان العدد المطلوب أكبر من الصفر");
                        else
                        {

                            for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                            {
                                if (int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) == back_item_id)
                                {
                                    found = true;
                                    index = i;
                                    break;
                                }
                            }
                            if (found)
                            {
                                dataGridView2.Rows[index].Cells[4].Value = float.Parse(dataGridView2.Rows[index].Cells[4].Value.ToString()) + float.Parse(export_number.Text);
                                dataGridView2.Rows[index].Cells[5].Value = float.Parse(dataGridView2.Rows[index].Cells[4].Value.ToString()) * float.Parse(dataGridView2.Rows[index].Cells[3].Value.ToString());
                            }
                            else
                            {
                                dataGridView2.Rows.Add(sell_label_catName.Text, back_item_id.ToString(), sell_label_itemName.Text, export_price.Text, export_number.Text, export_totalPrice.Text);

                                // command=new MySqlCommand("update item set count="+count_inStore+" where id ="+int.Parse(export_list.SelectedValue.ToString())+"",connection);
                                // command.ExecuteNonQuery();
                            }
                            total_price2 = total_price2 + float.Parse(export_totalPrice.Text);
                            export_total.Text = total_price2.ToString();
                            count_inStore += count_pay;
                            export_label_count.Text = count_inStore.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("برجاء ادخال البيانات بشكل صحيح");

                }
            }
            else
            {
                MessageBox.Show("برجاء التأكد من إدخال جميع البيانات قبل الحفظ");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            if (dataGridView2.RowCount > 0 && export_total.Text != "" && export_paid.Text != "")
            {


                SaveFileDialog save_file = new SaveFileDialog();
                save_file.Filter = "PDF (*.pdf)|*.pdf";
                save_file.FileName = "pdf1.pdf";
                bool fileError = false;
                if (save_file.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save_file.FileName))
                    {
                        try
                        {
                            File.Delete(save_file.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("Error" + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            string fontLoc = @"c:\windows\fonts\Arial.ttf";
                            BaseFont basFont = BaseFont.CreateFont(fontLoc, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            iTextSharp.text.Font font = new iTextSharp.text.Font(basFont, 12);


                            PdfPTable pdfT = new PdfPTable(dataGridView2.Columns.Count - 1);
                            pdfT.DefaultCell.Padding = 3;
                            pdfT.WidthPercentage = 50;
                            pdfT.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfT.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                            foreach (DataGridViewColumn column in dataGridView2.Columns)
                            {
                                if (column.HeaderText == "رقم المنتج")
                                {

                                }
                                else
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, font));
                                    pdfT.AddCell(cell);
                                }
                            }
                            foreach (DataGridViewRow row in dataGridView2.Rows)
                            {
                                int i = 0;
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    if (cell.Value == null)
                                    {

                                    }
                                    else
                                    {
                                        if (i == 1)
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            i++;
                                            PdfPCell c = new PdfPCell(new Phrase(cell.Value.ToString(), font));
                                            pdfT.AddCell(c);
                                        }
                                    }
                                }
                            }
                            //MessageBox.Show("hi");
                            using (FileStream stream = new FileStream(save_file.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A7, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                PdfPTable pdfT2 = new PdfPTable(1);
                                pdfT2.DefaultCell.Padding = 3;
                                pdfT2.WidthPercentage = 100;
                                pdfT2.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfT2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                                string client_name = "     بيان فاتورة للعميل   ...... " + export_company.Text;

                                PdfPCell cell = new PdfPCell(new Phrase(client_name, font));
                                pdfT2.AddCell(cell);
                                //pdfDoc.AddTitle("فاتوره");
                                //pdfDoc.Add(new Paragraph("بيان فاتورة"));
                                //pdfDoc.Add(new Phrase(12,"بيان فاتورة",font));
                                pdfDoc.Add(pdfT2);
                                pdfDoc.Add(pdfT);

                                PdfPTable pdfT3 = new PdfPTable(2);
                                pdfT3.DefaultCell.Padding = 3;
                                pdfT3.WidthPercentage = 100;
                                pdfT3.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfT3.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                                cell = new PdfPCell(new Phrase("اجمالى الفاتورة", font));
                                pdfT3.AddCell(cell);
                                cell = new PdfPCell(new Phrase(export_total.Text, font));
                                pdfT3.AddCell(cell);
                                cell = new PdfPCell(new Phrase("المدفوع", font));
                                pdfT3.AddCell(cell);
                                cell = new PdfPCell(new Phrase(export_paid.Text, font));
                                pdfT3.AddCell(cell);
                                cell = new PdfPCell(new Phrase("اجمالى الفاتورالباقى", font));
                                pdfT3.AddCell(cell);
                                cell = new PdfPCell(new Phrase(export_remain.Text, font));
                                pdfT3.AddCell(cell);
                                PdfPTable pdfT4 = new PdfPTable(1);
                                pdfT4.DefaultCell.Padding = 3;
                                pdfT4.WidthPercentage = 100;
                                pdfT4.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfT4.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                                cell = new PdfPCell(new Phrase("شكرا لزيارتكم لمعرض اليسر فون", font));
                                pdfT4.AddCell(cell);

                                pdfDoc.Add(pdfT3);
                                pdfDoc.Add(pdfT4);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            // MessageBox.Show("completed", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error  :" + ex.Message);
                        }
                    }
                    MessageBox.Show("تم حفظ الفاتوره بنجاح الرجاء تأكيد الفاتورة قبل الخروج ");
                    //default

                }

                else
                {
                    MessageBox.Show("يجب التأكد من وجود فاتوره وادخال المبلغ المدفوع منها قبل الحفظ");
                }
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (this.dataGridView2.SelectedRows.Count > 0)
            {

                int index = this.dataGridView2.SelectedRows[0].Index;
                DataGridViewRow row = dataGridView2.Rows[index];
                total_price2 = total_price2 - float.Parse(row.Cells["dataGridViewTextBoxColumn6"].Value.ToString());
                export_total.Text = total_price2.ToString();
                dataGridView2.Rows.RemoveAt(index);
                this.dataGridView2.Refresh();
            }
            else
            {
                MessageBox.Show("برجاء اختيار المراد حذفه  من الفاتوره اولآ ");
            }
        }

        private void export_total_TextChanged(object sender, EventArgs e)
        {

            if (bill_sellOrback.SelectedIndex == 0)
            {
                if (export_total.Text != "" && export_clientold.Text != "")
                {
                    export_clientTotal.Text = (float.Parse(export_total.Text) + float.Parse(export_clientold.Text)).ToString();
                }
            }
            else
            {
                if (export_total.Text != "" && export_clientold.Text != "")
                {

                    export_remain.Text = (float.Parse(export_clientold.Text) - (float.Parse(export_total.Text))).ToString();
                }
            }
        }

        private void export_paid_TextChanged(object sender, EventArgs e)
        {
            if (bill_sellOrback.SelectedIndex == 0)
            {
                if (export_clientTotal.Text != "" && export_paid.Text != "")
                {
                    if (float.Parse(export_paid.Text) > float.Parse(export_clientTotal.Text))
                    {
                        MessageBox.Show("الرجاء الانتبهاء فان قيمه المبلغ المدفوعه اكبر من القيمه الكليه");
                        export_paid.Text = "";
                    }
                    else
                        export_remain.Text = (float.Parse(export_clientTotal.Text) - float.Parse(export_paid.Text)).ToString();
                }
            }
        }

        private void updateClient_btnSave_Click(object sender, EventArgs e)
        {
            bool isfound = false;
            if (updateClient_txtPhone.Text != "")
                isfound = true;
            if (updateClient_txtName.Text != "" && updateClient_txt_cashremain.Text != "" && updateClient_list.Text.ToString() != "")
            {
                try
                {

                    command = new MySqlCommand("update userdata set name=@name,phone=@phone,address=@address,notes=@notes,remained=@remained where id=" + updateClient_list.SelectedValue.ToString(), connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();


                    command.Parameters.AddWithValue("@name", updateClient_txtName.Text);
                    if (isfound)
                        command.Parameters.AddWithValue("@phone", double.Parse(updateClient_txtPhone.Text));
                    else
                        command.Parameters.AddWithValue("@phone", updateClient_txtPhone.Text);
                    command.Parameters.AddWithValue("@address", updateClient_txtAddress.Text);
                    command.Parameters.AddWithValue("@notes", updateClient_txtNote.Text);
                    command.Parameters.AddWithValue("@remained", double.Parse(updateClient_txt_cashremain.Text));
                    //command.Parameters.AddWithValue("@number", double.Parse(updateClient_txt_number.Text));

                    int n = command.ExecuteNonQuery();
                    if (n > 0)
                    {
                        MessageBox.Show("تم الحفظ");
                    }
                }
                catch (Exception ex)
                {
                    //  MessageBox.Show(ex.Message);
                    MessageBox.Show("برجاء التأكد من صحة القيم المدخله");
                }
            }

            else
            {
                MessageBox.Show("برجاء التأكد من ادخال  الاسم والمتبقي قبل الحفظ");
            }
        }

        private void updateClient_txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                adapter = new MySqlDataAdapter("select * from userdata where type ='مشترى' and name like '%" + updateClient_txtSearch.Text + "%'", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                updateClient_list.SelectedIndexChanged -= updateClient_list_SelectedIndexChanged;
                updateClient_list.ValueMember = "id";
                updateClient_list.DisplayMember = "name";
                updateClient_list.DataSource = table;
                updateClient_list.SelectedIndexChanged += updateClient_list_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateClient_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                adapter = new MySqlDataAdapter("select * from userdata where id=" + updateClient_list.SelectedValue.ToString(), connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                updateClient_txtName.Text = dt.Rows[0]["name"].ToString();
                updateClient_txtPhone.Text = dt.Rows[0]["phone"].ToString();
                updateClient_txtAddress.Text = dt.Rows[0]["address"].ToString();
                updateClient_txtNote.Text = dt.Rows[0]["notes"].ToString();
                updateClient_txt_cashremain.Text = dt.Rows[0]["remained"].ToString();
                //updateClient_txt_number.Text = dt.Rows[0]["numbers"].ToString();
                label_clientName.Text = dt.Rows[0]["type"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bill_textSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable billTable = new DataTable();
            try
            {

                if (!check_date.Checked && !check_paid.Checked && !check_remain.Checked && check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and client like '%" + bill_textSearch.Text + "%' ", connection);
                    adapter.Fill(billTable);
                }
                else if (!check_date.Checked && !check_paid.Checked && !check_remain.Checked && !check_outOnly.Checked && check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and client like '%" + bill_textSearch.Text + "%' ", connection); adapter.Fill(billTable);
                }

                else if (check_date.Checked && !check_paid.Checked && !check_remain.Checked && !check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && !check_paid.Checked && !check_remain.Checked && check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && !check_paid.Checked && !check_remain.Checked && !check_outOnly.Checked && check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (!check_date.Checked && check_paid.Checked && !check_remain.Checked && !check_outOnly.Checked && check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where client like '%" + bill_textSearch.Text + "%' and remain <=0", connection);
                    adapter.Fill(billTable);
                }
                else if (!check_date.Checked && check_paid.Checked && !check_remain.Checked && check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and client like '%" + bill_textSearch.Text + "%' and remain <=0", connection);
                    adapter.Fill(billTable);
                }
                else if (!check_date.Checked && check_paid.Checked && !check_remain.Checked && !check_outOnly.Checked && check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and client like '%" + bill_textSearch.Text + "%' and remain <=0", connection);
                    adapter.Fill(billTable);
                }
                else if (!check_date.Checked && !check_paid.Checked && check_remain.Checked && !check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where client like '%" + bill_textSearch.Text + "%' and remain>0", connection);
                    adapter.Fill(billTable);
                }
                else if (!check_date.Checked && !check_paid.Checked && check_remain.Checked && check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and client like '%" + bill_textSearch.Text + "%' and remain>0", connection);
                    adapter.Fill(billTable);
                }
                else if (!check_date.Checked && !check_paid.Checked && check_remain.Checked && !check_outOnly.Checked && check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and client like '%" + bill_textSearch.Text + "%' and remain>0", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && check_paid.Checked && !check_remain.Checked && !check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "' and remain<=0", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && check_paid.Checked && !check_remain.Checked && check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "' and remain<=0", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && check_paid.Checked && !check_remain.Checked && !check_outOnly.Checked && check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "' and remain<=0", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && !check_paid.Checked && check_remain.Checked && !check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "' and remain>0", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && !check_paid.Checked && check_remain.Checked && check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "' and remain>0", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && !check_paid.Checked && check_remain.Checked && !check_outOnly.Checked && check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "' and remain>0", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && check_paid.Checked && check_remain.Checked && check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and  client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && check_paid.Checked && check_remain.Checked && !check_outOnly.Checked && check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (check_date.Checked && check_paid.Checked && check_remain.Checked && !check_outOnly.Checked && !check_inOnly.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where client like '%" + bill_textSearch.Text + "%' and date>='" + bill_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + bill_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else
                {
                    adapter = new MySqlDataAdapter("select * from bill where client like '%" + bill_textSearch.Text + "%'", connection);
                    adapter.Fill(billTable);

                }
                bill_list.SelectedIndexChanged -= bill_list_SelectedIndexChanged;
                bill_list.ValueMember = "id";
                bill_list.DisplayMember = "clientwitId";
                bill_list.DataSource = billTable;
                bill_list.SelectedIndexChanged += bill_list_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bill_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable tb = new DataTable();
                adapter = new MySqlDataAdapter("select cat_name as 'الفئه',item_name as 'اسم المنتج' ,price as 'سعرالوحده',number as 'الكميه',total_price as 'اجمالى السعر'  from orders where bill_id =" + int.Parse(bill_list.SelectedValue.ToString()) + "", connection);
                adapter.Fill(tb);

                bill_dgv.DataSource = tb;

                DataTable table = new DataTable();
                adapter = new MySqlDataAdapter("select * from bill where id =" + int.Parse(bill_list.SelectedValue.ToString()) + "", connection);
                adapter.Fill(table);
                txt_bill_no.Text = int.Parse(bill_list.SelectedValue.ToString()) + "";
                bill_name.Text = table.Rows[0]["client"].ToString();
                bill_date.Text = table.Rows[0]["date"].ToString();
                bill_type.Text = table.Rows[0]["type"].ToString();
                bill_total.Text = table.Rows[0]["total"].ToString();
                bill_paid.Text = table.Rows[0]["paid"].ToString();
                bill_remain.Text = table.Rows[0]["remain"].ToString();
                txt_inOut.Text = table.Rows[0]["outOrin"].ToString();
                bill_clientold.Text = table.Rows[0]["clientold"].ToString();
                bill_clienttotal.Text = table.Rows[0]["clienttotal"].ToString();
                //bill_number.Text = table.Rows[0]["clientNumbers"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void import_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            adapter = new MySqlDataAdapter("select * from item where id = " + int.Parse(import_list.SelectedValue.ToString()) + "", connection);
            adapter.Fill(table);

            import_label_count.Text = table.Rows[0]["count"].ToString();
            import_price.Text = table.Rows[0]["priceC"].ToString();
        }

        private void client_txtRemainMoney_TextChanged(object sender, EventArgs e)
        {

        }

        private void c_btn_show_Click(object sender, EventArgs e)
        {
            DataTable billTable = new DataTable();
            DataTable orderTable = new DataTable();
            c_dgv.Rows.Clear();
            if (c_combo_inOut.SelectedIndex == 0)
            {
                if (c_check_date.Checked && !c_check_in.Checked && !c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and  date>='" + c_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + c_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (c_check_date.Checked && c_check_in.Checked && !c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and type ='استيراد' and date>='" + c_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + c_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (c_check_date.Checked && !c_check_in.Checked && c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and type ='تصدير' and date>='" + c_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + c_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (!c_check_date.Checked && c_check_in.Checked && !c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and type ='استيراد'", connection);
                    adapter.Fill(billTable);
                }
                else if (!c_check_date.Checked && !c_check_in.Checked && c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' and type ='تصدير'", connection);
                    adapter.Fill(billTable);
                }
                else
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='داخلى' ", connection);
                    adapter.Fill(billTable);
                }
            }
            else if (c_combo_inOut.SelectedIndex == 1)
            {
                if (c_check_date.Checked && !c_check_in.Checked && !c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and date>='" + c_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + c_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (c_check_date.Checked && c_check_in.Checked && !c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and type ='استيراد' and date>='" + c_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + c_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (c_check_date.Checked && !c_check_in.Checked && c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and type ='تصدير' and date>='" + c_dateFrom.Value.ToString("yyyy-MM-dd") + "' and date <='" + c_dateTo.Value.ToString("yyyy-MM-dd") + "'", connection);
                    adapter.Fill(billTable);
                }
                else if (!c_check_date.Checked && c_check_in.Checked && !c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and type ='استيراد'", connection);
                    adapter.Fill(billTable);
                }
                else if (!c_check_date.Checked && !c_check_in.Checked && c_check_out.Checked)
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' and type ='تصدير'", connection);
                    adapter.Fill(billTable);
                }
                else
                {
                    adapter = new MySqlDataAdapter("select * from bill where outOrin='خارجى' ", connection);
                    adapter.Fill(billTable);
                }
            }
            // adapter=new MySqlDataAdapter("select * from orders where bill_id in ")

            int import_orders = 0, export_orders = 0;
            float total_out = 0, total_in = 0, total_earn = 0, remain_out = 0, remain_in = 0;
            for (int i = 0; i < billTable.Rows.Count; i++)
            {
                if (billTable.Rows[i]["type"].ToString() == "تصدير")
                {

                    export_orders += 1;
                    total_out += float.Parse(billTable.Rows[i]["total"].ToString());
                    if (billTable.Rows[i]["earn"].ToString() != "")
                        total_earn += float.Parse(billTable.Rows[i]["earn"].ToString());
                    remain_out += float.Parse(billTable.Rows[i]["remain"].ToString());

                }
                else
                {
                    import_orders += 1;
                    total_in += float.Parse(billTable.Rows[i]["total"].ToString());
                    remain_in += float.Parse(billTable.Rows[i]["remain"].ToString());

                }

                MySqlDataAdapter adapter2 = new MySqlDataAdapter("select * from orders where bill_id=" + int.Parse(billTable.Rows[i]["id"].ToString()) + "", connection);
                adapter2.Fill(orderTable);

                for (int j = 0; j < orderTable.Rows.Count; j++)
                {
                    c_dgv.Rows.Add(orderTable.Rows[j]["item_name"].ToString(),
                       orderTable.Rows[j]["cat_name"].ToString(),
                       orderTable.Rows[j]["number"].ToString(),
                       orderTable.Rows[j]["price"].ToString(),
                       orderTable.Rows[j]["total_price"].ToString()
                       , orderTable.Rows[j]["earn"].ToString()
                       , orderTable.Rows[j]["bill_id"].ToString()
                       , billTable.Rows[i]["type"].ToString()
                       , billTable.Rows[i]["client"].ToString()
                       , billTable.Rows[i]["date"].ToString());
                }

                c_totalOrders.Text = export_orders + import_orders + "";
                c_importOrders.Text = import_orders + "";
                c_exportOrders.Text = export_orders + "";
                c_totalEarn.Text = total_earn + "";
                c_remainIn.Text = remain_in + "";
                c_remainOut.Text = remain_out + "";
                c_totalOut.Text = total_out + "";
                c_totalIn.Text = total_in + "";
            }


        }

        private void r_txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            adapter = new MySqlDataAdapter("select * from userdata where name like '%" + r_txtSearch.Text + "%'", connection);
            adapter.Fill(table);

            r_list.SelectedIndexChanged -= r_list_SelectedIndexChanged;
            r_list.ValueMember = "id";
            r_list.DisplayMember = "name";
            r_list.DataSource = table;
            r_list.SelectedIndexChanged += r_list_SelectedIndexChanged;
        }

        private void r_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(r_list.SelectedValue.ToString());
            DataTable tb = new DataTable();
            adapter = new MySqlDataAdapter("select * from userdata where id =" + id + "", connection);
            adapter.Fill(tb);
            r_totalmoney.Text = tb.Rows[0]["cash"].ToString();
            r_totalRemain.Text = tb.Rows[0]["remained"].ToString();
            r_companyName.Text = tb.Rows[0]["name"].ToString();
            r_type.Text = tb.Rows[0]["type"].ToString();
        }

        private void r_btn_showCompany_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            adapter = new MySqlDataAdapter("select * from userdata where type='بائع'", connection);
            adapter.Fill(table);

            r_list.SelectedIndexChanged -= r_list_SelectedIndexChanged;
            r_list.ValueMember = "id";
            r_list.DisplayMember = "name";
            r_list.DataSource = table;
            r_list.SelectedIndexChanged += r_list_SelectedIndexChanged;
        }

        private void r_btn_showClient_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            adapter = new MySqlDataAdapter("select * from userdata where type='مشترى'", connection);
            adapter.Fill(table);

            r_list.SelectedIndexChanged -= r_list_SelectedIndexChanged;
            r_list.ValueMember = "id";
            r_list.DisplayMember = "name";
            r_list.DataSource = table;
            r_list.SelectedIndexChanged += r_list_SelectedIndexChanged;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            combo_inOrOut.Visible = false;
        }

        private void r_btn_pay_Click(object sender, EventArgs e)
        {
            if (r_totalCash.Text != "")
            {
                float cash = float.Parse(r_totalCash.Text);
                if (cash < 0)
                {
                    MessageBox.Show("تاكد من القيمه اللى مدخلها سيادتك يعنى");
                }
                command = new MySqlCommand("update userdata set remained =remained-" + cash + " where id =" + int.Parse(r_list.SelectedValue.ToString()) + "", connection);
                command.ExecuteNonQuery();
                MessageBox.Show("تم الحفظ بنجاح");
                r_totalCash.Text = "";
                r_totalmoney.Text = "";
                r_totalRemain.Text = "";
            }
        }

        private void انشاءنسخهاحتياطيهToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog folderBrowser = new OpenFileDialog();
                // Set validate names and check file exists to false otherwise windows will
                // not let you select "Folder Selection."
                folderBrowser.ValidateNames = false;
                folderBrowser.CheckFileExists = false;
                folderBrowser.CheckPathExists = true;
                // Always default to Folder Selection.
                folderBrowser.FileName = "Folder Selection.";


                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                    //MessageBox.Show(folderPath + "\\oil_backup" + DateTime.Now.ToString("yyyy,mm,dd-HH,mm,ss") + ".sql");
                    connection.Close();
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                mb.ExportToFile(folderPath + "\\oil_backup" + DateTime.Now.ToString("yyyy,MM,dd-HH,mm,ss") + ".sql");
                                conn.Close();

                            }
                        }
                    }

                    // cmd = new MySqlCommand(@"BACKUP DATABASE mobile1 TO DISK = '" + ,conn);
                    // cmd.ExecuteNonQuery();
                    MessageBox.Show("تم انشاء نسخه احتياطيه بنجاح");
                    connection.Open();
                    // ...
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void استعادهنسخهToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "sql|*.sql";
            DialogResult res = of.ShowDialog();
            if (res == DialogResult.OK)
            {
                try
                {
                    connection.Close();
                    using (MySqlConnection conn = new MySqlConnection(connstr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                mb.ImportFromFile(of.FileName);
                                connection.Close();
                            }
                        }
                    }
                    /*cmd = new MySqlCommand("use master", conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand("RESTORE DATABASE mobile1 FROM DISK = '" + of.FileName + "'", conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand("use mobile1", conn);
                    cmd.ExecuteNonQuery();*/
                    connection.Open();
                    MessageBox.Show("تمت الاستعاده بنجاح");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        private void خروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();

            f.Show();
            this.Hide();

        }

        private void الحسابToolStripMenuItem_Click(object sender, EventArgs e)
        {
            admin_account = false;
            Form2 f = new Form2();
            this.Hide();
            f.ShowDialog();

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void مننحنToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("م/مصطفى مبرمج ومطور تطبيقات اندرويد والذكاء الاصطناعى   ");
        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            adapter = new MySqlDataAdapter("select id,name,count,countinStore,priceC,priceB,priceA from item where cat_id=" + comboBox3.SelectedValue, connection);
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dt.Columns[0].ColumnName = "رقم المنتج";
                dt.Columns[1].ColumnName = "اسم المنتج";
                dt.Columns[2].ColumnName = "العدد فالمحل";
                dt.Columns[3].ColumnName = "العدد فالمخزن";
                dt.Columns[4].ColumnName = "سعر الشراء";
                dt.Columns[5].ColumnName = "سعر الجملة";
                dt.Columns[6].ColumnName = "سعر القطاعى";



                if (dt.Rows.Count > 0)
                    dataGridView3.DataSource = dt;
                else
                    MessageBox.Show("لا يوجد منتجات لهده الفئه");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            adapter = new MySqlDataAdapter("select id,name,count,countinStore,priceC,priceB,priceA from item where id=" + comboBox2.SelectedValue, connection);
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView3.DataSource = dt;
                dt.Columns[0].ColumnName = "رقم المنتج";
                dt.Columns[1].ColumnName = "اسم المنتج";
                dt.Columns[2].ColumnName = "العدد فالمحل";
                dt.Columns[3].ColumnName = "العدد فالمخزن";
                dt.Columns[4].ColumnName = "سعر الشراء";
                dt.Columns[5].ColumnName = "سعر الجملة";
                dt.Columns[6].ColumnName = "سعر القطاعى";

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            adapter = new MySqlDataAdapter("select cat.`name`,item.id,item.name,priceC,priceB,priceA,countinStore from item ,cat where count>0 and cat_id=cat.id", connection);
            try
            {
                DataTable dt1 = new DataTable();

                adapter.Fill(dt1);

                dataGridView3.DataSource = dt1;
                dt1.Columns[0].ColumnName = "الفئه";
                dt1.Columns[1].ColumnName = "رقم المنتج";
                dt1.Columns[2].ColumnName = "اسم المنتج";
                dt1.Columns[3].ColumnName = "سعر الشراء";
                dt1.Columns[4].ColumnName = "سعر الجمله";
                dt1.Columns[5].ColumnName = "سعر القطاعى";
                dt1.Columns[6].ColumnName = "العدد فالمخزن";
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            adapter = new MySqlDataAdapter("select cat.`name`,item.name from item ,cat where countinStore=0 and cat_id=cat.id", connection);
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView3.DataSource = dt;
                    dt.Columns[0].ColumnName = "الفئه";
                    dt.Columns[1].ColumnName = "اسم المنتج";
                }
                else
                    MessageBox.Show("لا يوجد عجز في المخزن");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void store_searchName_TextChanged(object sender, EventArgs e)
        {
            string name = store_searchName.Text.ToString();
            if (name != "" && name != null && name != " ")
            {
                try
                {
                    adapter = new MySqlDataAdapter("select cat.`name`,item.id,item.name,count,countinStore,priceC,priceB,priceA from item,cat where cat_id=cat.id and item.name like '%" + name + "%'", connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns[0].ColumnName = "الفئه";
                    dt.Columns[1].ColumnName = "رقم المنتج";
                    dt.Columns[2].ColumnName = "اسم المنتج";
                    dt.Columns[3].ColumnName = "العدد فالمحل";
                    dt.Columns[4].ColumnName = "العدد فالمخزن";
                    dt.Columns[5].ColumnName = "سعر الشراء";
                    dt.Columns[6].ColumnName = "سعر الجمله";
                    dt.Columns[7].ColumnName = "سعر القطاعى ";



                    if (dt.Rows.Count > 0)
                        dataGridView3.DataSource = dt;
                    else
                        MessageBox.Show("لا يوجد منتجات لهده الفئه");
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage11_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                history_dgv.Rows.Clear();
                DataTable table = new DataTable();
                adapter = new MySqlDataAdapter("select * from item_copy where date ='" + history_date.Value.ToString("yyyy-MM-dd") + "'", connection);
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    //MessageBox.Show(table.Rows.Count + "");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        history_dgv.Rows.Add(table.Rows[i]["name"].ToString(), table.Rows[i]["count"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("لا يوجد اى منتجات فى هذا اليوم ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button18_Click(object sender, EventArgs e)
        {

            SaveFileDialog save_file = new SaveFileDialog();
            save_file.Filter = "PDF (*.pdf)|*.pdf";
            save_file.FileName = "pdf1.pdf";
            bool fileError = false;
            if (save_file.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(save_file.FileName))
                {
                    try
                    {
                        File.Delete(save_file.FileName);
                    }
                    catch (IOException ex)
                    {
                        fileError = true;
                        MessageBox.Show("Error" + ex.Message);
                    }
                }
                if (!fileError)
                {
                    try
                    {
                        string fontLoc = @"c:\windows\fonts\Arial.ttf";
                        BaseFont basFont = BaseFont.CreateFont(fontLoc, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font font = new iTextSharp.text.Font(basFont, 12);


                        PdfPTable pdfT = new PdfPTable(dataGridView3.Columns.Count);
                        pdfT.DefaultCell.Padding = 3;
                        pdfT.WidthPercentage = 100;
                        pdfT.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfT.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                        foreach (DataGridViewColumn column in dataGridView3.Columns)
                        {

                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, font));
                            pdfT.AddCell(cell);

                        }
                        foreach (DataGridViewRow row in dataGridView3.Rows)
                        {

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value == null)
                                {

                                }
                                else
                                {

                                    PdfPCell c = new PdfPCell(new Phrase(cell.Value.ToString(), font));
                                    pdfT.AddCell(c);

                                }
                            }
                        }
                        //MessageBox.Show("hi");
                        using (FileStream stream = new FileStream(save_file.FileName, FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            PdfPTable pdfT2 = new PdfPTable(1);
                            pdfT2.DefaultCell.Padding = 3;
                            pdfT2.WidthPercentage = 100;
                            pdfT2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfT2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            PdfPCell cell = new PdfPCell(new Phrase("بيان المخزن", font));
                            pdfT2.AddCell(cell);
                            //pdfDoc.AddTitle("فاتوره");
                            //pdfDoc.Add(new Paragraph("بيان فاتورة"));
                            //pdfDoc.Add(new Phrase(12,"بيان فاتورة",font));
                            pdfDoc.Add(pdfT2);
                            pdfDoc.Add(pdfT);

                            /* PdfPTable pdfT3 = new PdfPTable(2);
                             pdfT3.DefaultCell.Padding = 3;
                             pdfT3.WidthPercentage = 100;
                             pdfT3.HorizontalAlignment = Element.ALIGN_LEFT;
                             pdfT3.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                             cell = new PdfPCell(new Phrase("اجمالى الفاتورة", font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase(export_total.Text, font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase("المدفوع", font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase(export_paid.Text, font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase("الباقي", font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase(export_remain.Text, font));
                             pdfT3.AddCell(cell);
                             PdfPTable pdfT4 = new PdfPTable(1);
                             pdfT4.DefaultCell.Padding = 3;
                             pdfT4.WidthPercentage = 100;
                             pdfT4.HorizontalAlignment = Element.ALIGN_LEFT;
                             pdfT4.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                             cell = new PdfPCell(new Phrase("شكرا لزيارتكم لمعرض الدوينى فون", font));
                             pdfT4.AddCell(cell);

                             pdfDoc.Add(pdfT3);
                             pdfDoc.Add(pdfT4);
                             */
                            pdfDoc.Close();
                            stream.Close();
                        }

                        MessageBox.Show("تم الحفظ بنجاح", "Info");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error  :" + ex.Message);
                    }
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            adapter = new MySqlDataAdapter("select name,priceC from item where count>0", connection);
            try
            {
                DataTable dt1 = new DataTable();

                adapter.Fill(dt1);
                adapter = new MySqlDataAdapter("select name  from cat where id in(select cat_id from item)as name_cat", connection);
                dataGridView3.DataSource = dt1;
                dt1.Columns[0].ColumnName = "اسم المنتج";
                dt1.Columns[1].ColumnName = "سعر البيع بالجمله";

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

            SaveFileDialog save_file = new SaveFileDialog();
            save_file.Filter = "PDF (*.pdf)|*.pdf";
            save_file.FileName = "pdf1.pdf";
            bool fileError = false;
            if (save_file.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(save_file.FileName))
                {
                    try
                    {
                        File.Delete(save_file.FileName);
                    }
                    catch (IOException ex)
                    {
                        fileError = true;
                        MessageBox.Show("Error" + ex.Message);
                    }
                }
                if (!fileError)
                {
                    try
                    {
                        string fontLoc = @"c:\windows\fonts\Arial.ttf";
                        BaseFont basFont = BaseFont.CreateFont(fontLoc, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font font = new iTextSharp.text.Font(basFont, 12);


                        PdfPTable pdfT = new PdfPTable(dataGridView3.Columns.Count);
                        pdfT.DefaultCell.Padding = 3;
                        pdfT.WidthPercentage = 100;
                        pdfT.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfT.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

                        foreach (DataGridViewColumn column in dataGridView3.Columns)
                        {

                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, font));
                            pdfT.AddCell(cell);

                        }
                        foreach (DataGridViewRow row in dataGridView3.Rows)
                        {

                            foreach (DataGridViewCell cell in row.Cells)
                            {

                                if (cell.Value == null)
                                {

                                }
                                else
                                {

                                    PdfPCell c = new PdfPCell(new Phrase(cell.Value.ToString(), font));
                                    pdfT.AddCell(c);

                                }
                            }
                        }
                        //MessageBox.Show("hi");
                        using (FileStream stream = new FileStream(save_file.FileName, FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            PdfPTable pdfT2 = new PdfPTable(1);
                            pdfT2.DefaultCell.Padding = 3;
                            pdfT2.WidthPercentage = 100;
                            pdfT2.HorizontalAlignment = Element.ALIGN_LEFT;
                            pdfT2.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            PdfPCell cell = new PdfPCell(new Phrase("فاتورة اسعار", font));
                            pdfT2.AddCell(cell);
                            //pdfDoc.AddTitle("فاتوره");
                            //pdfDoc.Add(new Paragraph("بيان فاتورة"));
                            //pdfDoc.Add(new Phrase(12,"بيان فاتورة",font));
                            pdfDoc.Add(pdfT2);
                            pdfDoc.Add(pdfT);

                            /* PdfPTable pdfT3 = new PdfPTable(2);
                             pdfT3.DefaultCell.Padding = 3;
                             pdfT3.WidthPercentage = 100;
                             pdfT3.HorizontalAlignment = Element.ALIGN_LEFT;
                             pdfT3.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                             cell = new PdfPCell(new Phrase("اجمالى الفاتورة", font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase(export_total.Text, font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase("المدفوع", font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase(export_paid.Text, font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase("الباقي", font));
                             pdfT3.AddCell(cell);
                             cell = new PdfPCell(new Phrase(export_remain.Text, font));
                             pdfT3.AddCell(cell);
                             PdfPTable pdfT4 = new PdfPTable(1);
                             pdfT4.DefaultCell.Padding = 3;
                             pdfT4.WidthPercentage = 100;
                             pdfT4.HorizontalAlignment = Element.ALIGN_LEFT;
                             pdfT4.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                             cell = new PdfPCell(new Phrase("شكرا لزيارتكم لمعرض الدوينى فون", font));
                             pdfT4.AddCell(cell);

                             pdfDoc.Add(pdfT3);
                             pdfDoc.Add(pdfT4);
                             */
                            pdfDoc.Close();
                            stream.Close();
                        }

                        MessageBox.Show("تم الحفظ بنجاح", "Info");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error  :" + ex.Message);
                    }
                }
            }
        }

        private void طباعهباركودToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sell_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                bool found = false;
                int index = 0;
                DataTable table = new DataTable();
                adapter = new MySqlDataAdapter("select * from item where barcode = '" + sell_barcode.Text + "'", connection);
                adapter.Fill(table);
                //MessageBox.Show(table.Rows[0]["id"].ToString());
                back_item_id = int.Parse(table.Rows[0]["id"].ToString());
                if (radio_priceA.Checked)
                {
                    export_price.Text = table.Rows[0]["priceA"].ToString();
                }
                else
                {
                    export_price.Text = table.Rows[0]["priceB"].ToString();
                }
                for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    if (dataGridView2.Rows[i].Cells[1].Value.ToString() == table.Rows[0]["id"].ToString())
                    {
                        found = true;
                        index = i;
                        break;
                    }
                }
                if (found)
                {
                    if (bill_sellOrback.SelectedIndex == 0)
                    {
                        int count = int.Parse(table.Rows[0]["count"].ToString()) - int.Parse(dataGridView2.Rows[index].Cells[4].Value.ToString());
                    }
                    else
                    {
                        int count = int.Parse(table.Rows[0]["count"].ToString()) + int.Parse(dataGridView2.Rows[index].Cells[4].Value.ToString());

                    }
                    export_label_count.Text = count.ToString();
                }
                else
                {
                    export_label_count.Text = table.Rows[0]["count"].ToString();
                }

                sell_label_itemName.Text = table.Rows[0]["name"].ToString();
                MySqlCommand cmd = new MySqlCommand("select name from cat where id = @v1", connection);
                cmd.Parameters.AddWithValue("@v1", table.Rows[0]["cat_id"].ToString());
                sell_label_catName.Text = cmd.ExecuteScalar().ToString();

            }
        }

        private void حسابالادمنToolStripMenuItem_Click(object sender, EventArgs e)
        {
            admin_account = true;
            Form2 f = new Form2();
            this.Hide();
            f.ShowDialog();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            FinishTheBill();
            dataGridView2.Rows.Clear();
            total_price2 = 0;
            export_total.Text = "0";
            export_remain.Text = "0";
            export_paid.Text = "0";
        }
        void FinishTheBill()
        {
            if (dataGridView2.RowCount > 0 && export_total.Text != "" && export_paid.Text != "")
            {
                //insert new row in bill table
                float total_earn = 0;
                try
                {
                    if (bill_sellOrback.SelectedIndex == 0)
                    {
                        MySqlCommand command1 = new MySqlCommand("insert into bill (type,client,date,total,paid,remain,outOrin,clientold,clienttotal,sellOrBack) values (@t,@c,@d,@total,@p,@r,@oi,@co,@ct,@sb)", connection);
                        command1.Parameters.AddWithValue("@t", "تصدير");
                        command1.Parameters.AddWithValue("@c", export_company.Text);
                        command1.Parameters.AddWithValue("@d", export_date.Value.ToString("yyyy-MM-dd"));
                        command1.Parameters.AddWithValue("@total", float.Parse(export_total.Text));
                        command1.Parameters.AddWithValue("@p", float.Parse(export_paid.Text));
                        command1.Parameters.AddWithValue("@r", float.Parse(export_remain.Text));
                        command1.Parameters.AddWithValue("@oi", (combo_inOrOut.Text));
                        command1.Parameters.AddWithValue("@co", float.Parse(export_clientold.Text));
                        command1.Parameters.AddWithValue("@ct", float.Parse(export_clientTotal.Text));
                        command1.Parameters.AddWithValue("@sb", (bill_sellOrback.Text));

                        //command1.Parameters.AddWithValue("@cn", int.Parse(export_clientNumber.Text));
                        command1.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand command1 = new MySqlCommand("insert into bill (type,client,date,total,remain,outOrin,clientold,sellOrBack) values (@t,@c,@d,@total,@r,@oi,@co,@sb)", connection);
                        command1.Parameters.AddWithValue("@t", "تصدير");
                        command1.Parameters.AddWithValue("@c", export_company.Text);
                        command1.Parameters.AddWithValue("@d", export_date.Value.ToString("yyyy-MM-dd"));
                        command1.Parameters.AddWithValue("@total", float.Parse(export_total.Text));
                        command1.Parameters.AddWithValue("@r", float.Parse(export_remain.Text));
                        command1.Parameters.AddWithValue("@oi", (combo_inOrOut.Text));
                        command1.Parameters.AddWithValue("@co", float.Parse(export_clientold.Text));
                        command1.Parameters.AddWithValue("@sb", (bill_sellOrback.Text));
                        //command1.Parameters.AddWithValue("@cn", int.Parse(export_clientNumber.Text));
                        command1.ExecuteNonQuery();
                        // MessageBox.Show("hi");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("يجب التأكد من اختيار اسم الشركه");
                }

                //update userData information 
                // MessageBox.Show("hi");
                command = new MySqlCommand("update userdata set remained =" + float.Parse(export_remain.Text) + " , cash=cash+" + float.Parse(export_total.Text) + " where id = " + int.Parse(export_company.SelectedValue.ToString()) + "", connection);
                command.ExecuteNonQuery();
                // get the id of the last bill we store 
                MySqlCommand command2 = new MySqlCommand("select id from bill order by id desc", connection);
                int id = int.Parse(command2.ExecuteScalar().ToString());

                // iterate the datagridview to insert all the rows in order table and decrease the count of the items and calculate the earn
                for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    MySqlCommand command = new MySqlCommand("select priceC from item where id=" + int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) + "", connection);
                    float priceC = float.Parse(command.ExecuteScalar().ToString());

                    float dgv_count = float.Parse(dataGridView2.Rows[i].Cells[4].Value.ToString());
                    float dgv_price = float.Parse(dataGridView2.Rows[i].Cells[3].Value.ToString());
                    float earnPerOne = dgv_price - priceC;
                    float earn = earnPerOne * (float)dgv_count;
                    total_earn += earn;
                    command = new MySqlCommand("insert into orders (item_id,number,price,total_price,item_name,cat_name,bill_id,earn) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)", connection);
                    command.Parameters.AddWithValue("@v1", int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()));
                    command.Parameters.AddWithValue("@v2", float.Parse(dataGridView2.Rows[i].Cells[4].Value.ToString()));
                    command.Parameters.AddWithValue("@v3", float.Parse(dataGridView2.Rows[i].Cells[3].Value.ToString()));
                    command.Parameters.AddWithValue("@v4", float.Parse(dataGridView2.Rows[i].Cells[5].Value.ToString()));
                    command.Parameters.AddWithValue("@v5", dataGridView2.Rows[i].Cells[2].Value.ToString());
                    command.Parameters.AddWithValue("@v6", dataGridView2.Rows[i].Cells[0].Value.ToString());
                    command.Parameters.AddWithValue("@v7", id);
                    if (bill_sellOrback.SelectedIndex == 0)
                    {
                        command.Parameters.AddWithValue("@v8", (earn));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@v8", (earn * -1));

                    }
                    command.ExecuteNonQuery();
                    //MessageBox.Show("hi");
                    command = new MySqlCommand("select count from item where id=" + float.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) + "", connection);
                    float count = float.Parse(command.ExecuteScalar().ToString());
                    if (bill_sellOrback.SelectedIndex == 0)
                    {
                        if (count < float.Parse(dataGridView2.Rows[i].Cells[4].Value.ToString()))
                        {
                            MessageBox.Show("الكميه الموجوده فالمخزن اقل من الكميه المراد بيعها");
                            MessageBox.Show("خطأ فالكميه الموجوده فالصف رقم " + (i + 1) + "");
                            return;
                        }
                        //MessageBox.Show("hi");
                        command = new MySqlCommand("update item set count=count-" + dgv_count + " where id =" + int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) + "", connection);
                        command.ExecuteNonQuery();
                        //MessageBox.Show("hi");
                        command = new MySqlCommand("update bill set earn=" + total_earn + " where id =" + id + "", connection);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command = new MySqlCommand("update item set count=count+" + dgv_count + " where id =" + int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) + "", connection);
                        command.ExecuteNonQuery();

                        command = new MySqlCommand("update bill set earn=" + (total_earn * -1) + " where id =" + id + "", connection);
                        command.ExecuteNonQuery();
                    }


                }
                //MessageBox.Show("hi");
                DataTable billTable = new DataTable();
                adapter = new MySqlDataAdapter("select * from bill", connection);
                adapter.Fill(billTable);
                for (int i = 0; i < billTable.Rows.Count; i++)
                {
                    string bill_id = billTable.Rows[i]["id"].ToString();
                    string client = billTable.Rows[i]["client"].ToString();
                    string clintWithId = client + "-" + bill_id;
                    command = new MySqlCommand("update bill set clientwitId ='" + clintWithId + "' where id=" + int.Parse(bill_id) + ";", connection);
                    command.ExecuteNonQuery();
                }
                //MessageBox.Show("hi");
                // add to client_details 
                if (bill_sellOrback.SelectedIndex == 0)
                {
                    float bill_total = float.Parse(export_total.Text);
                    float bill_paid = float.Parse(export_paid.Text);
                    int client_id = int.Parse(export_company.SelectedValue.ToString());
                    if (bill_total > bill_paid)
                    {
                        DataTable table = new DataTable();
                        adapter = new MySqlDataAdapter("select * from userdata where id=" + client_id + "", connection);
                        adapter.Fill(table);

                        float cash = bill_total - bill_paid;
                        command = new MySqlCommand("insert into client_details(client_id,type,value,date,total)value(@v1,@v2,@v3,@v4,@v5)", connection);
                        command.Parameters.AddWithValue("@v1", client_id);
                        command.Parameters.AddWithValue("@v2", "اجل فاتورة");
                        command.Parameters.AddWithValue("@v3", cash);
                        command.Parameters.AddWithValue("@v4", export_date.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@v5", float.Parse(table.Rows[0]["remained"].ToString()));
                        command.ExecuteNonQuery();
                        //MessageBox.Show("hi");
                    }

                    else if (bill_total < bill_paid)
                    {
                        DataTable table = new DataTable();
                        adapter = new MySqlDataAdapter("select * from userdata where id=" + client_id + "", connection);
                        adapter.Fill(table);

                        float cash = bill_paid - bill_total;
                        command = new MySqlCommand("insert into client_details(client_id,type,value,date,total)value(@v1,@v2,@v3,@v4,@v5)", connection);
                        command.Parameters.AddWithValue("@v1", client_id);
                        command.Parameters.AddWithValue("@v2", "قبض اثناء الفاتورة");
                        command.Parameters.AddWithValue("@v3", cash);
                        command.Parameters.AddWithValue("@v4", export_date.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@v5", float.Parse(table.Rows[0]["remained"].ToString()));
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    float cash = float.Parse(export_total.Text);
                    int client_id = int.Parse(export_company.SelectedValue.ToString());
                    DataTable table = new DataTable();
                    adapter = new MySqlDataAdapter("select * from userdata where id=" + client_id + "", connection);
                    adapter.Fill(table);


                    command = new MySqlCommand("insert into client_details(client_id,type,value,date,total)value(@v1,@v2,@v3,@v4,@v5)", connection);
                    command.Parameters.AddWithValue("@v1", client_id);
                    command.Parameters.AddWithValue("@v2", "مرتجع ");
                    command.Parameters.AddWithValue("@v3", cash);
                    command.Parameters.AddWithValue("@v4", export_date.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@v5", float.Parse(table.Rows[0]["remained"].ToString()));
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("تم حفظ الفاتوره بنجاح");
                //default

            }
            else
            {
                MessageBox.Show("برجاء اختيار المراد حذفه  من الفاتوره اولآ ");

            }
        }
        private void button21_Click(object sender, EventArgs e)
        {
            if (dataGridView2.RowCount > 0 && export_total.Text != "" && export_paid.Text != "")
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    FinishTheBill();
                    ((Form)printPreviewDialog1).WindowState = FormWindowState.Maximized;
                    if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                    {

                        printDocument1.Print();


                    }

                }
                else
                {
                    MessageBox.Show("لا يوجد اى منتجات لطباعتها ", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                dataGridView2.Rows.Clear();
                total_price2 = 0;
                export_total.Text = "0";
                export_remain.Text = "0";
                export_paid.Text = "0";
                //export_clientNumber.Text = "0";
                export_clientTotal.Text = "0";
                export_clientold.Text = "0";

                /* DataTable table = new DataTable();
                 adapter = new MySqlDataAdapter("Select * from userdata where type='مشترى'", connection);
                 table = new DataTable();
                 adapter.Fill(table);

                 export_company.SelectedIndexChanged -= export_company_SelectedIndexChanged;
                 export_company.ValueMember = "id";
                 export_company.DisplayMember = "name";
                 export_company.DataSource = table;
                 export_company.SelectedIndexChanged += export_company_SelectedIndexChanged;
                 */
                //export_company.SelectedIndex = -1;
                //loadData(1);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

            string name = textBox7.Text.ToString();
            if (name != "" && name != null && name != " ")
            {
                try
                {

                    //MessageBox.Show(export_combo.SelectedValue + "");
                    command = new MySqlCommand("select * from item where cat_id=@a and item.name like '%" + name + "%'", connection);
                    DataTable dt = new DataTable();
                    command.Parameters.AddWithValue("@a", export_combo.SelectedValue);
                    MySqlDataReader dr = command.ExecuteReader();
                    command.Parameters.Clear();
                    dt.Load(dr);
                    export_list.ValueMember = "id";
                    export_list.DisplayMember = "name";
                    export_list.DataSource = dt;

                }

                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            string name = textBox8.Text.ToString();
            if (name != "" && name != null && name != " ")
            {
                try
                {

                    //MessageBox.Show(export_combo.SelectedValue + "");
                    command = new MySqlCommand("select * from item where cat_id=@a and item.name like '%" + name + "%'", connection);
                    DataTable dt = new DataTable();
                    command.Parameters.AddWithValue("@a", import_combo.SelectedValue);
                    MySqlDataReader dr = command.ExecuteReader();
                    command.Parameters.Clear();
                    dt.Load(dr);
                    import_list.ValueMember = "id";
                    import_list.DisplayMember = "name";
                    import_list.DataSource = dt;

                }

                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }

        private void Home_MaximumSizeChanged(object sender, EventArgs e)
        {

        }

        private void Home_MaximizedBoundsChanged(object sender, EventArgs e)
        {

        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void label75_Click(object sender, EventArgs e)
        {

        }

        private void r_totalCash_TextChanged(object sender, EventArgs e)
        {

        }

        private void label82_Click(object sender, EventArgs e)
        {

        }

        private void r_totalRemain_TextChanged(object sender, EventArgs e)
        {

        }

        private void label81_Click(object sender, EventArgs e)
        {

        }

        private void r_totalmoney_TextChanged(object sender, EventArgs e)
        {

        }

        private void label80_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            adapter = new MySqlDataAdapter("select * from bill order by id desc limit 1 ", connection);
            DataTable billTable = new DataTable();
            adapter.Fill(billTable);

            System.Drawing.Font f1 = new System.Drawing.Font("Arial", 14, FontStyle.Bold);
            System.Drawing.Font f2 = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            System.Drawing.Font f3 = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
            System.Drawing.Font f4 = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
            System.Drawing.Font f5 = new System.Drawing.Font("Arial", 6, FontStyle.Bold);

            float margin = 30;

            string address = ">>>>>>الـفـاتــــورة<<<<<<";
            string StoreName = "اليـسـر للتوكيلات التجارية";
            string StorePhone1 = "ت/01026097624";
            string StorePhone2 = "ت/01110345857";
            string StoreName2 = "للمنظفات الصناعية ومستحضرات التجميل";
            string line = "_____________________________";
            string s1 = "المطلوب من السيد:  " + export_company.Text;
            string str_dateOnly = DateTime.Now.ToString("yyyy-MM-dd");
            string s2 = "تحريرا فى :  " + str_dateOnly;
            string bill_no = "رقم الفاتورة :" + billTable.Rows[0]["id"].ToString();

            SizeF fzAddress = e.Graphics.MeasureString(address, f1);
            SizeF fzStoreName = e.Graphics.MeasureString(StoreName, f1);
            SizeF fzStoreName2 = e.Graphics.MeasureString(StoreName2, f2);
            SizeF fzStorePhone2 = e.Graphics.MeasureString(StorePhone2, f2);
            SizeF fzStorePhone1 = e.Graphics.MeasureString(StorePhone1, f2);
            SizeF fzLine = e.Graphics.MeasureString(line, f2);
            SizeF fzs1 = e.Graphics.MeasureString(s1, f3);
            SizeF fzs2 = e.Graphics.MeasureString(s2, f3);
            SizeF fzsBill_no = e.Graphics.MeasureString(bill_no, f3);

            float total_height = margin;

            e.Graphics.DrawString(address, f1, Brushes.Black, (e.PageBounds.Width - fzAddress.Width) / 2, total_height);
            total_height += fzAddress.Height;

            e.Graphics.DrawString(StoreName, f1, Brushes.Black, (e.PageBounds.Width - fzStoreName.Width - margin), total_height);
            e.Graphics.DrawString(StorePhone1, f2, Brushes.Black, margin, total_height);

            total_height += fzStoreName.Height;

            e.Graphics.DrawString(StoreName2, f2, Brushes.Black, (e.PageBounds.Width - fzStoreName2.Width - margin), total_height);
            e.Graphics.DrawString(StorePhone2, f2, Brushes.Black, margin, total_height);
            total_height += fzStoreName2.Height;

            e.Graphics.DrawString(line, f2, Brushes.Black, (e.PageBounds.Width - fzLine.Width - margin) / 2, total_height);
            total_height += fzLine.Height;

            e.Graphics.DrawString(s1, f3, Brushes.Black, (e.PageBounds.Width - fzs1.Width - margin), total_height);
            total_height += fzLine.Height + fzs1.Height - 10;
            e.Graphics.DrawString(bill_no, f3, Brushes.Black, (e.PageBounds.Width - fzsBill_no.Width - margin), total_height);

            e.Graphics.DrawString(s2, f3, Brushes.Black, (margin), total_height);
            total_height += fzs1.Height;

            float colHeight = 30;
            int itemsCount = dataGridView2.Rows.Count;
            float rectangleHeight = (itemsCount * colHeight);
            float preHeights = total_height;
            e.Graphics.DrawRectangle(Pens.Black, margin, preHeights, e.PageBounds.Width - margin * 2, preHeights + rectangleHeight - margin + 50);


            float sizePerOne = (e.PageBounds.Width - margin * 2) / 6;
            float col1width = sizePerOne;
            float col2width = sizePerOne * 2 + col1width;
            float col3width = sizePerOne + col2width;
            float col4width = sizePerOne + col3width;
            float col5width = sizePerOne + col4width;

            e.Graphics.DrawLine(Pens.Black, margin, preHeights + colHeight, e.PageBounds.Width - margin, preHeights + colHeight);

            e.Graphics.DrawString("م", f3, Brushes.Black, (e.PageBounds.Width - col1width - 20), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col1width - margin, preHeights, e.PageBounds.Width - col1width - margin, (preHeights + rectangleHeight));

            e.Graphics.DrawString("الصنف", f3, Brushes.Black, (e.PageBounds.Width - col2width), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col2width - margin, preHeights, e.PageBounds.Width - col2width - margin, preHeights + rectangleHeight);

            e.Graphics.DrawString("الكمية", f3, Brushes.Black, (e.PageBounds.Width - col3width - 20), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col3width - margin, preHeights, e.PageBounds.Width - col3width - margin, preHeights + rectangleHeight);

            e.Graphics.DrawString("سعر الوحدة", f3, Brushes.Black, (e.PageBounds.Width - col4width - 25), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col4width - margin, preHeights, e.PageBounds.Width - col4width - margin, preHeights + rectangleHeight);

            e.Graphics.DrawString("الإجمالي", f3, Brushes.Black, (e.PageBounds.Width - col5width - 20), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col5width - margin, preHeights, e.PageBounds.Width - col5width - margin, (preHeights + rectangleHeight));

            float rowsheight = 30;
            int c = 1;
            for (int x = 0; x < dataGridView2.Rows.Count - 1; x++) {
                e.Graphics.DrawString(c + "", f4, Brushes.Black, e.PageBounds.Width - col1width - 20, preHeights + 5 + rowsheight);
                e.Graphics.DrawString(dataGridView2.Rows[x].Cells[2].Value.ToString(), f4, Brushes.Black, e.PageBounds.Width - col2width, preHeights + 5 + rowsheight);
                e.Graphics.DrawString(dataGridView2.Rows[x].Cells[4].Value.ToString(), f4, Brushes.Black, e.PageBounds.Width - col3width, preHeights + 5 + rowsheight);
                e.Graphics.DrawString(dataGridView2.Rows[x].Cells[3].Value.ToString(), f4, Brushes.Black, e.PageBounds.Width - col4width, preHeights + 5 + rowsheight);
                e.Graphics.DrawString(dataGridView2.Rows[x].Cells[5].Value.ToString(), f4, Brushes.Black, e.PageBounds.Width - col5width - 20, preHeights + 5 + rowsheight);
                e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, e.PageBounds.Width - margin, preHeights + rowsheight);
                rowsheight += 30;
                c++;


            }

            // string st0 = "العبوات:   " + int.Parse(export_clientNumber.Text);
            string st1 = "إجمالي الفاتورة";
            string stt1 = export_total.Text + "";
            string st2 = "الحساب القديم";
            string stt2 = "" + export_clientold.Text;
            string st3 = "الإجمالى";
            string stt3 = "" + export_clientTotal.Text;
            string st4 = "المبلغ المدفوع";
            string stt4 = "" + export_paid.Text;
            string st5 = "  المتبقى عليه";
            string stt5 = "" + export_remain.Text;
            //string st = st1 + "    " + st2 + "    " + st3;
            string st6 = "شكـــرا لزيــارتـكم شركـــة اليســــــــر";

            //SizeF fzSt1 = e.Graphics.MeasureString(st1, f3);
            //SizeF fzSt2 = e.Graphics.MeasureString(st2, f3);
            //SizeF fzSt3 = e.Graphics.MeasureString(st3, f3);
            // SizeF fzSt0 = e.Graphics.MeasureString(st0, f3);
            SizeF fzSt1 = e.Graphics.MeasureString(st1, f3);
            SizeF fzStt1 = e.Graphics.MeasureString(stt1, f3);
            SizeF fzSt2 = e.Graphics.MeasureString(st2, f3);
            SizeF fzStt2 = e.Graphics.MeasureString(stt1, f3);
            SizeF fzSt3 = e.Graphics.MeasureString(st3, f3);
            SizeF fzStt3 = e.Graphics.MeasureString(stt3, f3);
            SizeF fzSt4 = e.Graphics.MeasureString(st4, f3);
            SizeF fzStt4 = e.Graphics.MeasureString(stt4, f3);
            SizeF fzSt5 = e.Graphics.MeasureString(st5, f3);
            SizeF fzStt5 = e.Graphics.MeasureString(stt5, f3);
            SizeF fzSt6 = e.Graphics.MeasureString(st6, f3);

            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, e.PageBounds.Width - margin, preHeights + rowsheight);
            // e.Graphics.DrawString(st0, f3, Brushes.Black, e.PageBounds.Width - fzSt0.Width -margin-10, preHeights + 5 + rowsheight);

            //رسم العمود الافقى 
            e.Graphics.DrawLine(Pens.Black, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, (preHeights + rowsheight + 150));
            e.Graphics.DrawLine(Pens.Black, ((e.PageBounds.Width - margin) / 2) - 102, preHeights + rowsheight, ((e.PageBounds.Width - margin) / 2) - 102, (preHeights + rowsheight + 150));

            e.Graphics.DrawString(st1, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt1, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            //draw line
            rowsheight += 30;
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

            e.Graphics.DrawString(st2, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt2, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            rowsheight += 30;
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

            e.Graphics.DrawString(st3, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt3, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            rowsheight += 30;
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

            e.Graphics.DrawString(st4, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt4, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            rowsheight += 30;
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

            e.Graphics.DrawString(st5, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt5, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            rowsheight += 30;
            e.Graphics.DrawString(st6, f3, Brushes.Black, (e.PageBounds.Width - fzSt4.Width - margin) / 2, preHeights + 5 + rowsheight);
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);
        }

        private void button22_Click(object sender, EventArgs e)
        {

            ((Form)printPreviewDialog2).WindowState = FormWindowState.Maximized;
            if (printPreviewDialog2.ShowDialog() == DialogResult.OK)
            {

                printDocument2.Print();
            }

        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            adapter = new MySqlDataAdapter("select * from bill order by id desc limit 1 ", connection);
            DataTable billTable = new DataTable();
            adapter.Fill(billTable);

            System.Drawing.Font f1 = new System.Drawing.Font("Arial", 14, FontStyle.Bold);
            System.Drawing.Font f2 = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            System.Drawing.Font f3 = new System.Drawing.Font("Arial", 9, FontStyle.Bold);
            System.Drawing.Font f4 = new System.Drawing.Font("Arial", 8, FontStyle.Bold);
            System.Drawing.Font f5 = new System.Drawing.Font("Arial", 6, FontStyle.Bold);

            float margin = 30;

            string address = ">>>>>>الـفـاتــــورة<<<<<<";
            string StoreName = "اليـسـر للتوكيلات التجارية";
            string StorePhone1 = "ت/01026097624";
            string StorePhone2 = "ت/01110345857";
            string StoreName2 = "للمنظفات الصناعية ومستحضرات التجميل";
            string line = "_____________________________";
            string s1 = "المطلوب من السيد:  " + bill_name.Text;
            string str_dateOnly = bill_date.Value.ToShortDateString();
            string s2 = "تحريرا فى :  " + str_dateOnly;
            string bill_no = "رقم الفاتورة :" + txt_bill_no.Text;

            SizeF fzAddress = e.Graphics.MeasureString(address, f1);
            SizeF fzStoreName = e.Graphics.MeasureString(StoreName, f1);
            SizeF fzStoreName2 = e.Graphics.MeasureString(StoreName2, f2);
            SizeF fzStorePhone2 = e.Graphics.MeasureString(StorePhone2, f2);
            SizeF fzStorePhone1 = e.Graphics.MeasureString(StorePhone1, f2);
            SizeF fzLine = e.Graphics.MeasureString(line, f2);
            SizeF fzs1 = e.Graphics.MeasureString(s1, f3);
            SizeF fzs2 = e.Graphics.MeasureString(s2, f3);
            SizeF fzsBill_no = e.Graphics.MeasureString(bill_no, f3);

            float total_height = margin;

            e.Graphics.DrawString(address, f1, Brushes.Black, (e.PageBounds.Width - fzAddress.Width) / 2, total_height);
            total_height += fzAddress.Height;

            e.Graphics.DrawString(StoreName, f1, Brushes.Black, (e.PageBounds.Width - fzStoreName.Width - margin), total_height);
            e.Graphics.DrawString(StorePhone1, f2, Brushes.Black, margin, total_height);

            total_height += fzStoreName.Height;

            e.Graphics.DrawString(StoreName2, f2, Brushes.Black, (e.PageBounds.Width - fzStoreName2.Width - margin), total_height);
            e.Graphics.DrawString(StorePhone2, f2, Brushes.Black, margin, total_height);
            total_height += fzStoreName2.Height;

            e.Graphics.DrawString(line, f2, Brushes.Black, (e.PageBounds.Width - fzLine.Width - margin) / 2, total_height);
            total_height += fzLine.Height;

            e.Graphics.DrawString(s1, f3, Brushes.Black, (e.PageBounds.Width - fzs1.Width - margin), total_height);
            total_height += fzs1.Height;
            e.Graphics.DrawString(bill_no, f3, Brushes.Black, (e.PageBounds.Width - fzsBill_no.Width - margin), total_height);

            e.Graphics.DrawString(s2, f3, Brushes.Black, (margin), total_height);
            total_height += fzs1.Height;

            float colHeight = 30;
            int itemsCount = bill_dgv.Rows.Count;
            float rectangleHeight = itemsCount * colHeight;
            float preHeights = total_height;
            e.Graphics.DrawRectangle(Pens.Black, margin, preHeights, e.PageBounds.Width - margin * 2, preHeights + rectangleHeight - margin + 50);



            float sizePerOne = (e.PageBounds.Width - margin * 2) / 6;
            float col1width = sizePerOne;
            float col2width = sizePerOne * 2 + col1width;
            float col3width = sizePerOne + col2width;
            float col4width = sizePerOne + col3width;
            float col5width = sizePerOne + col4width;

            e.Graphics.DrawLine(Pens.Black, margin, preHeights + colHeight, e.PageBounds.Width - margin, preHeights + colHeight);

            e.Graphics.DrawString("م", f3, Brushes.Black, (e.PageBounds.Width - col1width - 20), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col1width - margin, preHeights, e.PageBounds.Width - col1width - margin, (preHeights + rectangleHeight));

            e.Graphics.DrawString("الصنف", f3, Brushes.Black, (e.PageBounds.Width - col2width), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col2width - margin, preHeights, e.PageBounds.Width - col2width - margin, preHeights + rectangleHeight);

            e.Graphics.DrawString("الكمية", f3, Brushes.Black, (e.PageBounds.Width - col3width - 20), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col3width - margin, preHeights, e.PageBounds.Width - col3width - margin, preHeights + rectangleHeight);

            e.Graphics.DrawString("سعر الوحدة", f3, Brushes.Black, (e.PageBounds.Width - col4width - 20), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col4width - margin, preHeights, e.PageBounds.Width - col4width - margin, preHeights + rectangleHeight);

            e.Graphics.DrawString("الإجمالي", f3, Brushes.Black, (e.PageBounds.Width - col5width - 20), preHeights + 5);
            e.Graphics.DrawLine(Pens.Black, e.PageBounds.Width - col5width - margin, preHeights, e.PageBounds.Width - col5width - margin, (preHeights + rectangleHeight));

            float rowsheight = 30;
            int c = 1;
            try {
                for (int x = 0; x < bill_dgv.Rows.Count - 1; x++)
                {
                    e.Graphics.DrawString(c + "", f4, Brushes.Black, e.PageBounds.Width - col1width - 20, preHeights + 5 + rowsheight);
                    e.Graphics.DrawString(bill_dgv.Rows[x].Cells[1].Value.ToString(), f4, Brushes.Black, e.PageBounds.Width - col2width, preHeights + 5 + rowsheight);
                    e.Graphics.DrawString(bill_dgv.Rows[x].Cells[3].Value.ToString(), f4, Brushes.Black, e.PageBounds.Width - col3width, preHeights + 5 + rowsheight);
                    e.Graphics.DrawString(bill_dgv.Rows[x].Cells[2].Value.ToString(), f4, Brushes.Black, e.PageBounds.Width - col4width, preHeights + 5 + rowsheight);
                    e.Graphics.DrawString(bill_dgv.Rows[x].Cells[4].Value.ToString(), f4, Brushes.Black, e.PageBounds.Width - col5width, preHeights + 5 + rowsheight);
                    e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, e.PageBounds.Width - margin, preHeights + rowsheight);
                    rowsheight += 30;
                    c++;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //string st0 = "العبوات:   " + int.Parse(bill_number.Text);
            string st1 = "إجمالي الفاتورة";
            string stt1 = bill_total.Text + "";
            string st2 = "الحساب القديم";
            string stt2 = "" + bill_clientold.Text;
            string st3 = "الإجمالى";
            string stt3 = "" + bill_clienttotal.Text;
            string st4 = "المبلغ المدفوع";
            string stt4 = "" + bill_paid.Text;
            string st5 = "  المتبقى عليه";
            string stt5 = "" + bill_remain.Text;
            //string st = st1 + "    " + st2 + "    " + st3;
            string st6 = "شكـــرا لزيــارتـكم شركـــة اليســــــــر";

            //SizeF fzSt1 = e.Graphics.MeasureString(st1, f3);
            //SizeF fzSt2 = e.Graphics.MeasureString(st2, f3);
            //SizeF fzSt3 = e.Graphics.MeasureString(st3, f3);
            //SizeF fzSt0 = e.Graphics.MeasureString(st0, f3);
            SizeF fzSt1 = e.Graphics.MeasureString(st1, f3);
            SizeF fzStt1 = e.Graphics.MeasureString(stt1, f3);
            SizeF fzSt2 = e.Graphics.MeasureString(st2, f3);
            SizeF fzStt2 = e.Graphics.MeasureString(stt1, f3);
            SizeF fzSt3 = e.Graphics.MeasureString(st3, f3);
            SizeF fzStt3 = e.Graphics.MeasureString(stt3, f3);
            SizeF fzSt4 = e.Graphics.MeasureString(st4, f3);
            SizeF fzStt4 = e.Graphics.MeasureString(stt4, f3);
            SizeF fzSt5 = e.Graphics.MeasureString(st5, f3);
            SizeF fzStt5 = e.Graphics.MeasureString(stt5, f3);
            SizeF fzSt6 = e.Graphics.MeasureString(st6, f3);

            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, e.PageBounds.Width - margin, preHeights + rowsheight);
            //e.Graphics.DrawString(st0, f3, Brushes.Black, e.PageBounds.Width - fzSt0.Width - margin - 10, preHeights + 5 + rowsheight);

            //رسم العمود الافقى 
            e.Graphics.DrawLine(Pens.Black, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, (preHeights + rowsheight + 150));
            e.Graphics.DrawLine(Pens.Black, ((e.PageBounds.Width - margin) / 2) - 102, preHeights + rowsheight, ((e.PageBounds.Width - margin) / 2) - 102, (preHeights + rowsheight + 150));

            e.Graphics.DrawString(st1, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt1, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            //draw line
            rowsheight += 30;
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

            e.Graphics.DrawString(st2, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt2, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            rowsheight += 30;
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

            e.Graphics.DrawString(st3, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt3, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            rowsheight += 30;
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

            e.Graphics.DrawString(st4, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt4, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            rowsheight += 30;
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

            e.Graphics.DrawString(st5, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 45, preHeights + 5 + rowsheight);
            e.Graphics.DrawString(stt5, f3, Brushes.Black, (e.PageBounds.Width - fzSt1.Width - margin) / 2 - 120, preHeights + 5 + rowsheight);

            rowsheight += 30;
            e.Graphics.DrawString(st6, f3, Brushes.Black, (e.PageBounds.Width - fzSt4.Width - margin) / 2, preHeights + 5 + rowsheight);
            e.Graphics.DrawLine(Pens.Black, margin, preHeights + rowsheight, (e.PageBounds.Width - margin) / 2, preHeights + rowsheight);

        }

        private void export_clientold_TextChanged(object sender, EventArgs e)
        {
            if (bill_sellOrback.SelectedIndex == 0)
            {
                if (export_total.Text != "" && export_clientold.Text != "")
                {
                    export_clientTotal.Text = (float.Parse(export_total.Text) + float.Parse(export_clientold.Text)).ToString();
                }
            }
            else
            {
                if (export_total.Text != "" && export_clientold.Text != "")
                {

                    export_remain.Text = (float.Parse(export_clientold.Text) - (float.Parse(export_total.Text))).ToString();
                }
            }
        }

        private void export_clientTotal_TextChanged(object sender, EventArgs e)
        {
            if (bill_sellOrback.SelectedIndex == 0)
            {
                if (export_clientTotal.Text != "" && export_clientold.Text != "")
                {
                    export_remain.Text = (float.Parse(export_clientTotal.Text) - float.Parse(export_paid.Text)).ToString();
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            adapter = new MySqlDataAdapter("select cat.`name`,item.id,item.name,priceC,priceB,priceA,count from item ,cat where count>0 and cat_id=cat.id", connection);
            try
            {
                DataTable dt1 = new DataTable();

                adapter.Fill(dt1);

                dataGridView3.DataSource = dt1;
                dt1.Columns[0].ColumnName = "الفئه";
                dt1.Columns[1].ColumnName = "رقم المنتج";
                dt1.Columns[2].ColumnName = "اسم المنتج";
                dt1.Columns[3].ColumnName = "سعر الشراء";
                dt1.Columns[4].ColumnName = "سعر الجمله";
                dt1.Columns[5].ColumnName = "سعر القطاعى";
                dt1.Columns[6].ColumnName = "العدد فالمحل";
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void combo_inOrOut_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            adapter = new MySqlDataAdapter("select cat.`name`,item.name from item ,cat where count=0 and cat_id=cat.id", connection);
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView3.DataSource = dt;
                    dt.Columns[0].ColumnName = "الفئه";
                    dt.Columns[1].ColumnName = "اسم المنتج";
                }
                else
                    MessageBox.Show("لا يوجد عجز في المحل");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void textBox11_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                adapter = new MySqlDataAdapter("select * from userdata where type ='بائع' and name like '%" + textBox11.Text + "%'", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                updateClient_list2.SelectedIndexChanged -= updateClient_list2_SelectedIndexChanged;
                updateClient_list2.ValueMember = "id";
                updateClient_list2.DisplayMember = "name";
                updateClient_list2.DataSource = table;
                updateClient_list2.SelectedIndexChanged += updateClient_list2_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            bool isfound = false;
            if (updateClient_txtPhone2.Text != "")
                isfound = true;
            if (updateClient_txtName2.Text != "" && updateClient_txt_cashremain2.Text != "" && updateClient_list2.Text.ToString() != "")
            {
                try
                {

                    command = new MySqlCommand("update userdata set name=@name,phone=@phone,address=@address,notes=@notes,remained=@remained where id=" + updateClient_list2.SelectedValue.ToString(), connection);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();


                    command.Parameters.AddWithValue("@name", updateClient_txtName2.Text);
                    if (isfound)
                        command.Parameters.AddWithValue("@phone", double.Parse(updateClient_txtPhone2.Text));
                    else
                        command.Parameters.AddWithValue("@phone", updateClient_txtPhone2.Text);
                    command.Parameters.AddWithValue("@address", updateClient_txtAddress2.Text);
                    command.Parameters.AddWithValue("@notes", updateClient_txtNote2.Text);
                    command.Parameters.AddWithValue("@remained", double.Parse(updateClient_txt_cashremain2.Text));
                    //command.Parameters.AddWithValue("@number", double.Parse(updateClient_txt_number2.Text));

                    int n = command.ExecuteNonQuery();
                    if (n > 0)
                    {
                        MessageBox.Show("تم الحفظ");
                    }
                }
                catch (Exception ex)
                {
                    //  MessageBox.Show(ex.Message);
                    MessageBox.Show("برجاء التأكد من صحة القيم المدخله");
                }
            }

            else
            {
                MessageBox.Show("برجاء التأكد من ادخال  الاسم والمتبقي قبل الحفظ");
            }
        }

        private void updateClient_list2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                adapter = new MySqlDataAdapter("select * from userdata where id=" + updateClient_list2.SelectedValue.ToString(), connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                updateClient_txtName2.Text = dt.Rows[0]["name"].ToString();
                updateClient_txtPhone2.Text = dt.Rows[0]["phone"].ToString();
                updateClient_txtAddress2.Text = dt.Rows[0]["address"].ToString();
                updateClient_txtNote2.Text = dt.Rows[0]["notes"].ToString();
                updateClient_txt_cashremain2.Text = dt.Rows[0]["remained"].ToString();
                //updateClient_txt_number2.Text = dt.Rows[0]["numbers"].ToString();
                label_clientName2.Text = dt.Rows[0]["type"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void export_remain_TextChanged(object sender, EventArgs e)
        {
            if (float.Parse(export_remain.Text) < 0.0)
            {
                label47.Text = "المتبقى للعميل";

            }
        }

        private void bill_sellOrback_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bill_sellOrback.SelectedIndex == 1)
            {
                MessageBox.Show("تأكد انه لا يوجد منتجات فالفاتورة ");
                export_paid.Visible = false;
                label48.Visible = false;
                export_clientTotal.Visible = false;
                label92.Visible = false;
            }
            else
            {
                export_paid.Visible = true;
                label48.Visible = true;
                export_clientTotal.Visible = true;
                label92.Visible = true;
            }
        }

        private void detail_SearchText_TextChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            adapter = new MySqlDataAdapter("select * from userdata where name like '%" + detail_SearchText.Text + "%'", connection);
            adapter.Fill(table);

            detail_ClientsList.SelectedIndexChanged -= detail_ClientsList_SelectedIndexChanged;
            detail_ClientsList.ValueMember = "id";
            detail_ClientsList.DisplayMember = "name";
            detail_ClientsList.DataSource = table;
            detail_ClientsList.SelectedIndexChanged += detail_ClientsList_SelectedIndexChanged;

    }

        private void detail_ClientsList_SelectedIndexChanged(object sender, EventArgs e)
        {

            int id = int.Parse(detail_ClientsList.SelectedValue.ToString());
            DataTable tb = new DataTable();
            adapter = new MySqlDataAdapter("select type,value,total,date from client_details where client_id =" + id + "", connection);
            adapter.Fill(tb);
            tb.Columns[0].ColumnName = "قبض /صرف";
            tb.Columns[1].ColumnName = "القيمه";
            tb.Columns[2].ColumnName = "الإجمالي";
            tb.Columns[3].ColumnName = "تاريخ العملية";

            detail_dgv.DataSource = tb;

        }
    }
    }

    


    
        

