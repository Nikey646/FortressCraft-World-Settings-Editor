using System;
using System.Windows.Forms;

namespace Simple.World.Settings.Editor.Dialogs
{
	public partial class NumericDialog : Form
	{
		public Decimal Result;

		#region .  Constructers  .

		private NumericDialog()
		{
			this.InitializeComponent();

		}

		public NumericDialog(decimal currentValue) : this()
		{
			this.numericUpDown1.Maximum = decimal.MaxValue;
			this.numericUpDown1.Minimum = decimal.MinValue;
			this.numericUpDown1.Value = currentValue;
		}

		public NumericDialog(Int64 currentValue) : this()
		{
			this.numericUpDown1.Maximum = Int64.MaxValue;
			this.numericUpDown1.Minimum = Int64.MinValue;
			this.numericUpDown1.Value = currentValue;
		}

		public NumericDialog(Int32 currentValue) : this()
		{
			this.numericUpDown1.Maximum = Int32.MaxValue;
			this.numericUpDown1.Minimum = Int32.MinValue;
			this.numericUpDown1.Value = currentValue;
		}

		public NumericDialog(Single currentValue) : this()
		{
			this.numericUpDown1.Maximum = decimal.MaxValue;
			this.numericUpDown1.Minimum = decimal.MinValue;
			this.numericUpDown1.Value = Convert.ToDecimal(currentValue);
		}

		public NumericDialog(Decimal currentValue, Int32 decimalPlaces) : this(currentValue)
		{
			this.numericUpDown1.DecimalPlaces = decimalPlaces;
		}

		public NumericDialog(Int64 currentValue, Int32 decimalPlaces) : this(currentValue)
		{
			this.numericUpDown1.DecimalPlaces = decimalPlaces;
		}

		public NumericDialog(Int32 currentValue, Int32 decimalPlaces) : this(currentValue)
		{
			this.numericUpDown1.DecimalPlaces = decimalPlaces;
		}

		public NumericDialog(Single currentValue, Int32 decimalPlaces) : this(currentValue)
		{
			this.numericUpDown1.DecimalPlaces = decimalPlaces;
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

		private void numericUpDown1_ValueChanged(Object sender, EventArgs e)
		{
			this.Result = this.numericUpDown1.Value;
		}
	}
}
