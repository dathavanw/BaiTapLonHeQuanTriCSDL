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
    public partial class ShipperOrderForm : Form
    {
        public ShipperOrderForm()
        {
            InitializeComponent();
            LoadShipperWithOrder();
        }

        public String connectionString = "Data Source=ADMIN-PC; Initial Catalog=QLMP;Integrated Security=True;";
        private void LoadShipperWithOrder()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM vw_ShipperWithOrder", conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable shipperOrderTable = new DataTable();
                        adapter.Fill(shipperOrderTable);
                        dgvShipperOrders.DataSource = shipperOrderTable;

                        // Optional: Format or rename columns if necessary
                        dgvShipperOrders.Columns["OrderDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                        dgvShipperOrders.Columns["ShippingDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading shipper with order data: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvShipperOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvShipperOrders.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvShipperOrders.SelectedRows[0];
                string customerID = selectedRow.Cells["Customer_ID"].Value.ToString();
                string employeeID = selectedRow.Cells["Employee_ID"].Value.ToString();
                string orderID = selectedRow.Cells["Order_ID"].Value.ToString();
                string orderDate = selectedRow.Cells["OrderDate"].Value.ToString();
                string shipAddress = selectedRow.Cells["ShipAddress"].Value.ToString();
                string shipCity = selectedRow.Cells["ShipCity"].Value.ToString();
                string shipPostalCode = selectedRow.Cells["ShipPostalCode"].Value.ToString();
                string shipperCompany = selectedRow.Cells["ShipperCompany"].Value.ToString();
                string shippingDate = selectedRow.Cells["ShippingDate"].Value.ToString();
                string shippingStatus = selectedRow.Cells["ShippingStatus"].Value.ToString();
                string transport = selectedRow.Cells["Transport"].Value.ToString();
            }
        }
    }
}
