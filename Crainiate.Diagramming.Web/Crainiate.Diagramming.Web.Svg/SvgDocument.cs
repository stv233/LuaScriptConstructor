// (c) Copyright Crainiate Software 2010




using System;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.ComponentModel;

namespace Crainiate.Diagramming.Web.Svg
{
	public class SvgDocument: XmlDocument
	{
		//Property variables
		private Point _shadowOffset = new Point(8, 6);

		private bool _drawShadows = true;
		private bool _optimised = true;

		private XmlNode _containerNode;
		private string _containerKey;

		private XmlNode _node;

		//Working variables
		private Hashtable _formatters;
		private Hashtable _formatterInstances;

		#region  Interface 

		public SvgDocument()
		{
			CreateNew();
			CreateFormatters();
		}

		//Sets or gets a value determining whether SVG output is optimised
		public bool Optimise
		{
			get
			{
				return _optimised;
			}
			set
			{
				_optimised = value;
			}
		}

		//Determines whether shadows are created for each shape added")]
		public virtual bool DrawShadows
		{
			get
			{
				return _drawShadows;
			}
			set
			{
				_drawShadows = value;
			}
		}

		//Sets or retrieves a point used to offset the shadows in the diagram
		public virtual Point ShadowOffset
		{
			get
			{
				return _shadowOffset;
			}
			set
			{
				_shadowOffset = value;
			}
		}

		public virtual Hashtable Formatters
		{
			get
			{
				return _formatters;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("Formatter may not be null.");
				_formatters = value;
			}
		}

		public virtual Hashtable FormatterInstances
		{
			get
			{
				return _formatterInstances;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("FormatterInstances may not be null.");
				_formatterInstances = value;
			}
		}

		public virtual XmlNode ContainerNode
		{
			get
			{
				return _containerNode;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("Parent node may not be set to null");
				_containerNode = value;
			}
		}

		public virtual XmlNode Node
		{
			get
			{
				return _node;
			}
		}

		public virtual string ContainerKey
		{
			get
			{
				return _containerKey;
			}
			set
			{
				_containerKey = value;
			}
		}

		//Register a type with the document to format
		public virtual void RegisterFormatter(Type elementType, Type formatterType)
		{
			if (!typeof(Element).IsAssignableFrom(elementType)) throw new SvgDocumentException("The elementType supplied is not an Element class.");
			if (!typeof(Formatter).IsAssignableFrom(formatterType)) throw new SvgDocumentException("The formatterType supplied is not a Formatter class.");

			_formatters.Add(elementType, formatterType);
		}

		//Register a type with the document to format
		public virtual void RemoveFormatter(Type elementType)
		{
			_formatters.Remove(elementType);
		}

		//Add a model to the svg document
		public virtual void AddModel(Model model)
		{
			AddModelImplementation(model);
		}

		//Adds a Shape to the SVG Document
		public virtual void AddElement(Element element)
		{
			//Get the formatter for the type of element
			if (!_formatters.ContainsKey(element.GetType())) throw new SvgDocumentException("A registered formatter for type " + element.GetType().ToString() + " not found.");

			//Get the formatter type 
			Type formatterType = (Type) _formatters[element.GetType()];
			
			//Get an instance
			if (!_formatterInstances.ContainsKey(formatterType))
			{
				object obj = Activator.CreateInstance(formatterType, null);
				_formatterInstances.Add(formatterType, obj);
			}
			
			Formatter formatter = (Formatter) _formatterInstances[formatterType];
			formatter.Reset();
			formatter.WriteElement(this, element);
		}

		//Add an SVG definition to the document
		public virtual string AddDefinition(string definition)
		{
			return AddDefinitionImplementation(definition, "");
		}

		//Add an SVG definition to the document
		public virtual string AddDefinition(string definition, string id)
		{
			return AddDefinitionImplementation(definition, id);
		}

		//Add an SVG clip path to the document with integer co-ordinates
		public virtual string AddClipPath(string definitionId, int x, int y)
		{
			return AddClippingImplementation(definitionId, x, y);
		}

		//Add an SVG clip path to the document with single co-ordinates
		public virtual string AddClipPath(string definitionId, float x, float y)
		{
			return AddClippingImplementation(definitionId, x, y);
		}

		//Add an SVG use to the document
		public virtual void AddUse(string useId, string definitionid, int x, int y)
		{
			AddUseImplementation(useId, definitionid, "", "", x, y);
		}

		//Add an SVG use to the document
		public virtual void AddUse(string useId, string definitionid, float x, float y)
		{
			AddUseImplementation(useId, definitionid, "", "", x, y);
		}

