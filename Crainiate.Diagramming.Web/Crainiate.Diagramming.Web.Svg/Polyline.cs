// (c) Copyright Crainiate Software 2010




using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace Crainiate.Diagramming.Web.Svg
{
	public class Polyline
	{
		//Property variables
		private Link _line;
		
		#region  Interface 

		//Create a new polyline class from a line
		public Polyline(Link line)
		{
			Line = line;
		}

		//Sets or gets the line used to extract the polyline
		public virtual Link Line
		{
			get
			{
				return _line;
			}
			set
			{
				_line = value;
			}
		}

		//Extracts a polyline definition for this ERM line
		public virtual string ExtractPolyline()
		{
			return ExtractPolylineImplementation();
		}

		#endregion

		#region  Implementation 

		private string ExtractPolylineImplementation()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

			stringBuilder.Append("<polyline id=\"");
			stringBuilder.Append(_line.Key);
			stringBuilder.Append("\" class=\"\" points=\"");

			foreach (PointF point in _line.Points)
			{
                stringBuilder.Append(XmlConvert.ToString(point.X));
				stringBuilder.Append(",");
				stringBuilder.Append(XmlConvert.ToString(point.Y));
				stringBuilder.Append(" ");
			}

			stringBuilder.Append("\" ");

			//Close tag
			stringBuilder.Append("/>");

			return stringBuilder.ToString();
		}

		#endregion
	}
}