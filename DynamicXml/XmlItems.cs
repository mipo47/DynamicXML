using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Linq;

namespace DynamicXml
{
	public class XmlItems : XmlItem, IEnumerable<XmlItem>
	{
		List<XmlItem> items;

		public XmlItems(IEnumerable<XElement> elements)
			: base(elements.First())
		{
			items = new List<XmlItem>();
			foreach (XElement e in elements)
				items.Add(new XmlItem(e));
		}

		public IEnumerator<XmlItem> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}
	}
}
