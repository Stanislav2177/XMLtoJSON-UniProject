const hierarchicalData = [];

// Modified data structure with document, parent, nextNode, and previousNode
function addRoot() {
  const rootDiv = document.createElement("div");
  rootDiv.className = "root";

  const rootLabel = document.createElement("label");
  rootLabel.innerText = "Root: ";
  const rootInput = document.createElement("input");
  rootInput.type = "text";

  const addChildButton = document.createElement("button");
  addChildButton.innerText = "Add Child";
  addChildButton.onclick = () => addChild(rootDiv, rootData);

  rootDiv.appendChild(rootLabel);
  rootDiv.appendChild(rootInput);
  rootDiv.appendChild(addChildButton);
  container.appendChild(rootDiv);

  // Create and add root data to hierarchicalData
  const rootData = {
    document: "",
    parent: "root", // Every root has a 'parent' label of 'root'
    nextNode: null,
    previousNode: null,
    children: [],
  };
  hierarchicalData.push(rootData);

  // Update root data when the input changes
  rootInput.addEventListener("input", () => {
    rootData.document = rootInput.value;
  });
}

// Function to add a child to a specified root
function addChild(parent, rootData) {
  const childDiv = document.createElement("div");
  childDiv.className = "child";

  const childLabel = document.createElement("label");
  childLabel.innerText = "Child: ";
  const childInput = document.createElement("input");
  childInput.type = "text";

  const addGrandchildButton = document.createElement("button");
  addGrandchildButton.innerText = "Add Grandchild";
  addGrandchildButton.onclick = () => addGrandchild(childDiv, childData);

  childDiv.appendChild(childLabel);
  childDiv.appendChild(childInput);
  childDiv.appendChild(addGrandchildButton);
  parent.appendChild(childDiv);

  // Create and add child data to the root's children array
  const childData = {
    document: "",
    parent: rootData.document, // Each child has the root as the parent
    nextNode: null,
    previousNode: null,
    children: [],
  };
  rootData.children.push(childData);

  // Update child data when the input changes
  childInput.addEventListener("input", () => {
    childData.document = childInput.value;
  });
}

// Function to add a grandchild to a specified child
function addGrandchild(parent, childData) {
  const grandchildDiv = document.createElement("div");
  grandchildDiv.className = "grandchild";

  const grandchildLabel = document.createElement("label");
  grandchildLabel.innerText = "Grandchild: ";
  const grandchildInput = document.createElement("input");
  grandchildInput.type = "text";

  grandchildDiv.appendChild(grandchildLabel);
  grandchildDiv.appendChild(grandchildInput);
  parent.appendChild(grandchildDiv);

  // Create and add grandchild data to the child's children array
  const grandchildData = {
    document: "",
    parent: childData.document, // Grandchild's parent is the child
    nextNode: null,
    previousNode: null,
  };
  childData.children.push(grandchildData);

  // Update grandchild data when the input changes
  grandchildInput.addEventListener("input", () => {
    grandchildData.document = grandchildInput.value;
  });
}

// Function to log the hierarchical data structure and generate XML
function logData() {
  // Convert hierarchical data to XML
  const xmlData = objectToXml(hierarchicalData);
  console.log(xmlData);
  document.getElementById("xmlDisplay").textContent = xmlData;
  return xmlData;
}

// Button to log the data structure to the console
const logButton = document.createElement("button");
logButton.innerText = "Log Data Structure";
logButton.onclick = logData;
document.body.appendChild(logButton);

// Convert the hierarchical data into XML format
function objectToXml(data) {
  let xml = "";

  data.forEach((root) => {
    xml += `<XElement>\n  <document>${root.document}</document>\n  <parent>${root.parent}</parent>\n`;

    if (root.nextNode) {
      xml += `  <nextNode>\n    <document>${root.nextNode.document}</document>\n    <nodeType>${root.nextNode.nodeType}</nodeType>\n    <parent>${root.nextNode.parent}</parent>\n    <nextNode>${root.nextNode.nextNode}</nextNode>\n    <previousNode>${root.nextNode.previousNode}</previousNode>\n  </nextNode>\n`;
    }

    if (root.children && root.children.length > 0) {
      xml += `  <children>\n${childrenToXml(root.children)}</children>\n`;
    }

    xml += `</XElement>\n`;
  });

  return xml;
}

