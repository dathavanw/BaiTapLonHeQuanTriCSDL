using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapLonHeQuanTriCSDL
{
    public partial class CategoryForm : Form
    {
        private readonly string connectionString;
        private int? selectedCategoryId = null;

        public CategoryForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            SetupDataGridView();
            LoadCategories();
        }

        private void SetupDataGridView()
        {
            dataGridViewCategories.AutoGenerateColumns = true;
            dataGridViewCategories.AllowUserToAddRows = false;
            dataGridViewCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCategories.MultiSelect = false;
        }

        // DATABASE OPERATIONS

        private bool CreateCategory(string categoryName, string description)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name cannot be empty", nameof(categoryName));

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Categories (CategoryName, Description) 
                                   VALUES (@CategoryName, @Description)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        cmd.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error when creating category: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating category: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Categories ORDER BY CategoryName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dtCategories = new DataTable();
                        adapter.Fill(dtCategories);
                        dataGridViewCategories.DataSource = dtCategories;

                        // Đặt tên hiển thị cho các cột
                        if (dataGridViewCategories.Columns["Category_ID"] != null)
                            dataGridViewCategories.Columns["Category_ID"].HeaderText = "ID";
                            dataGridViewCategories.Columns["Category_ID"].Visible = false;
                        if (dataGridViewCategories.Columns["CategoryName"] != null)
                            dataGridViewCategories.Columns["CategoryName"].HeaderText = "Category Name";
                        if (dataGridViewCategories.Columns["Description"] != null)
                            dataGridViewCategories.Columns["Description"].HeaderText = "Description";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UpdateCategory(int categoryId, string categoryName, string description)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Invalid category ID", nameof(categoryId));
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name cannot be empty", nameof(categoryName));

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Kiểm tra xem tên category mới có bị trùng không (trừ category hiện tại)
                    string checkQuery = @"SELECT COUNT(*) FROM Categories 
                                       WHERE CategoryName = @CategoryName 
                                       AND Category_ID != @CategoryId";

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        checkCmd.Parameters.AddWithValue("@CategoryId", categoryId);

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("A category with this name already exists!",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    // Thực hiện cập nhật nếu không có trùng lặp
                    string updateQuery = @"UPDATE Categories 
                                       SET CategoryName = @CategoryName,
                                           Description = @Description
                                       WHERE Category_ID = @CategoryId";

                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        updateCmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        updateCmd.Parameters.AddWithValue("@Description",
                            string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description);

                        int result = updateCmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error when updating category: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating category: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool DeleteCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Invalid category ID", nameof(categoryId));

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Check for related products
                            string checkQuery = "SELECT COUNT(*) FROM Products WHERE Category_ID = @CategoryId";
                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                                int count = (int)checkCmd.ExecuteScalar();
                                if (count > 0)
                                {
                                    MessageBox.Show("Cannot delete this category as it has associated products!",
                                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }

                            // Proceed with deletion
                            string deleteQuery = "DELETE FROM Categories WHERE Category_ID = @CategoryId";
                            using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn, transaction))
                            {
                                deleteCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                                int result = deleteCmd.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    transaction.Commit();
                                    return true;
                                }
                            }
                            transaction.Rollback();
                            return false;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // EVENT HANDLERS

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (CreateCategory(txtCategoryName.Text.Trim(), txtDescription.Text.Trim()))
                    {
                        MessageBox.Show("Category added successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!selectedCategoryId.HasValue)
                {
                    MessageBox.Show("Please select a category to update!",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ValidateInput())
                {
                    string categoryName = txtCategoryName.Text.Trim();
                    string description = txtDescription.Text.Trim();

                    // Hiển thị hộp thoại xác nhận trước khi cập nhật
                    var result = MessageBox.Show(
                        "Are you sure you want to update this category?",
                        "Confirm Update",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        if (UpdateCategory(selectedCategoryId.Value, categoryName, description))
                        {
                            MessageBox.Show("Category updated successfully!",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCategories();
                            ClearFields();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating category: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!selectedCategoryId.HasValue)
                {
                    MessageBox.Show("Please select a category to delete!",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete this category?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (DeleteCategory(selectedCategoryId.Value))
                    {
                        MessageBox.Show("Category deleted successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCategories();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridViewCategories.Rows[e.RowIndex];
                selectedCategoryId = Convert.ToInt32(row.Cells["Category_ID"].Value);
                txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value?.ToString();
            }
        }

        private void DataGridViewCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0)
            {
                var row = dataGridViewCategories.SelectedRows[0];
                selectedCategoryId = Convert.ToInt32(row.Cells["Category_ID"].Value);
                txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value?.ToString();
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // HELPER METHODS

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Please enter a category name!",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            selectedCategoryId = null;
            txtCategoryName.Clear();
            txtDescription.Clear();
            txtCategoryName.Focus();
        }

    }
}