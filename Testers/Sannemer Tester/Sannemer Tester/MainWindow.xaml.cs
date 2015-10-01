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

namespace Sannemer_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Deck deck = new Deck();

        public MainWindow()
        {
            InitializeComponent();
            Grid.DataContext = deck;

        }
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            deck.BuildDecks();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Deck
    {
        public int[] attack { get; set; } = new int[60];
        public int[] defense { get; set; } = new int[60];

        List<Vector> deck1 = new List<Vector>();
        List<Vector> deck2 = new List<Vector>();

        public void BuildDecks()
        {
            for (int i = 0; i < 30; i++)
            {
                deck1.Add(new Vector(attack[i], defense[i]));
            }

            for (int i = 30; i < 60; i++)
            {
                deck2.Add(new Vector(attack[i], defense[i]));
            }
        }
    }

    public class Play
    {
        public static void PlayGames(List<Vector> decka, List<Vector> deckb, int numOfGames)
        {
            List<Vector> deck1 = new List<Vector>();
            List<Vector> deck2 = new List<Vector>();
            Random rand = new Random();

            // Shuffles the decks
            for (int i = 0; i < 30; i++)
            {
                deck1.Add(decka[rand.Next(0, decka.Count)]);
                deck2.Add(deckb[rand.Next(0, deckb.Count)]);
            }
        }

        private void FindResult(List<Vector> deck1, List<Vector> deck2)
        {
            List<Vector> hand1 = new List<Vector>();
            List<Vector> hand2 = new List<Vector>();

            int health1 = 20;
            int health2 = 20;

            // deal out the hands
            for (int i = 0; i < 6; i++)
            {
                hand1.Add(deck1[0]);
                hand2.Add(deck2[0]);

                deck1.RemoveAt(0);
                deck2.RemoveAt(0);
            }



        }
    }
}
