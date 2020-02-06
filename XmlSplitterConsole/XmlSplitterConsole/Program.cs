using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XmlSplitterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Application Started!!");
            Console.WriteLine("Processing File!!");
       
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,Constants.defaultFileName);
            string fileName = Path.GetFileNameWithoutExtension(path);
            XDocument xdoc = XDocument.Load(path);
            XNamespace tagPrefix = xdoc.Root.Name.Namespace;
            int count = xdoc.Descendants(tagPrefix + Constants.Worker).Count();
            var summaryNode = xdoc.Descendants(tagPrefix + Constants.Summary).FirstOrDefault();
            int skip = 0;
            int take = 10;
            int fileNo = 0;

            // Split elements into chunks and save to disk.
            while (skip < count)
            {
                // Extract portion of the xml elements.
                var workerNodes = xdoc.Descendants(tagPrefix + Constants.Worker)
                            .Skip(skip)
                            .Take(take);
                // Setup number of elements to skip on next iteration.
                skip += take;
                // File sequence no for split file.
                fileNo += 1;
                // Filename for split file.
                var outputFilename = String.Format("{0}" + "_{1}.xml", fileName, fileNo);
                // Create a partial xml document.
                XElement workerEffectiveStack = new XElement(tagPrefix+Constants.Workers_Effective_Stack, workerNodes);
                // Save to disk.
                XElement xmlElements = new XElement(tagPrefix + Constants.Workers_Effective_Stack, new XAttribute(XNamespace.Xmlns + Constants.TagPrefix, tagPrefix.NamespaceName), summaryNode, workerNodes);                
                xmlElements.Save(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"XmlOutput"), outputFilename));
            }
            Console.WriteLine("Processing Completed!!");
            Console.ReadKey();
        }
    }
}
