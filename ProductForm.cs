using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapLonHeQuanTriCSDL
{
    public partial class ProductForm : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=ADMIN-PC ; Initial Catalog=QLMP ; Integrated Security=True; ";

        public ProductForm()
        {
            InitializeComponent();
            LoadSuppliers();
            LoadCategories();
            LoadProducts();
        }

        private void LoadSuppliers()
        {
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CompanyName FROM Suppliers";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                cmbSupplier.Items.Clear();
                while (reader.Read())
                {
                    cmbSupplier.Items.Add(reader["CompanyName"].ToString());
                }
            }
        }

        private void LoadCategories()
        {
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CategoryName FROM Categories";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                cmbCategory.Items.Clear();
                while (reader.Read())
                {
                    cmbCategory.Items.Add(reader["CategoryName"].ToString());
                }
            }
        }

        private void LoadProducts()
        {
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT p.Product_ID , p.ProductName, c.CategoryName, s.CompanyName, 
                               p.UnitPrice, p.Quantity, p.Size, p.Color
                               FROM Products p
                               INNER JOIN Categories c ON p.Category_ID = c.Category_ID
                               INNER JOIN Suppliers s ON p.Supplier_ID = s.Supplier_ID";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvProducts.DataSource = dt;
            }
        }

        private int GetSupplierIdByName(string companyName)
        {
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Supplier_ID FROM Suppliers WHERE CompanyName = @CompanyName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CompanyName", companyName);
                return (int)cmd.ExecuteScalar();
            }
        }

        private int GetCategoryIdByName(string categoryName)
        {
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Category_ID FROM Categories WHERE CategoryName = @CategoryName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                return (int)cmd.ExecuteScalar();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Products (ProductName, Category_ID, Supplier_ID, UnitPrice, 
                                   Quantity, Size, Color) 
                                   VALUES (@ProductName, @CategoryID, @SupplierID, @UnitPrice, 
                                   @Quantity, @Size, @Color)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@CategoryID", GetCategoryIdByName(cmbCategory.Text));
                    cmd.Parameters.AddWithValue("@SupplierID", GetSupplierIdByName(cmbSupplier.Text));
                    cmd.Parameters.AddWithValue("@UnitPrice", decimal.Parse(txtUnitPrice.Text));
                    cmd.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@Size", txtSize.Text);
                    cmd.Parameters.AddWithValue("@Color", txtColor.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product added successfully!");
                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                try
                {
                    using (conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"UPDATE Products 
                                       SET ProductName = @ProductName, 
                                           Category_ID = @CategoryID,
                                           Supplier_ID = @SupplierID,
                                           UnitPrice = @UnitPrice,
                                           Quantity = @Quantity,
                                           Size = @Size,
                                           Color = @Color
                                       WHERE Product_ID = @ProductID";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ProductID", int.Parse(dgvProducts.SelectedRows[0].Cells["Product_ID"].Value.ToString()));
                        cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                        cmd.Parameters.AddWithValue("@CategoryID", GetCategoryIdByName(cmbCategory.Text));
                        cmd.Parameters.AddWithValue("@SupplierID", GetSupplierIdByName(cmbSupplier.Text));
                        cmd.Parameters.AddWithValue("@UnitPrice", decimal.Parse(txtUnitPrice.Text));
                        cmd.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));
                        cmd.Parameters.AddWithValue("@Size", txtSize.Text);
                        cmd.Parameters.AddWithValue("@Color", txtColor.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Product updated successfully!");
                        LoadProducts();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "DELETE FROM Products WHERE Product_ID = @ProductID";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ProductID",
                                int.Parse(dgvProducts.SelectedRows[0].Cells["Product_ID"].Value.ToString()));

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Product deleted successfully!");
                            LoadProducts();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvProducts.SelectedRows[0];
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                cmbCategory.Text = row.Cells["CategoryName"].Value.ToString();
                cmbSupplier.Text = row.Cells["CompanyName"].Value.ToString();
                txtUnitPrice.Text = row.Cells["UnitPrice"].Value.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value.ToString();
                txtSize.Text = row.Cells["Size"].Value.ToString();
                txtColor.Text = row.Cells["Color"].Value.ToString();
            }
        }
    }
}
