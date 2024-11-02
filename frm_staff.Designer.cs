namespace BaiTapLonHeQuanTriCSDL
{
    partial class frm_staff
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_staff));
            this.panelMenu = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmbNhaCungCap = new System.Windows.Forms.ComboBox();
            this.cmbSanPham = new System.Windows.Forms.ComboBox();
            this.cmbTaoHoaDon = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.DarkBlue;
            this.panelMenu.Controls.Add(this.pictureBox1);
            this.panelMenu.Controls.Add(this.cmbNhaCungCap);
            this.panelMenu.Controls.Add(this.cmbSanPham);
            this.panelMenu.Controls.Add(this.cmbTaoHoaDon);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(200, 360);
            this.panelMenu.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 108);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // cmbNhaCungCap
            // 
            this.cmbNhaCungCap.BackColor = System.Drawing.SystemColors.Menu;
            this.cmbNhaCungCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cmbNhaCungCap.ForeColor = System.Drawing.Color.Black;
            this.cmbNhaCungCap.FormattingEnabled = true;
            this.cmbNhaCungCap.Items.AddRange(new object[] {
            "Thêm nhà cung cấp",
            "Sửa nhà cung cấp",
            "Tạo nhà cung cấp"});
            this.cmbNhaCungCap.Location = new System.Drawing.Point(0, 180);
            this.cmbNhaCungCap.Name = "cmbNhaCungCap";
            this.cmbNhaCungCap.Size = new System.Drawing.Size(200, 24);
            this.cmbNhaCungCap.TabIndex = 3;
            this.cmbNhaCungCap.Text = "Quản lý loại sản phẩm";
            // 
            // cmbSanPham
            // 
            this.cmbSanPham.BackColor = System.Drawing.SystemColors.Info;
            this.cmbSanPham.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cmbSanPham.ForeColor = System.Drawing.Color.Black;
            this.cmbSanPham.FormattingEnabled = true;
            this.cmbSanPham.Items.AddRange(new object[] {
            "Thêm sản phẩm",
            "Sửa sản phẩm",
            "Tạo sản phẩm",
            "Xoá sản phẩm"});
            this.cmbSanPham.Location = new System.Drawing.Point(0, 140);
            this.cmbSanPham.Name = "cmbSanPham";
            this.cmbSanPham.Size = new System.Drawing.Size(200, 24);
            this.cmbSanPham.TabIndex = 2;
            this.cmbSanPham.Text = "Quản lý sản phẩm";
            this.cmbSanPham.SelectedIndexChanged += new System.EventHandler(this.cmbSanPham_SelectedIndexChanged);
            // 
            // cmbTaoHoaDon
            // 
            this.cmbTaoHoaDon.BackColor = System.Drawing.SystemColors.Menu;
            this.cmbTaoHoaDon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cmbTaoHoaDon.ForeColor = System.Drawing.Color.Black;
            this.cmbTaoHoaDon.FormattingEnabled = true;
            this.cmbTaoHoaDon.Items.AddRange(new object[] {
            "Thêm hoá đơn"});
            this.cmbTaoHoaDon.Location = new System.Drawing.Point(0, 220);
            this.cmbTaoHoaDon.Name = "cmbTaoHoaDon";
            this.cmbTaoHoaDon.Size = new System.Drawing.Size(200, 24);
            this.cmbTaoHoaDon.TabIndex = 3;
            this.cmbTaoHoaDon.Text = "Tạo hoá đơn";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(206, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(658, 359);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // frm_staff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 360);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panelMenu);
            this.Name = "frm_staff";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trang nhân viên";
            this.panelMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.ComboBox cmbSanPham;
        private System.Windows.Forms.ComboBox cmbNhaCungCap;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cmbTaoHoaDon;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}
