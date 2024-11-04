using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace BaiTapLonHeQuanTriCSDL
{
    public partial class OrderForm : Form
    {
        private readonly string connectionString;
        private DataTable ordersTable;

        public OrderForm()
        {
            InitializeComponent();
            // Get connection string from configuration
            connectionString = "Data Source=ADMIN-PC; Initial Catalog=QLMP; Integrated Security=True;";
            LoadCustomerData();
            InitializeGridView();
        }

        private void InitializeGridView()
        {
            // Configure DataGridView settings
            dgvOrders.AutoGenerateColumns = true;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.MultiSelect = false;
            dgvOrders.ReadOnly = true;
        }

        private void LoadCustomerData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    const string query = @"
                        SELECT Customer_ID, 
                               TRIM(FirstName + ' ' + LastName) AS CustomerName 
                        FROM Customers 
                        ORDER BY LastName, FirstName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable customerTable = new DataTable();
                        adapter.Fill(customerTable);

                        comboBox1.DataSource = customerTable;
                        comboBox1.DisplayMember = "CustomerName";
                        comboBox1.ValueMember = "Customer_ID";
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = (int)comboBox1.SelectedValue;
            LoadOrders(customerId);
        }

        private void LoadOrders(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    const string query = @"
                        SELECT 
                            O.Order_ID,
                            TRIM(C.FirstName + ' ' + C.LastName) AS CustomerName,
                            O.OrderDate,
                            dbo.fn_GetCustomerSales(@CustomerID) AS TotalAmount
                        FROM Orders O
                        JOIN Customers C ON O.Customer_ID = C.Customer_ID
                        WHERE O.Customer_ID = @CustomerID
                        ORDER BY O.OrderDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            ordersTable = new DataTable();
                            adapter.Fill(ordersTable);
                            dgvOrders.DataSource = ordersTable;

                            // Format columns
                            if (dgvOrders.Columns["TotalAmount"] != null)
                            {
                                dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
                            }
                            if (dgvOrders.Columns["OrderDate"] != null)
                            {
                                dgvOrders.Columns["OrderDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //private void btnDeleteOrder_Click(object sender, EventArgs e)
        //{
        //    if (dgvOrders.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("Please select an order to delete.", "Warning",
        //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    var selectedRow = dgvOrders.SelectedRows[0];
        //    if (selectedRow.Cells["Order_ID"].Value == null)
        //    {
        //        MessageBox.Show("Invalid order selection.", "Error",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    if (MessageBox.Show("Are you sure you want to delete this order?", "Delete Confirmation",
        //        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        //    {
        //        return;
        //    }

        //    DeleteOrder(Convert.ToInt32(selectedRow.Cells["Order_ID"].Value));
        //}

        //private void DeleteOrder(int orderId)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            using (SqlTransaction transaction = conn.BeginTransaction())
        //            {
        //                try
        //                {
        //                    // Delete order details first
        //                    const string deleteDetailsQuery = "DELETE FROM Orders_Details WHERE Order_ID = @OrderId";
        //                    using (SqlCommand cmdDetails = new SqlCommand(deleteDetailsQuery, conn, transaction))
        //                    {
        //                        cmdDetails.Parameters.AddWithValue("@OrderId", orderId);
        //                        cmdDetails.ExecuteNonQuery();
        //                    }

        //                    // Then delete the order
        //                    const string deleteOrderQuery = "DELETE FROM Orders WHERE Order_ID = @OrderId";
        //                    using (SqlCommand cmdOrder = new SqlCommand(deleteOrderQuery, conn, transaction))
        //                    {
        //                        cmdOrder.Parameters.AddWithValue("@OrderId", orderId);
        //                        cmdOrder.ExecuteNonQuery();
        //                    }

        //                    transaction.Commit();
        //                    MessageBox.Show("Order deleted successfully!", "Success",
        //                        MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                    // Refresh the orders grid
        //                    if (comboBox1.SelectedValue != null)
        //                    {
        //                        LoadOrders((int)comboBox1.SelectedValue);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    throw new Exception($"Error during order deletion: {ex.Message}", ex);
        //                }
        //            }
        //        }
        //        catch (SqlException ex)
        //        {
        //            MessageBox.Show($"Database error: {ex.Message}", "Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error deleting order: {ex.Message}", "Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}
    }
}