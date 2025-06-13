using AppliNicolas.Classes;
using Npgsql;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class FicheDemande : UserControl
    {
        private Demande demande;

        public FicheDemande(Demande demande)
        {
            InitializeComponent();
            this.demande = demande;
            this.DataContext = demande;

            if (((MainWindow)Application.Current.MainWindow).estResponsable)
            {
                BtnValider.Visibility = Visibility.Visible;
            }
        }

        private void VoirFicheVin_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(demande.Vin));
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).RevenirEnArriere();
        }

        private void MettreEnAttente_Click(object sender, RoutedEventArgs e)
        {
            MiseAJourEtatDemande("En attente");
        }

        private void AnnulerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment annuler cette demande ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                MiseAJourEtatDemande("Supprimer");
            }
        }

        private void ValiderDemande_Click(object sender, RoutedEventArgs e)
        {
            MiseAJourEtatDemande("Valider");
        }

        private void MiseAJourEtatDemande(string nouvelEtat)
        {
            try
            {
                string sql = "UPDATE demande SET etat = @etat WHERE numdemande = @num";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@etat", nouvelEtat);
                    cmd.Parameters.AddWithValue("@num", demande.NumDemande);
                    ConnexionBD.Instance.ExecuteSet(cmd);
                }

                demande.Etat = nouvelEtat;
                MessageBox.Show($"Demande mise à jour : {nouvelEtat}", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheDemande(demande));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
