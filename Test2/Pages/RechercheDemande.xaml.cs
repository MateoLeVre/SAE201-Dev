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
        private List<Demande> toutesLesDemandes;
        private List<Demande> demandesFiltres;

        public List<Demande> ToutesLesDemandes
        {
            get { return toutesLesDemandes; }
            private set { toutesLesDemandes = value ?? new List<Demande>(); }
        }

        public List<Demande> DemandesFiltres
        {
            get { return demandesFiltres; }
            private set { demandesFiltres = value ?? new List<Demande>(); }
        }

        public RechercheDemande()
        {
            InitializeComponent();

            try
            {
                ChargerDonnees();
                ConfigurerInterface();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des demandes : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                InitialiserListesVides();
            }
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                FiltrerDemandes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtrer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Filtrage par statut ou date à implémenter.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void VoirDemande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Demande demande = (sender as Button)?.Tag as Demande;
                if (demande == null)
                {
                    MessageBox.Show("Impossible de charger les informations de cette demande.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!VerifierPermissionsAccesDemande(demande))
                {
                    MessageBox.Show("Vous n'avez pas les permissions pour accéder à cette demande.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NaviguerVersDemande(demande);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la demande : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VoirVin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vin vin = (sender as Button)?.Tag as Vin;
                if (vin == null)
                {
                    MessageBox.Show("Impossible de charger les informations de ce vin.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NaviguerVersVin(vin);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du vin : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Rafraichir_click(object sender, RoutedEventArgs e)
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

        // Méthodes dédiées pour la logique métier
        private void ChargerDonnees()
        {
            try
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow?.GestionVin?.LesDemandes == null)
                {
                    throw new Exception("Impossible d'accéder aux données des demandes");
                }

                ToutesLesDemandes = mainWindow.GestionVin.LesDemandes.ToList();
                DemandesFiltres = ToutesLesDemandes.OrderBy(d => d.DateDemande).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des données : {ex.Message}");
            }
        }

        private void ConfigurerInterface()
        {
            this.DataContext = this;
            IC_Demandes.ItemsSource = DemandesFiltres;
        }

        private void InitialiserListesVides()
        {
            ToutesLesDemandes = new List<Demande>();
            DemandesFiltres = new List<Demande>();
            ConfigurerInterface();
        }

        private void FiltrerDemandes()
        {
            string filtre = TxtRecherche.Text?.ToLower()?.Trim() ?? "";

            if (string.IsNullOrEmpty(filtre))
            {
                DemandesFiltres = ToutesLesDemandes.OrderBy(d => d.DateDemande).ToList();
            }
            else
            {
                DemandesFiltres = ToutesLesDemandes.Where(d =>
                    VerifierCorrespondanceFiltre(d, filtre)
                ).OrderBy(d => d.DateDemande).ToList();
            }

            IC_Demandes.ItemsSource = DemandesFiltres;
        }

        private bool VerifierCorrespondanceFiltre(Demande demande, string filtre)
        {
            if (demande == null) return false;

            if (demande.Vin?.Nom?.ToLower().Contains(filtre) == true)
                return true;

            if (demande.DateDemandeFormatted?.ToLower().Contains(filtre) == true)
                return true;

            if (demande.Etat?.ToLower().Contains(filtre) == true)
                return true;

            if (demande.NumDemande.ToString().Contains(filtre))
                return true;

            return false;
        }

        private void NaviguerVersDemande(Demande demande)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheDemande(demande));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la navigation vers la demande : {ex.Message}");
            }
        }

        private void NaviguerVersVin(Vin vin)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vin));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la navigation vers le vin : {ex.Message}");
            }
        }

        private void RafraichirDonnees()
        {
            try
            {
                ObservableCollection<Demande> nouvellesDemandes = ChargerDemandesDepuisBase();

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow?.GestionVin != null)
                {
                    mainWindow.GestionVin.LesDemandes = nouvellesDemandes;
                }

                ChargerDonnees();
                TxtRecherche.Text = "";
                IC_Demandes.ItemsSource = DemandesFiltres;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du rafraîchissement des données : {ex.Message}");
            }
        }

        private bool VerifierPermissionsAccesDemande(Demande demande)
        {
            try
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

                if (mainWindow?.EmployeConnecte == null)
                {
                    return false;
                }

                if (mainWindow.estResponsable)
                {
                    return true;
                }

                return demande.NumEmploye == mainWindow.EmployeConnecte.NumEmploye;
            }
            catch
            {
                return false;
            }
        }

        private ObservableCollection<Demande> ChargerDemandesDepuisBase()
        {
            try
            {
                List<Demande> demandes = new Demande().RecupereDemandeDansBDD();
                return new ObservableCollection<Demande>(demandes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des demandes depuis la base : {ex.Message}");
            }
        }
    }
}