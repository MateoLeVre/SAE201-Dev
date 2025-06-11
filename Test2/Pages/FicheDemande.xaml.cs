using AppliNicolas.Classes;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class FicheDemande : UserControl
    {
        private Demande demande;

        public FicheDemande(Demande demande)
        {
            InitializeComponent();
            this.demande = demande;
            this.DataContext = demande;
        }

        private void VoirFicheVin_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(demande.Vin));
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).RevenirEnArriere();
        }
    }
}
