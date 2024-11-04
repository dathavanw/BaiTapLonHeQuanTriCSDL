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
    public partial class frm_staff : Form
    {
        public String connectionString = @"Data Source=ADMIN-PC ; Initial Catalog=QLMP ; Integrated Security=True;";
        public frm_staff()
        {
            InitializeComponent();
            
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            var productForm = new ProductForm(); 
            productForm.Show();
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            var categoryForm = new CategoryForm(connectionString);
            categoryForm.Show();
        }

        private void btnHangHoa_Click(object sender, EventArgs e)
        {
            var orderForm = new OrderForm();
            orderForm.Show();
        }
    }
}
