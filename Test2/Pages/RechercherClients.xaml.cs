using AppliNicolas.Classes;
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

namespace AppliNicolas.Pages
{
    /// <summary>
    /// Logique d'interaction pour RechercherClients.xaml
    /// </summary>
    public partial class RechercherClients : UserControl
    {
        public List<Client> ToutsLesClients { get; set; }
        public List<Client> ClientsFiltres { get; set; }
        public RechercherClients()
        {
            InitializeComponent();

            ToutsLesClients = new Client().RecupereClientDansBDD();

            ClientsFiltres = ToutsLesClients.OrderBy(d => d.NumClient).ToList();
            this.DataContext = this;
            IC_Client.ItemsSource = ClientsFiltres;
        }


        

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtre = TxtRecherche.Text?.ToLower() ?? "";

            /*DemandesFiltres = ToutesLesDemandes.Where(d => d.Vin != null &&  (d.Vin.Nom.ToLower().Contains(filtre) || d.Vin.Detail.ToLower().Contains(filtre))).ToList();*/

            IC_Client.ItemsSource = ClientsFiltres;
        }

        private void Filtrer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Filtrage par statut ou date à implémenter.");
        }

        

        private void VoirClient_Click(object sender, RoutedEventArgs e)
        {
            var client = (sender as Button)?.Tag as Client;
            if (client != null)
            {
                /*((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheClient(client));*/
            }
        }
    }
}
