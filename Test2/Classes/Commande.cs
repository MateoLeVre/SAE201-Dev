using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class Commande
    {
        private int numCommande;
        private int numEmploye;
        private DateTime dateCommande;
        private Etat etatCommande;
        private double prixTotal;

        public virtual ICollection<Demande> Demandes { get; set; }
        public virtual ICollection<DetailCommande> DetailCommandes { get; set; }

        public Commande()
        {
            Demandes = new List<Demande>();
            DetailCommandes = new List<DetailCommande>();
        }
        public Commande(int numCommande, int numEmploye, Etat etatCommande)
        {
            this.NumCommande = numCommande;
            this.NumEmploye = numEmploye;
            this.DateCommande = DateTime.Now;
            this.EtatCommande = etatCommande;
            Demandes = new List<Demande>();
            DetailCommandes = new List<DetailCommande>();
        }

        public Commande(int numCommande, int numEmploye, DateTime dateCommande, Etat etatCommande)
        {
            this.NumCommande = numCommande;
            this.NumEmploye = numEmploye;
            this.DateCommande = dateCommande;
            this.EtatCommande = etatCommande;
            Demandes = new List<Demande>();
            DetailCommandes = new List<DetailCommande>();
        }

        public int NumCommande
        {
            get
            {
                return numCommande;
            }

            set
            {
                numCommande = value;
            }
        }

        public int NumEmploye
        {
            get
            {
                return numEmploye;
            }

            set
            {
                numEmploye = value;
            }
        }

        public DateTime DateCommande
        {
            get
            {
                return dateCommande;
            }

            set
            {
                dateCommande = value;
            }
        }

        public Etat EtatCommande
        {
            get
            {
                return etatCommande;
            }

            set
            {
                etatCommande = value;
            }
        }

        public double PrixTotal
        {
            get
            {
                return this.prixTotal;
            }

            set
            {
                this.prixTotal = value;
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

        public void CalculPrix()
        {
            PrixTotal = 0;
            foreach (DetailCommande detail in DetailCommandes)
            {
                PrixTotal += detail.PrixDetail;
            }
        }
        public List<Commande> RecupereCommandeDansBDD()
        {
            List<Commande> lesCommandes = new List<Commande>();
            using (NpgsqlCommand commandeSelect = new NpgsqlCommand("select * from Commande;"))
            {
                DataTable dt = ConnexionBD.Instance.ExecuteSelect(commandeSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesCommandes.Add(new Commande((Int32)dr["numcommande"], (Int32)dr["numemploye"],
                        (DateTime)dr["datecommande"], (Etat)dr["etat"]));
                }
            }
            return lesCommandes;
        }
    }
}
