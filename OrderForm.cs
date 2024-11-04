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

namespace BaiTapLonHeQuanTriCSDL
{
    public partial class OrderForm : Form
    {
        private string connectionString = "Data Source=ADMIN-PC ; Initial Catalog=QLMP ; Integrated Security=True;";
        private DataTable ordersTable;

        public OrderForm()
        {
            InitializeComponent();
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            LoadOrders(dtpStartDate.Value, dtpEndDate.Value);
        }

        private void LoadOrders(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetCustomerSalesByDate", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

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
                MessageBox.Show("Vui lòng chọn đơn hàng cần xóa.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa đơn hàng này?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                                // Lấy Order_ID từ dòng được chọn
                                int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["Order_ID"].Value);

                                // Xóa từ Orders_Details trước
                                using (SqlCommand cmdDetails = new SqlCommand(
                                    "DELETE FROM Orders_Details WHERE Order_ID = @OrderId", conn, transaction))
                                {
                                    cmdDetails.Parameters.AddWithValue("@OrderId", orderId);
                                    cmdDetails.ExecuteNonQuery();
                                }

                                // Sau đó xóa từ Orders
                                using (SqlCommand cmdOrder = new SqlCommand(
                                    "DELETE FROM Orders WHERE Order_ID = @OrderId", conn, transaction))
                                {
                                    cmdOrder.Parameters.AddWithValue("@OrderId", orderId);
                                    cmdOrder.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                MessageBox.Show("Xóa đơn hàng thành công!", "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Tải lại dữ liệu
                                LoadOrders(dtpStartDate.Value, dtpEndDate.Value);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception($"Lỗi khi xóa đơn hàng: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
