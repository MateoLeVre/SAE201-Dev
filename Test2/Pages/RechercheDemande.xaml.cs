using AppliNicolas.Classes;
using AppliNicolas.ClassesD_exmple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class RechercheDemande : UserControl
    {
        public List<Demande> ToutesLesDemandes { get; set; }
        public List<Demande> DemandesFiltres { get; set; }

        public RechercheDemande()
        {
            InitializeComponent();

            ToutesLesDemandes =new Demande().RecupereDemandeDansBDD();        

            DemandesFiltres = ToutesLesDemandes.OrderBy(d => d.DateDemande).ToList();
            this.DataContext = this;
            IC_Demandes.ItemsSource = DemandesFiltres;
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtre = TxtRecherche.Text?.ToLower() ?? "";

            /*DemandesFiltres = ToutesLesDemandes.Where(d => d.Vin != null &&  (d.Vin.Nom.ToLower().Contains(filtre) || d.Vin.Detail.ToLower().Contains(filtre))).ToList();*/

            IC_Demandes.ItemsSource = DemandesFiltres;
        }

        private void Filtrer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Filtrage par statut ou date à implémenter.");
        }

        

        private void VoirDemande_Click(object sender, RoutedEventArgs e)
        {
            var demande = (sender as Button)?.Tag as Demande;
            if (demande != null)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheDemande(demande));
            }
        }

    }
}
