using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Microarea.Mago4Butler.AutomaticUpdates
{
    partial class updates
    {
        public static updates Load(string path)
        {
            using (FileStream inputFile = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(updates));

                XmlSchema setupSchema = XmlSchema.Read(
                    typeof(updates).Assembly.GetManifestResourceStream("Microarea.Mago4Butler.AutomaticUpdates.Schemas.Updates.xsd"),
                    null
                    );

                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.Schemas.Add(setupSchema);
                readerSettings.ValidationType = ValidationType.Schema;

                XmlReader xmlReader = null;
                try
                {
                    xmlReader = XmlReader.Create(inputFile, readerSettings);
                }
                catch (SecurityException)
                {
                    
                }

                updates upd = null;
                try
                {
                    upd = (updates)serializer.Deserialize(xmlReader);
                }
                catch (InvalidOperationException)
                {
                }

                return upd;
            }
        }

        //public void Save(string path)
        //{
        //    using (FileStream outputFile = new FileStream(path, FileMode.Create, FileAccess.Write))
        //    {
        //        XmlWriterSettings settings = new XmlWriterSettings();
        //        settings.ConformanceLevel = ConformanceLevel.Document;
        //        settings.Encoding = Encoding.UTF8;
        //        settings.Indent = true;

        //        XmlWriter xmlWriter = null;
        //        try
        //        {
        //            xmlWriter = XmlWriter.Create(outputFile, settings);
        //        }
        //        catch (SecurityException)
        //        {
                    
        //        }

        //        XmlSerializer serializer = new XmlSerializer(typeof(updates));
        //        try
        //        {
        //            serializer.Serialize(outputFile, this);
        //        }
        //        catch (InvalidOperationException)
        //        {
                    
        //        }
        //    }
        //}
    }
}
