using Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApiApplication.Tools;

namespace WindowsFormsApiApplication.Forms
{
	public partial class MainForm : Form
	{
		public UserDTO User { get; set; }

		public MainForm()
		{
			InitializeComponent();
			using (var form = new LoginForm())
			{
				User = FormManager.ShowForm<LoginForm, UserDTO>(form);
			}
		}

		private void logOutButton_Click(object sender, EventArgs e)
		{
			User = null;
			User = FormManager.ShowForm<LoginForm, UserDTO>(new LoginForm());
		}
	}
}
