using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AppliNicolas.Classes
{
    public class Vin
    {
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

        // Constructeur pour rechercher un vin par son numéro
        public Vin(int reference)
        {
            Vin vinTrouve = ChercherVinParReference(reference);
            if (vinTrouve != null)
            {
                this.Nom = vinTrouve.Nom;
                this.NumAppelation = vinTrouve.NumAppelation;
                this.NumType = vinTrouve.NumType;
                this.Millesime = vinTrouve.Millesime;
                this.Photo = vinTrouve.Photo;
                this.Prix = vinTrouve.Prix;
                this.Reference = vinTrouve.Reference;
                this.NumFournisseur = vinTrouve.NumFournisseur;
                this.Detail = vinTrouve.Detail;
                this.NomFournisseur = GetNomFournisseur;
                this.TypeVin = GetType;
                this.AppelationVin = GetAppelation;
            }
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

        public string Nom
        {
            get { return nom; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    nom = "Non défini";
                }
                else
                {
                    if (value.Length > 100)
                    {
                        throw new ArgumentException("Le nom du vin ne peut pas dépasser 100 caractères");
                    }
                    nom = value;
                }
            }
        }

        public int NumAppelation
        {
            get { return numAppelation; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro d'appellation doit être positif");
                }
                numAppelation = value;
            }
        }

        public int NumType
        {
            get { return numType; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro de type doit être positif");
                }
                numType = value;
            }
        }

        public int Millesime
        {
            get { return millesime; }
            set
            {
                if (value < 1900 || value > DateTime.Now.Year)
                {
                    throw new ArgumentException("Millésime incorrect < 1900 ou > à l'année actuelle");
                }
                millesime = value;
            }
        }

        public string Photo
        {
            get { return photo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    photo = "/Images/Vin.jpg";
                }
                else
                {
                    if (value.Length > 200)
                    {
                        throw new ArgumentException("Le chemin de la photo ne peut pas dépasser 200 caractères");
                    }
                    photo = value;
                }
            }
        }

        public double Prix
        {
            get { return prix; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Le prix ne peut être négatif");
                }
                prix = value;
            }
        }

        public int Reference
        {
            get { return reference; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("La référence doit être positive");
                }
                reference = value;
            }
        }

        public int NumFournisseur
        {
            get { return numFournisseur; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro de fournisseur doit être positif");
                }
                numFournisseur = value;
            }
        }

        public string Detail
        {
            get { return detail; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    detail = "Aucun détail";
                }
                else
                {
                    if (value.Length > 500)
                    {
                        throw new ArgumentException("Le détail ne peut pas dépasser 500 caractères");
                    }
                    detail = value;
                }
            }
        }

        public string NomFournisseur
        {
            get { return nomFournisseur; }
            private set { nomFournisseur = value; }
        }

        public string TypeVin
        {
            get { return typeVin; }
            private set { typeVin = value; }
        }

        public string AppelationVin
        {
            get { return appelationVin; }
            private set { appelationVin = value; }
        }

        public string GetAppelation
        {
            get
            {
                return RecupererAppelation(this.Reference);
            }
        }

        public string GetNomFournisseur
        {
            get
            {
                return RecupererNomFournisseur(this.Reference);
            }
        }

        public string GetType
        {
            get
            {
                return RecupererTypeVin(this.Reference);
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

        public List<Vin> RecupereVinDansBDD()
        {
            List<Vin> lesVins = new List<Vin>();

            try
            {
                using (NpgsqlCommand vinSelect = new NpgsqlCommand("select * from Vin;"))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(vinSelect);
                    foreach (DataRow dr in dt.Rows)
                        lesVins.Add(new Vin((string)dr["nomvin"], (Int32)dr["numtype2"], (Int32)dr["numtype"],
                        (Int32)dr["millesime"], (double)dr["prixvin"], (Int32)dr["numvin"], (Int32)dr["numfournisseur"],
                        (string)dr["descriptif"]));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des vins : {ex.Message}");
            }

            return lesVins;
        }

        public List<Vin> TrouverSimilaires()
        {
            return this.RecupereVinDansBDD()
                .Where(vin => vin.Nom != this.Nom &&
                              this.CalculerDegreRessemblance(vin) >= 1)
                .OrderByDescending(vin => this.CalculerDegreRessemblance(vin))
                .Take(5)
                .ToList();
        }

        // Méthodes dédiées pour les requêtes SQL
        private Vin ChercherVinParReference(int reference)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Vin WHERE numvin = @reference"))
                {
                    cmd.Parameters.AddWithValue("@reference", reference);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        return new Vin((string)dr["nomvin"], (Int32)dr["numtype2"], (Int32)dr["numtype"],
                            (Int32)dr["millesime"], (double)dr["prixvin"], (Int32)dr["numvin"],
                            (Int32)dr["numfournisseur"], (string)dr["descriptif"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la recherche du vin {reference} : {ex.Message}");
            }

            return null;
        }

        private string RecupererAppelation(int reference)
        {
            string appelation = "";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT t.nomappelation FROM vin v JOIN appelation t ON t.numtype2 = v.numtype2 WHERE v.numvin = @reference"))
                {
                    cmd.Parameters.AddWithValue("@reference", reference);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);

                    foreach (DataRow dr in dt.Rows)
                        appelation = (string)dr["nomappelation"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de l'appellation : {ex.Message}");
            }

            return appelation;
        }

        private string RecupererNomFournisseur(int reference)
        {
            string nomFournisseur = "";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT f.nomfournisseur FROM Vin v JOIN fournisseur f ON v.numfournisseur = f.numfournisseur WHERE v.numvin = @reference"))
                {
                    cmd.Parameters.AddWithValue("@reference", reference);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);

                    foreach (DataRow dr in dt.Rows)
                        nomFournisseur = (string)dr["nomfournisseur"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération du nom fournisseur : {ex.Message}");
            }

            return nomFournisseur;
        }

        private string RecupererTypeVin(int reference)
        {
            string typeVin = "";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT t.nomtype FROM vin v JOIN typevin t ON t.numtype = v.numtype WHERE v.numvin = @reference"))
                {
                    cmd.Parameters.AddWithValue("@reference", reference);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);

                    foreach (DataRow dr in dt.Rows)
                        typeVin = (string)dr["nomtype"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération du type de vin : {ex.Message}");
            }

            return typeVin;
        }
    }
}