using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Gibbed.SaintsRow2.XmlTable
{
	public class SerializationBuilder
	{
		private void ReadElement(XPathNavigator navigator)
		{
			string name = navigator.SelectSingleNode("Name").Value;
			string type = navigator.SelectSingleNode("Type").Value;

			if (type == "String")
			{
			}
			else if (type == "Int")
			{
			}
			else if (type == "Float")
			{
			}
			else if (type == "Vector")
			{
			}
			else if (type == "Color")
			{
			}
			else if (type == "Reference")
			{
			}
			else if (type == "Filename")
			{
			}
			else if (type == "Flags")
			{
			}
			else if (type == "List")
			{
			}
			else if (type == "ComboElement")
			{
			}
			else if (type == "Selection") // Combo
			{
			}
			else if (type == "Element") // Class
			{
			}
			else if (type == "Grid") // Array
			{
			}
			else
			{
				throw new Exception("unknown " + type);
			}
		}

		private void ReadTableDescription(XPathNavigator navigator)
		{
			if (navigator.SelectSingleNode("Name") == null || navigator.SelectSingleNode("Type") == null)
			{
				return;
			}

			string name = navigator.SelectSingleNode("Name").Value;
			string type = navigator.SelectSingleNode("Type").Value;

			if (type != "TableDescription")
			{
				throw new Exception();
			}

			XPathNodeIterator elements = navigator.Select("Element");
			while (elements.MoveNext())
			{
				this.ReadElement(elements.Current);
			}
		}

		public void ReadXmlTable(Stream stream)
		{
			XPathDocument document = new XPathDocument(stream);
			XPathNavigator navigator = document.CreateNavigator();
			XPathNavigator description = navigator.SelectSingleNode("/root/TableDescription");

			if (description != null && description.GetAttribute("source", "") == "")
			{
				this.ReadTableDescription(description);
			}
		}
	}
}
