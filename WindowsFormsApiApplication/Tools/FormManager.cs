using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApiApplication.Tools
{
	public static class FormManager
	{
		public static TResult ShowForm<TForm, TResult>(TForm form) 
			where TForm : Form, IFormResult<TResult>, new()
		{
			form.ShowDialog();
			return form.Result;
		}
	}
}