// Convert child data into XML format
function childrenToXml(children) {
  let xml = "";

  children.forEach((child) => {
    xml += `    <child>\n      <document>${child.document}</document>\n      <parent>${child.parent}</parent>\n`;

    if (child.nextNode) {
      xml += `      <nextNode>\n        <document>${child.nextNode.document}</document>\n        <nodeType>${child.nextNode.nodeType}</nodeType>\n        <parent>${child.nextNode.parent}</parent>\n        <nextNode>${child.nextNode.nextNode}</nextNode>\n        <previousNode>${child.nextNode.previousNode}</previousNode>\n      </nextNode>\n`;
    }

    if (child.children && child.children.length > 0) {
      xml += `      <children>\n${childrenToXml(child.children)}</children>\n`;
    }

    xml += `    </child>\n`;
  });

  return xml;
}
// Convert the hierarchical data into XML format without unnecessary spaces
function objectToXmlNotFormated(data) {
  let xml = "";

  data.forEach((root) => {
    xml += `<XElement><document>${root.document}</document><parent>${root.parent}</parent>`;

    if (root.nextNode) {
      xml += `<nextNode><document>${root.nextNode.document}</document><nodeType>${root.nextNode.nodeType}</nodeType><parent>${root.nextNode.parent}</parent><nextNode>${root.nextNode.nextNode}</nextNode><previousNode>${root.nextNode.previousNode}</previousNode></nextNode>`;
    }

    if (root.children && root.children.length > 0) {
      xml += `<children>${childrenToXmlNotFormated(root.children)}</children>`;
    }

    xml += `</XElement>`;
  });

  return xml;
}

// Convert child data into XML format without unnecessary spaces
function childrenToXmlNotFormated(children) {
  let xml = "";

  children.forEach((child) => {
    xml += `<child><document>${child.document}</document><parent>${child.parent}</parent>`;

    if (child.nextNode) {
      xml += `<nextNode><document>${child.nextNode.document}</document><nodeType>${child.nextNode.nodeType}</nodeType><parent>${child.nextNode.parent}</parent><nextNode>${child.nextNode.nextNode}</nextNode><previousNode>${child.nextNode.previousNode}</previousNode></nextNode>`;
    }

    if (child.children && child.children.length > 0) {
      xml += `<children>${childrenToXmlNotFormated(child.children)}</children>`;
    }

    xml += `</child>`;
  });

  return xml;
}

// Make sure the DOM is fully loaded before accessing elements
document.addEventListener("DOMContentLoaded", function () {
  document
    .getElementById("generateXMLButton")
    .addEventListener("click", function () {
      // Convert the hierarchical data to XML and display
      logData();
    });
});
function wrapInRoot(xml) {
  return `<Root>${xml}</Root>`;
}
document.addEventListener("DOMContentLoaded", function () {
  document
    .getElementById("sendXMLToBackend")
    .addEventListener("click", function () {
      const xmlData = objectToXmlNotFormated(hierarchicalData); // Assuming logData generates your XML data
      console.log("xml data:", xmlData);
      const wrappedXml = wrapInRoot(xmlData);

      const token = localStorage.getItem("authToken");
      if (token) {
        console.log("Token retrieved:", token);
      }

      fetch("https://localhost:7078/api/XMLReceiver", {
        method: "POST",
        headers: {
          "Content-Type": "text/xml",
          Authorization: `Bearer ${token}`, // Add the Bearer token here
        },
        body: wrappedXml,
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
          }
          return response.text(); // Return response text to the next then block
        })
        .then((data) => {
          console.log("Response:", data);

          // Update the <h2> element with the response data
          const formattedJson = JSON.stringify(data, null, 2); // Pretty-print JSON

          const h2Element = document.querySelector("pre#xmlDisplay");
          h2Element.textContent = formattedJson; // Set the response data as the text content of <h2>
        })
        .catch((error) => console.error("Error:", error));
    });
});
