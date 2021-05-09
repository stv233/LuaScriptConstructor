﻿using System;
using System.Collections.Generic;

namespace LuaScriptConstructor.Types
{
    /// <summary>
    /// Lua function.
    /// </summary>
    class Function : Variable, Saves.IConstructorSerializable
    {
        /// <summary>
        /// Represents a class containing data for a <see cref="BuildComplite"/> and <see cref="BuildError"/> event.
        /// </summary>
        public class FunctionBuildEventArgs : EventArgs
        {
            /// <summary>
            /// Build warnings.
            /// </summary>
            public List<string> Warnings { get; protected set; }

            /// <summary>
            /// Build error.
            /// </summary>
            public Exception Error { get; protected set; }

            /// <summary>
            /// Represents a class containing data for a <see cref="BuildComplite"/> and <see cref="BuildError"/> event.
            /// </summary>
            /// <param name="error">Build error</param>
            public FunctionBuildEventArgs(Exception error) { Error = error; Warnings = new List<string>(); }

            /// <summary>
            /// Represents a class containing data for a <see cref="BuildComplite"/> and <see cref="BuildError"/> event.
            /// </summary>
            /// <param name="warnings">Build warnings</param>
            public FunctionBuildEventArgs(List<string> warnings) { Warnings = warnings; Error = null; }

            /// <summary>
            /// Represents a class containing data for a <see cref="BuildComplite"/> and <see cref="BuildError"/> event.
            /// </summary>
            /// <param name="error">Build error</param>
            /// <param name="warnings">Build warnings</param>
            public FunctionBuildEventArgs(Exception error, List<string> warnings) { Error = error; Warnings = warnings; }

        }

        /// <summary>
        /// Represents a method that handles <see cref="BuildComplite"/> and <see cref="BuildError"/> events.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        public delegate void FunctionBuildEvents(object sender, FunctionBuildEventArgs e);

        /// <summary>
        /// Lua function types.
        /// </summary>
        public enum FuntionTypes
        {
            Main,
            Init,
            Regular
        }
        
        /// <summary>
        /// Funtion type.
        /// </summary>
        public FuntionTypes Type { get; protected set; }

        /// <summary>
        /// Function description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Function argument list.
        /// </summary>
        public List<string> Arguments { get; protected set; }

        /// <summary>
        /// Function return list.
        /// </summary>
        public List<string> Returns { get; protected set; }

        /// <summary>
        /// Function diagram.
        /// </summary>
        public virtual Forms.ConstructorDiagram Diagram { get; set; }

        /// <summary>
        /// Funtion table.
        /// </summary>
        new public virtual Shapes.ConstructorTable Table
        {
            get
            {
                if (this is ProgrammaticallyDefinedFunction)
                {
                    return (this as ProgrammaticallyDefinedFunction).Table;
                }
                
                List<string> warnings = new List<string>();
                return BuildTable(Diagram, ref warnings);
            }
        }

