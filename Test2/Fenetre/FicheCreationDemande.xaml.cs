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
        private int employeId = 1; // Id d'exemple comme les employe ne sont pas a prendre en compte

        public FicheCreationDemande(Vin vin)
        {
            InitializeComponent();
            this.vin = vin;
            this.DataContext = vin;
        }

        private void ButRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TxtQuantite_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculerMontantTotal();
        }

        private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            ValiderCreationDemande();
        }

        private void ValiderCreationDemande()
        {
            try
            {
                // Validation de la quantité
                if (!int.TryParse(TxtQuantite.Text, out int quantite) || quantite <= 0)
                {
                    MessageBox.Show("Quantité invalide. Veuillez saisir un nombre entier positif.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validation de la quantité maximale
                if (quantite > 1000)
                {
                    MessageBox.Show("La quantité ne peut pas dépasser 1000.", "Quantité trop élevée", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Insertion de la nouvelle demande
                InsererNouvelleDemande(quantite);

                MessageBox.Show("Demande enregistrée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de la demande : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculerMontantTotal()
        {
            if (int.TryParse(TxtQuantite.Text, out int qte) && qte > 0)
            {
                double total = qte * vin.Prix;
                TxtMontant.Text = $"{total:N2} €";
            }
            else
            {
                TxtMontant.Text = "0,00 €";
            }
        }

        // Méthode dédiée pour la requête SQL
        private void InsererNouvelleDemande(int quantite)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Demande (numVin, numEmploye, dateDemande, quantiteDemande, etat) VALUES (@vin, @employe, @date, @qte, @etat)"))
                {
                    cmd.Parameters.AddWithValue("@vin", vin.Reference);
                    cmd.Parameters.AddWithValue("@employe", employeId);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@qte", quantite);
                    cmd.Parameters.AddWithValue("@etat", "En attente");

                    ConnexionBD.Instance.ExecuteInsert(cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'insertion de la demande : {ex.Message}");
            }
        }
    }
}