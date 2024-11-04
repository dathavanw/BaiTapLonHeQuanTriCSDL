using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BaiTapLonHeQuanTriCSDL
{
    public partial class frm_manager : Form
    {
        public frm_manager()
        {
            InitializeComponent();
        }



        SqlConnection conn = null;
        SqlDataAdapter dataAdapter;

        DataTable table;
        int vitrichon = -1;
        string connectionString = @"Data Source=LAPTOP-AOL11CSJ\SQLEXPRESS01;Initial Catalog=QLMP;Integrated Security=True";

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            pictute_home.Hide();
            panel_home.Show();






        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            pictute_home.Visible = true;
            pictute_home.Visible = true;
            //this.Close();
        }


      

        private void btn_addEmployee_Click(object sender, EventArgs e)
        {

            string firstname = txt_firstname.Text.Trim();
            string lastname = txt_lastname.Text.Trim();
            DateTime birthdated = birthdate.Value;
            string birth = birthdated.ToString("yyyy-MM-dd");
            string address = txt_address.Text.Trim();
            string phone = txt_phone.Text.Trim();
            string email = txt_email.Text.Trim();
            string city = txt_city.Text.Trim();
            string fax = txt_fax.Text.Trim();
            string homepage = txt_homepage.Text.Trim();
            DateTime hiredated = hiredate.Value;
            string hire = hiredated.ToString("yyyy-MM-dd");
            string password = txt_password.Text.Trim();
            string role = cmb_role.Text;
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string str = "Insert into Employees values(N'" + firstname + "',N'" + lastname + "',N'" + birth + "',N'" + address + "',N'" + phone + "',N'" + email + "',N'" + city + "',N'',N'" + fax + "',N'" + homepage + "',N'" + hire + "',N'" + role + "',N'" + password + "')";


                    if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname) ||
                    string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phone) ||
                    string.IsNullOrEmpty(email) || string.IsNullOrEmpty(city) ||
                    string.IsNullOrEmpty(fax) || string.IsNullOrEmpty(homepage) ||
                    string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
                    {
                      

                        MessageBox.Show("Information not empty !", "Error", MessageBoxButtons.OK);

                    } else {

                        SqlCommand cmd = new SqlCommand(str, connection);





                        DialogResult result = MessageBox.Show("Success ! Employee add data successfully", "Notification", MessageBoxButtons.OK);

                        if (result == DialogResult.OK)
                        {
                            cmd.ExecuteNonQuery();

                            table.Rows.Clear();
                            dataAdapter.Fill(table);


                        }


                    

                    }

                }

            }
            catch(SqlException ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi kết nối đến cơ sở dữ liệu: " + ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void deleteEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frm_view_list_employee frm_View_List_Employee = new frm_view_list_employee();
          //  frm_View_List_Employee.ShowDialog();
        }

        private void editEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {

          //  string employeeID = "";

           // frm_choose_edit frm_Choose_Edit = new frm_choose_edit(employeeID);
           // frm_Choose_Edit.ShowDialog();


         

        }

        private void label13_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn kết thúc phiên làm việc không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
               // this.Close();
                Application.Exit(); 
            }
          //  this.Close();
        }

        private void btn_manager_employee_Click(object sender, EventArgs e)
        {
           pictute_home.Visible = false;
            panel_manager_suppliers.Visible = false;
            panel_manager_shipper.Visible = false;
            panel_manager_employees.Visible =true    ;

          //  txt_search_employee.PlaceholderText = "Hãy nhập số điện thoại";



            conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT employees_ID , firstname , lastname , birthdate , address, phone , email , city , fax, homepage , hiredate , role , password   FROM Employees";
            dataAdapter = new SqlDataAdapter(sql, conn);
            table = new DataTable();
            dataAdapter.Fill(table);
            data_employee.DataSource = table;





        }

        private void pictute_home_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string employee_id = table.Rows[vitrichon][0].ToString();


            string checkOrdersQuery = "SELECT COUNT(*) FROM Orders WHERE Employee_ID = @EmployeeID";
            using (SqlCommand cmd = new SqlCommand(checkOrdersQuery, conn))
            {
                cmd.Parameters.AddWithValue("@EmployeeID", employee_id);
                int orderCount = (int)cmd.ExecuteScalar();

                if (orderCount > 0)
                {
                    MessageBox.Show("Không thể xóa nhân viên này vì có các đơn hàng liên quan.", "Lỗi Xóa", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                else
                {

                    ////////////////////////



                    string deleteEmployeeQuery = "DELETE FROM Employees WHERE Employees_ID = @EmployeeID";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteEmployeeQuery, conn))
                    {

                        deleteCmd.Parameters.AddWithValue("@EmployeeID", employee_id);


                        DialogResult result = MessageBox.Show("Are you sure ? ", "Notification", MessageBoxButtons.OKCancel);

                        if (result == DialogResult.OK)
                        {
                            deleteCmd.ExecuteNonQuery();

                            table.Rows.Clear();
                            dataAdapter.Fill(table);

                        }
                        else
                        {
                            DialogResult results = MessageBox.Show("Xóa không thành công !.", "Xóa nhân viên ", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                            if (results == DialogResult.OK)
                            {
                                table.Rows.Clear();
                                dataAdapter.Fill(table);

                            }
                        }


                    }

                    /////////////////////////

                   
                }
            }



        }

        private void pictute_home_Click_1(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_manager_suppliers_Click(object sender, EventArgs e)
        {
            pictute_home.Visible = false;
            panel_manager_shipper.Visible = false;
            panel_manager_suppliers.Show();

            conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT Supplier_ID, CompanyName , ContactName ,Address , City , Phone , HomePage , Fax  FROM Suppliers";
            dataAdapter = new SqlDataAdapter(sql, conn);
            table = new DataTable();
            dataAdapter.Fill(table);
            data_supp.DataSource = table;






        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_manager_shipper_Click(object sender, EventArgs e)
        {
            pictute_home.Visible = false;
            panel_manager_shipper.Show();

            conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT * FROM Shippers";
            dataAdapter = new SqlDataAdapter(sql, conn);
            table = new DataTable();
            dataAdapter.Fill(table);
            data_ship.DataSource = table;

        }

        private void btn_add_shipper_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionString);
            conn.Open();
            string companyNameshipper = txt_companyname.Text;
            string companyPhone = txt_phone_shipper.Text;
            string str = "Insert into Shippers values(N'" + companyNameshipper + "',N'" + companyPhone + "')";


            if (string.IsNullOrEmpty(companyNameshipper) || string.IsNullOrEmpty(companyPhone))
            {
                MessageBox.Show("Information not empty !", "Error", MessageBoxButtons.OK);

            }
            else
            {

                SqlCommand cmd = new SqlCommand(str, conn);
              
                DialogResult result =  MessageBox.Show("Success ! ShipperCompany add data successfully", "Notification", MessageBoxButtons.OK);

                if (result == DialogResult.OK)
                {
                    cmd.ExecuteNonQuery();

                    table.Rows.Clear();
                    dataAdapter.Fill(table);

                }
            }





        }

        private void panel_manager_shipper_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_cancel_shipper_Click(object sender, EventArgs e)
        {

         //  panel_manager_shipper.Visible = false;
       //   panel_manager_employees.Visible = false;
       //     panel_manager_suppliers.Visible = false;
        //     panel_home.Visible = true;
          pictute_home.Visible = true;
            //frm_manager frm_manager = new frm_manager();
            //frm_manager.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txt_phone_shipper.Text = "";
            txt_companyname.Text = "";
            txt_search_ship.Text = ""; 
        }

        private void btn_delete_shipper_Click(object sender, EventArgs e)
        {
            string Shipper_ID = table.Rows[vitrichon][0].ToString();


            string checkOrdersQuery = "SELECT COUNT(*) FROM Shippers_Orders WHERE Shipper_ID = @Shipper_ID";
            using (SqlCommand cmd = new SqlCommand(checkOrdersQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Shipper_ID", Shipper_ID);
                int orderCount = (int)cmd.ExecuteScalar();

                if (orderCount > 0)
                {
                    MessageBox.Show("Không thể xóa đơn vị vận chuyển này vì có các đơn hàng liên quan.", "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    string deleteEmployeeQuery = "DELETE FROM Shippers WHERE Shipper_ID = @Shipper_ID";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteEmployeeQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@Shipper_ID", Shipper_ID);



                        DialogResult result = MessageBox.Show("Are you sure ? ", "Notification", MessageBoxButtons.OKCancel);

                        if (result == DialogResult.OK)
                        {
                            deleteCmd.ExecuteNonQuery();

                           

                        }
                        else
                        {
                            DialogResult results = MessageBox.Show("Xóa không thành công !.", "Xóa đơn vị vận chuyển", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                            if (results == DialogResult.OK)
                            {
                                table.Rows.Clear();
                                dataAdapter.Fill(table);

                            }
                        }

                       
                    }
                }
            }
        }

        private void data_ship_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            vitrichon = e.RowIndex;
            if (vitrichon >= 0)
            {
                //lấy dữ liệu từ dòng đang chọn chuyển lên khung nhập liệu
                txt_companyname.Text = table.Rows[vitrichon][1].ToString();
                txt_phone_shipper.Text = table.Rows[vitrichon][2].ToString();
            }
        }

        private void btn_update_shipper_Click(object sender, EventArgs e)
        {


          
            string companyname_id = table.Rows[vitrichon][0].ToString();
            string phone = txt_phone_shipper.Text.Trim();
            string companyname = txt_companyname.Text.Trim();
            string str = "Update Shippers set CompanyName = N'" + companyname + "',Phone =  N'" + phone + "' where Shipper_ID = '" + companyname_id + "'";
            SqlCommand cmd = new SqlCommand(str, conn);
         //   cmd.ExecuteNonQuery();


            DialogResult result = MessageBox.Show("Thông tin về đơn vị vận chuyển đã được cập nhật !", "Cập nhật thông tin !", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {

                cmd.ExecuteNonQuery();

                table.Rows.Clear();
                dataAdapter.Fill(table);

            }


          




        }

        private void btn_clear_supplier_Click(object sender, EventArgs e)
        {
            txt_contactSupp.Text = "";
            txt_citySupp.Text = "";
            txt_FaxSupp.Text = "";
            txt_homepageSupp.Text = "";
            txt_phoneSupp.Text = "";
            txt_supp.Text = "";
            txt_addSupp.Text = "";
            txt_search_supp.Text = "";
        }

        private void btn_cancel_supplier_Click(object sender, EventArgs e)
        {
            pictute_home.Visible = true;
            pictute_home.Visible = true;
            // pictute_home.Show();
        }

        private void btn_add_supplier_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionString);
            conn.Open();
            string namecontactSupp =  txt_contactSupp.Text;
            string citySupp = txt_citySupp.Text;
            string faxSupp = txt_FaxSupp.Text;
            string pageSupp = txt_homepageSupp.Text;

            string phoneSupp = txt_phoneSupp.Text;
            string nameSupp = txt_supp.Text;
            string addressSupp = txt_addSupp.Text;

            string str = "Insert into Suppliers values(N'" + nameSupp + "',N'" + namecontactSupp + "',N'" + addressSupp + "',N'" + citySupp + "' ,N' ' ,N'" + phoneSupp + "',N'" + pageSupp + "',N'" + faxSupp + "')";


            if (string.IsNullOrEmpty(namecontactSupp) || string.IsNullOrEmpty(phoneSupp) ||
                    string.IsNullOrEmpty(citySupp) || string.IsNullOrEmpty(nameSupp) ||
                    string.IsNullOrEmpty(faxSupp) || string.IsNullOrEmpty(addressSupp) ||
                    string.IsNullOrEmpty(pageSupp) )
            {
                MessageBox.Show("Information not empty !", "Error", MessageBoxButtons.OK);

            }
            else
            {

                SqlCommand cmd = new SqlCommand(str, conn);
              //  cmd.ExecuteNonQuery();
               // MessageBox.Show("Success ! Supplier add data successfully", "Notification", MessageBoxButtons.OK);


                //table.Rows.Clear();
                //dataAdapter.Fill(table);




                DialogResult result = MessageBox.Show("Success ! Supplier add data successfully", "Notification", MessageBoxButtons.OK);


                if (result == DialogResult.OK)
                {

                    cmd.ExecuteNonQuery();

                    table.Rows.Clear();
                    dataAdapter.Fill(table);

                }






            }
        }

        private void btn_delete_supplier_Click(object sender, EventArgs e)
        {


            string supplier_ID = table.Rows[vitrichon][0].ToString();


            string checkOrdersQuery = "SELECT COUNT(*) FROM Products WHERE supplier_ID = @supplier_ID";


            SqlCommand cmd = new SqlCommand(checkOrdersQuery, conn);
            {
                cmd.Parameters.AddWithValue("@supplier_ID", supplier_ID);
                int orderCount = (int)cmd.ExecuteScalar();

                if (orderCount > 0)
                {
                    MessageBox.Show("Không thể xóa đơn vị cung cấp này vì có các sản phẩm liên quan.", "Lỗi Xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    string deleteEmployeeQuery = "Delete Suppliers where supplier_ID = '" + supplier_ID + "'";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteEmployeeQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@supplier_ID", supplier_ID);


                        DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi này không?", "Xác nhận xóa", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                        if (result == DialogResult.OK)
                        {

                            deleteCmd.ExecuteNonQuery();
                            table.Rows.Clear();
                            dataAdapter.Fill(table);

                        }
                        else
                        {
                            DialogResult results = MessageBox.Show("Xóa không thành công !.", "Xóa đơn vị cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                            if (results == DialogResult.OK)
                            {
                                table.Rows.Clear();
                                dataAdapter.Fill(table);

                            }
                        }




                    }
                }
            }

            //string supplier_ID = table.Rows[vitrichon][0].ToString();
            //string str = "Delete Suppliers where supplier_ID = '" + supplier_ID + "'";
            //SqlCommand cmd = new SqlCommand(str, conn);




            //DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi này không?", "Xác nhận xóa", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            //if (result == DialogResult.OK)
            //{

            //    cmd.ExecuteNonQuery();

            //    table.Rows.Clear();
            //    dataAdapter.Fill(table);

            //}
            //else
            //{
            //    DialogResult results = MessageBox.Show("Xóa không thành công !.", "Xóa đơn vị cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            //    if (results == DialogResult.OK)
            //    {
            //        table.Rows.Clear();
            //        dataAdapter.Fill(table);

            //    }
            //}




        }

        private void panel_manager_suppliers_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            txt_search_employee.Text = "";

             txt_firstname.Text = "";
             txt_lastname.Text = "";

            txt_address.Text = "";
            txt_phone.Text = "";
            txt_email.Text = "";
            txt_city.Text = "";
           txt_fax.Text = ""            ;
             txt_homepage.Text = "";


            txt_password.Text = "";
        




        }

        private void button1_Click(object sender, EventArgs e)
        {
            string employee_id = table.Rows[vitrichon][0].ToString();

            conn = new SqlConnection(connectionString);
            conn.Open();


            string firstname = txt_firstname.Text.Trim();
            string lastname = txt_lastname.Text.Trim();
            DateTime birthdated = birthdate.Value;
            string birth = birthdated.ToString("yyyy-MM-dd");
            string address = txt_address.Text.Trim();
            string phone = txt_phone.Text.Trim();
            string email = txt_email.Text.Trim();
            string city = txt_city.Text.Trim();
            string fax = txt_fax.Text.Trim();
            string homepage = txt_homepage.Text.Trim();
            DateTime hiredated = hiredate.Value;
            string hire = hiredated.ToString("yyyy-MM-dd");
            string password = txt_password.Text.Trim();
            string role = cmb_role.Text;


            string str = "UPDATE employees SET firstname = N'" + firstname +
              "', lastname = N'" + lastname +
              "', birthdate = N'" + birth +
              "', address = N'" + address +
              "', phone = N'" + phone +
              "', email = N'" + email +
              "', city = N'" + city +
              "', fax = N'" + fax +
              "', homepage = N'" + homepage +
              "', hiredate = N'" + hire +
              "', password = N'" + password +
              "', role = N'" + role +
              "' WHERE Employees_ID = '" + employee_id + "'";

            SqlCommand cmd = new SqlCommand(str, conn);


            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật thông tin này không ?.", "Cập nhật thông tin nhân viên", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);



            if (result == DialogResult.OK)
            {
                cmd.ExecuteNonQuery();
                table.Rows.Clear();
                dataAdapter.Fill(table);

            }

         
            else
            {
                DialogResult results = MessageBox.Show("Thông tin cập nhật thất bại !.", "Cập nhật thông tin nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                if (results == DialogResult.OK)
                {
                    table.Rows.Clear();
                    dataAdapter.Fill(table);

                }
            }









        }

        private void btn_update_supplier_Click(object sender, EventArgs e)
        {







            string supplier_ID = table.Rows[vitrichon][0].ToString();

            string namecontactSupp = txt_contactSupp.Text;
            string citySupp = txt_citySupp.Text;
            string faxSupp = txt_FaxSupp.Text;
            string pageSupp = txt_homepageSupp.Text;

            string phoneSupp = txt_phoneSupp.Text;
            string nameSupp = txt_supp.Text;
            string addressSupp = txt_addSupp.Text;

            string str = "Update Suppliers set CompanyName = N'" + nameSupp + "', city = N'" + citySupp + "',fax =  N'" + faxSupp + "', homepage =  N'" + pageSupp + "' ,phone = N'" + phoneSupp + "' ,contactname =  N'" + namecontactSupp + "' ,address =  N'" + addressSupp + "' where Supplier_ID = '" + supplier_ID + "'";
            SqlCommand cmd = new SqlCommand(str, conn);






            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật thông tin này không ?.", "Cập nhật thông tin đơn vị cung cấp", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);



            if (result == DialogResult.OK)
            {
                cmd.ExecuteNonQuery();
                table.Rows.Clear();
                dataAdapter.Fill(table);

            }


            else
            {
                DialogResult results = MessageBox.Show("Thông tin cập nhật thất bại !.", "Cập nhật thông tin đơn vị cung cấp", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (results == DialogResult.OK)
                {
                    table.Rows.Clear();
                    dataAdapter.Fill(table);

                }
            }





















        }

        private void data_supp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            vitrichon = e.RowIndex;
            if (vitrichon >= 0)
            {
                //lấy dữ liệu từ dòng đang chọn chuyển lên khung nhập liệu
                txt_supp.Text = table.Rows[vitrichon][1].ToString();
                txt_contactSupp.Text = table.Rows[vitrichon][2].ToString();
                txt_addSupp.Text = table.Rows[vitrichon][3].ToString();
                txt_citySupp.Text = table.Rows[vitrichon][4].ToString();
                txt_phoneSupp.Text = table.Rows[vitrichon][5].ToString();
                txt_homepageSupp.Text = table.Rows[vitrichon][6].ToString();
                txt_FaxSupp.Text = table.Rows[vitrichon][7].ToString();

            }



        }

        private void data_employee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            vitrichon = e.RowIndex;
            if (vitrichon >= 0)
            {
                //lấy dữ liệu từ dòng đang chọn chuyển lên khung nhập liệu
                txt_firstname.Text = table.Rows[vitrichon][1].ToString();
                txt_lastname.Text = table.Rows[vitrichon][2].ToString();
                birthdate.Value = DateTime.Parse( table.Rows[vitrichon][3].ToString());

                txt_address.Text = table.Rows[vitrichon][4].ToString();
                txt_phone.Text= table.Rows[vitrichon][5].ToString();
                txt_email.Text = table.Rows[vitrichon][6].ToString();
                txt_city.Text = table.Rows[vitrichon][7].ToString();

                txt_fax.Text = table.Rows[vitrichon][8].ToString();
                txt_homepage.Text = table.Rows[vitrichon][9].ToString();
                hiredate.Value = DateTime.Parse( table.Rows[vitrichon][10].ToString());
                txt_password.Text = table.Rows[vitrichon][12].ToString();
                cmb_role.Text = table.Rows[vitrichon][11].ToString();

            }

        }

        private void btn_search_ship_Click(object sender, EventArgs e)
        {
            string companyname = txt_search_ship.Text.Trim(); 

            if (string.IsNullOrEmpty(companyname))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị vận chuyển cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in data_ship.Rows)
            {
                if (row.Cells["CompanyName"].Value != null && row.Cells["CompanyName"].Value.ToString().Contains(companyname))
                {
                    row.Selected = true; 
                    data_ship.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }
            }

            MessageBox.Show("Không tìm thấy đơn vị vận chuyển !.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        }

        private void btn_search_supp_Click(object sender, EventArgs e)
        {


            string companyname_supp = txt_search_supp.Text.Trim();

            if (string.IsNullOrEmpty(companyname_supp))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị cung cấp cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in data_supp.Rows)
            {
                if (row.Cells["CompanyName"].Value != null && row.Cells["CompanyName"].Value.ToString().Contains(companyname_supp))
                {
                    row.Selected = true;
                    data_supp.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }
            }

            MessageBox.Show("Không tìm thấy đơn vị cung cấp !.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);







        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void btn_search_emp_Click(object sender, EventArgs e)
        {
            string phone_employee = txt_search_employee.Text.Trim();

            if (string.IsNullOrEmpty(phone_employee))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in data_employee.Rows)
            {
                if (row.Cells["Phone"].Value != null && row.Cells["Phone"].Value.ToString().Contains("+" + phone_employee))
                {
                    row.Selected = true;
                    data_employee.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }
            }

            MessageBox.Show("Không tìm thấy nhân viên  !.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void txt_search_employee_TextChanged(object sender, EventArgs e)
        {
            //txt.PlaceholderText = 

        }

        private void btn_dashboard_Click(object sender, EventArgs e)
        {
            //panel_dashboard.Show();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            pictute_home.Visible = false;
            panel_manager_suppliers.Visible = false;
            panel_manager_shipper.Visible = false;
            panel_manager_employees.Visible = true;

            //  txt_search_employee.PlaceholderText = "Hãy nhập số điện thoại";



            conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT employees_ID , firstname , lastname , birthdate , address, phone , email , city , fax, homepage , hiredate , role , password   FROM Employees";
            dataAdapter = new SqlDataAdapter(sql, conn);
            table = new DataTable();
            dataAdapter.Fill(table);
            data_employee.DataSource = table;

            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            pictute_home.Visible = false;
            panel_manager_shipper.Visible = false;
            panel_manager_suppliers.Show();

            conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT Supplier_ID, CompanyName , ContactName ,Address , City , Phone , HomePage , Fax  FROM Suppliers";
            dataAdapter = new SqlDataAdapter(sql, conn);
            table = new DataTable();
            dataAdapter.Fill(table);
            data_supp.DataSource = table;


            this.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            pictute_home.Visible = false;
            panel_manager_shipper.Show();

            conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT * FROM Shippers";
            dataAdapter = new SqlDataAdapter(sql, conn);
            table = new DataTable();
            dataAdapter.Fill(table);
            data_ship.DataSource = table;

            this.Close();
        }
    }
}
