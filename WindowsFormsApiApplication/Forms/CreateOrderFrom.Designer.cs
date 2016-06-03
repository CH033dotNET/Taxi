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
			this.AddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.AddressLabel.Location = new System.Drawing.Point(3, 23);
			this.AddressLabel.Name = "AddressLabel";
			this.AddressLabel.Size = new System.Drawing.Size(80, 20);
			this.AddressLabel.TabIndex = 0;
			this.AddressLabel.Text = "Address:";
			// 
			// AddressTextBox
			// 
			this.AddressTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.AddressTextBox.Location = new System.Drawing.Point(89, 17);
			this.AddressTextBox.Name = "AddressTextBox";
			this.AddressTextBox.Size = new System.Drawing.Size(212, 29);
			this.AddressTextBox.TabIndex = 1;
			// 
			// OrderBtn
			// 
			this.OrderBtn.BackColor = System.Drawing.Color.Transparent;
			this.OrderBtn.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OrderBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.OrderBtn.Location = new System.Drawing.Point(89, 90);
			this.OrderBtn.Name = "OrderBtn";
			this.OrderBtn.Size = new System.Drawing.Size(111, 43);
			this.OrderBtn.TabIndex = 2;
			this.OrderBtn.Text = "Order";
			this.OrderBtn.UseVisualStyleBackColor = false;
			this.OrderBtn.Click += new System.EventHandler(this.OrderBtn_Click);
			// 
			// CreateOrderFrom
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.ClientSize = new System.Drawing.Size(313, 145);
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