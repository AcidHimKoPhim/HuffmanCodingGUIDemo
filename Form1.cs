using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using WindowsFormsApplication1;
using System.Threading;
using QuickGraph;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        StreamReader sr;
        String text_inp;
        Hashtable hashtable_inp;
        Hashtable CompressedCode; // caculate compressed code // input compressed code
       
        string filename;
        ArrayList AL_NodesForTree;
        Tree HuffmanTree;
        StreamReader sr_decodedcode; //input file with compressed code
        StreamReader sr_text_encoded;
        string compressedstringinput ="";
        Hashtable CompressedCodeInput;

        int sleepTime = 3000;
        int numberofaddbit = 0;
        Thread tid1 = null;
        Thread tid2 = null;

        GraphSharpControl graphControl = new GraphSharpControl();
        TextHash textControl = new TextHash();
        String directory = Directory.GetCurrentDirectory();
        public Form1()
        {
            InitializeComponent();

            elementHost1.Child = graphControl;

            elementHost2.Child = textControl;

        }

        void timer_tick(object sender, EventArgs e)
        {

        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                graphControl = new GraphSharpControl();

                elementHost1.Child = graphControl;
                filename = openFileDialog1.FileName;
            }
        }

        void IS_AL(ref ArrayList AL)//interchange sort arraylist
        {
            int i, j;
            for (i = 0; i < AL_NodesForTree.Count - 1; i++)
            {
                Tree A = new Tree((Tree)AL_NodesForTree[i]);
                for (j = i + 1; j < AL_NodesForTree.Count; j++)
                {                    
                    Tree  B = new Tree((Tree) AL_NodesForTree[j]);
                    if (A.GetRoot().GetFrequency() < B.GetRoot().GetFrequency())                    
                    {
                        Tree temp = new Tree(B);
                        B = A;
                        A = temp;
                        //change AL_node[i] by a new A

                        AL_NodesForTree[i] = A;
                        AL_NodesForTree[j] = B;
                    }
                    else if (A.GetRoot().GetFrequency() == B.GetRoot().GetFrequency())
                    {
                        if (A.GetRoot().GetData()[0] > B.GetRoot().GetData()[0])
                        {
                            Tree temp = new Tree(B);
                            B = A;
                            A = temp;
                            //change AL_node[i] by a new A

                            AL_NodesForTree[i] = A; 
                            AL_NodesForTree[j] = B;
                        };
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)//compressed
        {
            sleepTime = 1000;
            CalcHuffman();
        }

        public async void CalcHuffman()
        {
            sr = new StreamReader(filename);
            text_inp = sr.ReadToEnd();
            hashtable_inp = new Hashtable();
            int i = 0, s = 0;

            //giai đoạn 1: phân tách các kí tự trong chuỗi thành bảng băm
            while (i < text_inp.Length)
            {
                if (text_inp[i] != '\r')
                {
                    if (hashtable_inp.ContainsKey(text_inp[i].ToString()) == false)
                        hashtable_inp.Add(text_inp[i].ToString(), 1);
                    else
                    {
                        int a = int.Parse((hashtable_inp[text_inp[i].ToString()]).ToString()) + 1;
                        hashtable_inp[text_inp[i].ToString()] = a;
                    }
                    i++;
                }
                else
                {
                    string temp = "\r\n";
                    if (hashtable_inp.ContainsKey(temp) == false)
                        hashtable_inp.Add(temp, 1);
                    else
                    {
                        int a = (int)hashtable_inp[temp] + 1;
                        hashtable_inp[temp] = a;
                    }
                    i += 2;
                }

            }
            AL_NodesForTree = new ArrayList();
         

            tid1 = new Thread(new ThreadStart(threadCreateHashtable));
            tid1.Start();

        }

        public void threadCreateHashtable()
        {
            ICollection key = hashtable_inp.Keys;
            int i = 0;
            foreach (string k in key)
            {
                NodeForTree p = new NodeForTree();
                p.CreateNode(k.ToString(), (int)hashtable_inp[k], "-1", null, null);
                textControl.Dispatcher.Invoke(new Action(() =>
                {
                    string temp = k;
                    if (k == "\n") temp = "\r\n";
                    String t2 = ((int)hashtable_inp[k]).ToString();

                    textControl.AddText(k.ToString(), t2);
                }));
               // Thread.Sleep(sleepTime);
                Tree T = new Tree(p);
                AL_NodesForTree.Insert(i, T);
                i++;
            }
            tid2 = new Thread(new ThreadStart(threadCreateTree));
            tid2.Start();
            tid1.Abort();
        }

        public void threadCreateTree()
        {
            IS_AL(ref AL_NodesForTree);//sort
            while (AL_NodesForTree.Count >= 2)
            {               
                Tree p = (Tree)AL_NodesForTree[AL_NodesForTree.Count - 1];
                Tree q = (Tree)AL_NodesForTree[AL_NodesForTree.Count - 2];
                Tree newtree = new Tree();
                newtree.CreateTree(ref p, ref q);

                graphControl.Dispatcher.Invoke(new Action(() =>
                {
                    graphControl.AddEdgeForVertex(newtree.GetRoot().GetData() + "    |    " + newtree.GetRoot().GetFrequency().ToString(), p.GetRoot().GetData() + "    |    " + p.GetRoot().GetFrequency().ToString());
                    graphControl.AddEdgeForVertex(newtree.GetRoot().GetData() + "    |    " + newtree.GetRoot().GetFrequency().ToString(), q.GetRoot().GetData() + "    |    " + q.GetRoot().GetFrequency().ToString());
                }));

                Thread.Sleep(sleepTime);
                AL_NodesForTree.RemoveAt(AL_NodesForTree.Count - 1);
                AL_NodesForTree.RemoveAt(AL_NodesForTree.Count - 1);

                if (AL_NodesForTree.Count > 0)
                {
                    int i = 0;
                    Tree CurTree = (Tree)AL_NodesForTree[i];

                    while(CurTree.GetRoot().GetFrequency() >= newtree.GetRoot().GetFrequency())
                    {
                        i++;
                        if (i == AL_NodesForTree.Count) break;
                        CurTree = (Tree)AL_NodesForTree[i];
                    }
                    AL_NodesForTree.Insert(i, newtree);   
                }
                else AL_NodesForTree.Insert(0, newtree);
            }
            HuffmanTree = (Tree)AL_NodesForTree[0];
            CreateCompressedCode();
            CreateCompressedFile();
            tid2.Abort();
        }

        public string Traverse(NodeForTree T, string leaf)
        {
            NodeForTree pT = T;
            string s="";
            while(pT != null)
            {
                if (pT.GetData() == leaf) return s;
                if(pT.GetLeftChild().GetData().Contains(leaf))
                {
                    s += "0";
                    pT = pT.GetLeftChild();
                }
                else
                {
                    s += "1";
                    pT = pT.GetRightChild();
                }
            }
            return null;
        }
        
        public void CreateCompressedCode()
        {
            CompressedCode = new Hashtable();
            ICollection keys = hashtable_inp.Keys;
            foreach (string k in keys)
            {
                string codeword = "";
                //Traverse(HuffmanTree.GetRoot(), k, ref codeword);
                codeword = Traverse(HuffmanTree.GetRoot(), k);
                CompressedCode.Add(k, codeword);
            }

            tid1 = new Thread(new ThreadStart(threadShowBinary));
            tid1.Start();
            
        }

        void threadShowBinary()
        {

            textControl.Dispatcher.Invoke(new Action(() =>
            {
                textControl.Clear();
            }));
            foreach (string key in hashtable_inp.Keys)
            {
                textControl.Dispatcher.Invoke(new Action(() =>
                {
                    textControl.AddText(key, CompressedCode[key].ToString());
                }));
            }

            tid1.Abort();
        }



        char ConvertBinaryToAsciiCode(string s)
        {
            char charresult;
            int decimalcode = 0;
            for (int i = 0; i < 8; i++)
            {
                decimalcode += int.Parse((Math.Pow(2, 7-i)).ToString()) * int.Parse(s[i].ToString());
            }
            charresult = Convert.ToChar(decimalcode);
            return charresult;
        }

        void CreateCompressedFile()
        {
            if (File.Exists(directory + @"\\CompressedCode.txt"))
                File.Delete(directory + @"\\CompressedCode.txt");
            System.IO.FileStream fs = new System.IO.FileStream(directory + @"\\CompressedCode.txt", FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            int i = 0;
            string s = "";
            //create binary file
            while(i < text_inp.Length)
            {
                if (text_inp[i].ToString() == "\r")
                {
                    s = CompressedCode["\r\n"].ToString();
                    sw.Write(s);
                    i += 2;
                }
                else
                {
                    s = CompressedCode[text_inp[i].ToString()].ToString();
                    sw.Write(s);

                    i++;
                }
            }


            sw.Flush();
            fs.Close();
            i = 0;
            if (File.Exists(directory + @"\\CompressedFile.txt"))
                File.Delete(directory + @"\\CompressedFile.txt");
            System.IO.FileStream ff = new System.IO.FileStream(directory + @"\\CompressedFile.txt", FileMode.CreateNew, FileAccess.Write);

            StreamWriter sw2 = new StreamWriter(ff,Encoding.UTF32);

            StreamReader sw1 = new StreamReader(directory + @"\\CompressedCode.txt");
            string comcode = sw1.ReadToEnd();
            int j = 0;
            s = ""; string k1 = "";
            while(j < comcode.Length)
            {
                i++;
                s += comcode[j];
                if (i == 8)
                {
                    char numDec = ConvertBinaryToAsciiCode(s);
                    k1 += numDec;
                    sw2.Write(numDec);    
                    s = "";
                    i = 0;
                }
                j++;
            }
            int themvao = 0;
            char AsCode;
            if(s!= "")
            {
                while (s.Length < 8)
                {
                    s = s.Insert(0, "0");
                    themvao++;
                }
                AsCode = ConvertBinaryToAsciiCode(s);
                sw2.Write(AsCode);
            }
            sw2.Flush();

            sw1.Close();
            StreamWriter sw3 = new StreamWriter(directory + @"\\CompressedCode.txt");
            j = 0;
            ICollection keys = CompressedCode.Keys;
            int inde = 0;
            ICollection key2s = CompressedCode.Keys;
            sw3.WriteLine(CompressedCode.Count);
            sw3.WriteLine(themvao);
            foreach ( string k in keys)
            {
                if (k == "\n") sw3.WriteLine("\r\n" + " "+ CompressedCode[k]);
                else sw3.WriteLine(k + " " + CompressedCode[k]);
                
            }
            sw3.Close();
            
            ff.Close();

        }

        public void CalcHuffman_NoDebug()
        {
            sr = new StreamReader(filename);
            text_inp = sr.ReadToEnd();
            hashtable_inp = new Hashtable();
            int i = 0, s = 0;

            while (i < text_inp.Length)
            {
                if (text_inp[i] != '\r')
                {
                    if (hashtable_inp.ContainsKey(text_inp[i].ToString()) == false)
                        hashtable_inp.Add(text_inp[i].ToString(), 1);
                    else
                    {
                        int a = int.Parse((hashtable_inp[text_inp[i].ToString()]).ToString()) + 1;
                        hashtable_inp[text_inp[i].ToString()] = a;
                    }
                    i++;
                }
                else
                {
                    string temp = "\r\n";
                    if (hashtable_inp.ContainsKey(temp) == false)
                        hashtable_inp.Add(temp, 1);
                    else
                    {
                        int a = (int)hashtable_inp[temp] + 1;
                        hashtable_inp[temp] = a;
                    }
                    i += 2;
                }

            }
            AL_NodesForTree = new ArrayList();

        }
        void CreateTree_NoDebug()
        {
            IS_AL(ref AL_NodesForTree);//sort
            while (AL_NodesForTree.Count >= 2)
            {
                Tree p = (Tree)AL_NodesForTree[AL_NodesForTree.Count - 1];
                Tree q = (Tree)AL_NodesForTree[AL_NodesForTree.Count - 2];
                Tree newtree = new Tree();
                newtree.CreateTree(ref p, ref q);

                AL_NodesForTree.RemoveAt(AL_NodesForTree.Count - 1);
                AL_NodesForTree.RemoveAt(AL_NodesForTree.Count - 1);

                if (AL_NodesForTree.Count > 0)
                {
                    int i = 0;
                    Tree CurTree = (Tree)AL_NodesForTree[i];

                    while (CurTree.GetRoot().GetFrequency() >= newtree.GetRoot().GetFrequency())
                    {
                        i++;
                        if (i == AL_NodesForTree.Count) break;
                        CurTree = (Tree)AL_NodesForTree[i];
                    }
                    AL_NodesForTree.Insert(i, newtree);
                }
                else AL_NodesForTree.Insert(0, newtree);
            }
            HuffmanTree = (Tree)AL_NodesForTree[0];

            CreateCompressedCode();
            CreateCompressedFile();
        }

        void CreateArrayTree()
        {
            ICollection key = hashtable_inp.Keys;
            int i = 0;
            foreach (string k in key)
            {
                NodeForTree p = new NodeForTree();
                p.CreateNode(k.ToString(), (int)hashtable_inp[k], "-1", null, null);
                Tree T = new Tree(p);
                AL_NodesForTree.Insert(i, T);
                i++;
            }
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            //CalcHuffman_NoDebug();
            //CreateArrayTree();
            //CreateTree_NoDebug();
            sleepTime = 0;
            CalcHuffman();

        }
        public string AsciiToBinary(string AsciiText)//chuyển từ mã ascii sang mã thập phân rồi từ thập phân sang nhị phân
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in AsciiText.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        public string HastableReturnKey(Hashtable h, string value)
        {
            string s ="";

            ICollection keys = h.Keys;
            foreach(string k in keys)
            {
                if (h[k].ToString() == value) return k.ToString();
            }
            return s;
        }

        public string stringOriginalText(Hashtable ComprTable, string s)//binary to text (result)
        {
            string index = "";
            ICollection keys = ComprTable.Keys;
            foreach(string k in keys)
            {
                index += ComprTable[k].ToString();
            }
            string original = "";
            int i, j = 0;
            string temp = "";
            if(numberofaddbit > 0)
            {
               s= s.Remove(s.Length-8, numberofaddbit);
            }
            for(i = 0; i < s.Length; i++)
            {

                temp += s[i];
                if (i == 945) { 
                    int k = 0; }
                if(ComprTable.ContainsValue(temp))
                {
                    if (HastableReturnKey(ComprTable, "\r\n") == temp) original += "\r\n";
                    else   original += HastableReturnKey(ComprTable, temp).ToString();
                    temp = "";
                }
            }
            return original;
        }
        string filename2;//open compressed file

        string filename3;//open comressed code
        private void button3_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                filename2 = openFileDialog2.FileName;
            }
           sr_text_encoded = new StreamReader(filename2);           
           compressedstringinput = sr_text_encoded.ReadToEnd();
         
           sr_text_encoded.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                filename3 = openFileDialog3.FileName;
            }
            sr_decodedcode = new StreamReader(filename3);
           
            CompressedCodeInput = new Hashtable();
           // string temp = sr_decodedcode.ReadToEnd();
            
            string s1 = sr_decodedcode.ReadLine();//read number row of hastable
            int numberOfHast = int.Parse(s1);
            s1 = sr_decodedcode.ReadLine();
            numberofaddbit = int.Parse(s1);
            s1 = sr_decodedcode.ReadLine();
            int i = 0;
            while (i < numberOfHast)
             {
                 if (s1 == "")
                 {
                     string key = "\r\n";
                     s1 = sr_decodedcode.ReadLine();
                     char space = s1[1];
                     string value = "";
                     for (int j = 1; j < s1.Length; j++)
                     {
                         value += s1[j];
                     }
                     CompressedCodeInput.Add(key, value);
                     s1 = sr_decodedcode.ReadLine();
                 }
                 else
                 {
                     string key = s1[0].ToString();
                     char space = s1[1];
                     string value = "";
                     for (int j = 2; j < s1.Length; j++)
                     {
                         value += s1[j];
                     }
                     CompressedCodeInput.Add(key, value);
                     s1 = sr_decodedcode.ReadLine();
                 }
                 i++;
             }
            sr_decodedcode.Close();
         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string originaltext = "";
            string binaryfile = AsciiToBinary(compressedstringinput);
            originaltext = stringOriginalText(CompressedCodeInput, binaryfile);

            if (File.Exists(directory + @"\\ExtractFile.txt"))
            {
                File.Delete(directory + @"\\ExtractFile.txt");
            }

            System.IO.FileStream ff = new System.IO.FileStream(directory + @"\\ExtractFile.txt", FileMode.CreateNew, FileAccess.Write);
            StreamWriter sw2 = new StreamWriter(ff);
            sw2.Write(originaltext);
            sw2.Close();
            ff.Close();
        }


        
    }
}