		//Add an SVG use to the document
		public virtual void AddUse(string useId, string definitionid, string classId, string style, int x, int y)
		{
			AddUseImplementation(useId, definitionid, classId, style, x, y);
		}

		//Add an SVG use to the document
		public virtual void AddUse(string useId, string definitionid, string classId, string style, float x, float y)
		{
			AddUseImplementation(useId, definitionid, classId, style, x, y);
		}

		//Add an SVG class to the style section of the document")]
		public virtual string AddClass(string style)
		{
			return AddClassImplementation(style, "");
		}

		//Add an SVG class to the style section of the document")]
		public virtual string AddClass(string style, string gradient)
		{
			return AddClassImplementation(style, gradient);
		}

		//Sets the hyperlink for a use element
		public void UseHyperlink(string useId, string hyperlink)
		{
			ElementHyperlink("use", useId, hyperlink);
		}

		//Sets the hyperlink for an image element
		public void ImageHyperlink(string imageId, string hyperlink)
		{
			ElementHyperlink("image", imageId, hyperlink);
		}

		//Sets the hyperlink for a text element
		public void TextHyperlink(string textId, string hyperlink)
		{
			ElementHyperlink("text", textId, hyperlink);
		}

		//Sets the hyperlink for a specified SVG element
		public virtual void ElementHyperlink(string element, string id, string hyperlink)
		{
			ElementHyperlinkImplementation(element, id, hyperlink);
		}

		protected virtual void SetNode(XmlNode node)
		{
			_node = node;
		}

		#endregion

		#region  Implementation 

		private void CreateNew()
		{
			XmlDeclaration declare = null;
			XmlNode parent = null;
			XmlNode node = null;
			XmlElement newElement = null;

			//Add xml declaration
			declare = base.CreateXmlDeclaration("1.0", "", "no");
			base.AppendChild(declare);

			//Add document type 
			//XmlDocumentType docType = null;
			//docType = MyBase.CreateDocumentType("svg", "-//W3C//DTD SVG 1.0//EN", "http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd", "")
			//MyBase.AppendChild(docType)

			//Find main svg node
			parent = base.SelectSingleNode("//svg");
			if (parent == null)
			{
				newElement = base.CreateElement("svg");
				parent = base.AppendChild(newElement);
			}

			//Add style node
			node = parent.SelectSingleNode("./style");
			if (node == null)
			{
				newElement = base.CreateElement("style");
				newElement.SetAttribute("type", "text/css");
				newElement.AppendChild(base.CreateCDataSection(""));
				parent.AppendChild(newElement);
			}

			//Add definitions node
			node = parent.SelectSingleNode("./defs");
			if (node == null)
			{
				newElement = base.CreateElement("defs");
				parent.AppendChild(newElement);
			}

			base.DocumentElement.SetAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");

			//Set up the parent node
			ContainerNode = parent;
		}

		private void CreateFormatters()
		{
			_formatters = new Hashtable();
			_formatterInstances = new Hashtable();

			RegisterFormatter(typeof(Solid),typeof(SolidFormatter));
			RegisterFormatter(typeof(Shape),typeof(ShapeFormatter));
			RegisterFormatter(typeof(Group),typeof(ShapeFormatter));
			RegisterFormatter(typeof(ComplexShape),typeof(ComplexFormatter));
			RegisterFormatter(typeof(Table),typeof(TableFormatter));
			RegisterFormatter(typeof(Port),typeof(PortFormatter));
			
			RegisterFormatter(typeof(Link),typeof(LineFormatter));
			RegisterFormatter(typeof(Connector),typeof(LineFormatter));
			RegisterFormatter(typeof(ComplexLine), typeof(LineFormatter));
			RegisterFormatter(typeof(Curve), typeof(CurveFormatter));
		}

		//Adds a diagram lines and shapes to an svg document
		private void AddModelImplementation(Model model)
		{
			ElementList list = new ElementList(true);
			
			//Add each sorted shape to the svg document			
			foreach (Element element in model.Lines.Values)
			{
				if (element.Visible) list.Add(element);
			}

			//Add each sorted line to the svg document
			foreach (Element element in model.Shapes.Values)
			{
				if (element.Visible) list.Add(element);
			}

			list.Sort();

			//Go through the diagram and add the element by layer;
			foreach (Layer layer in model.Layers)
			{
				if (layer.Visible)
				{
					foreach (Element element in list)
					{
						if (element.Layer == layer) AddElement(element);
					}
				}
			}
		}

