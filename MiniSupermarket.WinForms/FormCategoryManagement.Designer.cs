namespace MiniSupermarket.WinForms
{
    partial class FormCategoryManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvCategories = new DataGridView();
            txtId = new TextBox();
            txtCategoryName = new TextBox();
            txtDescription = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnSearch = new Button();
            btnLoad = new Button();
            textBox1 = new TextBox();

            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            SuspendLayout();

            // 
            // dgvCategories
            // 
            dgvCategories.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgvCategories.Location = new Point(16, 82);
            dgvCategories.Name = "dgvCategories";
            dgvCategories.Size = new Size(410, 157);
            dgvCategories.TabIndex = 0;

            // Gắn sự kiện click vào dòng
            dgvCategories.CellClick += dgvCategories_CellClick;

            // 
            // txtId
            // 
            txtId.Location = new Point(448, 82);
            txtId.Name = "txtId";
            txtId.Size = new Size(244, 23);
            txtId.TabIndex = 1;
            txtId.Text = "Nhập mã id";

            // 
            // txtCategoryName
            // 
            txtCategoryName.Location = new Point(447, 128);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(245, 23);
            txtCategoryName.TabIndex = 2;
            txtCategoryName.Text = "Nhập tên nhóm hàng";

            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(450, 171);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(242, 23);
            txtDescription.TabIndex = 3;
            txtDescription.Text = "Nhập mô tả";

            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(450, 227);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Thêm mới";
            btnAdd.UseVisualStyleBackColor = true;

            // Gắn sự kiện
            btnAdd.Click += btnAdd_Click;

            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(536, 227);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(75, 23);
            btnUpdate.TabIndex = 6;
            btnUpdate.Text = "Cập nhật";
            btnUpdate.UseVisualStyleBackColor = true;

            // Gắn sự kiện
            btnUpdate.Click += btnUpdate_Click;

            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(617, 227);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = true;

            // Gắn sự kiện
            btnDelete.Click += btnDelete_Click;

            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(246, 29);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 8;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = true;

            // Gắn sự kiện
            btnSearch.Click += btnSearch_Click;

            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(329, 29);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 9;
            btnLoad.Text = "Tải lại";
            btnLoad.UseVisualStyleBackColor = true;

            // Gắn sự kiện
            btnLoad.Click += btnLoad_Click;

            // 
            // textBox1
            // 
            textBox1.Location = new Point(16, 29);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(210, 23);
            textBox1.TabIndex = 10;
            textBox1.Text = "Nhập từ khóa...";

            // 
            // FormCategoryManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 292);

            Controls.Add(textBox1);
            Controls.Add(btnLoad);
            Controls.Add(btnSearch);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(txtDescription);
            Controls.Add(txtCategoryName);
            Controls.Add(txtId);
            Controls.Add(dgvCategories);

            Name = "FormCategoryManagement";
            Text = "Quản lý nhóm hàng";

            // Form Load
            Load += FormCategoryManagement_Load;

            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvCategories;
        private TextBox txtId;
        private TextBox txtCategoryName;
        private TextBox txtDescription;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnSearch;
        private Button btnLoad;
        private TextBox textBox1;
    }
}
