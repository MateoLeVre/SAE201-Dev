using AppliNicolas.Classes;
using AppliNicolas.ClassesD_exmple;
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

        public FicheVin(Vin vin)
        {
            InitializeComponent();

            Vin = vin;
            Similaires = CatalogueDeVin.TrouverSimilaires(vin);

            this.DataContext = this; 
        }

        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Vin vinClique = btn?.Tag as Vin;
            if (vinClique != null)
            {
                var nouvelleFiche = new FicheVin(vinClique);
            }
        }
    }

}
