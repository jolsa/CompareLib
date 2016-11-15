using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ComparerLib
{
	public class DiffItem
	{
		private readonly List<string> _names;
		internal CompareData Parent { get; set; }

		/// <summary>
		/// Create a DiffItem
		/// </summary>
		/// <param name="name">A name to describe the item (e.g. filename)</param>
		/// <param name="contentsA">Item A content</param>
		/// <param name="contentsB">Item B content</param>
		public DiffItem(string name, string contentsA, string contentsB) :
			this(new[] { name }, contentsA, contentsB)
		{ }
		/// <summary>
		/// Create a DiffItem
		/// </summary>
		/// <param name="names">Names to describe the item (e.g. filename, extension, type, etc.)</param>
		/// <param name="contentsA">Item A content</param>
		/// <param name="contentsB">Item B content</param>
		public DiffItem(IEnumerable<string> names, string contentsA, string contentsB)
		{
			_names = new List<string>(names);
			if (!_names.Any())
				_names.Add("<unknown>");
			ContentsA = contentsA;
			ContentsB = contentsB;
		}

		/// <summary>
		/// Returns the comparison condition of A content to B content
		/// </summary>
		public DiffConditions Condition
		{
			get
			{
				DiffConditions val;
				if (ContentsA != null && ContentsB == null)
					val = DiffConditions.AOnly;
				else if (ContentsB != null && ContentsA == null)
					val = DiffConditions.BOnly;
				else
					val = (ContentsA == ContentsB) ? DiffConditions.Same : DiffConditions.Different;
				return val;
			}
		}
		/// <summary>
		/// Returns Name Labels
		/// </summary>
		public ReadOnlyCollection<string> Names
		{
			get { return _names.AsReadOnly(); }
		}
		/// <summary>
		/// Returns Name Label (if there's only one)
		/// </summary>
		public string Name
		{
			get { return _names.Count == 1 ? _names.First() : null; }
		}
		public string ContentsA { get; private set; }
		public string ContentsB { get; private set; }

		/// <summary>
		/// Returns a user-friendly description of the difference
		/// </summary>
		public string DiffDescription
		{
			get
			{
				var condition = Condition;
				string result = condition.ToString();
				if (condition == DiffConditions.AOnly && !string.IsNullOrEmpty(Parent?.DescriptionA))
					result = Parent.DescriptionA + " only";
				else if (condition == DiffConditions.BOnly && !string.IsNullOrEmpty(Parent?.DescriptionB))
					result = Parent.DescriptionB + " only";
				else if (condition == DiffConditions.AOnly)
					result = "A Only";
				else if (condition == DiffConditions.BOnly)
					result = "B Only";
				return result;
			}
		}

		/// <summary>
		/// Returns the action the UI can offer for the item
		/// </summary>
		internal string ActionName
		{
			get
			{
				var condition = Condition;
				string action;
				if (condition == DiffConditions.Same)
					action = null;
				else if (condition == DiffConditions.Different)
					action = Actions.Compare.ToString();
				else
					action = Actions.View.ToString();
				return action;
			}
		}
		public void Update(UpdateTypes type)
		{
			if (type == UpdateTypes.AToB)
				ContentsB = ContentsA;
			else if (type == UpdateTypes.BToA)
				ContentsA = ContentsB;
			Parent.UpdateItem(this, type);
		}
	}
}
