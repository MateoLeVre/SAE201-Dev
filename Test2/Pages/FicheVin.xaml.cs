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
    public partial class FicheVin : UserControl
    {
        private Vin vin;
        private List<Vin> similaires;

        public Vin Vin
        {
            get { return vin; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Le vin ne peut pas être null");
                }
                vin = value;
            }
        }

        public List<Vin> Similaires
        {
            get { return similaires; }
            private set { similaires = value ?? new List<Vin>(); }
        }

        public event Action<Vin> VinSelectionne;

        public FicheVin(Vin vin)
        {
            InitializeComponent();

            try
            {
                Vin = vin;
                ChargerVinsSimilaires();
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement de la fiche vin : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                Vin vinClique = btn?.Tag as Vin;

                if (vinClique == null)
                {
                    MessageBox.Show("Impossible de charger les informations de ce vin.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NaviguerVersVin(vinClique);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).RevenirEnArriere();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du retour : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCreerDemande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Vérification que l'utilisateur est connecté
                if (!VerifierUtilisateurConnecte())
                {
                    MessageBox.Show("Vous devez être connecté pour créer une demande.", "Connexion requise", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Vérification de la disponibilité du vin
                if (!VerifierDisponibiliteVin())
                {
                    MessageBox.Show("Ce vin n'est pas disponible pour une demande.", "Vin indisponible", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                OuvrirFenetreCreationDemande();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de la demande : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthodes dédiées pour la logique métier
        private void ChargerVinsSimilaires()
        {
            try
            {
                Similaires = Vin.TrouverSimilaires();
                IC_Similaires.ItemsSource = Similaires;
            }
            catch (Exception ex)
            {
                Similaires = new List<Vin>();
                IC_Similaires.ItemsSource = Similaires;
                throw new Exception($"Erreur lors du chargement des vins similaires : {ex.Message}");
            }
        }

        private void NaviguerVersVin(Vin vinClique)
        {
            try
            {
                // Déclencher l'événement si des abonnés existent
                VinSelectionne?.Invoke(vinClique);

                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vinClique));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la navigation vers le vin : {ex.Message}");
            }
        }

        private bool VerifierUtilisateurConnecte()
        {
            try
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                return mainWindow?.EmployeConnecte != null;
            }
            catch
            {
                return false;
            }
        }

        private bool VerifierDisponibiliteVin()
        {
            // Vérification de base : le vin doit avoir un prix et une référence valides
            if (Vin.Prix <= 0)
            {
                return false;
            }

            if (Vin.Reference <= 0)
            {
                return false;
            }

            // Ici, on pourrait ajouter d'autres vérifications comme le stock, etc.
            return true;
        }

        private void OuvrirFenetreCreationDemande()
        {
            try
            {
                FicheCreationDemande fenetre = new FicheCreationDemande(this.Vin);
                fenetre.ShowDialog();

                // Après fermeture de la fenêtre, on pourrait rafraîchir les données si nécessaire
                // RefreshData();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'ouverture de la fenêtre de création : {ex.Message}");
            }
        }
    }
}