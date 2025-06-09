using System;

namespace AppliNicolas.Classes
{
    public enum EtatDemande { Valider, Attente, Supprimer }

    public class Demande
    {
        public Demande(int numDemande, Vin vin, int numEmploye, DateTime dateDemande, int quantiteDemande, EtatDemande etat)
        {
            NumDemande = numDemande;
            Vin = vin;
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

        // Propriétés
        public int NumDemande { get; set; }

        public int NumVin { get; set; }

        public int NumEmploye { get; set; }

        public DateTime DateDemande { get; set; }

        private int quantiteDemande;
        public int QuantiteDemande
        {
            get => quantiteDemande;
            set => quantiteDemande = value >= 0 ? value : 0;
        }

        public EtatDemande Etat { get; set; }

        public string EtatDemandeToString
        {
            get
            {
                if (this.Etat == EtatDemande.Valider)
                    return "Validé";
                else if (this.Etat == EtatDemande.Attente)
                    return "En Attente";
                else 
                    return "Annulé";

            }
        }

        public Vin Vin { get; set; }

        // Montant total = prix du vin * quantité
        public double MontantTotal => Vin != null ? Math.Round(Vin.Prix * QuantiteDemande, 2) : 0;

        public override bool Equals(object? obj)
        {
            return obj is Demande demande &&
                   NumDemande == demande.NumDemande;
        }

        public override string ToString()
        {
            return $"Demande {NumDemande} : {QuantiteDemande} x {Vin?.Nom} le {DateDemande.ToShortDateString()} ({Etat})";
        }
    }
}
