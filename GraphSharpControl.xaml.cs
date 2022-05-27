using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using QuickGraph;
using GraphSharp;
using GraphSharp.Controls;
using QuickGraph.Serialization;
using System.Collections;
using System.Threading;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Interaction logic for GraphSharpControl.xaml
    /// </summary>
    public partial class GraphSharpControl : UserControl
    {
             BidirectionalGraph<object, IEdge<object>> g = new BidirectionalGraph<object, IEdge<object>>();


        public GraphSharpControl()
        {
            InitializeComponent();
            layout.LayoutAlgorithmType = "EfficientSugiyama";
            layout.OverlapRemovalConstraint = AlgorithmConstraints.Automatic;
            layout.OverlapRemovalAlgorithmType = "FSA";
            layout.HighlightAlgorithmType = "Simple";

            

            //g.AddVertex("h");

            //g.AddVertex("t");

             //g.AddVertex("k");

             //g.AddVertex("l");

             //g.AddEdge(new Edge<object>("h", "t"));
             //g.AddEdge(new Edge<object>("h", "k"));
             //g.AddEdge(new Edge<object>("k", "l"));

             //l.LayoutAlgorithmType = "LinLog";
             //l.HighlightAlgorithmType = "Simple";
             //l.Graph = g;


            //layout.LayoutMode = LayoutMode.Automatic;


            
        }

        public void AddNode(String s)
        {
            g.AddVertex(s);
        }



        public void AddEdgeForVertex(String str, String end)
        {
            g.AddVertex(str);

            g.AddVertex(end);
            g.AddEdge(new Edge<object>(str, end));
            layout.Graph = g;
            //Thread.

        }


        void threadAddEdgeForVertex(String str, String end)
        {
            g.AddEdge(new Edge<object>(str, end));
        }



        private void Relayout_Click(object sender, RoutedEventArgs e)
        {
            layout.Relayout();
        }

    }
}
