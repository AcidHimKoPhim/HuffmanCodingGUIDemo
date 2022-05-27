using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.draw
{
    public class NodeLabel
    {

        public Panel Content { get; set; }

        public Label Data { get; set; }

        public Label Weight { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Point RootPos { get; set; }
        
        public NodeForTree Node { get; set; }

        public int TopX { get; set; }

        public int TopY { get; set; }

        public NodeLabel()
        {
            this.Content = new Panel();
            this.Data = new Label();
            this.Data.Width = 20;
            this.Data.Height = 10;
            this.Weight = new Label();
            this.Weight.Width = 20;
            this.Weight.Height = 10;
            this.Node = new NodeForTree();
            this.RootPos = new Point();
            this.Width = 40;
            this.Height = 10;
            this.TopX = 10;
            this.TopY = 0;

        }

        // neu no la node la
        public NodeLabel(NodeForTree _Node)
        {
            this.Node = _Node;

        }

        public NodeLabel(Node _Left, Node _Right, Node Root)
        {


            if (_Left != null && _Right != null)
            {

                // kiem tra chieu cao va chieu dai cua 2 thang.
                // Set khoang cach mac dinh la 20 

                
            }
            else
            {
                this.Data.Text = this.Node.GetData();

                this.Weight.Text = this.Node.GetFrequency().ToString();

                this.Content.Controls.Add(this.Data);
                this.Content.Controls.Add(this.Weight);
            }





        }

    }
}
