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
        public String connectionString = "Data Source=ADMIN-PC ; Initial Catalog=QLMP ; Integrated Security=True; Trust Server Certificate = True;"
        public frm_staff()
        {
            InitializeComponent();           
        }

        private void cmbSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
