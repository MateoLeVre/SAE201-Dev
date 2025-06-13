using AppliNicolas.Pages;
using Npgsql;
using System;
using System.Data;
using AppliNicolas.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AppliNicolas.Classes
{
    public partial class Demande
    {
        private int numDemande;
        private int numVin;
        private int numEmploye;
        private DateTime dateDemande;
        private int quantiteDemande;
        private string etat;
        private Vin vin;

        private static Dictionary<int, Vin> DictionnaireVins = new Dictionary<int, Vin>();
        private static List<Vin> TousLesVins = ((MainWindow)Application.Current.MainWindow).GestionVin.LesVins.ToList();

        public Demande()
        {
        }

        public Demande(int numDemande, int numVin, int numEmploye, int quantiteDemande, string etat)
        {
            this.NumDemande = numDemande;
            this.NumVin = numVin;
            this.NumEmploye = numEmploye;
            this.DateDemande = DateTime.Now;
            this.QuantiteDemande = quantiteDemande;
            this.Etat = etat;
        }

        public Demande(int numDemande, int numVin, int numEmploye, DateTime dateDemande, int quantiteDemande, string etat)
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
            get { return numDemande; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro de demande doit être positif");
                }
                numDemande = value;
            }
        }

        public int NumVin
        {
            get { return numVin; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro de vin doit être positif");
                }
                numVin = value;
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

        public DateTime DateDemande
        {
            get { return dateDemande; }
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("La date de demande ne peut pas être dans le futur");
                }
                dateDemande = value;
            }
        }

        public int QuantiteDemande
        {
            get { return quantiteDemande; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("La quantité demandée doit être positive");
                }
                quantiteDemande = value;
            }
        }

        public string Etat
        {
            get { return etat; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("L'état ne peut pas être vide");
                }
                if (value.Length > 20)
                {
                    throw new ArgumentException("L'état ne peut pas dépasser 20 caractères");
                }
                etat = value;
            }
        }

        public string NomVin
        {
            get { return Vin.Nom; }
        }

        public string DateDemandeFormatted
        {
            get { return DateDemande.ToString("dd/MM/yyyy"); }
        }

        public double MontantTotal
        {
            get
            {
                return Vin.Prix * QuantiteDemande;
            }
        }

        public Vin Vin
        {
            get
            {
                if (vin == null)
                {
                    vin = RecupereVinParId(NumVin);
                }
                return vin;
            }
        }

        public string Description
        {
            get
            {
                return $"Vin : {Vin.Nom} \n Fournisseur : {Vin.NomFournisseur} \n Qté : {QuantiteDemande} \n État : {Etat} \n Prix : {MontantTotal} €";
            }
        }

        private Vin RecupereVinParId(int idVin)
        {
            if (!DictionnaireVins.ContainsKey(idVin))
            {
                Vin vinTrouve = TousLesVins.FirstOrDefault(v => v.Reference == idVin);
                if (vinTrouve != null)
                {
                    DictionnaireVins.Add(idVin, vinTrouve);
                }
                else
                {
                    throw new InvalidOperationException($"Vin avec l'id {idVin} non trouvé.");
                }
            }

            return DictionnaireVins[idVin];
        }

        public List<Demande> RecupereDemandeDansBDD()
        {
            List<Demande> lesDemandes = new List<Demande>();

            try
            {
                using (NpgsqlCommand demandeSelect = new NpgsqlCommand("SELECT * FROM Demande;"))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(demandeSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        lesDemandes.Add(new Demande(
                            (int)dr["numdemande"],
                            (int)dr["numVin"],
                            (int)dr["numemploye"],
                            (DateTime)dr["datedemande"],
                            (int)dr["quantitedemande"],
                            (string)dr["etat"]
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des demandes : {ex.Message}");
            }

            return lesDemandes;
        }
    }
}