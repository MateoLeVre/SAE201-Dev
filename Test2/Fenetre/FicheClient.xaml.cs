using AppliNicolas.Classes;
using AppliNicolas.Pages;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppliNicolas.Fenetre
{
    /// <summary>
    /// Logique d'interaction pour FicheClient.xaml
    /// </summary>
    public partial class FicheClient : Window
    {
        private Client LeMec { get; set; }

        public FicheClient(Client client)
        {
            InitializeComponent();
            LeMec = client;
            
            // Création du client pour le DataContext en contournant les validations
            Client clientModifie = new Client();
            clientModifie.GetType().GetField("numClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(clientModifie, client.NumClient);
            clientModifie.GetType().GetField("nomClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(clientModifie, client.NomClient);
            clientModifie.GetType().GetField("prenomClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(clientModifie, client.PrenomClient);
            clientModifie.GetType().GetField("mailClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(clientModifie, client.MailClient);
            
            this.DataContext = clientModifie;
        }

        private void ButRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            ValiderModificationClient();
        }

        private void ButSuprimer_Click(object sender, RoutedEventArgs e)
        {
            // Demander confirmation avant suppression
            MessageBoxResult result = MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer le client {LeMec.PrenomClient} {LeMec.NomClient} ?",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SupprimerClient();
            }
        }

        public void ValiderModificationClient()
        {
            try
            {
                string nom = this.TxtBoxNom.Text.Trim();
                string prenom = this.TxtBoxPrenomClient.Text.Trim();
                string mail = this.TxtBoxMailClient.Text.Trim();

                // Validation des champs obligatoires
                if (string.IsNullOrWhiteSpace(nom))
                {
                    MessageBox.Show("Le nom ne peut pas être vide.", "Champ obligatoire", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(prenom))
                {
                    MessageBox.Show("Le prénom ne peut pas être vide.", "Champ obligatoire", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(mail))
                {
                    MessageBox.Show("L'email ne peut pas être vide.", "Champ obligatoire", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validation de la longueur des champs
                if (nom.Length > 50)
                {
                    MessageBox.Show("Le nom ne peut pas dépasser 50 caractères.", "Champ trop long", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (prenom.Length > 50)
                {
                    MessageBox.Show("Le prénom ne peut pas dépasser 50 caractères.", "Champ trop long", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (mail.Length > 100)
                {
                    MessageBox.Show("L'email ne peut pas dépasser 100 caractères.", "Champ trop long", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validation du format email (simplifié)
                if (!mail.Contains("@") || !mail.Contains("."))
                {
                    MessageBox.Show("Le format de l'email n'est pas valide.", "Email invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Vérification de l'unicité de l'email (sauf pour le client actuel)
                if (VerifierEmailExistantPourAutreClient(mail, LeMec.NumClient))
                {
                    MessageBox.Show("Cette adresse email est déjà utilisée par un autre client !", "Email existant", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Mise à jour du client
                ModifierClient(LeMec.NumClient, nom, prenom, mail);

                MessageBox.Show("Client modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercherClients());
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification du client : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SupprimerClient()
        {
            try
            {
                SupprimerClientDeLaBase(LeMec.NumClient);

                MessageBox.Show("Client supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercherClients());
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression du client : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthodes dédiées pour les requêtes SQL
        private bool VerifierEmailExistantPourAutreClient(string email, int numClientActuel)
        {
            try
            {
                using (NpgsqlCommand commande = new NpgsqlCommand("SELECT COUNT(*) FROM client WHERE LOWER(mailclient) = LOWER(@email) AND numclient != @numClient"))
                {
                    commande.Parameters.AddWithValue("@email", email);
                    commande.Parameters.AddWithValue("@numClient", numClientActuel);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(commande);
                    int count = Convert.ToInt32(dt.Rows[0][0]);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, on autorise la modification (pas de blocage)
                return false;
            }
        }

        private void ModifierClient(int numClient, string nom, string prenom, string mail)
        {
            try
            {
                using (NpgsqlCommand commandeUpdate = new NpgsqlCommand("UPDATE Client SET nomclient = @nom, prenomclient = @prenom, mailclient = @mail WHERE numclient = @id"))
                {
                    commandeUpdate.Parameters.AddWithValue("@nom", nom);
                    commandeUpdate.Parameters.AddWithValue("@prenom", prenom);
                    commandeUpdate.Parameters.AddWithValue("@mail", mail);
                    commandeUpdate.Parameters.AddWithValue("@id", numClient);
                    ConnexionBD.Instance.ExecuteSet(commandeUpdate);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour du client : {ex.Message}");
            }
        }

        private void SupprimerClientDeLaBase(int numClient)
        {
            try
            {
                using (NpgsqlCommand commandeDelete = new NpgsqlCommand("DELETE FROM Client WHERE numclient = @numclient"))
                {
                    commandeDelete.Parameters.AddWithValue("@numclient", numClient);
                    ConnexionBD.Instance.ExecuteSet(commandeDelete);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la suppression du client : {ex.Message}");
            }
        }
    }
}