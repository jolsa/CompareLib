using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComparerLib
{

	public static class Comparer
	{
		/// <summary>
		/// Show Compare Dialog
		/// </summary>
		/// <param name="items">Items to show in the dialog</param>
		/// <param name="descriptionA">Optional description of "A Only" items</param>
		/// <param name="descriptionB">Optional description of "B Only" items</param>
		/// <param name="nameLabels">Optional column-header names</param>
		public static void Compare(IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)
		{
			Compare(null, items, descriptionA, descriptionB, nameLabels);
		}
		/// <summary>
		/// Show Compare Dialog
		/// </summary>
		/// <param name="ownerHandle">Window handle of owner window</param>
		/// <param name="items">Items to show in the dialog</param>
		/// <param name="descriptionA">Optional description of "A Only" items</param>
		/// <param name="descriptionB">Optional description of "B Only" items</param>
		/// <param name="nameLabels">Optional column-header names</param>
		public static void Compare(IntPtr ownerHandle, IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)
		{
			Compare(ownerHandle.ToWin32Window(), items, descriptionA, descriptionB, nameLabels);
		}
		/// <summary>
		/// Show Compare Dialog
		/// </summary>
		/// <param name="owner">Owner Window</param>
		/// <param name="items">Items to show in the dialog</param>
		/// <param name="descriptionA">Optional description of "A Only" items</param>
		/// <param name="descriptionB">Optional description of "B Only" items</param>
		/// <param name="nameLabels">Optional column-header names</param>
		public static void Compare(IWin32Window owner, IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)
		{
			var cmp = new CompareData(items, descriptionA, descriptionB, nameLabels);
			using (var frm = new ListForm(cmp))
				frm.ShowDialog(owner);
		}
		/// <summary>
		/// Extension method to create a Win32Window wrapper for an IntPtr
		/// </summary>
		public static Win32Window ToWin32Window(this IntPtr hWnd)
		{
			return new Win32Window(hWnd);
		}
	}

}
