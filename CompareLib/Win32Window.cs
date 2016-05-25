using System;
using System.Windows.Forms;

namespace ComparerLib
{
	public class Win32Window : IWin32Window
	{
		public IntPtr Handle { get; private set; }
		public Win32Window(IntPtr hWnd)
		{ Handle = hWnd; }
	}
}
