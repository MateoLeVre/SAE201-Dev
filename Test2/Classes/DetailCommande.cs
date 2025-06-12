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



        //Pour le prix du vin il faut que je code le truc du vin// 
        private Vin vin;
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

        public DetailCommande(){ }

        public DetailCommande(int numCommande, int numVin, int quantite, double prix, int numDemande)
        {
            this.NumCommande = numCommande;
            this.NumVin = numVin;
            this.Quantite = quantite;
            this.Prix = prix;
            this.NumDemande = numDemande;
        }

        // Prix unitaire
        public double Prix
        {
            get
            {
                return prix;
            }

            set
            {
                prix = value;
            }
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

        public int Quantite
        {
            get
            {
                return quantite;
            }

            set
            {
                quantite = value;
            }
        }

        private int numDemande;
        public int NumDemande
        {
            get => numDemande;
            set => numDemande = value;
        }


        // Prix total de l'association vin/commande
        public double PrixDetail
        {
            get
            {
                return Quantite * Prix;
            }
        }
        private Demande demandeDeDetail;
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

            return details;
        }

        public static List<DetailCommande> ChargerDetailsParCommande(int numCommande)
        {
            List<DetailCommande> details = new List<DetailCommande>();

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

            return details;
        }


        public override string? ToString()
        {
            return "Num commande "+ this.NumCommande + " numvin "+this.NumVin
                +" quantite " +this.Quantite + " prix  " +this.Prix +" prix 2 : " +this.PrixDetail;
        }
    }
}
