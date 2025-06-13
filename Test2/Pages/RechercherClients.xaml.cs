using AppliNicolas.Classes;
using AppliNicolas.Fenetre;
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
    /// <summary>
    /// Logique d'interaction pour RechercherClients.xaml
    /// </summary>
    public partial class RechercherClients : UserControl
    {
        private List<Client> toutsLesClients;
        private List<Client> clientsFiltres;

        public List<Client> ToutsLesClients
        {
            get { return toutsLesClients; }
            private set { toutsLesClients = value ?? new List<Client>(); }
        }

        public List<Client> ClientsFiltres
        {
            get { return clientsFiltres; }
            private set { clientsFiltres = value ?? new List<Client>(); }
        }

        public RechercherClients()
        {
            InitializeComponent();

            try
            {
                ChargerDonnees();
                ConfigurerInterface();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des clients : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                InitialiserListesVides();
            }
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                FiltrerClients();
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

        private void VoirClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Client client = (sender as Button)?.Tag as Client;
                if (client == null)
                {
                    MessageBox.Show("Impossible de charger les informations de ce client.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                OuvrirFicheClient(client);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la fiche client : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButCreerFicheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!VerifierPermissionsCreation())
                {
                    MessageBox.Show("Vous n'avez pas les permissions nécessaires pour créer un client.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                OuvrirFenetreAjoutClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la fenêtre d'ajout : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChargerDonnees()
        {
            try
            {
                ToutsLesClients = ChargerClientsDepuisBase();
                ClientsFiltres = ToutsLesClients.OrderBy(d => d.NumClient).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des données : {ex.Message}");
            }
        }

        private void ConfigurerInterface()
        {
            this.DataContext = this;
            IC_Client.ItemsSource = ClientsFiltres;
        }

        private void InitialiserListesVides()
        {
            ToutsLesClients = new List<Client>();
            ClientsFiltres = new List<Client>();
            ConfigurerInterface();
        }

        private void FiltrerClients()
        {
            string filtre = TxtRecherche.Text?.ToLower()?.Trim() ?? "";

            if (string.IsNullOrEmpty(filtre))
            {
                ClientsFiltres = ToutsLesClients.OrderBy(d => d.NumClient).ToList();
            }
            else
            {
                ClientsFiltres = ToutsLesClients.Where(c =>
                    VerifierCorrespondanceFiltre(c, filtre)
                ).OrderBy(d => d.NumClient).ToList();
            }

            IC_Client.ItemsSource = ClientsFiltres;
        }

        private bool VerifierCorrespondanceFiltre(Client client, string filtre)
        {
            if (client == null) return false;

            if (client.NomClient?.ToLower().Contains(filtre) == true)
                return true;

            if (client.PrenomClient?.ToLower().Contains(filtre) == true)
                return true;

            if (client.MailClient?.ToLower().Contains(filtre) == true)
                return true;

            if (client.NumClient.ToString().Contains(filtre))
                return true;

            return false;
        }

        private void OuvrirFicheClient(Client client)
        {
            try
            {
                FicheClient ficheClient = new FicheClient(client);
                ficheClient.Show();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'ouverture de la fiche client : {ex.Message}");
            }
        }

        private void OuvrirFenetreAjoutClient()
        {
            try
            {
                FicheAjoutClient ficheAjout = new FicheAjoutClient();
                ficheAjout.Show();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'ouverture de la fenêtre d'ajout : {ex.Message}");
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

        // Méthode dédiée pour la requête SQL
        private List<Client> ChargerClientsDepuisBase()
        {
            try
            {
                return new Client().RecupereClientDansBDD();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des clients depuis la base : {ex.Message}");
            }
        }
    }
}