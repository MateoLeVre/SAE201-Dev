using System;

namespace AppliNicolas.Classes
{
    public enum TypeVin { Rouge, Blanc, Rosé }

    public class Vin
    {
        private string nom;
        private string appelation;
        private TypeVin type;
        private int millesime;
        private string photo;
        private int stock;
        private double prix;
        private int reference;
        private string fournisseur;
        private string detail;
        private bool estNouveau;


        public Vin(string nom, string appelation, TypeVin type, int millesime, string photo, int stock, double prix,
                   int reference, string fournisseur, string detail, bool estNouveau)
        {
            Nom = nom;
            Appelation = appelation;
            Type = type;
            Millesime = millesime;
            Photo = photo;
            Stock = stock;
            Prix = prix;
            Reference = reference;
            Fournisseur = fournisseur;
            Detail = detail;
            EstNouveau = estNouveau;
        }

        public bool EstSimilaireA(Vin autre)
        {
            return this.Type == autre.Type &&
                   this.Appelation == autre.Appelation &&
                   Math.Abs(this.Millesime - autre.Millesime) <= 1;
        }

        // Encapsulation 

        public string Nom
        {
            get => nom;
            set => nom = string.IsNullOrWhiteSpace(value) ? "Non défini" : value;
        }

        public string Appelation
        {
            get => appelation;
            set => appelation = string.IsNullOrWhiteSpace(value) ? "Non défini" : value;
        }

        public TypeVin Type
        {
            get => type;
            set => type = value;
        }

        public int Millesime
        {
            get => millesime;
            set
            {
                if (value >= 1900 && value <= DateTime.Now.Year)
                    millesime = value;
                else
                    throw new ArgumentException("Millésime incorrect < 1900 ou > à l'année actuelle");
            }
        }

        public string Photo
        {
            get => photo;
            set => photo = string.IsNullOrWhiteSpace(value) ? "/Images/Vin.jpg" : value;
        }

        public int Stock
        {
            get => stock;
            set => stock = value >= 0 ? value : 0;
        }

        public double Prix
        {
            get => prix;
            set
            {
                if (value >= 0)
                    prix = value;
                else
                    throw new ArgumentException("Le prix ne peut être négatif");
            }
        }

        public int Reference
        {
            get => reference;
            set => reference = value;
        } 

        public string Fournisseur
        {
            get => fournisseur;
            set => fournisseur = string.IsNullOrWhiteSpace(value) ? "Non défini" : value;
        }

        public string Detail
        {
            get => detail;
            set => detail = string.IsNullOrWhiteSpace(value) ? "Aucun détail" : value;
        }

        public bool EstNouveau
        {
            get => estNouveau;
            set => estNouveau = value;
        }
    }
}
