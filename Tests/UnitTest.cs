using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicXml;
using System.Diagnostics;
using System.Xml.Linq;

namespace Tests
{
	/// <summary>
	/// Summary description for UnitTest
	/// </summary>
	[TestClass]
	public class UnitTest
	{
		string xmlFile = @"c:\users\mipo\documents\visual studio 2010\Projects\DynamicXml\Tests\CV.xml";
		string updatedXmlFile = @"c:\users\mipo\documents\visual studio 2010\Projects\DynamicXml\Tests\CV_Updated.xml";

		public UnitTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

	
		[TestMethod]
		public void CheckData()
		{
			try
			{
				dynamic xml = new XmlItem(xmlFile);
				Assert.IsNotNull(xml, "Xml is not loaded");

				// Element value
				string name = xml.Personal.Name; // short conversion to string
				string name2 = xml.Personal.Name.Value;
				Assert.AreEqual("Mipo", name);
				Assert.AreEqual("Mipo", name2);

				// Attribute value
				string year = xml.Personal.Birth["year"];
				Assert.AreEqual("1984", year);

				// Element by position
				var jobs = xml.Jobs();
				Assert.AreEqual("WPF developer", (string)jobs[0]);
				Assert.AreEqual("ASP.NET MVC developer", (string)jobs[1]);

				// Find element by attribute value
				var sqlExp = xml.Experiences.Experience("name", "SQL");
				Assert.IsNotNull(sqlExp, "Experience not found");
				Assert.AreEqual("5", sqlExp["years"], false, "Another experience found");
			}
			catch (Exception exc)
			{
				Assert.Fail(exc.Message);
			}
		}

		[TestMethod]
		public void Update()
		{
			try
			{
				dynamic xml = new XmlItem(xmlFile);
				Assert.IsNotNull(xml, "Xml is not loaded");

				string updated = DateTime.Now.ToString();
				xml["updated"] = updated;
				Assert.AreEqual(updated, xml["updated"]);

				string newName = "Mipoberg";
				xml.Personal.Name.Value = newName;
				Assert.AreEqual(newName, xml.Personal.Name.ToString());

				xml.Save(updatedXmlFile);
			}
			catch (Exception exc)
			{
				Assert.Fail(exc.Message);
			}
		}
	}
}
