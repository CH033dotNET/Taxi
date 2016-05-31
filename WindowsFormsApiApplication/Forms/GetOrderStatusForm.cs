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

namespace WindowsFormsApiApplication.Forms {
	public partial class GetOrderStatusForm : Form, IFormResult<string> {

		public string Result { get; set; }

		public GetOrderStatusForm() {
			InitializeComponent();
		}

		private async void CheckBtn_Click(object sender, EventArgs e) {
			var orderId = IdTextBox.Text;
			Result = await RequestHelper.Get<string>(string.Format("api/Order?order_id={0}", orderId));
			if (Result == null) {
				MessageBox.Show("Error");
			} else {
				MessageBox.Show("Status code:" + Result);
				
				this.DialogResult = DialogResult.OK;
			}
		}
	}
}
