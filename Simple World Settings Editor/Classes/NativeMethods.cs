using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Simple.World.Editor.Classes
{
	internal class NativeMethods
	{

		[DllImport("uxtheme")]
		internal static extern int SetWindowTheme(IntPtr handle,
			[MarshalAs(UnmanagedType.LPWStr)] string subAppName,
			[MarshalAs(UnmanagedType.LPWStr)] string subIdList);

	}
}
