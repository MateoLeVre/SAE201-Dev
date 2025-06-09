using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public enum EtatDemande { Valider, Attente, Supprimer }
    public class Demande
    {
        public Demande(int numDemande, Vin vin, int numEmploye, DateTime dateDemande, int quantiteDemande, EtatDemande etat)
        {
            NumDemande = numDemande;
            NumVin = vin.Reference;
            NumEmploye = numEmploye;
            DateDemande = dateDemande;
            QuantiteDemande = quantiteDemande;
            Etat = etat;
        }

        public Demande(int numDemande, int numVin, int numEmploye, DateTime dateDemande, int quantiteDemande, EtatDemande etat)
        {
            NumDemande = numDemande;
            NumVin = numVin;
            NumEmploye = numEmploye;
            DateDemande = dateDemande;
            QuantiteDemande = quantiteDemande;
            Etat = etat;
        }

        public Vin vin;

        private int numDemande;
        public int NumDemande
        {
            get { return numDemande; }
            set { numDemande = value; }
        }

        private int numVin;
        public int NumVin
        {
            get { return numVin; }
            set { numVin = value; }
        }

        private int numEmploye;
        public int NumEmploye
        {
            get { return numEmploye; }
            set { numEmploye = value; }
        }

        private DateTime dateDemande;
        public DateTime DateDemande
        {
            get { return dateDemande; }
            set { dateDemande = value; }
        }

        private int quantiteDemande;
        public int QuantiteDemande
        {
            get { return quantiteDemande; }
            set { quantiteDemande = value; }
        }

        private EtatDemande etat;
        public EtatDemande Etat
        {
            get { return etat; }
            set { etat = value; }
        }

        public override bool Equals(object? obj)
        {
            return obj is Demande demande &&
                   NumDemande == demande.NumDemande;
        }
    }
}
