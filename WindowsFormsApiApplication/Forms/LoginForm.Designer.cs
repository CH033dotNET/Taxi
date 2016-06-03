namespace WindowsFormsApiApplication.Forms
{
	partial class LoginForm
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
			this.PasswordTextBox = new System.Windows.Forms.TextBox();
			this.loginTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.loginButton = new System.Windows.Forms.Button();
			this.exitButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// PasswordTextBox
			// 
			this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.PasswordTextBox.Location = new System.Drawing.Point(112, 64);
			this.PasswordTextBox.Name = "PasswordTextBox";
			this.PasswordTextBox.Size = new System.Drawing.Size(226, 31);
			this.PasswordTextBox.TabIndex = 1;
			this.PasswordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PasswordTextBox_KeyPress);
			// 
			// loginTextBox
			// 
			this.loginTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.loginTextBox.Location = new System.Drawing.Point(112, 27);
			this.loginTextBox.Name = "loginTextBox";
			this.loginTextBox.Size = new System.Drawing.Size(226, 31);
			this.loginTextBox.TabIndex = 0;
			this.loginTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.loginTextBox_KeyPress);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(22, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Login:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(20, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Password:";
			// 
			// loginButton
			// 
			this.loginButton.BackColor = System.Drawing.Color.Transparent;
			this.loginButton.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.loginButton.Location = new System.Drawing.Point(12, 120);
			this.loginButton.Name = "loginButton";
			this.loginButton.Size = new System.Drawing.Size(117, 52);
			this.loginButton.TabIndex = 4;
			this.loginButton.Text = "Login";
			this.loginButton.UseVisualStyleBackColor = false;
			this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
			// 
			// exitButton
			// 
			this.exitButton.BackColor = System.Drawing.Color.Transparent;
			this.exitButton.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.exitButton.Location = new System.Drawing.Point(221, 120);
			this.exitButton.Name = "exitButton";
			this.exitButton.Size = new System.Drawing.Size(117, 52);
			this.exitButton.TabIndex = 5;
			this.exitButton.Text = "Exit";
			this.exitButton.UseVisualStyleBackColor = false;
			this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
			// 
			// LoginForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.ClientSize = new System.Drawing.Size(350, 183);
			this.Controls.Add(this.exitButton);
			this.Controls.Add(this.loginButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.loginTextBox);
			this.Controls.Add(this.PasswordTextBox);
			this.Name = "LoginForm";
			this.Text = "LogIn";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoginForm_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox PasswordTextBox;
		private System.Windows.Forms.TextBox loginTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button loginButton;
		private System.Windows.Forms.Button exitButton;
	}
}

