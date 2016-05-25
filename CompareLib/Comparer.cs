using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComparerLib
{

	public static class Comparer
	{
		public static void Compare(IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null)
		{
			Compare(null, items, descriptionA, descriptionB);
		}
		public static void Compare(IWin32Window owner, IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null)
		{
			var cmp = new CompareData(items, descriptionA, descriptionB);
			using (var frm = new ListForm(cmp))
				frm.ShowDialog(owner);
		}
		public static Win32Window ToWin32Window(this IntPtr hWnd)
		{
			return new Win32Window(hWnd);
		}
	}

}
