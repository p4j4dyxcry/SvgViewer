using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml;
using SvgViewer.Utility;

namespace SvgViewer.Core
{
    internal static class SvgThumbnailUtil
    {
        private static ConcurrentDictionary<string, Geometry> GeometryCache { get; } = new ConcurrentDictionary<string, Geometry>();

        internal static IEnumerable<string> ReadSvgFromFile(string filePath)
        {
            var result = new List<string>();
            var file = new XmlDocument();
            using (var reader = new StreamReader(filePath, new UTF8Encoding(false)))
            {
                file.Load(reader);
                reader.Close();

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
        internal static Geometry GetGeometry(string filepath)
        {
            if (GeometryCache.ContainsKey(filepath))
                return GeometryCache[filepath];

            var svgDatas = ReadSvgFromFile(filepath);

            //! convert string array to Geometry
            var result = new PathGeometry();
            foreach (var geometry in svgDatas.Select(x => Geometry.Parse(Regex.Replace(x, @"\n+", "")).GetFlattenedPathGeometry(0, ToleranceType.Absolute)))
                result.AddGeometry(geometry);

            GeometryCache[filepath] = result;

            return result.DoFreeze();
        }
    }
}
