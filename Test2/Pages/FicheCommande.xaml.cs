using AppliNicolas.Classes;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class FicheCommande : UserControl
    {
        private Commande commande;

        public FicheCommande(Commande commande)
        {
            InitializeComponent();
            this.commande = commande;


            this.DataContext = commande;
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).RevenirEnArriere();
        }

        private void VoirVin_Click(object sender, RoutedEventArgs e)
        {
            Vin vin = (sender as Button)?.Tag as Vin;
            if (vin != null)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vin));
            }
        }
        private void VoirDemande_Click(object sender, RoutedEventArgs e)
        {
            Demande demande = (sender as Button)?.Tag as Demande;
            if (demande != null)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheDemande(demande));
            }
        }

    }
}
