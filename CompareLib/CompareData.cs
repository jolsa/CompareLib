using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ComparerLib
{
	internal class CompareData
	{
		/// <summary>
		/// Gets or sets extra labels used in UI
		/// </summary>
		public ReadOnlyCollection<string> CustomActionLabels { get; private set; }
		public Action<string, DiffItem> CustomAction { get; private set; }
		public Func<string, DiffItem, bool> CheckEnabled { get; private set; }
		internal Action<DiffItem, UpdateTypes> ItemUpdated { get; set; }

		public string DescriptionA { get; private set; }
		public string DescriptionB { get; private set; }
		public ReadOnlyCollection<DiffItem> Items { get; private set; }
		public ReadOnlyCollection<string> NameLabels { get; private set; }
		public CompareData(IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null,
			IEnumerable<string> customActionLabels = null, Action<string, DiffItem> customAction = null, Func<string, DiffItem, bool> checkEnabled = null)
		{
			//	Set empty descriptions to null
			if (descriptionA != null && descriptionA.Trim() == "") descriptionA = null;
			if (descriptionB != null && descriptionB.Trim() == "") descriptionB = null;

			//	Set the parent
			var list = items.ToList();
			list.ForEach(item => item.Parent = this);

			//	Set Items and labels
			Items = list.AsReadOnly();
			NameLabels = (nameLabels ?? new string[0]).ToList().AsReadOnly();

			//	Set the descriptions
			DescriptionA = descriptionA;
			DescriptionB = descriptionB;

			//	Set custom action properties
			if (customActionLabels != null && customActionLabels.Any())
				CustomActionLabels = customActionLabels.ToList().AsReadOnly();
			CustomAction = customAction;
			CheckEnabled = checkEnabled;

		}
		internal void UpdateItem(DiffItem item, UpdateTypes type)
		{
			if (type == UpdateTypes.Delete)
			{
				var list = Items.ToList();
				list.Remove(item);
				Items = list.AsReadOnly();
			}
			ItemUpdated?.Invoke(item, type);
		}
	}
}
