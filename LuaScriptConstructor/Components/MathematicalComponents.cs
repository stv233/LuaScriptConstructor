﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaScriptConstructor.Components
{
    /// <summary>
    /// Represents math components.
    /// </summary>
    static class MathematicalComponents
    {
        /// <summary>
        /// Constants.
        /// </summary>
        public static List<Types.Constant> Constants { get { return new List<Types.Constant>(); } }

        /// <summary>
        /// Variables.
        /// </summary>
        public static List<Types.Variable> Variables { get { return new List<Types.Variable>(); } }


        /// <summary>
        /// Functions.
        /// </summary>
        public static List<Types.Function> Functions
        {
            get
            {
                List<Types.Function> functions = new List<Types.Function>();

                #region /// Addition

                var addition = new Types.ProgrammaticallyDefinedFunction
                {
                    Name = "Addition (+)",
                    Prefix = "addition",
                    Type = Types.Function.FuntionTypes.Regular,
                    Identifier = new Properties.Settings().LibraryName + ".Addition",
                    IsLibraryFunction = true,
                    NeedRebuild = true,
                    Diagram = new Forms.ConstructorDiagram(),
                    AccessType = Types.Variable.VariableAccessTypes.InputOutput
                };
                addition.Diagram.DeserializeFromString(System.Text.Encoding.UTF8.GetString(Properties.Resources._2in1out));
                addition.TableRebuilding += (object s, Types.ProgrammaticallyDefinedFunction.RebuildEventArgs e) =>
                {
                    e.Table.Icon = Properties.Resources.Calculator_16x;
                    e.Table.Type = Shapes.ConstructorTable.ConstructorTableTypes.Function;
                    e.Table.Label = new Crainiate.Diagramming.Label("Addition (+)");
                    e.Table.Function = addition;
                    e.Table.Heading = addition.Name;
                    e.Table.GradientColor = System.Drawing.Color.White;
                    e.Table.SubHeading = "Adding two values";

                    foreach (Crainiate.Diagramming.TableGroup group in e.Table.Groups)
                    {
                        group.Text = group.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        foreach (Crainiate.Diagramming.TableRow row in group.Rows)
                        {
                            row.Text = row.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        }
                    }
                };

                functions.Add(addition);

                #endregion

                #region /// Subtraction

                var subtraction = new Types.ProgrammaticallyDefinedFunction
                {
                    Name = "Subtraction (-)",
                    Prefix = "subtraction",
                    Identifier = new Properties.Settings().LibraryName + ".Subtraction",
                    IsLibraryFunction = true,
                    NeedRebuild = true,
                    Diagram = new Forms.ConstructorDiagram(),
                    Type = Types.Function.FuntionTypes.Regular,
                    AccessType = Types.Variable.VariableAccessTypes.InputOutput
                };
                subtraction.Diagram.DeserializeFromString(System.Text.Encoding.UTF8.GetString(Properties.Resources._2in1out));
                subtraction.TableRebuilding += (object s, Types.ProgrammaticallyDefinedFunction.RebuildEventArgs e) =>
                {
                    e.Table.Icon = Properties.Resources.Calculator_16x;
                    e.Table.Type = Shapes.ConstructorTable.ConstructorTableTypes.Function;
                    e.Table.Label = new Crainiate.Diagramming.Label("Subtraction (-)");
                    e.Table.Function = subtraction;
                    e.Table.Heading = subtraction.Name;
                    e.Table.GradientColor = System.Drawing.Color.White;
                    e.Table.SubHeading = "Subtracts the second value from the first";

                    foreach (Crainiate.Diagramming.TableGroup group in e.Table.Groups)
                    {
                        group.Text = group.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        foreach (Crainiate.Diagramming.TableRow row in group.Rows)
                        {
                            row.Text = row.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        }
                    }
                };

                functions.Add(subtraction);


                #endregion

                #region /// Multiplication

                var multiplication = new Types.ProgrammaticallyDefinedFunction
                {
                    Name = "Multiplication (*)",
                    Prefix = "multiplication",
                    Identifier = new Properties.Settings().LibraryName + ".Multiplication",
                    IsLibraryFunction = true,
                    NeedRebuild = true,
                    Diagram = new Forms.ConstructorDiagram(),
                    Type = Types.Function.FuntionTypes.Regular,
                    AccessType = Types.Variable.VariableAccessTypes.InputOutput
                };
                multiplication.Diagram.DeserializeFromString(System.Text.Encoding.UTF8.GetString(Properties.Resources._2in1out));
                multiplication.TableRebuilding += (object s, Types.ProgrammaticallyDefinedFunction.RebuildEventArgs e) =>
                {
                    e.Table.Icon = Properties.Resources.Calculator_16x;
                    e.Table.Type = Shapes.ConstructorTable.ConstructorTableTypes.Function;
                    e.Table.Label = new Crainiate.Diagramming.Label("Multiplication (*)");
                    e.Table.Function = multiplication;
                    e.Table.Heading = multiplication.Name;
                    e.Table.GradientColor = System.Drawing.Color.White;
                    e.Table.SubHeading = "Multiplies two values";

                    foreach (Crainiate.Diagramming.TableGroup group in e.Table.Groups)
                    {
                        group.Text = group.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        foreach (Crainiate.Diagramming.TableRow row in group.Rows)
                        {
                            row.Text = row.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        }
                    }
                };

                functions.Add(multiplication);


                #endregion

                #region /// Division

                var division = new Types.ProgrammaticallyDefinedFunction
                {
                    Name = "Division (/)",
                    Prefix = "division",
                    Identifier = new Properties.Settings().LibraryName + ".Division",
                    IsLibraryFunction = true,
                    NeedRebuild = true,
                    Diagram = new Forms.ConstructorDiagram(),
                    Type = Types.Function.FuntionTypes.Regular,
                    AccessType = Types.Variable.VariableAccessTypes.InputOutput
                };
                division.Diagram.DeserializeFromString(System.Text.Encoding.UTF8.GetString(Properties.Resources._2in1out));
                division.TableRebuilding += (object s, Types.ProgrammaticallyDefinedFunction.RebuildEventArgs e) =>
                {
                    e.Table.Icon = Properties.Resources.Calculator_16x;
                    e.Table.Type = Shapes.ConstructorTable.ConstructorTableTypes.Function;
                    e.Table.Label = new Crainiate.Diagramming.Label("Division (/)");
                    e.Table.Function = division;
                    e.Table.Heading = division.Name;
                    e.Table.GradientColor = System.Drawing.Color.White;
                    e.Table.SubHeading = "Divides the first value by the second";

                    foreach (Crainiate.Diagramming.TableGroup group in e.Table.Groups)
                    {
                        group.Text = group.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        foreach (Crainiate.Diagramming.TableRow row in group.Rows)
                        {
                            row.Text = row.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        }
                    }
                };

                functions.Add(division);


                #endregion

                #region /// Modulo

                var modulo = new Types.ProgrammaticallyDefinedFunction
                {
                    Name = "Modulo (%)",
                    Prefix = "modulo",
                    Identifier = new Properties.Settings().LibraryName + ".Modulo",
                    IsLibraryFunction = true,
                    NeedRebuild = true,
                    Diagram = new Forms.ConstructorDiagram(),
                    Type = Types.Function.FuntionTypes.Regular,
                    AccessType = Types.Variable.VariableAccessTypes.InputOutput
                };
                modulo.Diagram.DeserializeFromString(System.Text.Encoding.UTF8.GetString(Properties.Resources._2in1out));
                modulo.TableRebuilding += (object s, Types.ProgrammaticallyDefinedFunction.RebuildEventArgs e) =>
                {
                    e.Table.Icon = Properties.Resources.Calculator_16x;
                    e.Table.Type = Shapes.ConstructorTable.ConstructorTableTypes.Function;
                    e.Table.Label = new Crainiate.Diagramming.Label("Modulo (/)");
                    e.Table.Function = modulo;
                    e.Table.Heading = modulo.Name;
                    e.Table.GradientColor = System.Drawing.Color.White;
                    e.Table.SubHeading = "Divides the first value by the second modulo";

                    foreach (Crainiate.Diagramming.TableGroup group in e.Table.Groups)
                    {
                        group.Text = group.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        foreach (Crainiate.Diagramming.TableRow row in group.Rows)
                        {
                            row.Text = row.Text.Replace("Argument", "Value").Replace("Return", "Result");
                        }
                    }
                };

                functions.Add(modulo);

                #endregion

                return functions;
            }
        }
    }
}