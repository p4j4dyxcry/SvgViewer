using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml;

namespace SvgViewer.Core
{
    internal static class SvgThumbnailUtil
    {
        internal static IEnumerable<string> ReadSvgFromFile(string filePath)
        {
            var result = new List<string>();

            var file = new XmlDocument();
            using (var reader = new StreamReader(filePath, new UTF8Encoding(false)))
            {
                file.Load(reader);

                foreach (var path in file.GetElementsByTagName("path"))
                {
                    if (path is XmlNode node && node.Attributes != null)
                    {
                        foreach (var attribute in node.Attributes)
                        {
                            if (attribute is XmlAttribute xmlAttribute && xmlAttribute.Name == "d")
                            {
                                result.Add(xmlAttribute.Value);
                                break;
                            }
                        }
                    }
                }
            }
            return result.ToArray();
        }
        internal static PathGeometry GetGeometry(string filepath)
        {
            var svgDatas = ReadSvgFromFile(filepath);

            //! convert string array to Geometry
            var result = new PathGeometry();
            foreach (var svgPath in svgDatas)
            {
                var geometry = Geometry.Parse(Regex.Replace(svgPath, @"\n+", ""));
                result.AddGeometry(geometry);
            }
            return result;
        }
    }
}
