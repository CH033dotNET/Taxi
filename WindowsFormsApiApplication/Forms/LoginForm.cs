using Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApiApplication.Tools;
using System.Web.Script.Serialization;

namespace WindowsFormsApiApplication.Forms
{
	public partial class LoginForm : Form, IFormResult<UserDTO>
	{
		public UserDTO Result { get; set; }

		public LoginForm()
		{
			InitializeComponent();
		}

		private async void loginButton_Click(object sender, EventArgs e)
		{
			var login = loginTextBox.Text;
			var password = PasswordTextBox.Text;
			Result = await RequestHelper.Get<UserDTO>(string.Format("api/User/Login?login={0}&password={1}", login, password));
			if (Result == null)
			{
				MessageBox.Show("Invalid login or password");
			}
			else
			{
				this.DialogResult = DialogResult.OK;
			}

		}

		private void exitButton_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (Result == null)
				Application.Exit();
			this.DialogResult = DialogResult.Cancel;
		}
	}
}
