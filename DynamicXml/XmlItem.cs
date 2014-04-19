using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Xml.Linq;
using System.Collections;
using System.Globalization;

namespace DynamicXml 
{
	/// <summary>
	/// See demo xml file "CV.xml"
	/// </summary>
	public class XmlItem : DynamicObject
	{
		XElement element;

		#region Attributes and Value

		public string this[string attr]
		{
			get { return element.Attribute(attr).Value; }
			set { element.Attribute(attr).SetValue(value); }
		}

		public XmlItem this[int i]
		{
			get
			{
				XElement e = element.Elements().ElementAt(i);
				if (e != null)
					return new XmlItem(e);
				else
					return null;
			}
		}

		public string Value
		{
			get { return element.Value; }
			set { element.SetValue(value); }
		}

		#endregion

		#region Load/Save

		public static XmlItem Parse(string xmlText, LoadOptions loadOptions = LoadOptions.None)
		{
			return new XmlItem(XElement.Parse(xmlText, loadOptions));
		}

		public XmlItem(XDocument doc)
		{
			element = doc.Root;
		}

		public XmlItem(XElement element)
		{
			this.element = element;
		}

		public XmlItem(string xmlFile)
		{
			this.element = XElement.Load(xmlFile);
		}

		public void Save(string xmlFile)
		{
			element.Save(xmlFile);
		} 

		#endregion

		#region Overrides

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = null;
			XElement e = element.Element(binder.Name);
			if (e == null)
				return false;

			result = new XmlItem(e);
			return true;
		}

		public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
		{
			var elements = element.Elements();
			if (elements == null)
			{
				result = null;
				return false;
			}
			//if (elements.Count() == 1)
			//	result = new XmlItem(elements.First());
			//else
				result = new XmlItems(elements);
			return true;
		}

		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			result = null;
			var elements = element.Elements(binder.Name);
			if (elements == null)
				return false;

			if (args.Length > 0)
			{
				elements = elements.Where((e) =>
				{
					for (int i = 0; i < args.Length; i += 2)
					{
						var a = e.Attribute(args[i].ToString());
						if (a == null || a.Value != args[i + 1].ToString())
							return false;
					}
					return true;
				});
			}

			//if (elements.Count() == 1)
			//	result = new XmlItem(elements.First());
			//else
				result = new XmlItems(elements);
			return true;
		}

		public override bool TryConvert(ConvertBinder binder, out object result)
		{
			string value = element.Value;
			if (binder.Type == typeof(string))
				result = value;
			else if (binder.Type == typeof(DateTime))
				result = DateTime.Parse(value);
			else if (binder.Type == typeof(DateTime?))
			{
				DateTime dt;
				if (DateTime.TryParse(value, out dt))
					result = dt;
				else
					result = null;
			}
			else
				result = Convert.ChangeType(value, binder.Type, CultureInfo.InvariantCulture);

			return true;
		}

		public override string ToString()
		{
			return element.Value;
		} 
		#endregion
	}
}
