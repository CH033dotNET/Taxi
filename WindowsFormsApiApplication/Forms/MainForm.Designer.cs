namespace WindowsFormsApiApplication.Forms
{
	partial class MainForm
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
			this.logOutButton = new System.Windows.Forms.Button();
			this.CreateOrderFormBtn = new System.Windows.Forms.Button();
			this.GetOrderStatusFormBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// logOutButton
			// 
			this.logOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.logOutButton.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.logOutButton.Location = new System.Drawing.Point(49, 179);
			this.logOutButton.Name = "logOutButton";
			this.logOutButton.Size = new System.Drawing.Size(238, 46);
			this.logOutButton.TabIndex = 0;
			this.logOutButton.Text = "LogOut";
			this.logOutButton.UseVisualStyleBackColor = true;
			this.logOutButton.Click += new System.EventHandler(this.logOutButton_Click);
			// 
			// CreateOrderFormBtn
			// 
			this.CreateOrderFormBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CreateOrderFormBtn.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.CreateOrderFormBtn.Location = new System.Drawing.Point(49, 12);
			this.CreateOrderFormBtn.Name = "CreateOrderFormBtn";
			this.CreateOrderFormBtn.Size = new System.Drawing.Size(238, 46);
			this.CreateOrderFormBtn.TabIndex = 1;
			this.CreateOrderFormBtn.Text = "Order Taxi";
			this.CreateOrderFormBtn.UseVisualStyleBackColor = true;
			this.CreateOrderFormBtn.Click += new System.EventHandler(this.CreateOrderFormBtn_Click);
			// 
			// GetOrderStatusFormBtn
			// 
			this.GetOrderStatusFormBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GetOrderStatusFormBtn.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.GetOrderStatusFormBtn.Location = new System.Drawing.Point(29, 97);
			this.GetOrderStatusFormBtn.Name = "GetOrderStatusFormBtn";
			this.GetOrderStatusFormBtn.Size = new System.Drawing.Size(277, 46);
			this.GetOrderStatusFormBtn.TabIndex = 2;
			this.GetOrderStatusFormBtn.Text = "Get Order Status";
			this.GetOrderStatusFormBtn.UseVisualStyleBackColor = true;
			this.GetOrderStatusFormBtn.Click += new System.EventHandler(this.GetOrderStatusFormBtn_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightGray;
			this.ClientSize = new System.Drawing.Size(338, 237);
			this.Controls.Add(this.GetOrderStatusFormBtn);
			this.Controls.Add(this.CreateOrderFormBtn);
			this.Controls.Add(this.logOutButton);
			this.Name = "MainForm";
			this.Text = "EasyTaxi";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button logOutButton;
		private System.Windows.Forms.Button CreateOrderFormBtn;
		private System.Windows.Forms.Button GetOrderStatusFormBtn;
	}
}