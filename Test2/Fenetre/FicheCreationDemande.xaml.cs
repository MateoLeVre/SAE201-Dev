using AppliNicolas.Classes;
using System;
using System.Data;
using System.Windows;
using Npgsql;

namespace AppliNicolas.Fenetre
{
    public partial class FicheCreationDemande : Window
    {
        private Vin vin;
        private int employeId = 1; //Id d'exemple comme les employe ne sont pas a prendre en compte

        public FicheCreationDemande(Vin vin)
        {
            InitializeComponent();
            this.vin = vin;
            this.DataContext = vin;

            //this.Deactivated += (s, e) => this.Close();
        }

        private void ButRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TxtQuantite_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(TxtQuantite.Text, out int qte))
            {
                double total = qte * vin.Prix;
                TxtMontant.Text = $"{total:N2} €";
            }
            else
            {
                TxtMontant.Text = "0 €";
            }
        }

        private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtQuantite.Text, out int quantite) || quantite <= 0)
            {
                MessageBox.Show("Quantité invalide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string sql = "INSERT INTO Demande (numVin, numEmploye, dateDemande, quantiteDemande, etat) " +
                             "VALUES (@vin, @employe, @date, @qte, @etat)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@vin", vin.Reference);
                    cmd.Parameters.AddWithValue("@employe", employeId);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@qte", quantite);
                    cmd.Parameters.AddWithValue("@etat", "En attente");

                    ConnexionBD.Instance.ExecuteInsert(cmd);
                }

                MessageBox.Show("Demande enregistrée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
