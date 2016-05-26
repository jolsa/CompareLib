using ComparerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ExeTester
{
	static class Program
	{
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
			var items = new List<DiffItem>()
			{
				new DiffItem(new[] { "ProcedureA", "Stored Procedures" }, "CREATE PROCEDURE [ProcedureA]", "CREATE PROCEDURE ProcedureA"),
				new DiffItem(new[] { "ProcedureB", "Stored Procedures" }, "CREATE PROCEDURE [ProcedureB]", "CREATE PROCEDURE [ProcedureB]"),
				new DiffItem(new[] { "spSomeProcedure1", "Stored Procedures" }, text, null),
				new DiffItem(new[] { "spSomeProcedure2", "Stored Procedures" }, null, text)
			};
			Comparer.Compare(items, "DB1", "DB2");
		}
	}
}
