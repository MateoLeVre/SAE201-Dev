using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppliNicolas.Classes;
using System.Windows;
using System.Windows.Controls;
using AppliNicolas.Pages;
using System.Collections.ObjectModel;

namespace AppliNicolas.Classes
{
    public class Commande
    {
        private int numCommande;
        private int numEmploye;
        private DateTime dateCommande;
        private string etatCommande;
        private double prixTotal;
        private Demande demande;

        public virtual ICollection<Demande> Demandes { get; set; }
        public virtual ICollection<DetailCommande> DetailCommandes { get; set; }

        public Commande()
        {
            Demandes = new List<Demande>();
            DetailCommandes = new List<DetailCommande>();
        }

        public Commande(int numCommande, int numEmploye, string etatCommande)
        {
            this.NumCommande = numCommande;
            this.NumEmploye = numEmploye;
            this.DateCommande = DateTime.Now;
            this.EtatCommande = etatCommande;
            Demandes = new List<Demande>();
            DetailCommandes = DetailCommande.ChargerDetailsParCommande(this.NumCommande);
        }

        public Commande(int numCommande, int numEmploye, DateTime dateCommande, string etatCommande)
        {
            this.NumCommande = numCommande;
            this.NumEmploye = numEmploye;
            this.DateCommande = dateCommande;
            this.EtatCommande = etatCommande;
            Demandes = new List<Demande>();
            DetailCommandes = DetailCommande.ChargerDetailsParCommande(this.NumCommande);
        }

        public int NumDemande { get; set; }

        public Demande Demande
        {
            get
            {
                if (demande == null && NumDemande > 0)
                {
                    demande = ((MainWindow)Application.Current.MainWindow)
                                .GestionVin
                                .LesDemandes
                                .FirstOrDefault(d => d.NumDemande == NumDemande);
                }
                return demande;
            }
        }

        public string NomFournisseur
        {
            get
            {
                List<string> fournisseurs = DetailCommandes
                            .Select(dc => dc.Vin.NomFournisseur)
                            .Distinct()
                            .ToList();

                return fournisseurs.Count == 1 ? fournisseurs[0] : "Erreur Fournisseurs multiples pour une commande";
            }
        }

        public int NumCommande
        {
            get { return numCommande; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro de commande doit être positif");
                }
                numCommande = value;
            }
        }

        public int NumEmploye
        {
            get { return numEmploye; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro d'employé doit être positif");
                }
                numEmploye = value;
            }
        }

        public DateTime DateCommande
        {
            get { return dateCommande; }
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("La date de commande ne peut pas être dans le futur");
                }
                dateCommande = value;
            }
        }

        public string EtatCommande
        {
            get { return etatCommande; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("L'état de la commande ne peut pas être vide");
                }
                if (value.Length > 20)
                {
                    throw new ArgumentException("L'état de la commande ne peut pas dépasser 20 caractères");
                }
                etatCommande = value;
            }
        }

        public double PrixTotal
        {
            get
            {
                double prix = 0;
                foreach (DetailCommande dc in DetailCommandes)
                {
                    prix += dc.PrixDetail;
                }
                return prix;
            }
        }

        public void AjouterDemande(Demande demande)
        {
            Demandes.Add(demande);
        }

        public void RetirerDemande(Demande demande)
        {
            Demandes.Remove(demande);
        }

        public void AjoutDetail(DetailCommande detail)
        {
            DetailCommandes.Add(detail);
        }

        public List<Commande> RecupereCommandeDansBDD()
        {
            List<Commande> lesCommandes = new List<Commande>();

            try
            {
                using (NpgsqlCommand commandeSelect = new NpgsqlCommand("select * from Commande;"))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(commandeSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        lesCommandes.Add(new Commande((Int32)dr["numcommande"], (Int32)dr["numemploye"],
                            (DateTime)dr["datecommande"], (string)dr["etat"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des commandes : {ex.Message}");
            }

            return lesCommandes;
        }

        public override int GetHashCode()
        {
            return NumCommande.GetHashCode();
        }
    }
}