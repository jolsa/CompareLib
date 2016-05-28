using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ComparerLib
{
	/// <summary>
	/// Read/Write custom .config file
	/// </summary>
	internal class AppSettings
	{
		//	Hard-coded for now
		private const string FileName = "CompareLib.config";

		private readonly string _filePath;
		private Dictionary<string, string> _settings;

		public AppSettings()
		{
			//	Set the filename to the app directory
			_filePath = AppDomain.CurrentDomain.BaseDirectory;
			if (!_filePath.EndsWith("\\"))
				_filePath += '\\';
			_filePath += FileName;

			//	If we have one, load it
			if (File.Exists(_filePath))
				_settings = XDocument.Load(_filePath).Element("appSettings")?.Elements()
					.Select(e => new
					{
						key = e.Attribute("key")?.Value,
						value = e.Attribute("value")?.Value
					}).GroupBy(g => g.key)
					.Select(g => g.Last())
					.ToDictionary(k => k.key, v => v.value, StringComparer.OrdinalIgnoreCase);

			//	If we have no settings, create the dictionary
			if (_settings == null)
				_settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}
		//	Get/Set settings
		public string this[string key, string defaultValue]
		{
			get { return this[key] ?? defaultValue; }
		}
		public string this[string key]
		{
			get
			{
				return _settings.ContainsKey(key) ? _settings[key] : null;
			}
			set
			{
				_settings[key] = value ?? "";
			}
		}

		//	Write the file
		public void SaveSettings()
		{
			new XDocument(
				new XElement("appSettings",
					_settings.Select(s => new XElement("add", new[] { new XAttribute("key", s.Key), new XAttribute("value", s.Value) }))
					)
				).Save(_filePath);
		}
	}
}
