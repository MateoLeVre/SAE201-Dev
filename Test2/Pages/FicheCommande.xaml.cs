using AppliNicolas.Classes;
using Npgsql;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class FicheCommande : UserControl
    {
        private Commande commande;

        public FicheCommande(Commande commande)
        {
            InitializeComponent();
            this.commande = commande;


            this.DataContext = commande;
        }

        public bool estResponsable
        {
            get
            {
                return ((MainWindow)Application.Current.MainWindow).estResponsable;
            }
        }


        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).RevenirEnArriere();
        }

        private void VoirVin_Click(object sender, RoutedEventArgs e)
        {
            Vin vin = (sender as Button)?.Tag as Vin;
            if (vin != null)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vin));
            }
        }
        private void VoirDemande_Click(object sender, RoutedEventArgs e)
        {
            Demande demande = (sender as Button)?.Tag as Demande;
            if (demande != null)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheDemande(demande));
            }
        }
        private void MettreEnAttente_Click(object sender, RoutedEventArgs e)
        {
            MiseAJourEtatCommande("en attente");
        }

        private void AnnulerCommande_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment annuler cette commande ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                MiseAJourEtatCommande("annulée");
            }
        }
        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            MiseAJourEtatCommande("validée");
        }

        private void MiseAJourEtatCommande(string nouvelEtat)
        {
            try
            {
                string sql = "UPDATE commande SET etat = @etat WHERE numcommande = @num";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@etat", nouvelEtat);
                    cmd.Parameters.AddWithValue("@num", commande.NumCommande);
                    ConnexionBD.Instance.ExecuteSet(cmd);
                }

                commande.EtatCommande = nouvelEtat;
                MessageBox.Show("Etat de la commande mis à jour.", "succes", MessageBoxButton.OK, MessageBoxImage.Information);
                //((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheCommande(commande));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du changement d'état : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
