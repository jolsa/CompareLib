using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ComparerLib
{
	internal class CompareData
	{
		public string DescriptionA { get; private set; }
		public string DescriptionB { get; private set; }
		public ReadOnlyCollection<DiffItem> Items { get; private set; }
		public CompareData(IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null)
		{
			//	Set empty descriptions to null
			if (descriptionA != null && descriptionA.Trim() == "") descriptionA = null;
			if (descriptionB != null && descriptionB.Trim() == "") descriptionB = null;

			//	Set the parent
			var list = items.ToList();
			list.ForEach(item => item.Parent = this);
			Items = list.AsReadOnly();

			//	Set the descriptions
			DescriptionA = descriptionA;
			DescriptionB = descriptionB;
		}
	}
}
