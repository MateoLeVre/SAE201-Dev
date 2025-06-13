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

            //this.Deactivated += CliqueArrierePlan;
        }
        private void CliqueArrierePlan(object sender, EventArgs e)
        {
            this.Close();
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
            string emailRecherche = TxtBoxMailClient.Text.Trim();
    
            {
                string sql = "SELECT COUNT(*) from client WHERE LOWER(mailclient) = LOWER(@email)";
                using (NpgsqlCommand commande = new NpgsqlCommand(sql))
                {
                    commande.Parameters.AddWithValue("@email", emailRecherche);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(commande);
                    int count = Convert.ToInt32(dt.Rows[0][0]);

                    if (count > 0)
                    {
                        MessageBox.Show(this, "Cette adresse email existe déjà !", "Email existant",MessageBoxButton.OK, MessageBoxImage.Warning);
                        Console.WriteLine(count);
                        return;
                    }
                }
                try
                {
                    string nom = TxtBoxNom.Text;
                    string prenom = TxtBoxPrenomClient.Text;
                    string mail = TxtBoxMailClient.Text;

                    string sql2 = "INSERT INTO Client (nomClient, prenomClient, mailClient) VALUES (@nom, @prenom, @mail)";

                    using (NpgsqlCommand commandeInsert = new NpgsqlCommand(sql2))
                    {
                        commandeInsert.Parameters.AddWithValue("@nom", nom);
                        commandeInsert.Parameters.AddWithValue("@prenom", prenom);
                        commandeInsert.Parameters.AddWithValue("@mail", mail);

                        ConnexionBD.Instance.ExecuteInsert(commandeInsert);
                    }

                    
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
