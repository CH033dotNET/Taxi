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
	public partial class CreateOrderFrom : Form, IFormResult<OrderExDTO> {

		public OrderExDTO Result { get; set; }

		public CreateOrderFrom() {
			InitializeComponent();
		}

		private async void OrderBtn_Click(object sender, EventArgs e) {
			var address = AddressTextBox.Text;
			Result = await RequestHelper.Get<OrderExDTO>(string.Format("api/Order?address={0}", address));
			if (Result == null) {
				MessageBox.Show("Error");
			} else {
				MessageBox.Show("Order id:" + Result.Id);
				this.DialogResult = DialogResult.OK;
			}
		}
	}
}
