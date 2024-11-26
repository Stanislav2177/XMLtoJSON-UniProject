using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace Project.Models
{

    [XmlRoot("Root")]
    public class RootModel
    {
        [XmlElement("XElement")]
        public List<XElementModel> XElements { get; set; }
    }

    public class XElementModel
    {
        [XmlElement("document")]
        public string Document { get; set; }

        [XmlElement("parent")]
        public string Parent { get; set; }

        [XmlArray("children")]
        [XmlArrayItem("child")]
        public List<ChildModel> Children { get; set; }
    }

    public class ChildModel
    {
        [XmlElement("document")]
        public string Document { get; set; }

        [XmlElement("parent")]
        public string Parent { get; set; }

        [XmlArray("children")]
        [XmlArrayItem("child")]
        public List<ChildModel> Children { get; set; }
    }

}
