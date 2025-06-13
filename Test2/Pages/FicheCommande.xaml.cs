using AppliNicolas.Classes;
using Npgsql;
using System;
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
            MessageBoxResult result = MessageBox.Show(
                "Voulez-vous vraiment annuler cette commande ?",
                "Confirmation d'annulation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                MiseAJourEtatCommande("annulée");
            }
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Voulez-vous vraiment valider cette commande ?",
                "Confirmation de validation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MiseAJourEtatCommande("validée");
            }
        }

        private void MiseAJourEtatCommande(string nouvelEtat)
        {
            try
            {
                // Validation de l'état
                if (!ValiderNouvelEtat(nouvelEtat))
                {
                    return;
                }

                // Vérification des permissions
                if (!VerifierPermissionsModification())
                {
                    MessageBox.Show("Vous n'avez pas les permissions nécessaires pour modifier cette commande.", "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Mise à jour en base de données
                ModifierEtatCommandeEnBase(nouvelEtat);

                // Mise à jour de l'objet local
                commande.EtatCommande = nouvelEtat;

                MessageBox.Show("État de la commande mis à jour avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Rafraîchissement de la page
                ((MainWindow)Application.Current.MainWindow).RefreshPage(new FicheCommande(commande));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour de l'état : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthodes dédiées pour la logique métier et les requêtes SQL
        private bool ValiderNouvelEtat(string nouvelEtat)
        {
            if (string.IsNullOrWhiteSpace(nouvelEtat))
            {
                MessageBox.Show("L'état ne peut pas être vide.", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validation des transitions d'état autorisées
            string etatActuel = commande.EtatCommande?.ToLower();
            string etatNouveau = nouvelEtat.ToLower();

            if (etatActuel == "annulée" || etatActuel == "validée")
            {
                MessageBox.Show("Impossible de modifier une commande annulée ou validée.", "Modification interdite", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool VerifierPermissionsModification()
        {
            // Seuls les responsables peuvent modifier les commandes
            if (!estResponsable)
            {
                return false;
            }

            return true;
        }

        private void ModifierEtatCommandeEnBase(string nouvelEtat)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE commande SET etat = @etat WHERE numcommande = @num"))
                {
                    cmd.Parameters.AddWithValue("@etat", nouvelEtat);
                    cmd.Parameters.AddWithValue("@num", commande.NumCommande);

                    ConnexionBD.Instance.ExecuteSet(cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour en base de données : {ex.Message}");
            }
        }
    }
}