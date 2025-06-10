using Npgsql;
using System;
using System.Data;

namespace AppliNicolas.Classes
{
    public enum Etat { Valider, Attente, Supprimer }
    public class Demande
    {
        private int numDemande;
        private int numVin;
        private int numEmploye;
        private DateTime dateDemande;
        private int quantiteDemande;
        private Etat etat;

        public Demande()
        {

        }
        public Demande(int numDemande, int numVin, int numEmploye, int quantiteDemande, Etat etat)
        {
            this.NumDemande = numDemande;
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.DateDemande = DateTime.Now;
            this.QuantiteDemande = quantiteDemande;
            this.Etat = etat;
        }
        public Demande(int numDemande, int numVin, int numEmploye, DateTime dateDemande, int quantiteDemande, Etat etat)
        {
            this.NumDemande = numDemande;
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.DateDemande = dateDemande;
            this.QuantiteDemande = quantiteDemande;
            this.Etat = etat;
        }

        public int NumDemande
        {
            get
            {
                return numDemande;
            }

            set
            {
                numDemande = value;
            }
        }

        public int NumVin
        {
            get
            {
                return numVin;
            }

            set
            {
                numVin = value;
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

        public DateTime DateDemande
        {
            get
            {
                return dateDemande;
            }

            set
            {
                dateDemande = value;
            }
        }

        public int QuantiteDemande
        {
            get
            {
                return quantiteDemande;
            }

            set
            {
                quantiteDemande = value;
            }
        }

        public Etat Etat
        {
            get
            {
                return this.etat;
            }

            set
            {
                this.etat = value;
            }
        }

        public List<Demande> RecupereDemandeDansBDD()
        {
            List<Demande> lesDemandes = new List<Demande>();
            using (NpgsqlCommand demandeSelect = new NpgsqlCommand("select * from Demande;"))
            {
                DataTable dt = ConnexionBD.Instance.ExecuteSelect(demandeSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesDemandes.Add(new Demande((Int32)dr["numdemande"], (Int32)dr["numVin"],
                        (Int32)dr["numemploye"], (DateTime)dr["datedemande"],
                        (Int32)dr["quantitedemande"], (Etat)(Int32)dr["etat"]));
                }
            }
            return lesDemandes;
        }
    }
}