		private void AddCurveImplementation(Curve curve)
		{
			if (curve == null) return;

			Curvedline curved = new Curvedline(curve);
			Style style = new Style(curve);

			XmlNode newNode = null;
			XmlDocumentFragment fragment = null;

			string classId = null;

			//Add the line
			fragment = base.CreateDocumentFragment();
			fragment.InnerXml = curved.ExtractCurve();

			newNode = ContainerNode.AppendChild(fragment);

			//Determine style
			classId = AddClassImplementation(style.GetStyle(), "");
			newNode.Attributes.GetNamedItem("class").InnerText = classId;
		}

		private string AddClippingImplementation(string ExistingDefId, float TransformX, float TransformY)
		{
			XmlNode svg = null;
			XmlNode def = null;
			XmlElement newElement = null;
			XmlNode path = null;

			string id = null;

			//Find main svg node
			svg = base.SelectSingleNode("//svg");
			if (svg == null) throw new SvgDocumentException("SVG Document node not found");

			//Find or create def node
			def = svg.SelectSingleNode("./defs");
			if (def == null) throw new SvgDocumentException("SVG Document Definitions node not found");

			id = "def" + (def.ChildNodes.Count + 1).ToString();

			//Create new path node
			newElement = base.CreateElement("clipPath");
			newElement.SetAttribute("id", id);
			path = def.AppendChild(newElement);

			//Create new clip path node
			newElement = base.CreateElement("use");
			newElement.SetAttribute("href", "http://www.w3.org/1999/xlink", "#" + ExistingDefId);
			newElement.SetAttribute("transform", "translate(" + XmlConvert.ToString(TransformX) + "," + XmlConvert.ToString(TransformY) + ")");
			path.AppendChild(newElement);

			return id;

		}

		private void AddUseImplementation(string useId, string definitionId, string classId, string style, float x, float y)
		{
			XmlElement newElement = null;

			string key = useId;
			if (ContainerKey != null && ContainerKey != string.Empty) key = ContainerKey + "," + useId;

			newElement = base.CreateElement("use");
			newElement.SetAttribute("id", key);
			newElement.SetAttribute("href", "http://www.w3.org/1999/xlink", "#" + definitionId);
			if (classId != "") newElement.SetAttribute("class", classId);
			if (style != "") newElement.SetAttribute("style", style);
			newElement.SetAttribute("x", XmlConvert.ToString(x));
			newElement.SetAttribute("y", XmlConvert.ToString(y));

			ContainerNode.AppendChild(newElement);

			SetNode(newElement);
		}

		private string AddClassImplementation(string style, string gradient)
		{
			XmlNode svg = null;
			XmlNode styleNode = null;
			XmlNodeList nodes = null;
			XmlDocumentFragment frag = null;

			string linearId = null;
			string classId = null;

			bool add = true;

			//Find main svg node
			svg = base.SelectSingleNode("//svg");
			if (svg == null) throw new SvgDocumentException("SVG Document node not found");

			//Find or create style node
			styleNode = svg.SelectSingleNode("./style");
			if (styleNode == null) throw new SvgDocumentException("SVG Document Style node not found");

			//See if we need to add the linear definition
			if (gradient != null && gradient != string.Empty)
			{
				linearId = GetExistingGradientId(gradient);
				if (linearId == "")
				{
					nodes = svg.SelectNodes("//linearGradient");

					linearId = "lg" + (nodes.Count + 1).ToString();

					frag = base.CreateDocumentFragment();
					frag.InnerXml = gradient;
					frag.FirstChild.Attributes.GetNamedItem("id").InnerText = linearId;

					svg.AppendChild(frag);
				}
				style = style.Replace("url(#none)", "url(#" + linearId + ")");
			}

			//See if the style exists or if we need to add it
			if (_optimised)
			{
				classId = GetExistingClassId(styleNode, style);
				add = classId == "";
			}

			//Create a new classid
			if (classId == "") classId = CreateClassId(styleNode);

			//Add the style to the style section cdata
			if (add)
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

				stringBuilder.Append(".");
				stringBuilder.Append(classId);
				stringBuilder.Append("{");
				stringBuilder.Append(style);
				stringBuilder.Append("}");

				AddCData(styleNode, stringBuilder.ToString());
			}

			return classId;
		}

		private string GetExistingClassId(XmlNode styleNode, string style)
		{
			string cData = GetCData(styleNode.InnerXml);
			if (cData == "") return "";

			string[] styles = null;
			int i = 0;

			string compare = null;
			string classId = null;

			styles = cData.Split("{}".ToCharArray());

			for (i = 0; i <= styles.GetUpperBound(0); i++)
			{
				classId = styles[i];
				classId = classId.Replace("\t", "");
				classId = classId.Replace("\r", "");

				if (classId != null && classId != "" && classId.Substring(0, 6) == ".style")
				{
					compare = styles[i + 1];
					if (compare == style)
					{
						return classId.Substring(1);
					}
				}
			}

			return "";
		}

