// aprongen
// A tool to generate MSFS XML aprons from OpenStreetMap data.
// Created by Melody Wass <mel -at- strangemelody -dot- xyz>

using System.Xml;

// Print the program name, version, and copyright.
Console.WriteLine("aprongen v0.1.0");
Console.WriteLine("Copyright (c) 2023 Melody Wass <github.com/strangemelody>\n");

// Check if there are two arguments. If not, show usage and exit.
if (args.Length != 2)
{
    Console.WriteLine("Usage: aprongen <OSM file> <MSFS scenery XML file>");
    Environment.Exit(1);
}

// Get the OSM file and MSFS scenery XML file paths from the command line
string osmPath = args[0];
string sceneryPath = args[1];

// Check if the paths were provided. If not, show usage and exit.
if (osmPath == null || sceneryPath == null)
{
    Console.WriteLine("Usage: aprongen <OSM file> <MSFS scenery XML file>");
    Environment.Exit(1);
}

// Check if the OSM file exists. If not, show an error and exit.
if (!File.Exists(osmPath))
{
    Console.WriteLine("Error: OSM file does not exist.");
    Environment.Exit(1);
}

// Check if the MSFS scenery XML file exists. If not, show an error and exit.
if (!File.Exists(sceneryPath))
{
    Console.WriteLine("Error: MSFS scenery XML file does not exist.");
    Environment.Exit(1);
}

// Create a new XML document to store the MSFS scenery XML file
XmlDocument sceneryXml = new XmlDocument();
sceneryXml.Load(sceneryPath);

// Create a new XML document to store the OSM file
XmlDocument osmXml = new XmlDocument();
osmXml.Load(osmPath);

// Get all <node> elements from the OSM file.
XmlNodeList nodeNodes = osmXml.GetElementsByTagName("node");

// Get the <Aprons> node from the MSFS scenery XML file.
XmlNode? apronsNode = sceneryXml.GetElementsByTagName("Aprons")[0];

// Get the <way> nodes from the OSM file.
XmlNodeList wayNodes = osmXml.GetElementsByTagName("way");

// Create a new list to store the <way> nodes that are aprons.
List<XmlNode> apronNodes = new List<XmlNode>();

// Loop through all <way> nodes and look for the ones that are aprons.
foreach (XmlNode wayNode in wayNodes)
{
    // Get all child nodes of the <way> node.
    XmlNodeList wayChildNodes = wayNode.ChildNodes;

    // Loop through all child nodes of the <way> node.
    foreach (XmlNode wayChildNode in wayChildNodes)
    {
        // Check if wayChildNode is null. If so, skip this child node.
        if (wayChildNode == null) continue;

        // Check if the child node is a <tag> node.
        if (wayChildNode.Name == "tag")
        {
            // Get the "k" attribute of the <tag> node.
            string? k = wayChildNode?.Attributes?["k"]?.Value;

            // Check if the "k" attribute is "aeroway" and the "v" attribute is "apron".
            if (k == "aeroway" && wayChildNode?.Attributes?["v"]?.Value == "apron")
            {
                // Add the <way> node to the apron nodes list.
                apronNodes.Add(wayNode);
            }
        }
    }
}

// Check if the apron nodes count is greater than zero. If not, show an error and exit.
if (apronNodes.Count == 0)
{
    Console.WriteLine("Error: No aprons found in OSM file.");
    Environment.Exit(1);
} else {
    Console.WriteLine("Found " + apronNodes.Count + " aprons.");
}

// Loop through all apron nodes.
foreach (XmlNode apronNode in apronNodes)
{
    // Show progress with count.
    Console.WriteLine("\nProcessing apron " + (apronNodes.IndexOf(apronNode) + 1) + " of " + apronNodes.Count);

    // Get all <nd> elements from the apron node.
    XmlNodeList? ndNodes = apronNode.SelectNodes("nd");

    // Check if the ndNodes list is null. If so, skip this apron node.
    if (ndNodes == null) continue;

    // Create a new <Apron> element.
    XmlElement apronElement = sceneryXml.CreateElement("Apron");

    // Set the surface attribute of the <Apron> element.
    apronElement.SetAttribute("surface", "ASPHALT");

    // Loop all <nd> elements.
    foreach (XmlNode ndNode in ndNodes)
    {
        // get the <nd> node's "ref" attribute.
        string? ndRef = ndNode?.Attributes?["ref"]?.Value;

        // Match the ndRef to a <node> element, and return the first or default value.
        XmlNode? nodeId = nodeNodes.Cast<XmlNode>().FirstOrDefault(x => x.Attributes?["id"]?.Value == ndRef);

        // Get the <node> element's "lat" and "lon" attributes.
        string? lat = nodeId?.Attributes?["lat"]?.Value;
        string? lon = nodeId?.Attributes?["lon"]?.Value;

        // Create a new <Vertex /> element.
        XmlElement vertexElement = sceneryXml.CreateElement("Vertex");
        // Set the "lat" and "lon" attributes of the <Vertex /> element.
        vertexElement.SetAttribute("lat", lat);
        vertexElement.SetAttribute("lon", lon);

        // Append the <Vertex /> element to the <Apron /> element.
        apronElement.AppendChild(vertexElement);
        
        // Show the user the progress.
        Console.WriteLine("Added vertex " + lat + ", " + lon);
    }

    // Append the <Apron /> element to the <Aprons /> element.
    apronsNode?.AppendChild(apronElement);
}

// Save the MSFS scenery XML file.
sceneryXml.Save(sceneryPath);

// Show the user that the program has finished.
Console.WriteLine("\nFinished.");

// Exit the program.
Environment.Exit(0);