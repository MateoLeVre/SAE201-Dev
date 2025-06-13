using AppliNicolas.Classes;
using AppliNicolas.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppliNicolas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Employe employeConnecte;
        private bool estResponsableValue;
        private GestionVin gestionVin;
        private Stack<UserControl> historiquePages;

        public Employe EmployeConnecte
        {
            get { return employeConnecte; }
            set { employeConnecte = value; }
        }

        public bool estResponsable
        {
            get { return estResponsableValue; }
            set { estResponsableValue = value; }
        }

        public GestionVin GestionVin
        {
            get { return gestionVin; }
            set { gestionVin = value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                InitialiserFenetre();
                InitialiserHistorique();
                NaviguerVers(new Connection());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'initialisation de l'application : {ex.Message}", "Erreur critique", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        public void ChargeData()
        {
            try
            {
                GestionVin = ChargerDonneesGestion();
                this.DataContext = GestionVin;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données, veuillez consulter votre admin", "Erreur de données", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        public void Connection()
        {
            try
            {
                ChargeData();
                ConfigurerInterfaceSelonRole();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la connexion : {ex.Message}", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Selection_Menu_Item(MenuItem mi_choisi)
        {
            try
            {
                if (mi_choisi == null) return;

                if (mi_choisi.Style != (Style)this.FindResource("StyleMenuItemActif"))
                {
                    ReinitialiserStylesMenus();
                    mi_choisi.Style = (Style)this.FindResource("StyleMenuItemActif");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sélection du menu : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MI_Acceuil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Selection_Menu_Item(MI_Acceuil);
                NaviguerVersPagePrincipale(new Acceuil());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation vers l'accueil : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MI_Vin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Selection_Menu_Item(MI_Vin);
                NaviguerVersPagePrincipale(new RechercheVin());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation vers les vins : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MI_Demande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Selection_Menu_Item(MI_Demande);
                NaviguerVersPagePrincipale(new RechercheDemande());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation vers les demandes : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MI_Client_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Selection_Menu_Item(MI_Client);
                NaviguerVersPagePrincipale(new RechercherClients());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation vers les clients : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MI_Deconnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeconnecterUtilisateur();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la déconnexion : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MI_Commande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!VerifierPermissionsCommandes())
                {
                    MessageBox.Show("Vous n'avez pas les permissions pour accéder aux commandes.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Selection_Menu_Item(MI_Commande);
                NaviguerVersPagePrincipale(new RechercheCommande());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation vers les commandes : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NaviguerVers(UserControl nouvellePage)
        {
            try
            {
                if (nouvellePage == null)
                {
                    throw new ArgumentException("La nouvelle page ne peut pas être null");
                }

                if (MainContent.Content is UserControl pageActuelle)
                {
                    historiquePages.Push(pageActuelle);
                }
                MainContent.Content = nouvellePage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RevenirEnArriere()
        {
            try
            {
                if (historiquePages.Count > 0)
                {
                    MainContent.Content = historiquePages.Pop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du retour en arrière : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RefreshPage(UserControl nouvellePage)
        {
            try
            {
                if (nouvellePage == null)
                {
                    throw new ArgumentException("La nouvelle page ne peut pas être null");
                }

                if (historiquePages.Count > 0)
                {
                    MainContent.Content = historiquePages.Pop();
                }
                NaviguerVers(nouvellePage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du rafraîchissement de la page : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthodes dédiées pour la logique métier
        private void InitialiserFenetre()
        {
            try
            {
                double ecranLargeur = SystemParameters.PrimaryScreenWidth;
                double ecranHauteur = SystemParameters.PrimaryScreenHeight;

                this.Width = ecranLargeur;
                this.Height = ecranHauteur;

                MenuPrincipale.DataContext = this;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'initialisation de la fenêtre : {ex.Message}");
            }
        }

        private void InitialiserHistorique()
        {
            historiquePages = new Stack<UserControl>();
        }

        private GestionVin ChargerDonneesGestion()
        {
            try
            {
                return new GestionVin();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des données de gestion : {ex.Message}");
            }
        }

        private void ConfigurerInterfaceSelonRole()
        {
            try
            {
                if (!estResponsable)
                {
                    MI_Commande.Visibility = Visibility.Collapsed;
                    MI_Role.Header = "Vendeur";
                    MI_Role.FontSize = 25;
                }
                else
                {
                    MI_Commande.Visibility = Visibility.Visible;
                    MI_Role.Header = "Responsable";
                    MI_Role.FontSize = 16;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la configuration de l'interface : {ex.Message}");
            }
        }

        private void ReinitialiserStylesMenus()
        {
            try
            {
                Style styleBase = (Style)this.FindResource("StyleMenuItem");

                MI_Commande.Style = styleBase;
                MI_Acceuil.Style = styleBase;
                MI_Vin.Style = styleBase;
                MI_Demande.Style = styleBase;
                MI_Client.Style = styleBase;
                MI_Deconnection.Style = styleBase;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la réinitialisation des styles de menu : {ex.Message}");
            }
        }

        private void NaviguerVersPagePrincipale(UserControl nouvellePage)
        {
            try
            {
                NaviguerVers(nouvellePage);
                historiquePages = new Stack<UserControl>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la navigation vers la page principale : {ex.Message}");
            }
        }

        private void DeconnecterUtilisateur()
        {
            try
            {
                // Réinitialiser les données utilisateur
                EmployeConnecte = null;
                estResponsable = false;
                GestionVin = null;

                // Cacher le menu principal
                MenuPrincipale.Visibility = Visibility.Collapsed;

                // Naviguer vers la page de connexion
                Selection_Menu_Item(MI_Deconnection);
                NaviguerVersPagePrincipale(new Connection());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la déconnexion : {ex.Message}");
            }
        }

        private bool VerifierPermissionsCommandes()
        {
            try
            {
                return EmployeConnecte != null && estResponsable;
            }
            catch
            {
                return false;
            }
        }
    }
}