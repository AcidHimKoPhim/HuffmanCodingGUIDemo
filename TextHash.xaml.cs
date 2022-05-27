using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WindowsFormsApplication1
{
    /// <summary>
    /// Interaction logic for TextHash.xaml
    /// </summary>
    public partial class TextHash : UserControl
    {
        public TextHash()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            textBox.Children.Clear();
        }
        public void AddText(String key,  String value)
        {
            Rectangle rec = new Rectangle();
            rec.Width = 20;
            TextBlock t1 = new TextBlock();
            t1.Width = 20;
            t1.Text = key;
            TextBlock t2 = new TextBlock();

            t2.Width = 70;
            t2.Text = value;

            StackPanel st = new StackPanel();
            st.Orientation = Orientation.Horizontal;

            st.Children.Add(rec);
            st.Children.Add(t1);
            st.Children.Add(t2);

            textBox.Children.Add(st);



        }
        public void AddTextWriline(String text)
        {

        }
    }
}
