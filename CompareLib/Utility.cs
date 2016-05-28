using System;
using System.Linq;

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
	internal static class Utility
	{
		internal static Actions GetActionFromName(string actionName)
		{
			return (Actions)Enum.Parse(typeof(Actions), actionName);
		}
		internal static DiffConditions GetConditionFromName(string conditionName)
		{
			return (DiffConditions)Enum.Parse(typeof(DiffConditions), conditionName);
		}
	}
}
