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
        private Client LeMec {  get; set; }

        public FicheClient(Client client)
        {
            InitializeComponent();

            LeMec = client;

            Client clientModifie = new Client {NumClient = client.NumClient, NomClient = client.NomClient, PrenomClient = client.PrenomClient, MailClient = client.MailClient };

            this.DataContext = clientModifie;

            this.Deactivated += CliqueArrierePlan;
        }
        private void CliqueArrierePlan(object sender, EventArgs e)
        {
             this.Close();
        }

        private void ButRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Deactivated -= CliqueArrierePlan;
            this.Close();
        }

        private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            ValiderModificationClient();
        }

        private void ButSuprimer_Click(object sender, RoutedEventArgs e)
        {
            SupprimerClient();
        }

        public void ValiderModificationClient()
        {
            string nom = this.TxtBoxNom.Text;
            string prenom = this.TxtBoxPrenomClient.Text;
            string mail = this.TxtBoxMailClient.Text;
            int id = LeMec.NumClient;

            try
            {
                using (NpgsqlCommand commandeSelect = new NpgsqlCommand(
                    "UPDATE Client SET nomclient = @nom, prenomclient = @prenom, mailclient = @mail WHERE numclient = @id"))
                {
                    commandeSelect.Parameters.AddWithValue("@nom", nom);
                    commandeSelect.Parameters.AddWithValue("@prenom", prenom);
                    commandeSelect.Parameters.AddWithValue("@mail", mail);
                    commandeSelect.Parameters.AddWithValue("@id", id);

                    ConnexionBD.Instance.ExecuteSet(commandeSelect);
                }

                this.Deactivated -= CliqueArrierePlan;

                MessageBox.Show("Client modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercherClients());
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème avec les modifications client", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void SupprimerClient()
        {
            try
            {
                int numClient = LeMec.NumClient;

                using (var commandeDelete = new NpgsqlCommand("DELETE FROM Client WHERE numclient = @numclient"))
                {
                    commandeDelete.Parameters.AddWithValue("@numclient", numClient);

                    ConnexionBD.Instance.ExecuteSet(commandeDelete);
                }

                this.Deactivated -= CliqueArrierePlan;

                MessageBox.Show("Client supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercherClients());
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de la suppression du client\n" + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
