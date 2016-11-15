using ComparerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ExeTester
{
	static class Program
	{
		private static class Actions
		{
			public const string Push = "Push to DB2";
			public const string Delete = "Delete from DB2";
		}

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			string text = @"
CREATE PROCEDURE dbo.spSomeProcedure
AS

SET NOCOUNT ON

SELECT	name
FROM		sys.tables
ORDER BY name
	<- tab here
";
			var rnd = new Random();
			var items = new List<DiffItem>()
			{
				new DiffItem(new[] { "ProcedureA", "Stored Procedures" }, "CREATE PROCEDURE [ProcedureA]", "CREATE PROCEDURE ProcedureA"),
				new DiffItem(new[] { "ProcedureB", "Stored Procedures" }, "CREATE PROCEDURE [ProcedureB]", "CREATE PROCEDURE [ProcedureB]"),
				new DiffItem(new[] { "spSomeProcedure1", "Stored Procedures" }, text, null),
				new DiffItem(new[] { "spSomeProcedure2", "Stored Procedures" }, null, text)
			};

			//	Default usage:
			//	Comparer.Compare(items, "DB1", "DB2", new string[] { "Item Name", "Type Name" });

			Comparer.Compare(items, "DB1", "DB2", new string[] { "Item Name", "Type Name" }, new[] { Actions.Push, Actions.Delete }, CustomAction, CheckEnabled);
		}
		private static void CustomAction(string action, DiffItem item)
		{
			if (action == Actions.Push)
				item.Update(UpdateTypes.AToB);
			else if (action == Actions.Delete)
				item.Update(UpdateTypes.Delete);
		}
		private static bool CheckEnabled(string action, DiffItem item)
		{
			if (action == Actions.Push)
				return item.Condition == DiffConditions.AOnly || item.Condition == DiffConditions.Different;
			else if (action == Actions.Delete)
				return item.Condition == DiffConditions.BOnly;
			return true;
		}
	}
}
