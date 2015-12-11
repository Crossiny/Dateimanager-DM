using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Dateimanager
{
    public partial class Form1 : Form
    {
        Transfer transfer = new Transfer();


        List<Button> Buttons = new List<Button>();

        int x = 0;
        int y = 0;
        int i = 0;
        string PrevPfad = null;

        public Form1()
        {
            InitializeComponent();
            showButton();
            RefreshTreeView();
        }

        private void RefreshTreeView()
        {
            foreach (string drives in Directory.GetLogicalDrives())
            {
                TreeNode node = new TreeNode();
                node.Name = drives;
                node.Text = drives;
                treeView1.Nodes.Add(node);
                RefreshTreeView(node);
            }
        }

        private void RefreshTreeView(TreeNode node)
        {
            try
            {
                foreach (string dir in Directory.GetDirectories(node.Name))
                {
                    TreeNode TNode = new TreeNode();
                    TNode.Name = dir;
                    TNode.Text = dir.Split('\\')[dir.Split('\\').Length - 1];
                    node.Nodes.Add(TNode);
                }
            }
            catch { }

        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                RefreshTreeView(node);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            toolStripLabel1.Text = e.Node.Name;
            showButton();
        }

        private void AddButton(string name, string text, Color color)
        {
            if (x + 85 > splitContainer1.Panel2.Width)
            {
                y += 85;
                x = 0;
            }

            // 
            // button(i)
            // 
            Buttons.Add(new Button());
            Buttons[i].Location = new System.Drawing.Point(3 + x, 28 + y);
            Buttons[i].Name = name;
            Buttons[i].Size = new System.Drawing.Size(75, 75);
            Buttons[i].TabIndex = 1;
            Buttons[i].Text = text;
            Buttons[i].UseVisualStyleBackColor = true;
            Buttons[i].BackColor = color;
            Buttons[i].ForeColor = Color.Black;
            Buttons[i].MouseClick += new MouseEventHandler(Click);
            //Buttons[i].MouseDoubleClick += new MouseEventHandler(DoubleClick);
            splitContainer1.Panel2.Controls.Add(Buttons[i]);
            x = x + 85;
            i++;
        }

        private void showButton()
        {
            foreach (Button b in Buttons)
            {
                b.Dispose();
            }

            x = 0;
            y = 0;
            i = 0;

            Buttons.Clear();

            foreach (String d in Directory.GetDirectories(toolStripLabel1.Text))
            {
                AddButton(d, d.Split('\\')[d.Split('\\').Length - 1], Color.Orange);
            }

            foreach (String d in Directory.GetFiles(toolStripLabel1.Text))
            {
                AddButton(d, d.Split('\\')[d.Split('\\').Length - 1], Color.YellowGreen);
            }

        }

        private new void Click(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = e;

            Button bs = (Button)sender;
            if (bs.ForeColor == Color.Black)
                bs.ForeColor = Color.White;

            else
                bs.ForeColor = Color.Black;

        }

        private new void DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("test");
            Button bs = (Button)sender;
            toolStripLabel1.Text = bs.Name;
            showButton();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            showButton();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            //AddButton("Pfad/Neuer Ordner", "Neuer Ordner");
            transfer.CreateFolder(toolStripLabel1.Text, "Neuer Ordner");
            showButton();

        }

        private void bindingNavigatorMovePreviousItem_Click_1(object sender, EventArgs e)
        {
            PrevPfad = Path.GetDirectoryName(toolStripLabel1.Text);
            if (PrevPfad == null)
            {
                toolStripLabel1.Text = @"C:\";
            }
            else
            {
                toolStripLabel1.Text = PrevPfad;
            }
            showButton();
        }

        private void neuToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void ausschneidenToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void kopierenToolStripButton_Click(object sender, EventArgs e)
        {
            System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
            foreach (Button b in Buttons)
            {
                if (b.ForeColor == Color.White)
                {
                    sc.Add(b.Name);
                }
            }
            Clipboard.SetFileDropList(sc);
        }

        private void einfügenToolStripButton_Click(object sender, EventArgs e)
        {
            System.Collections.Specialized.StringCollection sc = Clipboard.GetFileDropList();
            foreach (string b in sc)
            {
                Transfer t = new Transfer();
                t.Copy(b, toolStripLabel1.Text);
            }

            showButton();

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            foreach (Button b in Buttons)
            {
                if (b.ForeColor == Color.White)
                {
                    Transfer t = new Transfer();
                    t.Remove(b.Name);
                }
            }
            showButton();
        }
    }
}
