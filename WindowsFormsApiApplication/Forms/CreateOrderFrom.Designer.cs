namespace WindowsFormsApiApplication.Forms {
	partial class CreateOrderFrom {
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
			this.AddressLabel = new System.Windows.Forms.Label();
			this.AddressTextBox = new System.Windows.Forms.TextBox();
			this.OrderBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// AddressLabel
			// 
			this.AddressLabel.AutoSize = true;
			this.AddressLabel.Location = new System.Drawing.Point(3, 15);
			this.AddressLabel.Name = "AddressLabel";
			this.AddressLabel.Size = new System.Drawing.Size(45, 13);
			this.AddressLabel.TabIndex = 0;
			this.AddressLabel.Text = "Address";
			// 
			// AddressTextBox
			// 
			this.AddressTextBox.Location = new System.Drawing.Point(44, 12);
			this.AddressTextBox.Name = "AddressTextBox";
			this.AddressTextBox.Size = new System.Drawing.Size(100, 20);
			this.AddressTextBox.TabIndex = 1;
			// 
			// OrderBtn
			// 
			this.OrderBtn.Location = new System.Drawing.Point(69, 38);
			this.OrderBtn.Name = "OrderBtn";
			this.OrderBtn.Size = new System.Drawing.Size(75, 23);
			this.OrderBtn.TabIndex = 2;
			this.OrderBtn.Text = "Order";
			this.OrderBtn.UseVisualStyleBackColor = true;
			this.OrderBtn.Click += new System.EventHandler(this.OrderBtn_Click);
			// 
			// CreateOrderFrom
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(150, 74);
			this.Controls.Add(this.OrderBtn);
			this.Controls.Add(this.AddressTextBox);
			this.Controls.Add(this.AddressLabel);
			this.Name = "CreateOrderFrom";
			this.Text = "Create Order";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label AddressLabel;
		private System.Windows.Forms.TextBox AddressTextBox;
		private System.Windows.Forms.Button OrderBtn;
	}
}