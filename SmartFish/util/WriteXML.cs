/*
 * Created by SharpDevelop.
 * User: kchien
 * Date: 17/05/2012
 * Time: 10:03 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml;

namespace SmartFish
{
	/// <summary>
	/// Description of WriteXML.
	/// </summary>
	public class WriteXML
	{
		public WriteXML()
		{}
		
		public static void Write()
		{
			//Creating XmlWriter Settings
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "  ";
			settings.NewLineOnAttributes = true;

			//Using XmlWriter to create xml file.
			using (XmlWriter writer = XmlWriter.Create("c:\\OrderDetails.xml",settings))
			{
				writer.WriteComment("Order Details of PowerToYou");
				writer.WriteStartElement("OrderDetails");
				writer.WriteStartElement("Order");
				writer.WriteAttributeString("OrderID","O001");
				writer.WriteElementString("ProductName","ASP.NET2.0");
				writer.WriteElementString("Price","0");
				writer.WriteEndElement();
				writer.WriteStartElement("Order");
				writer.WriteAttributeString("OrderID","O002");
				writer.WriteElementString("ProductName","Mono-Develop-SDK");
				writer.WriteElementString("Price","0");
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.Flush();
				
			}
		}
	}
}
