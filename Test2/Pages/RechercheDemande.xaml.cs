using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            ToutesLesDemandes = ((MainWindow)Application.Current.MainWindow).GestionVin.LesDemandes.ToList();        

            DemandesFiltres = ToutesLesDemandes.OrderBy(d => d.DateDemande).ToList();
            this.DataContext = this;
            IC_Demandes.ItemsSource = DemandesFiltres;
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtre = TxtRecherche.Text?.ToLower() ?? "";

            DemandesFiltres = ToutesLesDemandes.Where(d => d.Vin != null && (d.Vin.Nom.ToLower().Contains(filtre) || d.DateDemandeFormatted.ToLower().Contains(filtre)
            || d.Etat.ToLower().Contains(filtre)
            )).ToList();

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

        private void VoirVin_Click(object sender, RoutedEventArgs e)
        {
            Vin vin = (sender as Button)?.Tag as Vin;

            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vin));
        }

        private void Rafraichir_click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).GestionVin.LesDemandes = new ObservableCollection<Demande>(new Demande().RecupereDemandeDansBDD());
            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercheDemande());
        }
    }
}
