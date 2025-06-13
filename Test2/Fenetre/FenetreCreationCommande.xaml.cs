using AppliNicolas.Classes;
using AppliNicolas.Pages;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Fenetre
{
    public partial class FenetreCreationCommande : Window
    {
        private List<Demande> demandesDisponibles;
        private List<Demande> demandesSelectionnees;

        private string fournisseurActif = null;


        public FenetreCreationCommande()
        {
            InitializeComponent();
            LoadDemandes();
            this.ShowDialog(); // Ouvre la fenêtre modale
        }

        private void LoadDemandes()
        {
            var toutesLesDemandes = ((MainWindow)Application.Current.MainWindow)
                                        .GestionVin
                                        .LesDemandes;

            //  uniquement celles "en attente" ou "supprimée"
            demandesDisponibles = toutesLesDemandes
                .Where(d => d.Etat.ToLower() == "en attente" || d.Etat.ToLower() == "supprimée")
                .ToList();

            demandesSelectionnees = new List<Demande>();

            LB_Disponibles.ItemsSource = demandesDisponibles;
            LB_Commande.ItemsSource = demandesSelectionnees;
        }

        private void AjouterDemande_Click(object sender, RoutedEventArgs e)
        {
            Demande d = (sender as Button)?.Tag as Demande;
            if (d == null || demandesSelectionnees.Contains(d))
                return;

            if (fournisseurActif == null)
            {
                fournisseurActif = d.Vin.NomFournisseur;
            }

            if (d.Vin.NomFournisseur == fournisseurActif)
            {
                demandesSelectionnees.Add(d);
                RefreshListes();
            }
            else
            {
                MessageBox.Show($"Cette demande appartient à un autre fournisseur ({d.Vin.NomFournisseur}).\nFournisseur actif : {fournisseurActif}", "Fournisseur différent", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void RefreshListes()
        {
            LB_Disponibles.ItemsSource = null;
            LB_Commande.ItemsSource = null;

            if (fournisseurActif == null)
            {
                // Toutes les demandes en attente ou supprimées
                LB_Disponibles.ItemsSource = demandesDisponibles;
            }
            else
            {
                LB_Disponibles.ItemsSource = demandesDisponibles
                    .Where(d => d.Vin.NomFournisseur == fournisseurActif && !demandesSelectionnees.Contains(d))
                    .ToList();
            }

            LB_Commande.ItemsSource = demandesSelectionnees;
        }


        private void ValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            CreerCommande("Validée");
        }

        private void MettreEnAttente_Click(object sender, RoutedEventArgs e)
        {
            CreerCommande("En attente");
        }

        private void CreerCommande(string etat)
        {
            if (demandesSelectionnees.Count == 0)
            {
                MessageBox.Show("Aucune demande sélectionnée.");
                return;
            }

            double prixTotal = 0;

            foreach (Demande de in demandesSelectionnees)
            {
                prixTotal += de.MontantTotal;
            }


            int numEmploye = ((MainWindow)Application.Current.MainWindow).EmployeConnecte.NumEmploye;

            // Insertion commande
            int numCommande;
            using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO commande (numemploye, datecommande, etat, prixtotal) VALUES (@emp, @date, @etat, @prix) RETURNING numcommande"))
            {
                cmd.Parameters.AddWithValue("@emp", numEmploye);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@etat", etat);
                cmd.Parameters.AddWithValue("@prix",prixTotal);

                DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);
                numCommande = Convert.ToInt32(dt.Rows[0]["numcommande"]);
            }

            // Insertion détails
            foreach (var d in demandesSelectionnees)
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO detailcommande (numcommande, numvin, numdemande, quantite, prix) VALUES (@cmd, @vin, @numd, @qte, @prix)"))
                {
                    cmd.Parameters.AddWithValue("@cmd", numCommande);
                    cmd.Parameters.AddWithValue("@vin", d.NumVin);
                    cmd.Parameters.AddWithValue("@qte", d.QuantiteDemande);
                    cmd.Parameters.AddWithValue("@prix", d.Vin.Prix);
                    cmd.Parameters.AddWithValue("@numd", d.NumDemande);

                    ConnexionBD.Instance.ExecuteInsert(cmd);
                }

                // Mise à jour de l'état de la demande
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE demande SET etat = 'Validée' WHERE numdemande = @id"))
                {
                    cmd.Parameters.AddWithValue("@id", d.NumDemande);
                    ConnexionBD.Instance.ExecuteSet(cmd);
                }
            }

            MessageBox.Show($"Commande {etat.ToLower()} avec succès.");

            // Redirection vers la fiche commande
            Commande nouvelleCommande = new Commande().RecupereCommandeDansBDD().FirstOrDefault(c => c.NumCommande == numCommande);
            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheCommande(nouvelleCommande));
            

            this.Close();
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Reinitialiser_Click(object sender, RoutedEventArgs e)
        {
            fournisseurActif = null;
            demandesSelectionnees.Clear();
            RefreshListes();
        }

    }
}
