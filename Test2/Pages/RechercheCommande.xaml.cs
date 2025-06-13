using AppliNicolas.Classes;
using AppliNicolas.Fenetre;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class RechercheCommande : UserControl
    {
        private List<Commande> toutesLesCommandes;
        private List<Commande> commandesFiltrees;
        private List<DetailCommande> detailCommandes;

        public List<Commande> ToutesLesCommandes
        {
            get { return toutesLesCommandes; }
            private set { toutesLesCommandes = value ?? new List<Commande>(); }
        }

        public List<Commande> CommandesFiltrees
        {
            get { return commandesFiltrees; }
            private set { commandesFiltrees = value ?? new List<Commande>(); }
        }

        public List<DetailCommande> DetailCommandes
        {
            get { return detailCommandes; }
            private set { detailCommandes = value ?? new List<DetailCommande>(); }
        }

        public RechercheCommande()
        {
            InitializeComponent();

            try
            {
                ChargerDonnees();
                ConfigurerInterface();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des commandes : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                InitialiserListesVides();
            }
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                FiltrerCommandes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtrer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Filtrage avancé à implémenter.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void VoirCommande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Commande commande = (sender as Button)?.Tag as Commande;
                if (commande == null)
                {
                    MessageBox.Show("Impossible de charger les informations de cette commande.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NaviguerVersCommande(commande);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la commande : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Rafraichir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RafraichirDonnees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du rafraîchissement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenererCommandesAuto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérification des permissions
                if (!VerifierPermissionsCreation())
                {
                    MessageBox.Show("Vous n'avez pas les permissions nécessaires pour créer une commande.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                OuvrirFenetreCreationCommande();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la fenêtre de création : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthodes dédiées pour la logique métier
        private void ChargerDonnees()
        {
            try
            {
                ToutesLesCommandes = ChargerCommandes();
                DetailCommandes = ChargerDetailsCommandes();
                CommandesFiltrees = ToutesLesCommandes.OrderBy(c => c.NumCommande).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des données : {ex.Message}");
            }
        }

        private void ConfigurerInterface()
        {
            this.DataContext = this;
            IC_Commandes.ItemsSource = CommandesFiltrees;
        }

        private void InitialiserListesVides()
        {
            ToutesLesCommandes = new List<Commande>();
            CommandesFiltrees = new List<Commande>();
            DetailCommandes = new List<DetailCommande>();
            ConfigurerInterface();
        }

        private void FiltrerCommandes()
        {
            string filtre = TxtRecherche.Text?.ToLower()?.Trim() ?? "";

            if (string.IsNullOrEmpty(filtre))
            {
                CommandesFiltrees = ToutesLesCommandes.OrderBy(c => c.NumCommande).ToList();
            }
            else
            {
                CommandesFiltrees = ToutesLesCommandes.Where(c =>
                    c.NumCommande.ToString().Contains(filtre) ||
                    c.DateCommande.ToString("dd/MM/yyyy").Contains(filtre) ||
                    (c.EtatCommande?.ToLower().Contains(filtre) ?? false)
                ).OrderBy(c => c.NumCommande).ToList();
            }

            IC_Commandes.ItemsSource = CommandesFiltrees;
        }

        private void NaviguerVersCommande(Commande commande)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheCommande(commande));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la navigation vers la commande : {ex.Message}");
            }
        }

        private void RafraichirDonnees()
        {
            try
            {
                ChargerDonnees();
                TxtRecherche.Text = ""; 
                IC_Commandes.ItemsSource = CommandesFiltrees;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du rafraîchissement des données : {ex.Message}");
            }
        }

        private bool VerifierPermissionsCreation()
        {
            try
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                if (mainWindow?.EmployeConnecte == null)
                {
                    return false;
                }

                return true; 
            }
            catch
            {
                return false;
            }
        }

        private void OuvrirFenetreCreationCommande()
        {
            try
            {
                FenetreCreationCommande fenetre = new FenetreCreationCommande();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'ouverture de la fenêtre de création : {ex.Message}");
            }
        }

        // Méthodes dédiées pour les requêtes SQL
        private List<Commande> ChargerCommandes()
        {
            try
            {
                return new Commande().RecupereCommandeDansBDD();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des commandes : {ex.Message}");
            }
        }

        private List<DetailCommande> ChargerDetailsCommandes()
        {
            try
            {
                return DetailCommande.ChargerDetails();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des détails de commandes : {ex.Message}");
            }
        }
    }
}