		private string CreateClassId(XmlNode styleNode)
		{
			string cData = GetCData(styleNode.FirstChild.InnerText);
			if (cData == "") return "style1";

			string[] styles = null;
			styles = cData.Split("{}".ToCharArray());

			return "style" + ((Convert.ToInt32(styles.GetUpperBound(0) / 2) + 1).ToString());
		}


		//Add a definition with optionally a user defined id
		//If the definition is found and the document is optimised the existing def id will be returned
		//otherwise the new def id will be returned
		private string AddDefinitionImplementation(string def, string userId)
		{
			XmlNode svg = null;
			XmlNode node = null;

			string existingId = null;

			//Find main svg node
			svg = base.SelectSingleNode("//svg");
			if (svg == null) throw new SvgDocumentException("SVG Document node not found");
			
			//Find or create def node
			node = svg.SelectSingleNode("./defs");
			if (node == null) throw new SvgDocumentException("SVG Document Definitions node not found");

			//If optimisation is on then check if definition already exists
			if (_optimised & userId == "") existingId = GetExistingDefinitionId(def);

			//If id not found then add id
			if (existingId == "" | userId != "")
			{
				XmlDocumentFragment frag = null;

				if (userId != "")
				{
					existingId = userId;
				}
				else
				{
					existingId = "def" + (node.ChildNodes.Count + 1).ToString();
				}

				//Add the definition, make sure that the id is set
				frag = base.CreateDocumentFragment();
				frag.InnerXml = def;
				frag.FirstChild.Attributes.GetNamedItem("id").InnerText = existingId;
				node.AppendChild(frag);
			}

			return existingId;
		}

		private string GetExistingDefinitionId(string strXML)
		{
			XmlNode svg = null;
			XmlNode def = null;
			XmlNode compareNode = null;

			string compare = null;

			//Find main svg node
			svg = base.SelectSingleNode("//svg");
			if (svg == null) throw new SvgDocumentException("SVG Document node not found");

			//Find node
			def = svg.SelectSingleNode("./defs");
			if (def == null) return "";

			strXML = strXML.Replace(" ", "");

			foreach (XmlNode objChild in def.ChildNodes)
			{
				compareNode = objChild.CloneNode(true);
				compareNode.Attributes.GetNamedItem("id").InnerText = "";
				compare = compareNode.OuterXml.Replace(" ", "");

				if (compare == strXML) return objChild.Attributes.GetNamedItem("id").InnerText;
			}
			return "";
		}

		private string GetExistingGradientId(string strXML)
		{
			XmlNode svg = null;
			XmlNode compareNode = null;

			string compare = null;

			strXML = strXML.Replace(" ", "");

			//Find main svg node
			svg = base.SelectSingleNode("//svg");
			if (svg == null) throw new SvgDocumentException("SVG Document node not found");

			foreach (XmlNode objChild in svg.SelectNodes("//linearGradient"))
			{
				compareNode = objChild.CloneNode(true);
				compareNode.Attributes.GetNamedItem("id").InnerText = "";
				compare = compareNode.OuterXml.Replace(" ", "");

				if (compare == strXML) return objChild.Attributes.GetNamedItem("id").InnerText;
			}
			return "";
		}

		private string GetCData(string dataStringXML)
		{
			if (dataStringXML.StartsWith("<![CDATA[")) dataStringXML = dataStringXML.TrimStart("<![CDATA[".ToCharArray());
			if (dataStringXML.EndsWith("]]>")) dataStringXML = dataStringXML.TrimEnd("]]>".ToCharArray());
			return dataStringXML;
		}

		private void AddCData(XmlNode node, string dataString)
		{
			dataString = GetCData(node.InnerXml) + "\r" + "\t" + dataString;

			XmlNode objCData = base.CreateCDataSection(dataString);
			node.InnerXml = objCData.OuterXml;
		}

		//Add a hyperlink or set a hyperlink for a use element
		private void ElementHyperlinkImplementation(string Name, string Id, string Hyperlink)
		{
			XmlNode node = null;
			XmlNode parent = null;

			node = base.SelectSingleNode("//" + Name + "[@id='" + Id + "']");
			if (node == null) return;

			parent = node.ParentNode;
			if (parent.Name == "a")
			{
				if (parent.Attributes.GetNamedItem("xlink:href") != null) parent.Attributes.GetNamedItem("xlink:href").Value = Hyperlink;
			}
			else
			{
				XmlElement newElement = base.CreateElement("a");
				newElement.SetAttribute("href", "http://www.w3.org/1999/xlink", Hyperlink);
				newElement.InnerXml = node.OuterXml;

				parent.ReplaceChild(newElement, node);
			}
		}
		
		#endregion
	}
}