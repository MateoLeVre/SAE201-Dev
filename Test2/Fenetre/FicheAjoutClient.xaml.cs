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

        private void ButAjouter_Click(object sender, RoutedEventArgs e)
        {
            ValiderAjoutClient();
        }

        public void ValiderAjoutClient()
        {
            List<string> lesMails = new List<string>();
            try
            {
                string sql = "SELECT mailclient from client";

                using (NpgsqlCommand commandeInsert = new NpgsqlCommand(sql))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(commandeInsert);

                    foreach (DataRow dr in dt.Rows)
                        lesMails.Add((string)dr["mailclient"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors des récupération de mail : {ex.Message}");
            }

            string emailRecherche = TxtBoxMailClient.Text;
            string emailTrouve = lesMails.Find(mail => mail == emailRecherche);

            if (emailTrouve != null)
            {
                MessageBox.Show("Cette adresse email existe déjà !");
                Console.WriteLine("2");
            }
            else
            {
                try
                {
                    string nom = TxtBoxNom.Text;
                    string prenom = TxtBoxPrenomClient.Text;
                    string mail = TxtBoxMailClient.Text;

                    string sql = "INSERT INTO Client (nomClient, prenomClient, mailClient) VALUES (@nom, @prenom, @mail)";

                    using (NpgsqlCommand commandeInsert = new NpgsqlCommand(sql))
                    {
                        commandeInsert.Parameters.AddWithValue("@nom", nom);
                        commandeInsert.Parameters.AddWithValue("@prenom", prenom);
                        commandeInsert.Parameters.AddWithValue("@mail", mail);

                        ConnexionBD.Instance.ExecuteInsert(commandeInsert);
                    }

                    this.Deactivated -= CliqueArrierePlan;

                    MessageBox.Show("Client ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercherClients());
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout du client : {ex.Message}");
                }
            }
        }
    }
}
