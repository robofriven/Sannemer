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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Book book1 = new Book();
        public MainWindow()
        {
            InitializeComponent();
            book1.Title = "Title #1";
            book1.Publisher = "publisher #1";
            book1.Isbn = "ISBN";
            bookGrid.DataContext = book1;
        }
    }

    public class Book
    {
        public Book (string title, string publisher, string isbn, params string[] authors)
        {
            this.title = title;
            this.publisher = publisher;
            this.isbn = isbn;
            foreach (string author in authors)
            {
                this.authors.Add(author);
            }
        }

        public Book()
            : this ("unkown", "unkown", "unkown")
        { }

        private string title;
        public string Title { get; set; }

        private string publisher;
        public string Publisher { get; set; }

        private string isbn;
        public string Isbn { get; set; }

        public override string ToString()
        {
            return title;
        }

        private readonly List<string> authors = new List<string>();
        public string[] Authors
        {
            get { return authors.ToArray(); }
        }

    }
}
