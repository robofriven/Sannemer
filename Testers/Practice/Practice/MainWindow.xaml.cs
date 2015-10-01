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

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Test test = new Test();
        public MainWindow()
        {
            InitializeComponent();
            grid.DataContext = test;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
        }
    }

    public class Test
    {
        public string var1 { get; set; }
        public string var2 { get; set; }

        public Test(string var1, string var2)
        {
            this.var1 = var1;
            this.var2 = var2;
        }

        public Test() : this("No input", "Still no input")
        {  }



    }
}
