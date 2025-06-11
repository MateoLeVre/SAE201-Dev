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

                this.Close();

                if (Application.Current?.MainWindow is MainWindow mainWindow)
                {
                    ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercherClients());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout du client : {ex.Message}");
            }
        }
    }
}
