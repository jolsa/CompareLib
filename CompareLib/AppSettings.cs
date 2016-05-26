using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ComparerLib
{
	internal class AppSettings
	{
		private const string FileName = "CompareLib.config";
		private readonly string _filePath;
		private Dictionary<string, string> _settings;
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
		public void SaveSettings()
		{
			new XDocument(
				new XElement("appSettings",
					_settings.Select(s => new XElement("add", new[] { new XAttribute("key", s.Key), new XAttribute("value", s.Value) }))
					)
				).Save(_filePath);
		}
		public AppSettings()
		{
			_filePath = AppDomain.CurrentDomain.BaseDirectory;
			if (!_filePath.EndsWith("\\"))
				_filePath += '\\';
			_filePath += FileName;
			if (File.Exists(_filePath))
				_settings = XDocument.Load(_filePath).Element("appSettings")?.Elements()
					.Select(e => new
					{
						key = e.Attribute("key")?.Value,
						value = e.Attribute("value")?.Value
					}).GroupBy(g => g.key)
					.Select(g => g.Last())
					.ToDictionary(k => k.key, v => v.value, StringComparer.OrdinalIgnoreCase);
			if (_settings == null)
				_settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}
	}
}
