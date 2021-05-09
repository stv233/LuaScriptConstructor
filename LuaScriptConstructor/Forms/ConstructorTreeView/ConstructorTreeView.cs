﻿using System;
using System.Windows.Forms;

namespace LuaScriptConstructor.Forms.ConstructorTreeView
{
    class ConstructorTreeView : Kesoft.Windows.Forms.Win7StyleTreeView.Win7StyleTreeView
    {
        ToolTip toolTip = new ToolTip();

        private TreeNode mouseCureentNode; 

        public ConstructorTreeView() : base() {}

        /// <summary>
        /// Adds node.
        /// </summary>
        /// <param name="node">Constructor tree node</param>
        public virtual void Add(ConstructorTreeNode node) { Add("Basic", node); }

        /// <summary>
        /// Add node to category.
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="node">Constructor tree node</param>
        public virtual void Add(string category, ConstructorTreeNode node)
        {
            if (this.ImageList == null)
            {
                this.ImageList = new ImageList();
                this.ImageList.Images.Add(new System.Drawing.Bitmap(1, 1));
            }

            if (node.Image != null)
            {
                this.ImageList.Images.Add(node.Image);
                node.ImageIndex = this.ImageList.Images.Count - 1;
            }

            try
            {
                AddCategory(category);
            }
            catch { }

            Add(category, this.Nodes, node);
        }

        public static void Add(string category, TreeNodeCollection nodes, ConstructorTreeNode node)
        {
            string subCategory = "";

            if (category.Contains("\\"))
            {
                subCategory = category.Substring(0, category.IndexOf('\\') - 1);
                category = category.Substring(category.IndexOf('\\') + 1);
            }
            else
            {
                subCategory = category;
                category = "";
            }

            foreach (TreeNode categoryNode in nodes)
            {
                if (categoryNode.Name == subCategory)
                {
                    if (String.IsNullOrEmpty(category))
                    {
                        categoryNode.Nodes.Add(node);
                        return;
                    }
                    else
                    {
                        Add(category, categoryNode.Nodes, node);
                    }
                }
            }

        }

        /// <summary>
        /// Adds a category with the given name.
        /// </summary>
        /// <param name="category">Category name</param>
        public virtual void AddCategory(string category)
        {
            AddCategory(category, this.Nodes);
        }

        /// <summary>
        /// Adds a category with the given name.
        /// </summary>
        /// <param name="category">Category name</param>
        /// <param name="nodes">Parent nodes collection</param>
        public static void AddCategory(string category, TreeNodeCollection nodes)
        {
            string subCategory = "";

            if (category.Contains("\\"))
            {
                subCategory = category.Substring(0, category.IndexOf('\\') - 1);
                category = category.Substring(category.IndexOf('\\') + 1);
            }
            else
            {
                subCategory = category;
                category = "";
            }

            foreach (TreeNode categoryNode in nodes)
            {
                if ((categoryNode.Name == subCategory) && (String.IsNullOrEmpty(category)))
                {
                    throw new Exception("Tree already contains a category with the given name.");
                }
            }

            var newCategory = new TreeNode
            {
                Name = subCategory,
                Text = subCategory
            };
            nodes.Add(newCategory);

            if (!String.IsNullOrEmpty(category))
            {
                AddCategory(category, newCategory.Nodes);
            }
        }
        
        /// <summary>
        /// Remove category from tree by name.
        /// </summary>
        /// <param name="category">Category name</param>
        public virtual void RemoveCategory(string category)
        {
            RemoveCategory(category, this.Nodes);
        }

        /// <summary>
        /// Remove category from tree by name.
        /// </summary>
        /// <param name="category">Category name</param>
        /// <param name="nodes">Parent nodes collection</param>
        public static void RemoveCategory(string category, TreeNodeCollection nodes)
        {
            string subCategory = "";

            if (category.Contains("\\"))
            {
                subCategory = category.Substring(0, category.IndexOf('\\') - 1);
                category = category.Substring(category.IndexOf('\\') + 1);
            }
            else
            {
                subCategory = category;
                category = "";
            }

            foreach (TreeNode categoryNode in nodes)
            {
                if (categoryNode.Name == subCategory)
                {
                    if (String.IsNullOrEmpty(category))
                    {
                        nodes.Remove(categoryNode);
                        return;
                    }
                    else
                    {
                        RemoveCategory(category, categoryNode.Nodes);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Clear category items by category name.
        /// </summary>
        /// <param name="category">Category name</param>
        public virtual void ClearCategory(string category)
        {
            ClearCategory(category, this.Nodes);
        }

        /// <summary>
        /// Clear category items by category name.
        /// </summary>
        /// <param name="category">Category name</param>
        /// <param name="nodes">Parent nodes collection</param>
        public static void ClearCategory(string category, TreeNodeCollection nodes)
        {
            string subCategory = "";

            if (category.Contains("\\"))
            {
                subCategory = category.Substring(0, category.IndexOf('\\') - 1);
                category = category.Substring(category.IndexOf('\\') + 1);
            }
            else
            {
                subCategory = category;
                category = "";
            }

            foreach(TreeNode categoryNode in nodes)
            {
                if (categoryNode.Name == subCategory)
                {
                    if (String.IsNullOrEmpty(category))
                    {
                        categoryNode.Nodes.Clear();
                        return;
                    }
                    else
                    {
                        ClearCategory(category, categoryNode.Nodes);
                        return;
                    }
                }
            }
        }

        protected override void OnNodeMouseHover(TreeNodeMouseHoverEventArgs e)
        {
            mouseCureentNode = e.Node;
            toolTip.Hide(this);
            toolTip.Show(e.Node.ToolTipText, this);
            base.OnNodeMouseHover(e);
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            mouseCureentNode = e.Node;
            base.OnNodeMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (mouseCureentNode != null)
                {
                    if (mouseCureentNode is ConstructorTreeNode)
                    {
                        SelectedNode = SelectedNode;
                        DoDragDrop(((ConstructorTreeNode)mouseCureentNode).Table, DragDropEffects.All);
                    }
                }
            }

            base.OnMouseDown(e);
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
            base.OnBeforeSelect(e);
        }
        
    }
}