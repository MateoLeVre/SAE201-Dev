using AppliNicolas.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class RechercheVin : UserControl
    {
        public List<Vin> TousLesVins { get; set; }
        public List<Vin> VinsFiltres { get; set; }

        public RechercheVin()
        {
            InitializeComponent();

            TousLesVins = ((MainWindow)Application.Current.MainWindow).GestionVin.LesVins.OrderBy(v => v.Nom).ToList();
            

            VinsFiltres = new List<Vin>(TousLesVins);
            DataContext = this;
            IC_Vins.ItemsSource = VinsFiltres;
        }


        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtRecherche?.Text == null)
                return ;

            string filtre = TxtRecherche.Text.ToLower();

            VinsFiltres = TousLesVins.Where(v => v.Nom.ToLower().Contains(filtre) || v.TypeVin.ToLower().Contains(filtre) || v.AppelationVin.ToLower().Contains(filtre)).OrderBy(v => v.Nom).ToList();

            IC_Vins.ItemsSource = VinsFiltres;
            return ;
        }




        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            Vin vin = (sender as Button)?.Tag as Vin;

            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vin));

        }

        private void Rafraichir_click(object sender, RoutedEventArgs e)
        {
            TousLesVins = new Vin().RecupereVinDansBDD()
                                        .OrderBy(v => v.Nom)
                                        .ToList();
            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercheVin());
        }
    }
}
