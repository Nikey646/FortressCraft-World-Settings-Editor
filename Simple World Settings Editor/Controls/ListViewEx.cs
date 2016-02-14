using System;
using System.Windows.Forms;
using Simple.World.Editor.Classes;

namespace Simple.World.Settings.Editor.Controls
{
	public sealed class ListViewEx : ListView
	{
		public ListViewEx()
		{
			this.DoubleBuffered = true;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
				NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
		}
	}
}
