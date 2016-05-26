using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComparerLib
{

	public static class Comparer
	{
		public static void Compare(IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)
		{
			Compare(null, items, descriptionA, descriptionB, nameLabels);
		}
		public static void Compare(IntPtr ownerHandle, IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)
		{
			Compare(ownerHandle.ToWin32Window(), items, descriptionA, descriptionB, nameLabels);
		}
		public static void Compare(IWin32Window owner, IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)
		{
			var cmp = new CompareData(items, descriptionA, descriptionB, nameLabels);
			using (var frm = new ListForm(cmp))
				frm.ShowDialog(owner);
		}
		public static Win32Window ToWin32Window(this IntPtr hWnd)
		{
			return new Win32Window(hWnd);
		}
	}

}
