using AppliNicolas.Classes;
using AppliNicolas.ClassesD_exmple;
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

            TousLesVins = CatalogueDeVin.ObtenirExemples()
                                        .OrderBy(v => v.Nom)
                                        .ToList();

            VinsFiltres = new List<Vin>(TousLesVins);
            DataContext = this;
            IC_Vins.ItemsSource = VinsFiltres;
        }


        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtRecherche?.Text == null)
                return ;

            string filtre = TxtRecherche.Text.ToLower();
            /*
                        VinsFiltres = TousLesVins.Where(v => v.Nom.ToLower().Contains(filtre) || v.Appelation.ToLower().Contains(filtre)).OrderBy(v => v.Nom).ToList();*/
            VinsFiltres = TousLesVins.Where(v => v.Nom.ToLower().Contains(filtre)).OrderBy(v => v.Nom).ToList();
            IC_Vins.ItemsSource = VinsFiltres;
            return ;
        }


        

        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            var vin = (sender as Button)?.Tag as Vin;
            if (vin != null)
            {
                Window mainWindow = Application.Current.MainWindow;
                if (mainWindow is MainWindow mw)
                    mw.MainContent.Content = new FicheVin(vin);
            }
        }
    }
}
