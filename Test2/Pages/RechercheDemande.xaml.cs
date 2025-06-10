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

            //  données de test
            var vin1 = new Vin("Château Margaux", 1, 0, 2018, 34.50, 1, 1, "Grand vin de Bordeaux.");
            var vin2 = new Vin("Esporão Reserva", 2, 0, 2020, 19.90, 2, 2, "Vin portugais.");
            ToutesLesDemandes = new List<Demande>
            {
                new Demande(101, vin1.Reference, 1, DateTime.Now.AddDays(-1), 2, Etat.Attente),
                new Demande(102, vin2.Reference, 1, DateTime.Now.AddDays(-3), 4, Etat.Valider),
                new Demande(103, vin1.Reference, 2, DateTime.Now.AddDays(-7), 1, Etat.Supprimer),
            };

            DemandesFiltres = ToutesLesDemandes.OrderBy(d => d.DateDemande).ToList();
            this.DataContext = this;
            IC_Demandes.ItemsSource = DemandesFiltres;
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtre = TxtRecherche.Text?.ToLower() ?? "";

            DemandesFiltres = ToutesLesDemandes.Where(d => d.Vin != null &&  (d.Vin.Nom.ToLower().Contains(filtre) || d.Vin.Detail.ToLower().Contains(filtre))).ToList();

            IC_Demandes.ItemsSource = DemandesFiltres;
        }

        private void Filtrer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Filtrage par statut ou date à implémenter.");
        }

        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            var vin = (sender as Button)?.Tag as Vin;
            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.MainContent.Content = new FicheVin(vin);
            }
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
