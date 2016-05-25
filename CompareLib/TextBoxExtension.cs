﻿using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	public static class TextBoxExtension
	{
		private const int EM_SETTABSTOPS = 0x00CB;

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int[] lParam);

		public static Point GetCaretPosition(this TextBox textBox)
		{
			Point point = new Point(0, 0);
			if (textBox.Focused)
			{
				point.X = textBox.SelectionStart - textBox.GetFirstCharIndexOfCurrentLine() + 1;
				point.Y = textBox.GetLineFromCharIndex(textBox.SelectionStart) + 1;
			}
			return point;
		}

		public static void SetTabStopWidth(this TextBox textbox, int width)
		{
			SendMessage(textbox.Handle, EM_SETTABSTOPS, 1, new int[] { width * 4 });
		}
	}
}
