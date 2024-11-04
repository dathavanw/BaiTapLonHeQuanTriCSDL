using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;


namespace BaiTapLonHeQuanTriCSDL
{
    public partial class frm_login : Form
    {
        public frm_login()
        {
            InitializeComponent();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            string url = "https://www.behance.net/gallery/208119527/Log-in-page?tracking_source=search_projects|WinForms+login+design&l=21";


            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        // Hàm chuẩn hóa số điện thoại để kiểm tra thông tin đăng nhập 
        string NormalizePhone(string phone)
        {
            if (!phone.StartsWith("+"))
            {
                return "+" + phone;
            }
            return phone;

        }


        private void btn_login_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus(); 
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus(); 
                return;
            }

            // đây là chuỗi kết nối đến csdl trên máy của tôi 
            string connectionString = @"Data Source=ADMIN-PC;Initial Catalog=QLMP;Integrated Security=True";

            try
            {


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();



                    // 3. Truy vấn để kiểm tra thông tin đăng nhập
                    string query = "SELECT Role FROM Employees WHERE phone = @phone and password = @password";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {



                        string PhoneInput = NormalizePhone(txtUsername.Text);

                        command.Parameters.AddWithValue("@phone", PhoneInput);
                        command.Parameters.AddWithValue("@password", txtPassword.Text);


                        var role = command.ExecuteScalar();


                        if (role != null)
                        {

                            if (role.ToString() == "staff")
                            {
                              //  Mở form dành cho nhân viên
                                frm_staff frm_staff = new frm_staff();
                                frm_staff.Show();

                            }
                            else if (role.ToString() == "management")
                            {
                                // Mở form dành cho quản lý
                                //frm_manager frm_manager = new frm_manager();
                                //frm_manager.Show();
                            }

                            this.Hide();

                        }
                        else
                        {

                            MessageBox.Show("Sai thông tin đăng nhập! Vui lòng kiểm tra lại !");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi kết nối đến cơ sở dữ liệu: " + ex.Message);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

