using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Simple.World.Settings.Editor.Dialogs
{
	public partial class EnumDialog : Form
	{
		private readonly Type _enumType;

		public Enum Result;

		#region .  Constructers  .

		private EnumDialog()
		{
			this.InitializeComponent();
		}

		public EnumDialog(Enum input)
		{
			this.InitializeComponent();
			this._enumType = input.GetType();

			foreach (var name in Enum.GetNames(input.GetType()))
			{
				this.comboBox1.Items.Add(name);
			}
			this.comboBox1.Text = input.ToString();
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

		private void comboBox1_SelectedValueChanged(Object sender, EventArgs e)
		{
			this.Result = (Enum) Enum.Parse(this._enumType, this.comboBox1.Text);
		}
	}
}
