using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapLonHeQuanTriCSDL
{
    public partial class OrderForm : Form
    {
        private string connectionString = "Data Source=ADMIN-PC ; Initial Catalog=QLMP ; Integrated Security=True;";
        private DataTable ordersTable;

        public OrderForm()
        {
            InitializeComponent();
            LoadCustomerData(); // Load customer data on form initialization
        }

        private void LoadCustomerData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Customer_ID, FirstName + ' ' + LastName AS CustomerName FROM Customers "; // Combine first and last names
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable customerTable = new DataTable();
                            adapter.Fill(customerTable);

                            comboBox1.DataSource = customerTable;
                            comboBox1.DisplayMember = "CustomerName"; // Display combined name in comboBox
                            comboBox1.ValueMember = "Customer_ID"; // Use Customer_ID as the value
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null)
            {
                int customerId = (int)comboBox1.SelectedValue;
                LoadOrders(customerId);
            }
        }

        private void LoadOrders(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 FirstName + ' ' + LastName AS CustomerName , dbo.fn_GetCustomerSales(@CustomerID) AS TotalAmount from Customers", conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        ordersTable = new DataTable();
                        adapter.Fill(ordersTable);
                        dgvOrders.DataSource = ordersTable;

                        // Format columns
                        if (dgvOrders.Columns.Contains("TotalAmount"))
                        {
                            dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu đơn hàng: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this order?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["Order_ID"].Value);

                                using (SqlCommand cmdDetails = new SqlCommand("DELETE FROM Orders_Details WHERE Order_ID = @OrderId", conn, transaction))
                                {
                                    cmdDetails.Parameters.AddWithValue("@OrderId", orderId);
                                    cmdDetails.ExecuteNonQuery();
                                }

                                using (SqlCommand cmdOrder = new SqlCommand("DELETE FROM Orders WHERE Order_ID = @OrderId", conn, transaction))
                                {
                                    cmdOrder.Parameters.AddWithValue("@OrderId", orderId);
                                    cmdOrder.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                MessageBox.Show("Order deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadOrders((int)comboBox1.SelectedValue);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"Error deleting order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
