namespace MiniSupermarket.WinForms
{
    partial class FormAccountManagement
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
            SuspendLayout();
            // 
            // FormAccountManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "FormAccountManagement";
            Text = "Quản trị tài khoản";
            Load += FormAccountManagement_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}