using AppliNicolas.Classes;
using AppliNicolas.Pages;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace AppliNicolas.Fenetre
{
    /// <summary>
    /// Logique d'interaction pour FicheAjoutClient.xaml
    /// </summary>
    public partial class FicheAjoutClient : Window
    {
        public FicheAjoutClient()
        {
            InitializeComponent();
        }

        private void ButRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButAjouter_Click(object sender, RoutedEventArgs e)
        {
            ValiderAjoutClient();
        }

        public void ValiderAjoutClient()
        {
            try
            {
                string nom = TxtBoxNom.Text.Trim();
                string prenom = TxtBoxPrenomClient.Text.Trim();
                string mail = TxtBoxMailClient.Text.Trim();

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

                if (!mail.Contains("@") || !mail.Contains("."))
                {
                    MessageBox.Show("Le format de l'email n'est pas valide.", "Email invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (VerifierEmailExistant(mail))
                {
                    MessageBox.Show("Cette adresse email existe déjà !", "Email existant", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                InsererNouveauClient(nom, prenom, mail);

                MessageBox.Show("Client ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercherClients());
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout du client : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthodes dédiées pour les requêtes SQL
        private bool VerifierEmailExistant(string email)
        {
            try
            {
                using (NpgsqlCommand commande = new NpgsqlCommand("SELECT COUNT(*) FROM client WHERE LOWER(mailclient) = LOWER(@email)"))
                {
                    commande.Parameters.AddWithValue("@email", email);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(commande);
                    int count = Convert.ToInt32(dt.Rows[0][0]);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void InsererNouveauClient(string nom, string prenom, string mail)
        {
            try
            {
                using (NpgsqlCommand commandeInsert = new NpgsqlCommand("INSERT INTO Client (nomClient, prenomClient, mailClient) VALUES (@nom, @prenom, @mail)"))
                {
                    commandeInsert.Parameters.AddWithValue("@nom", nom);
                    commandeInsert.Parameters.AddWithValue("@prenom", prenom);
                    commandeInsert.Parameters.AddWithValue("@mail", mail);
                    ConnexionBD.Instance.ExecuteInsert(commandeInsert);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'insertion du client : {ex.Message}");
            }
        }
    }
}