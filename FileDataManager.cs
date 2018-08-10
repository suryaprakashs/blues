using SimpleMan.Blues.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SimpleMan.Blues
{
    public class FileDataManager
    {
        private string fileName = "Blues.xml";

        static FileDataManager fileDataManager;
        public static FileDataManager GetInstance()
        {
            if (fileDataManager == null)
                fileDataManager = new FileDataManager();

            return fileDataManager;
        }

        public void WriteFile(SystemAction sysAction)
        {

            if (File.Exists(fileName))
            {
                var fileInfo = new FileInfo(fileName);
                if (fileInfo.Length >= 100000)
                {
                    string currentTimeString = fileInfo.CreationTime.ToString("yyyyMMddhhmmss");
                    string backUpFileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - 4) + "_" + currentTimeString + fileInfo.Extension;
                    if (!File.Exists(backUpFileName))
                        File.Move(fileName, backUpFileName);

                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
            }

            if (!File.Exists(fileName))
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter xmlWriter = XmlWriter.Create(fileName, xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("SysActions");

                    xmlWriter.WriteStartElement("Action");
                    xmlWriter.WriteElementString("OrderNo", sysAction.OrderNo.ToString());
                    xmlWriter.WriteElementString("CurrentAction", sysAction.CurrentAction.ToString());
                    xmlWriter.WriteElementString("ActionTime", sysAction.ActionTime.ToString());
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
            else
            {

                XDocument xDocument = XDocument.Load(fileName);

                XElement root = xDocument.Element("SysActions");
                IEnumerable<XElement> rows = root.Descendants("Action");
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                   new XElement("Action",
                       new XElement("OrderNo", sysAction.OrderNo.ToString()),
                       new XElement("CurrentAction", sysAction.CurrentAction.ToString()),
                       new XElement("ActionTime", sysAction.ActionTime.ToString())
                   ));
                xDocument.Save(fileName);
            }
        }
    }
}
