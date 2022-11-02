using System;
using System.IO;
using System.Xml.Serialization;

namespace Onyix
{
	public class Configuration
	{
		[XmlElement]
		public string Token;

		public static Configuration Load(string path)
		{
			// Get file info
			FileInfo file = new(path);

			if (!file.Exists)
			{
				throw new IOException($"Config file {path} does not exist");
			}

			// Deserialize config file
			Configuration? config;
			XmlSerializer serializer = new(typeof(Configuration));
			FileStream reader = new(path, FileMode.Open);

			config = (Configuration?)serializer.Deserialize(reader);

			if (config == null)
			{
				throw new Exception($"Failed to deserialize config file {path}");
			}

			// Return config
			reader.Close();
			return config;
		}
	}
}
