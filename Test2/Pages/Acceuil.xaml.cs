using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class Acceuil : UserControl
    {
        public string MessageBienvenue { get; set; }
        public string DateConnexion { get; set; }
        public List<Demande> DemandesEmploye { get; set; }

        private Employe employe;
        public ObservableCollection<VinRecherche> HistoriqueRecherches { get; set; }

        public Acceuil()
        {
            InitializeComponent();

            // Récupérer employé connecté
            employe = ((MainWindow)Application.Current.MainWindow).EmployeConnecte;

            // Message d’accueil personnalisé
            MessageBienvenue = $"Bonjour {employe.Prenom} {employe.Nom}, bienvenue dans le système de gestion des vins Nicolas.";
            DateConnexion = $"Connexion le {DateTime.Now:dd MMMM yyyy à HH:mm}";

            // Récupération des demandes de l'employé (non validées)
            ObservableCollection<Demande> toutesLesDemandes = ((MainWindow)Application.Current.MainWindow).GestionVin.LesDemandes;
            DemandesEmploye = toutesLesDemandes
                                .Where(d => d.NumEmploye == employe.NumEmploye)
                                .OrderByDescending(d => d.DateDemande)
                                .ToList();

            // Historique des recherches
            HistoriqueRecherches = new ObservableCollection<VinRecherche>(
                                    employe.HistoriqueRecherches
                                           .OrderByDescending(r => r.HeureRecherche)
                                           .Take(5) );


            // Affectation du contexte de données après initialisation
            this.DataContext = this;
        }

        private void VoirDemande_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Demande d)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheDemande(d));
            }
        }

        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Vin vin)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vin));
            }
        }
    }
}
