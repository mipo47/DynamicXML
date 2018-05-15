# Lightweight XML library for .NET #

Use XML like dynamic object

## Usage ##

### Sample XML ###


```
#!xml
<CV updated="never">
	<Personal>
		<Name>Mipo</Name>
		<Birth year="1984" month="12" day="22" />
	</Personal>
	<Jobs>
		<Job>WPF developer</Job>
		<Job>ASP.NET MVC developer</Job>
	</Jobs>
	<Experiences>
		<Experience name="JQuery" years="3" />
		<Experience name="SQL" years="5" />
	</Experiences>
</CV>
```

### Read ###

```
#!C#
// Load XML from file
dynamic xml = new XmlItem(xmlFile);

// Element value
string name = xml.Personal.Name;
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

```

### Write ###


```
#!C#

// Load XML from file
dynamic xml = new XmlItem(xmlFile);

// Change attribute value
xml["updated"] = DateTime.Now.ToString();;

// Change element value
xml.Personal.Name.Value = "New name";

// Save changes to file
xml.Save(updatedXmlFile);
```