        /// <summary>
        /// Function code.
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// Function value.
        /// </summary>
        public override string Value
        {
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// Function value interaction type.
        /// </summary>
        public override ValueInteractionTypes InteractionType
        {
            get
            {
                return ValueInteractionTypes.GetSet;
            }
        }

        /// <summary>
        /// Occurs when the assembly of the function is complete.
        /// </summary>
        public FunctionBuildEvents BuildComplite;

        /// <summary>
        /// Occurs when the assembly of a function fails.
        /// </summary>
        public FunctionBuildEvents BuildError;

        /// <summary>
        /// Lua function.
        /// </summary>
        protected Function() { }

        /// <summary>
        /// Lua function.
        /// </summary>
        /// <param name="name">Function name.</param>
        public Function(string name) : this(name, FuntionTypes.Regular) { }

        /// <summary>
        /// Lua function.
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="type">Function type</param>
        public Function(string name, FuntionTypes type)
        {
            Name = name;
            Type = type;
            Arguments = new List<string>();
            Returns = new List<string>();       
            BuildComplite += (s, e) => { };
            BuildError += (s, e) => { };
        }

        public void Build(Forms.ConstructorDiagram diagram)
        {
            List<string> warnings = new List<string>();

            try
            {
                BuildTable(diagram, ref warnings);
            }
            catch (Exception e)
            {
                BuildError(this, new FunctionBuildEventArgs(e, warnings));
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

            BuildComplite(this, new FunctionBuildEventArgs(warnings));
        }

        /// <summary>
        /// Build function table.
        /// </summary>
        /// <param name="diagram">Function diagramm</param>
        /// <param name="warnings">Warnings list</param>
        /// <returns></returns>
        private Shapes.ConstructorTable BuildTable(Forms.ConstructorDiagram diagram, ref List<string> warnings)
        {
            Arguments = new List<string>();
            Returns = new List<string>();

            Shapes.ConstructorTable table = new Shapes.ConstructorTable();
            table.Heading = Name;
            table.SubHeading = Description;
            table.Label = new Crainiate.Diagramming.Label(Name.Replace("_", " "));
            table.Type = Shapes.ConstructorTable.ConstructorTableTypes.Function;
            table.Size = new System.Drawing.SizeF(100f, 100f);
            table.Icon = Properties.Resources.UserFunction_16x;
            table.Function = this;
            table.SetKey(Prefix + "_" + Name + "_" + DateTime.Now.GetHashCode());

            if ((AccessType == VariableAccessTypes.Input) || (AccessType == VariableAccessTypes.InputOutput))
            {
                var input = new Crainiate.Diagramming.Port
                {
                    Direction = Crainiate.Diagramming.Direction.In,
                    Orientation = Crainiate.Diagramming.PortOrientation.Top,
                    Style = Crainiate.Diagramming.PortStyle.Default,
                    AllowMove = false
                };
                input.SetKey("input_" + Prefix + "_" + Name + "_" + DateTime.Now.GetHashCode());
                table.Ports.Add(input);
            }

            if ((AccessType == VariableAccessTypes.Output) || (AccessType == VariableAccessTypes.InputOutput))
            {
                var output = new Crainiate.Diagramming.Port
                {
                    Direction = Crainiate.Diagramming.Direction.Out,
                    Orientation = Crainiate.Diagramming.PortOrientation.Bottom,
                    Style = Crainiate.Diagramming.PortStyle.Default,
                    AllowMove = false
                };
                output.SetKey("output_" + Prefix + "_" + Name + "_" + DateTime.Now.GetHashCode());
                table.Ports.Add(output);
            }

            foreach (Shapes.ConstructorConnector connector in diagram.Connectors.Values)
            {
                try
                {
                    if (connector.StartPort.Key.Contains("functionargument"))
                    {
                        if (connector.Start.DockedElement is Shapes.ConstructorTable)
                        {
                            Arguments.Add("argument-"
                                + ((Shapes.ConstructorTable)connector.Start.DockedElement).Heading
                                + "_" + (Arguments.Count + 1).ToString());
                        }

                    }
                }
                catch (System.NullReferenceException)
                {
                    warnings.Add("Connector start is not connected to port!");
                }

                try
                {
                    if (connector.EndPort.Key.Contains("functionreturn"))
                    {
                        if (connector.End.DockedElement is Shapes.ConstructorTable)
                        {
                            Returns.Add("return-"
                                + ((Shapes.ConstructorTable)connector.End.DockedElement).Heading
                                + "_ " + (Returns.Count + 1).ToString());
                        }

                    }
                }
                catch (System.NullReferenceException)
                {
                    warnings.Add("Connector start is not connected to port!");
                }
            }

            Crainiate.Diagramming.TableGroup arguments = new Crainiate.Diagramming.TableGroup("Arguments");

            foreach (var argument in Arguments)
            {
                Crainiate.Diagramming.TableRow row = new Crainiate.Diagramming.TableRow(argument.Replace("argument-", "").Replace("_", " "));
                Crainiate.Diagramming.TablePort port = new Crainiate.Diagramming.TablePort(row);
                port.Orientation = Crainiate.Diagramming.PortOrientation.Left;
                port.Direction = Crainiate.Diagramming.Direction.In;
                port.Style = Crainiate.Diagramming.PortStyle.Input;
                port.SetKey(argument + "_" + DateTime.Now.GetHashCode());
                arguments.Rows.Add(row);
                table.Ports.Add(port);
            }
            table.Groups.Add(arguments);

            Crainiate.Diagramming.TableGroup returns = new Crainiate.Diagramming.TableGroup("Returns");

            foreach (var @return in Returns)
            {
                Crainiate.Diagramming.TableRow row = new Crainiate.Diagramming.TableRow(@return.Replace("return-", "").Replace("_", " "));
                Crainiate.Diagramming.TablePort port = new Crainiate.Diagramming.TablePort(row);
                port.Orientation = Crainiate.Diagramming.PortOrientation.Left;
                port.Direction = Crainiate.Diagramming.Direction.Out;
                port.Style = Crainiate.Diagramming.PortStyle.Output;
                port.SetKey(@return + "_" + DateTime.Now.GetHashCode());
                returns.Rows.Add(row);
                table.Ports.Add(port);
            }
            table.Groups.Add(returns);

            return table;
        }

        /// <summary>
        /// Serialize function to string.
        /// </summary>
        /// <returns>Serialized function</returns>
        public string SerializeToString()
        {
            string result = "{";
            result += "Name=∴Name=>" + Name + "<=Name∴;";
            result += "Prefix=∴Prefix=>" + Prefix + "<=Prefix∴;";
            result += "Type=" + Type + ";";
            result += "Description=∴Description=>" + Description + "<=Description∴;";
            result += "Value=∴Value=>" + Value + "<=Value∴;";
            result += "Diagram=" + Diagram.SerializeToString() + ";";
            result += "Code=∴Code=>" + Code + "<=Code∴;";
            result += "}";
            return result;
        }

        /// <summary>
        /// Deserialize function from string.
        /// </summary>
        /// <param name="serializedFunction">Serialized function</param>
        public void DeserializeFromString(string serializedFunction)
        {
            serializedFunction = serializedFunction.Substring(1, serializedFunction.Length - 2);
            while (serializedFunction.Length > 0)
            {
                int propertySign = serializedFunction.IndexOf('=');
                string propertyName = serializedFunction.Substring(0, propertySign);
                int delimiter = Saves.Saves.FindPropertyDelemiter(serializedFunction, propertySign);
                switch (propertyName)
                {
                    case "Name":
                        this.Name = (serializedFunction.Substring(propertySign + 1, delimiter - (propertySign + 1)).Replace("∴Name=>","").Replace("<=Name∴",""));
                        serializedFunction = serializedFunction.Substring(delimiter + 1);
                        break;
                    case "Prefix":
                        this.Prefix = (serializedFunction.Substring(propertySign + 1, delimiter - (propertySign + 1)).Replace("∴Prefix=>","").Replace("<=Prefix∴",""));
                        serializedFunction = serializedFunction.Substring(delimiter + 1);
                        break;
                    case "Type":
                        string type = (serializedFunction.Substring(propertySign + 1, delimiter - (propertySign + 1)));
                        switch (type)
                        {

                            case "Init":
                                this.Type = FuntionTypes.Init;
                                break;
                            case "Main":
                                this.Type = FuntionTypes.Main;
                                break;
                            case "Regular":
                                this.Type = FuntionTypes.Regular;
                                break;
                        }
                        serializedFunction = serializedFunction.Substring(delimiter + 1);
                        break;
                    case "Description":
                        this.Description = (serializedFunction.Substring(propertySign + 1, delimiter - (propertySign + 1))).Replace("∴Description=>", "").Replace("<=Description∴", "");
                        serializedFunction = serializedFunction.Substring(delimiter + 1);
                        break;
                    case "Value":
                        this.Value = (serializedFunction.Substring(propertySign + 1, delimiter - (propertySign + 1)).Replace("∴Value=>","").Replace("<=Value∴",""));
                        serializedFunction = serializedFunction.Substring(delimiter + 1);
                        break;
                    case "Diagram":
                        this.Diagram = new Forms.ConstructorDiagram();
                        this.Diagram.DeserializeFromString(serializedFunction.Substring(propertySign + 1, delimiter - (propertySign + 1)));
                        serializedFunction = serializedFunction.Substring(delimiter + 1);
                        break;
                    case "Code":
                        this.Code = (serializedFunction.Substring(propertySign + 1, delimiter - (propertySign + 1))).Replace("∴Code=>", "").Replace("<=Code∴", "");
                        serializedFunction = serializedFunction.Substring(delimiter + 1);
                        break;

                }
            }
        }
    }
}
