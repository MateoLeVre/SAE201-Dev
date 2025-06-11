using Npgsql;
using System;
using System.Data;

namespace AppliNicolas.Classes
{
    public enum TypeVin { Rouge, Blanc, Rosé }

    public class Vin
    {
        //rajoutez un constructeur de vin a partir juste du numero du vi n qui va rechercher le vin dans la bd 

        private string nom;
        private int numAppelation;
        private int numType;
        private int millesime;
        private string photo;
        private double prix;
        private int reference;
        private int numFournisseur;
        private string detail;
        private string nomFournisseur;
        private string typeVin;
        private string appelationVin;

        public Vin()
        {
        }

        public Vin(string nom, int numAppelation, int numtype, int millesime, double prix, int reference, int numFournisseur, string detail)
        {
            Nom = nom;
            NumAppelation = numAppelation;
            NumType = numtype;
            Millesime = millesime;
            Photo = "";
            Prix = prix;
            Reference = reference;
            NumFournisseur = numFournisseur;
            Detail = detail;
            NomFournisseur = GetNomFournisseur;
            TypeVin = GetType;
            AppelationVin = GetAppelation;
        }


        public string GetAppelation
        {
            get
            {
                string appelation = "";
                using (NpgsqlCommand cmd = new NpgsqlCommand($"select t.nomappelation from vin v join appelation t on t.numtype2= v.numtype2 where v.numvin={ this.Reference};"))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);
                    foreach (DataRow dr in dt.Rows)
                        appelation = (string)dr["nomappelation"];
                }
                return appelation;
            }
        }
        

        public string GetNomFournisseur {
            get
            {
                string nomFournisseur = "";
                using (NpgsqlCommand cmd = new NpgsqlCommand("select f.nomfournisseur from Vin v join fournisseur f on v.numfournisseur=f.numfournisseur" +
                    $" where v.numvin={this.Reference};"))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);
                    foreach (DataRow dr in dt.Rows)
                        nomFournisseur = (string)dr["nomfournisseur"];
                }
                return nomFournisseur;
            }
        }
        public string GetType {
            get
            {
                string typeVin = "";
                using (NpgsqlCommand cmd = new NpgsqlCommand("select t.nomtype from vin v join typevin t on t.numtype=v.numtype" +
                    $" where v.numvin={this.Reference};"))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);
                    foreach (DataRow dr in dt.Rows)
                        typeVin = (string)dr["nomtype"];
                }
                return typeVin;
            }
        }

        public int CalculerDegreRessemblance(Vin autre)
        {
            int degre = 0;
            if (this.NumType == autre.NumType)
                degre++;
            if (this.NumAppelation == autre.NumAppelation)
                degre++;
            if (this.Millesime == autre.Millesime)
                degre++;
            return degre;
        }


        // Encapsulation 

        public string Nom
        {
            get => nom;
            set => nom = string.IsNullOrWhiteSpace(value) ? "Non défini" : value;
        }

        public int NumAppelation
        {
            get => numAppelation;
            set => numAppelation = value;
        }

        public int NumType
        {
            get => numType;
            set => numType = value;
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

        public int NumFournisseur
        {
            get => numFournisseur;
            set => numFournisseur = value;
        }

        public string Detail
        {
            get => detail;
            set => detail = string.IsNullOrWhiteSpace(value) ? "Aucun détail" : value;
        }
        public string NomFournisseur
        {
            get => nomFournisseur;
            private set => nomFournisseur = value;
        }

        public string TypeVin
        {
            get => typeVin;
            private set => typeVin = value;
        }

        public string AppelationVin
        {
            get => appelationVin;
            private set => appelationVin = value;
        }



        public List<Vin> RecupereVinDansBDD()
        {
            List<Vin> lesVins = new List<Vin>();
            using (NpgsqlCommand vinSelect = new NpgsqlCommand("select * from Vin;"))
            {
                DataTable dt = ConnexionBD.Instance.ExecuteSelect(vinSelect);
                foreach (DataRow dr in dt.Rows)
                    lesVins.Add(new Vin((string)dr["nomvin"], (Int32)dr["numtype2"], (Int32)dr["numtype"],
                    (Int32)dr["millesime"], (double)dr["prixvin"], (Int32)dr["numvin"], (Int32)dr["numfournisseur"],
                    (string)dr["descriptif"]));
            }
            return lesVins;
        }
       



        public  List<Vin> TrouverSimilaires()
        {
            return this.RecupereVinDansBDD()
                .Where(vin => vin.Nom != this.Nom &&
                              this.CalculerDegreRessemblance(vin) >= 1)
                .OrderByDescending(vin => this.CalculerDegreRessemblance(vin))
                .Take(5)
                .ToList();
        }
    }
}
