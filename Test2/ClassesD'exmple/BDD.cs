using AppliNicolas.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.ClassesD_exmple
{
    public class BDD
    {
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
                        (Int32)dr["quantitedemande"], (EtatDemande)(Int32)dr["etat"]));
                }
            }
            return lesDemandes;
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
                        (DateTime)dr["datecommande"], (string)dr["etat"]));
                }
            }
            return lesCommandes;
        }

        public List<Client> RecupereClientDansBDD()
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand commandeSelect = new NpgsqlCommand("select * from Client;"))
            {
                DataTable dt = ConnexionBD.Instance.ExecuteSelect(commandeSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesClients.Add(new Client((Int32)dr["numclient"], (string)dr["nomclient"],
                        (string)dr["prenomclient"], (string)dr["mailclient"]));
                }
            }
            return lesClients;
        }
    }
}
