using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparerLib
{
	public enum DiffConditions
	{
		Different,
		Same,
		AOnly,
		BOnly
	}
	internal enum Actions
	{
		View,
		Compare
	}
	public class DiffItem
	{
		private readonly List<string> _names;
		internal CompareData Parent { get; set; }
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
		public ReadOnlyCollection<string> Names
		{
			get { return _names.AsReadOnly(); }
		}
		public string Name
		{
			get { return _names.Count == 1 ? _names.First() : null; }
		}
		public string ContentsA { get; private set; }
		public string ContentsB { get; private set; }

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
		public string ActionName
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
		internal static Actions GetActionFromName(string actionName)
		{
			return (Actions)Enum.Parse(typeof(Actions), actionName);
		}
		internal static DiffConditions GetConditionFromName(string conditionName)
		{
			return (DiffConditions)Enum.Parse(typeof(DiffConditions), conditionName);
		}

		public DiffItem(string name, string contentsA, string contentsB) :
			this(new[] { name }, contentsA, contentsB)
		{ }
		public DiffItem(IEnumerable<string> names, string contentsA, string contentsB)
		{
			_names = new List<string>(names);
			ContentsA = contentsA;
			ContentsB = contentsB;
		}

	}
}
