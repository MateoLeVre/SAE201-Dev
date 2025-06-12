using AppliNicolas.Classes;
using AppliNicolas.Fenetre;
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
    public partial class FicheVin : UserControl
    {
        public Vin Vin { get; set; }
        public List<Vin> Similaires { get; set; }

        public event Action<Vin> VinSelectionne;
        public FicheVin(Vin vin)
        {
            InitializeComponent();

            Vin = vin;
            Similaires = vin.TrouverSimilaires();

            this.DataContext = this;
        }

        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Vin vinClique = btn?.Tag as Vin;

            IC_Similaires.ItemsSource = Similaires;

            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vinClique));

        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).RevenirEnArriere();
        }

        private void BtnCreerDemande_Click(object sender, RoutedEventArgs e)
        {
            var fenetre = new FicheCreationDemande(this.Vin); 
            fenetre.ShowDialog();
        }

    }

}
