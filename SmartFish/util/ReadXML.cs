/*
 * Created by SharpDevelop.
 * User: kchien
 * Date: 05/01/2012
 * Time: 13:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml;

namespace SmartFish
{
	/// <summary>
	/// Description of ReadXML.
	/// </summary>
	public class ReadXML
	{
		public ReadXML()
		{
		}
		
		
		
		public static void Read(string fileName)
		{
			if(fileName == "test.txt"){
				readTest();
				return;
			}
			
			using (XmlReader reader = XmlReader.Create(fileName)) 
			{
				while(reader.Read())
				{
					// Only detect start elements.
					if (reader.IsStartElement())
					{
						// Get element name and switch on it.
						switch (reader.Name)
						{
							case "World":
								Console.Out.WriteLine("Read World!");
								break;
						}//end switch
					}
				}//end while
			}//end using
				
			
		}
		
		public static void readTest()
		{
			// Create an XML reader for this file.
			using (XmlReader reader = XmlReader.Create("test.txt"))
			{	
				while (reader.Read())
				{
					// Only detect start elements.
					if (reader.IsStartElement())
					{
						// Get element name and switch on it.
						switch (reader.Name)
						{
							case "perls":
								// Detect this element.
								Console.WriteLine("Start <perls> element.");
								break;
							case "article":
								// Detect this article element.
								Console.WriteLine("Start <article> element.");
								// Search for the attribute name on this current node.
								string attribute = reader["name"];
								if (attribute != null)
								{
									Console.WriteLine("  Has attribute name: " + attribute);
								}
								// Next read will contain text.
								if (reader.Read())
								{
									Console.WriteLine("  Text node: " + reader.Value.Trim());
								}
								break;
						}//end switch
					}//end if
				}//end while
			}//end using
		}
		
		/*
		XmlReaderSettings settings = new XmlReaderSettings();
		settings.ConformanceLevel = ConformanceLevel.Auto;
		settings.IgnoreWhitespace = true;
		settings.IgnoreComments = true;
		XmlReader reader = XmlReader.Create("books.xml", settings);
		
		using (XmlReader reader = XmlReader.Create("book3.xml")) {

  			// Parse the XML document.  ReadString is used to 
  			// read the text content of the elements.
  			reader.Read(); 
  			reader.ReadStartElement("book");  
  			reader.ReadStartElement("title");   
  			Console.Write("The content of the title element:  ");
  			Console.WriteLine(reader.ReadString());
  			reader.ReadEndElement();
  			reader.ReadStartElement("price");
  			Console.Write("The content of the price element:  ");
 			Console.WriteLine(reader.ReadString());
  			reader.ReadEndElement();
  			reader.ReadEndElement();

		}*/

	}
}
