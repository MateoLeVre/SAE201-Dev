using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class DetailCommande
    {
        private int numCommande;
        private int numVin;
        private int quantite;
        private double prix;
        private int numDemande;
        private Vin vin;
        private Demande demandeDeDetail;

        public DetailCommande() { }

        public DetailCommande(int numCommande, int numVin, int quantite, double prix, int numDemande)
        {
            this.NumCommande = numCommande;
            this.NumVin = numVin;
            this.Quantite = quantite;
            this.Prix = prix;
            this.NumDemande = numDemande;
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

        public int Quantite
        {
            get { return quantite; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("La quantité doit être positive");
                }
                quantite = value;
            }
        }

        public double Prix
        {
            get { return prix; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Le prix ne peut pas être négatif");
                }
                prix = value;
            }
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

        public Vin Vin
        {
            get
            {
                vin = ((MainWindow)System.Windows.Application.Current.MainWindow)
                        .GestionVin.LesVins
                        .FirstOrDefault(v => v.Reference == NumVin);

                return vin;
            }
        }

        public double PrixDetail
        {
            get
            {
                return Quantite * Prix;
            }
        }

        public Demande DemandeDeDetail
        {
            get
            {
                demandeDeDetail = ((MainWindow)System.Windows.Application.Current.MainWindow)
                            .GestionVin.LesDemandes
                            .FirstOrDefault(d => d.NumDemande == NumDemande);

                return demandeDeDetail;
            }
        }

        public static List<DetailCommande> ChargerDetails()
        {
            List<DetailCommande> details = new List<DetailCommande>();

            try
            {
                using (var cmd = new NpgsqlCommand("SELECT * FROM detailcommande"))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);

                    foreach (DataRow row in dt.Rows)
                    {
                        details.Add(new DetailCommande
                        {
                            numCommande = (int)row["numcommande"],
                            numVin = (int)row["numvin"],
                            quantite = (int)row["quantite"],
                            prix = Convert.ToDouble(row["prix"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des détails de commande : {ex.Message}");
            }

            return details;
        }

        public static List<DetailCommande> ChargerDetailsParCommande(int numCommande)
        {
            List<DetailCommande> details = new List<DetailCommande>();

            try
            {
                using (var cmd = new NpgsqlCommand("SELECT * FROM detailcommande WHERE numcommande = @num"))
                {
                    cmd.Parameters.AddWithValue("@num", numCommande);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);

                    foreach (DataRow row in dt.Rows)
                    {
                        var detail = new DetailCommande(
                            (int)row["numcommande"],
                            (int)row["numvin"],
                            (int)row["quantite"],
                            Convert.ToDouble(row["prix"]),
                            (int)row["numdemande"]
                        );

                        details.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des détails pour la commande {numCommande} : {ex.Message}");
            }

            return details;
        }

        public override string? ToString()
        {
            return "Num commande " + this.NumCommande + " numvin " + this.NumVin
                + " quantite " + this.Quantite + " prix " + this.Prix + " prix total : " + this.PrixDetail;
        }
    }
}