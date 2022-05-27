using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.draw
{
    public class Node
    {
        

        public Panel Content { get; set; }

        public Label Data { get; set; }

        public Label Weight { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Point RootPos { get; set; }

        public NodeForTree NodeTree { get; set; }

        public int TopX { get; set; }

        public int TopY { get; set; }

        public Node()
        {
            this.Content = new Panel();
            this.Data = new Label();
            this.Data.Width = 20;
            this.Data.Height = 10;
            this.Weight = new Label();
            this.Weight.Width = 20;
            this.Weight.Height = 10;
            this.NodeTree = new NodeForTree();
            this.RootPos = new Point();
            this.Width = 40;
            this.Height = 10;
            this.TopX = 10;
            this.TopY = 0;

        }


        public Node(NodeForTree _Node)
        {
            this.Content = new Panel();
            this.Data = new Label();
            this.Data.Width = 40;
            this.Data.Height = 20;
            this.Weight = new Label();
            this.Weight.Width = 40;
            this.Weight.Height = 20;
            this.Weight.Location = new Point( 40, 0);

            this.NodeTree = new NodeForTree();
            this.RootPos = new Point();
            this.Width = 80;
            this.Height = 20;
            this.TopX = 20;
            this.TopY = 0;

            this.NodeTree = _Node;
            this.Data.Text = this.NodeTree.GetData();
            this.Weight.Text = this.NodeTree.GetFrequency().ToString();

            this.Content.Width = this.Data.Width + this.Weight.Width;
            this.Content.Height = this.Data.Height;

            this.Content.Controls.Add(this.Data);
            this.Content.Controls.Add(this.Weight);
            
        }

        public Node(Panel contentLeft, Panel contentRight, Panel content)
        {
            this.Content = new Panel();
            this.Content.Controls.Add(content);
        }

        public Node(Node _Left, Node _Right, Node Root)
        {
            this.Content = new Panel();

            _Left.Content.Location = new Point(0, 40);
            _Left.Content.BackColor = Color.Blue;

            _Right.Content.Location = new Point(_Left.Width + 20, 40);

            _Right.Content.BackColor = Color.Red;

            if (_Left.TopX > _Right.TopX)
            {
                this.TopX = _Left.TopX - 20;
            }
            else
            {
                this.TopX = _Right.TopX - 20;
            }

            // Lay toa do cua node root

            this.RootPos = new Point((this.TopX + _Left.Width + _Right.Width) / 2, 0);

            // Ve thang root

            Root.Content.Location = this.RootPos;
            Root.Content.BackColor = Color.PaleGreen;

            this.Content.Controls.Add(_Left.Content);

            this.Content.Controls.Add(_Right.Content);

            this.Content.Controls.Add(Root.Content);

        }
    }
}
