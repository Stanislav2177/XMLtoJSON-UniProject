using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Models;
using System.Text;

//using System.Xml;
using System.Xml.Linq;

[Route("api/[controller]")]
[ApiController]
public class XMLReceiverController : ControllerBase
{
    //[HttpPost]
    //[Consumes("text/xml")]
    //[Authorize]
    //public IActionResult ReceiveXml([FromBody] string xmlData)
    //{
    //    try
    //    {
    //        // Parse the XML data into an IEnumerable of XElement(s)
    //        var roots = XElement.Parse(xmlData).Elements();

    //        StringBuilder stringBuilder = new StringBuilder();
    //        foreach (var root in roots)
    //        {
    //            // Process each root element separately
    //            CleanXml(root); // Clean XML if necessary

    //            // Convert each root XElement to JSON
    //            string json = JsonConvert.SerializeXNode(root, Formatting.Indented);
    //            stringBuilder.Append(json);
    //            stringBuilder.AppendLine();
    //            Console.WriteLine(json);
    //        }

    //        return Ok(stringBuilder.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest($"Error processing XML: {ex.Message}");
    //    }
    //}
    [HttpPost]
    [Consumes("text/xml")]
    [Authorize]
    public IActionResult ReceiveXml([FromBody] RootModel rootData)
    {
        if (rootData != null && rootData.XElements != null && rootData.XElements.Any())
        {
            // Prepare a structured response from the input data
            var response = new List<object>();

            foreach (var xElement in rootData.XElements)
            {
                // Add XElement details to the response
                var elementData = new
                {
                    Document = xElement.Document,
                    Parent = xElement.Parent,
                    Children = ProcessChildren(xElement.Children)
                };

                response.Add(elementData);

                // Log XElement details to the console (optional)
                Console.WriteLine($"Document: {xElement.Document}, Parent: {xElement.Parent}");
            }

            // Return the JSON response
            return Ok(new
            {
                data = response
            });
        }
        else
        {
            return BadRequest(new
            {
                error = "Invalid XML data or missing elements."
            });
        }
    }

    // Helper method to recursively process child elements
    private List<object> ProcessChildren(List<ChildModel> children)
    {
        var childData = new List<object>();

        if (children != null && children.Any())
        {
            foreach (var child in children)
            {
                childData.Add(new
                {
                    Document = child.Document,
                    Parent = child.Parent,
                    Children = ProcessChildren(child.Children) // Recursive processing
                });
            }
        }

        return childData;
    }




private void CleanXml(XElement element)
    {
        // Remove empty text nodes (e.g., whitespace or newlines)
        foreach (var child in element.Elements().ToList())
        {
            CleanXml(child); // Recursively clean child elements

            // Check if the element contains only whitespace or is empty
            var textNode = child.NodeType == System.Xml.XmlNodeType.Text ? child : null;
            if (textNode != null && string.IsNullOrWhiteSpace(textNode.Value))
            {
                child.Remove(); // Remove the whitespace-only text node
            }
        }
    }
}
