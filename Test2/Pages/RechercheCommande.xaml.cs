using AppliNicolas.Classes;
using AppliNicolas.Fenetre;
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
            new FenetreCreationCommande();
        }


    }
}
