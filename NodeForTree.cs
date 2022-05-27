using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class NodeForTree
    {
        string Data;
        int frequency;
        string code;
        NodeForTree leftChild;
        NodeForTree rightChild;

        
        public NodeForTree()
        {
            Data = null;
            frequency = 0;
            code = "-1";
            leftChild = null;
            rightChild = null;
        }

        public NodeForTree(NodeForTree p)
        {
            Data = p.Data;
            frequency = p.frequency;
            code = p.code;
            leftChild = p.leftChild;
            rightChild = p.rightChild;
        }
        public void CreateNode(string _data, int _fre, string _code, NodeForTree _l, NodeForTree _r)
        {
            Data = _data;
            frequency = _fre;
            code = _code;
            leftChild = _l;
            rightChild = _r;
        }

        public string GetData()
        {
            return Data;
        }
        public int GetFrequency()
        {
            return frequency;
        }
        public string GetCode()
        {
            return code;
        }
        public NodeForTree GetLeftChild()
        {
            return leftChild;
        }
        public NodeForTree GetRightChild()
        {
            return rightChild;
        }
        public void SetData(string s)
        {
            Data = s;
        }
        public void SetFrequency(int n)
        {
            frequency = n;
        }
        public void SetCode(string n)
        {
            code = n;
        }
        public void SetLeftChild(NodeForTree lef)
        {
            leftChild = lef;
        }
        public void SetRightChild(NodeForTree right)
        {
            rightChild = right;
        }
    }

    public class Tree
    {
        NodeForTree root;
        int count;

        public Tree() {root= new NodeForTree(); count = 0; }
        public Tree(NodeForTree p)
        {

            root = p;
            count = 1;
        }

        public Tree(Tree p)
        {
            root = p.GetRoot();
            count = p.GetCount();
        }

        public NodeForTree GetRoot()
        {
            return root;
        }
       
        public int GetCount()
        {
            return count;
        }

        public Tree CreateTree(ref Tree left, ref Tree right)
        {
            string s = left.GetRoot().GetData() + right.GetRoot().GetData();
            root.SetData(s);
            root.SetFrequency(left.GetRoot().GetFrequency() + right.GetRoot().GetFrequency());
            root.SetLeftChild(left.GetRoot()); //lef has frequen > righ.frequen 
            root.SetRightChild(right.GetRoot());
            count += (left.GetCount() + right.GetCount());
            left.GetRoot().SetCode("0");
            right.GetRoot().SetCode("1");
            return this;
        }
    }
}