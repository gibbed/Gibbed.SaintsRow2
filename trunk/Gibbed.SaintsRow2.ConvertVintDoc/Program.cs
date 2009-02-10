using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Gibbed.SaintsRow2.FileFormats;

namespace Gibbed.SaintsRow2.ConvertVintDoc
{
	class Program
	{
		private static void WriteProperty(XmlWriter writer, string name, VintProperty property)
		{
			writer.WriteElementString("name", name);
			writer.WriteElementString("type", property.Tag);
			writer.WriteElementString("value", property.ToString());
		}

		private static void WriteObject(XmlWriter writer, VintObject o)
		{
			writer.WriteElementString("name", o.Name);
			writer.WriteElementString("type", o.Type);

			writer.WriteStartElement("baseline");
			foreach (KeyValuePair<string, VintProperty> property in o.Baseline)
			{
				writer.WriteStartElement("property");
				WriteProperty(writer, property.Key, property.Value);
				writer.WriteEndElement(); // property
			}
			writer.WriteEndElement(); // baseline

			writer.WriteStartElement("overrides");
			foreach (KeyValuePair<string, Dictionary<string, VintProperty>> resolution in o.Overrides)
			{
				writer.WriteStartElement("resolution");
				writer.WriteElementString("name", resolution.Key);

				foreach (KeyValuePair<string, VintProperty> property in resolution.Value)
				{
					writer.WriteStartElement("property");
					WriteProperty(writer, property.Key, property.Value);
					writer.WriteEndElement(); // property
				}
				
				writer.WriteEndElement(); // resolution
			}
			writer.WriteEndElement(); // overrides

			writer.WriteStartElement("children");
			foreach (VintObject child in o.Children)
			{
				writer.WriteStartElement("object");
				WriteObject(writer, child);
				writer.WriteEndElement(); // object
			}
			writer.WriteEndElement(); // children
		}

		private static void WriteDocument(Stream input, Stream output)
		{
			VintFile vint = new VintFile();
			vint.Read(input);

			XmlTextWriter writer = new XmlTextWriter(output, Encoding.UTF8);
			writer.Formatting = Formatting.Indented;

			writer.WriteStartDocument();
			writer.WriteStartElement("root");

			writer.WriteElementString("vint_doc_type", "vint_document");
			writer.WriteElementString("name", Path.ChangeExtension(vint.Name, ".vint_xdoc"));
			writer.WriteElementString("vint_doc_version", "2");
			writer.WriteElementString("anim_time", vint.AnimationTime.ToString());

			writer.WriteStartElement("metadata");
			foreach (KeyValuePair<string,string> metadata in vint.Metadata)
			{
				writer.WriteStartElement("metadata_item");
				writer.WriteElementString("name", metadata.Key);
				writer.WriteElementString("value", metadata.Value);
				writer.WriteEndElement(); // metadata_item
			}
			writer.WriteEndElement(); // metadata

			writer.WriteStartElement("critical_resources");
			writer.WriteStartElement("pegs");
			foreach (string filename in vint.CriticalResources)
			{
				writer.WriteElementString("filename", filename);
			}
			writer.WriteEndElement(); // pegs
			writer.WriteEndElement(); // critical_resources

			writer.WriteStartElement("elements");
			foreach (VintObject element in vint.Elements)
			{
				writer.WriteStartElement("object");
				WriteObject(writer, element);
				writer.WriteEndElement(); // object
			}
			writer.WriteEndElement(); // elements

			writer.WriteStartElement("animations");
			foreach (VintObject animation in vint.Animations)
			{
				writer.WriteStartElement("object");
				WriteObject(writer, animation);
				writer.WriteEndElement(); // object
			}
			writer.WriteEndElement(); // animations
			
			writer.WriteEndElement(); // root
			writer.WriteEndDocument();

			writer.Flush();
		}

		public static void Main(string[] args)
		{
			string directory;

			if (args.Length == 0)
			{
				directory = ".";
			}
			else if (args.Length == 1)
			{
				directory = args[1];
			}
			else
			{
				Console.WriteLine("{0} [directory with vint_doc files]", Path.GetFileName(Application.ExecutablePath));
				return;
			}

			foreach (string inputPath in Directory.GetFiles(Path.GetFullPath(directory), "*.vint_doc"))
			{
				string outputPath = Path.ChangeExtension(inputPath, ".vint_xdoc");
				if (File.Exists(outputPath))
				{
					//continue;
				}

				Stream input = File.OpenRead(inputPath);
				Stream output = File.OpenWrite(outputPath);

				WriteDocument(input, output);

				output.Close();
				input.Close();
			}
		}
	}
}
