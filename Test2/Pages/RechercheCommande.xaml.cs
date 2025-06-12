using AppliNicolas.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Pages
{
    public partial class RechercheCommande : UserControl
    {
        public List<Commande> ToutesLesCommandes { get; set; }
        public List<Commande> CommandesFiltrees { get; set; }

        public List<DetailCommande> DetailCommandes { get; set; }

        public RechercheCommande()
        {
            InitializeComponent();

            ToutesLesCommandes = new Commande().RecupereCommandeDansBDD();

            DetailCommandes = DetailCommande.ChargerDetails();



            CommandesFiltrees = ToutesLesCommandes.OrderBy(c => c.NumCommande).ToList();
            this.DataContext = this;
            IC_Commandes.ItemsSource = CommandesFiltrees;
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtre = TxtRecherche.Text?.ToLower() ?? "";

            CommandesFiltrees = ToutesLesCommandes.Where(c =>
                c.NumCommande.ToString().Contains(filtre) ||
                c.DateCommande.ToString("dd/MM/yyyy").ToLower().Contains(filtre) ||
                c.EtatCommande.ToLower().Contains(filtre)).ToList();

            IC_Commandes.ItemsSource = CommandesFiltrees;
        }

        private void Filtrer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Filtrage avancé à implémenter.");
        }

        private void VoirCommande_Click(object sender, RoutedEventArgs e)
        {
            Commande commande = (sender as Button)?.Tag as Commande;
            if (commande != null)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheCommande(commande));
            }
        }

        private void Rafraichir_Click(object sender, RoutedEventArgs e)
        {
            ToutesLesCommandes = new Commande().RecupereCommandeDansBDD();
            

            CommandesFiltrees = ToutesLesCommandes;
            IC_Commandes.ItemsSource = CommandesFiltrees;

            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercheCommande());
        }

        private void GenererCommandesAuto_Click(object sender, RoutedEventArgs e)
        {
            List<Demande> demandesValidees = ((MainWindow)Application.Current.MainWindow)
                                    .GestionVin
                                    .LesDemandes
                                    .Where(d => d.Etat.ToLower() == "validée")
                                    .ToList();

            List<DetailCommande> detailsExistants = DetailCommande.ChargerDetails();

            List<Demande> demandesSansCommande = new List<Demande>();

            int nbCommandeCree = 0;

            foreach (Demande demande in demandesValidees)
            {
                bool dejaCommandee = false;
                foreach (DetailCommande dc in detailsExistants)
                {
                    if (dc.NumVin == demande.NumVin)
                    {
                        dejaCommandee = true;
                        break;
                    }
                }

                if (dejaCommandee == false)
                {
                    demandesSansCommande.Add(demande);
                }
            }

            

            // Regroupement par fournisseur
            Dictionary<string, List<Demande>> demandesParFournisseur = new Dictionary<string, List<Demande>>();
            foreach (Demande demande in demandesSansCommande)
            {
                string fournisseur = demande.Vin.NomFournisseur;
                if (!demandesParFournisseur.ContainsKey(fournisseur))
                {
                    demandesParFournisseur[fournisseur] = new List<Demande>();
                }
                demandesParFournisseur[fournisseur].Add(demande);
            }

            int employeId = 1; //Emplye fixe



            foreach (KeyValuePair<string, List<Demande>> groupe in demandesParFournisseur)
            {
                nbCommandeCree += 1;

                List<DetailCommande> details = new List<DetailCommande>();

                foreach (Demande demande in groupe.Value)
                {

                    DetailCommande detail = new DetailCommande
                    {
                        NumVin = demande.NumVin,
                        Quantite = demande.QuantiteDemande,
                        Prix = demande.Vin.Prix
                    };
                    details.Add(detail);

                    
                }


                string insertCommande = "INSERT INTO commande (numemploye, datecommande, etat) VALUES (@emp, @date, @etat) RETURNING numcommande";
                int numCommande = -1;

                using (NpgsqlCommand cmd = new NpgsqlCommand(insertCommande))
                {

                    cmd.Parameters.AddWithValue("@emp", employeId);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@etat", "En attente");

                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);
                    numCommande = Convert.ToInt32(dt.Rows[0]["numcommande"]);
                }


                //detail
                foreach (DetailCommande dc in details)
                {
                    string insertDetail = "INSERT INTO detailcommande (numcommande, numvin, quantite, prix) VALUES (@cmd, @vin, @qte, @prix)";
                    using (NpgsqlCommand detailCmd = new NpgsqlCommand(insertDetail))
                    {
                        detailCmd.Parameters.AddWithValue("@cmd", numCommande);
                        detailCmd.Parameters.AddWithValue("@vin", dc.NumVin);
                        detailCmd.Parameters.AddWithValue("@qte", dc.Quantite);
                        detailCmd.Parameters.AddWithValue("@prix", dc.Prix);

                        ConnexionBD.Instance.ExecuteInsert(detailCmd);
                    }
                }
            }

            MessageBox.Show($"{nbCommandeCree} commandes generee.", "info", MessageBoxButton.OK, MessageBoxImage.Information);
            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new RechercheCommande());
        }


    }
}
