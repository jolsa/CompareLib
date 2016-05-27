# CompareLib

I specifically created this library as a component for LINQPad.  I frequently write scripts to compare content from one source to another, but having a user interface to display the differences—especially side-by-side is invaluable.

So CompareLib is useful in LINQPad but can be called by an application as well.

### Requirements:
* External compare application. I like [KDiff3](http://kdiff3.sourceforge.net/).
* Compare application must be in your app.config.  In the case of LINQPad, use the LINQPad.config file in the LINQPad application directory, usually C:\Program Files (x86)\LINQPad5. (create the LINQPad.config file if you don’t have one—do *not* use LINQPad.exe.config).
```xml
<?xml version="1.0"?>
<configuration>
	<appSettings>
		<add key="comparer" value="C:\Program Files (x86)\KDiff3\kdiff3.exe"/>
	</appSettings>
</configuration>
```
* If you use it in LINQPad, you will need to be able to have write access the application directory (usually C:\Program Files (x86)\LINQPad5) or the application will not run without errors.

### Public Methods:
* Compare
  * The only *required* parameter is `IEnumerable<DiffItem> items`.  Omitting `descriptionA` or `descriptionB` results in “A Only” and “B Only”.  If `nameLabels`is omitted, the first name will be “Name” and subsequent names will be blank.
  * Overload signatures:
    * `Compare(IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)`
	  * The UI will have no parent window.
	  * e.g. `Compare(items, "Development Database", "Prodution Database", new[] { "Object Name", "Object Type", "Last Modified" } );`
	* `Compare(IntPtr ownerHandle, IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)`
	  * ownerHandle is the handle of the window that will be the parent for the compare UI.
	* `Compare(IWin32Window owner, IEnumerable<DiffItem> items, string descriptionA = null, string descriptionB = null, IEnumerable<string> nameLabels = null)`
	  * owner will be the parent for the compare UI.  Windows Forms implement the IWin32Window interface.

### Extension Method:
* `Win32Window ToWin32Window(this IntPtr hWnd)`
  * Returns an `IWin32Window` implementation for an hWnd.

### LINQPad Example:
The example uses one of my extension methods (.FullJoin).  See [Extensions](https://github.com/jolsa/Extensions/blob/master/ExtensionLib/JoinExtensions.cs)

```csharp
string basicContent = @"The quick brown
fox jumps over
the lazy dog.";
string shorterContent = @"The quick brown
fox jumps over...";
string longerContent = @"The quick brown
fox jumps over
the lazy dog
then runs away.";

var list1 = new[]
{
	new { fileName = @"C:\DirectoryA\Text1.txt", content = basicContent }, // content = File.ReadAllText(fileName)
	new { fileName = @"C:\DirectoryA\Text2.txt", content = shorterContent },
	new { fileName = @"C:\DirectoryA\Text3.txt", content = longerContent },
	new { fileName = @"C:\DirectoryA\Text4.txt", content = basicContent },
	new { fileName = @"C:\DirectoryA\Text7.txt", content = (string)null },
	new { fileName = @"C:\DirectoryA\Text8.txt", content = "not null" },
	new { fileName = @"C:\DirectoryA\Text9.txt", content = (string)null }
};

var list2 = new[]
{
	new { fileName = @"C:\DirectoryB\Text1.txt", content = basicContent },
	new { fileName = @"C:\DirectoryB\Text2.txt", content = longerContent },
	new { fileName = @"C:\DirectoryB\Text3.txt", content = shorterContent },
	new { fileName = @"C:\DirectoryB\Text5.txt", content = basicContent },
	new { fileName = @"C:\DirectoryA\Text7.txt", content = (string)null },
	new { fileName = @"C:\DirectoryA\Text8.txt", content = (string)null },
	new { fileName = @"C:\DirectoryA\Text9.txt", content = "not null" }
};

//	Create a list that compares the 2 lists and .Dump() it (LINQPad feature)
//	FullJoin is one of my extension methods (see https://github.com/jolsa/Extensions/blob/master/ExtensionLib/JoinExtensions.cs)
var diffs = list1.FullJoin(list2,
	l1 => Path.GetFileName(l1.fileName),
	l2 => Path.GetFileName(l2.fileName),
	(l1, l2) => new
	{
		issue = l2 == null
			? DiffConditions.AOnly
			: l1 == null
				? DiffConditions.BOnly
				: (l1.content == null && l2.content == null) || (l1.content?.Equals(l2.content) ?? false)
					? DiffConditions.Same
					: DiffConditions.Different,
		file = Path.GetFileName(l1?.fileName ?? l2.fileName),
		path = Path.GetDirectoryName(l1?.fileName ?? l2.fileName),
		ext = Path.GetExtension(l1?.fileName ?? l2.fileName),
		contentA = l1?.content,
		contentB = l2?.content
	})
	.OrderBy(l => l.issue)
	.ThenBy(l => l.file)
	.ToList()
	.Dump();

if (diffs.Any())
{
	//	Get LINQPad window and convert to IWin32Window
	var lpWindow = Process.GetProcessById(Util.HostProcessID).MainWindowHandle.ToWin32Window();
	//	Convert to DiffItem
	//		Simple syntax, but limited to just the name
	var simpleItems = diffs.Select(d => new DiffItem(d.file, d.contentA, d.contentB));
	var complexItems = diffs.Select(d => new DiffItem(new[] { d.file, d.path, d.ext }, d.contentA, d.contentB));
	var items = complexItems;
	//	Simplest syntax: Parentless UI with default labels
	ComparerLib.Comparer.Compare(items);
	//	Full syntax
	ComparerLib.Comparer.Compare(lpWindow, items, "Path A", "Path B", new[] { "File Name", "File Path", "Extension" });
}
```

### Screenshot:

![CompareLibUI.png](CompareLibUI.png?raw=true "Screen Shot")

Choosing *Compare* will launch the compare application in your app.config.
Choosing *View* will show the contents in a simple viewer:

	![Viewer.png](Viewer.png?raw=true "Screen Shot")

	An undocumented feature of the viewer is you can hold down Ctrl and press 1-9 to choose the tab setting.