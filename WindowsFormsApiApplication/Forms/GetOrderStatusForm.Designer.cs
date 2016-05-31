namespace WindowsFormsApiApplication.Forms {
	partial class GetOrderStatusForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.CheckBtn = new System.Windows.Forms.Button();
			this.IdTextBox = new System.Windows.Forms.TextBox();
			this.idLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// CheckBtn
			// 
			this.CheckBtn.Location = new System.Drawing.Point(79, 39);
			this.CheckBtn.Name = "CheckBtn";
			this.CheckBtn.Size = new System.Drawing.Size(75, 23);
			this.CheckBtn.TabIndex = 0;
			this.CheckBtn.Text = "Check";
			this.CheckBtn.UseVisualStyleBackColor = true;
			this.CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
			// 
			// IdTextBox
			// 
			this.IdTextBox.Location = new System.Drawing.Point(54, 13);
			this.IdTextBox.Name = "IdTextBox";
			this.IdTextBox.Size = new System.Drawing.Size(100, 20);
			this.IdTextBox.TabIndex = 1;
			// 
			// idLabel
			// 
			this.idLabel.AutoSize = true;
			this.idLabel.Location = new System.Drawing.Point(13, 13);
			this.idLabel.Name = "idLabel";
			this.idLabel.Size = new System.Drawing.Size(18, 13);
			this.idLabel.TabIndex = 2;
			this.idLabel.Text = "ID";
			// 
			// GetOrderStatusForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(180, 83);
			this.Controls.Add(this.idLabel);
			this.Controls.Add(this.IdTextBox);
			this.Controls.Add(this.CheckBtn);
			this.Name = "GetOrderStatusForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button CheckBtn;
		private System.Windows.Forms.TextBox IdTextBox;
		private System.Windows.Forms.Label idLabel;
	}
}