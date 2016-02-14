using System;
using System.Windows.Forms;

namespace Simple.World.Settings.Editor.Dialogs
{
	public partial class StringDialog : Form
	{
		public string Result;

		#region .  Constructers  .

		private StringDialog()
		{
			this.InitializeComponent();
		}

		public StringDialog(string input) : this()
		{
			this.textBox1.Text = input;
		}


		#endregion

		private void button1_Click(Object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button2_Click(Object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void textBox1_TextChanged(Object sender, EventArgs e)
		{
			this.Result = this.textBox1.Text;
		}
	}
}
