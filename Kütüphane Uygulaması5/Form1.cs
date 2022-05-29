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

namespace Kütüphane_Uygulaması5
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-VLFVSAN;Initial Catalog=Kütüphane;Integrated Security=True");
        int kod = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand($"SELECT [Id],[Kadi],[sifre] FROM[Kütüphane].[dbo].[Uye_Bilgileri] where Kadi = '{textBox1.Text}' and sifre = '{textBox2.Text}'", conn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sqlCommand;
            DataSet ds = new DataSet();
            da.Fill(ds);
            if(ds.Tables[0].Rows.Count > 0)
            {
                GetKullaniciBilgileri();
                flowLayoutPanel1.Visible = true;
                panel1.Visible = false;
                MessageBox.Show("Login successfully");
            }
            else
            {
                MessageBox.Show("kullanıcı adı veya sifre hatalı");
            }
            conn.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand($"SELECT [Id],[Kadi],[sifre] FROM[Kütüphane].[dbo].[Uye_Bilgileri] where Kadi = '{textBox1.Text}'", conn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sqlCommand;
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (!(ds.Tables[0].Rows.Count > 0))
            {
                SqlCommand sqlCommand2 = new SqlCommand($"insert into Uye_Bilgileri(Kadi,sifre) VALUES ('{textBox1.Text}','{textBox2.Text}')", conn);

                int i = sqlCommand2.ExecuteNonQuery();
                if (i != 0)
                {
                    GetKullaniciBilgileri();
                    flowLayoutPanel1.Visible = true;
                    panel1.Visible = false;
                    MessageBox.Show("SignIn successfully");
                }
                else
                {
                    MessageBox.Show("error");
                }
            }
            else
            {
                MessageBox.Show("Bu kullanıcı adıyla kullanıcı bulunmaktadır.");
            }
            conn.Close();
        }
        private bool validationForm()
        {
            if(txtOgrenciName.Text == String.Empty)
            {
                MessageBox.Show("Ogrenci ismi boş");
                return false;
            }
            if (txtOgrenciSurname.Text == String.Empty)
            {
                MessageBox.Show("Ogrenci soyadı alanı boş");
                return false;
            }
            if (txtDepartment.Text == String.Empty)
            {
                MessageBox.Show("Department alanı boş");
                return false;
            }
            return true;
        }
        private void GetKullaniciBilgileri()
        {
            try
            {
                listView1.Items.Clear();
                SqlCommand komut = new SqlCommand("SELECT [Id],[Name],[Surname],[DepartmentName] FROM[Kütüphane].[dbo].[Ogrenci_Bilgileri]", conn);
                SqlDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    listView1.Items.Add(oku["Id"].ToString());
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(oku["Name"].ToString());
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(oku["Surname"].ToString());
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(oku["DepartmentName"].ToString());
                }
                listView1.Columns[0].Width = -2;
                listView1.Columns[1].Width = -2;
                listView1.Columns[2].Width = -2;
                listView1.Columns[3].Width = -2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Mesajı:" + ex.Message);
            }
        }      
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            kod = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);            
            txtOgrenciName.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txtOgrenciSurname.Text = listView1.SelectedItems[0].SubItems[2].Text;
            txtDepartment.Text = listView1.SelectedItems[0].SubItems[3].Text;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listView2.View = View.Details;
            listView2.GridLines = true;
            listView2.FullRowSelect = true;
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            flowLayoutPanel1.Visible = false;
            panel1.Visible = true;
        }       

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            if (validationForm())
            {            
                SqlCommand sqlCommand = new SqlCommand($"insert into Ogrenci_Bilgileri(Name,Surname,DepartmentName) VALUES ('{txtOgrenciName.Text}','{txtOgrenciSurname.Text}','{txtDepartment.Text}')", conn);

                int i = sqlCommand.ExecuteNonQuery();
                if (i != 0)
                {
                    GetKullaniciBilgileri();
                    MessageBox.Show("Ogrenci eklendi successfully");
                }
                else
                {
                    MessageBox.Show("error");
                }
            }

            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (validationForm())
            {
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand($"UPDATE Ogrenci_Bilgileri SET Name = '" + txtOgrenciName.Text.ToString() + "', Surname = '" + txtOgrenciSurname.Text.ToString() + "', DepartmentName = '" + txtDepartment.Text.ToString() + "' WHERE Id = '" + kod + "'", conn);

                int i = sqlCommand.ExecuteNonQuery();
                if (i != 0)
                {
                    GetKullaniciBilgileri();
                    MessageBox.Show("Ogrenci güncellendis successfully");
                }
                else
                {
                    MessageBox.Show("error");
                }
            }

            conn.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand($"DELETE FROM Ogrenci_Bilgileri WHERE Id=(" + kod + ")", conn);

            int i = sqlCommand.ExecuteNonQuery();
            if (i != 0)
            {
                GetKullaniciBilgileri();
                MessageBox.Show("Ogrenci silindi.");
            }
            else
            {
                MessageBox.Show("error");
            }

            conn.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            conn.Open();
            GetKullaniciBilgileri();
            conn.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {            
            try
            {
                conn.Open();
                listView2.Items.Clear();
                SqlCommand komut = new SqlCommand($"SELECT [Id],[Name],[Surname],[DepartmentName] FROM[Kütüphane].[dbo].[Ogrenci_Bilgileri] where [Name] like '%{textBox3.Text}%'", conn);
                SqlDataReader oku = komut.ExecuteReader();
                while (oku.Read())
                {
                    listView2.Items.Add(oku["Id"].ToString());
                    listView2.Items[listView2.Items.Count - 1].SubItems.Add(oku["Name"].ToString());
                    listView2.Items[listView2.Items.Count - 1].SubItems.Add(oku["Surname"].ToString());
                    listView2.Items[listView2.Items.Count - 1].SubItems.Add(oku["DepartmentName"].ToString());
                }
               listView2.Columns[0].Width = -2;
               listView2.Columns[1].Width = -2;
               listView2.Columns[2].Width = -2;
               listView2.Columns[3].Width = -2;
               conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Mesajı:" + ex.Message);
            }
        }
    }
}
