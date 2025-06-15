using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AppliNicolas.Classes
{
    public class Client
    {
        private int numClient;
        private string nomClient;
        private string prenomClient;
        private string mailClient;

        public Client()
        {
        }
        public Client(string nomClient, string prenomClient, string mailClient)
        {
            NomClient = nomClient;
            PrenomClient = prenomClient;
            MailClient = mailClient;
        }


        public Client(int numClient, string nomClient, string prenomClient, string mailClient)
        {
            NumClient = numClient;
            NomClient = nomClient;
            PrenomClient = prenomClient;
            MailClient = mailClient;
        }

        public int NumClient
        {
            get { return numClient; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro client doit être positif");
                }
                numClient = value;
            }
        }

        public string NomClient
        {
            get { return nomClient; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom ne peut pas être vide");
                }
                if (value.Length > 50)
                {
                    throw new ArgumentException("Le nom ne peut pas dépasser 50 caractères");
                }
                nomClient = value;
            }
        }

        public string PrenomClient
        {
            get { return prenomClient; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le prénom ne peut pas être vide");
                }
                if (value.Length > 50)
                {
                    throw new ArgumentException("Le prénom ne peut pas dépasser 50 caractères");
                }
                prenomClient = value;
            }
        }

        public string MailClient
        {
            get { return mailClient; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("L'email ne peut pas être vide");
                }
                if (value.Length > 100)
                {
                    throw new ArgumentException("L'email ne peut pas dépasser 100 caractères");
                }
                if (!Regex.IsMatch(value, @"^[a-z0-9]{1,25}(\.[a-z0-9]{1,25})?@[a-z0-9.-]{1,20}\.[a-z]{2,6}$"))
                {
                    throw new ArgumentException("Format d'email invalide");
                }
                mailClient = value;
            }
        }

        public List<Client> RecupereClientDansBDD()
        {
            List<Client> lesClients = new List<Client>();

            try
            {
                using (NpgsqlCommand commandeSelect = new NpgsqlCommand("select * from Client;"))
                {
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(commandeSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        lesClients.Add(new Client((Int32)dr["numclient"], (string)dr["nomclient"],
                            (string)dr["prenomclient"], (string)dr["mailclient"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des clients : {ex.Message}");
            }

            return lesClients;
        }

        public bool VerifierEmailExistantPourAutreClient(string email, int numClientActuel)
        {
            try
            {
                using (NpgsqlCommand commande = new NpgsqlCommand("SELECT COUNT(*) FROM client WHERE LOWER(mailclient) = LOWER(@email) AND numclient != @numClient"))
                {
                    commande.Parameters.AddWithValue("@email", email);
                    commande.Parameters.AddWithValue("@numClient", numClientActuel);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(commande);
                    int count = Convert.ToInt32(dt.Rows[0][0]);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public void ModifierClient(int numClient, string nom, string prenom, string mail)
        {
            try
            {
                using (NpgsqlCommand commandeUpdate = new NpgsqlCommand("UPDATE Client SET nomclient = @nom, prenomclient = @prenom, mailclient = @mail WHERE numclient = @id"))
                {
                    commandeUpdate.Parameters.AddWithValue("@nom", nom);
                    commandeUpdate.Parameters.AddWithValue("@prenom", prenom);
                    commandeUpdate.Parameters.AddWithValue("@mail", mail);
                    commandeUpdate.Parameters.AddWithValue("@id", numClient);
                    ConnexionBD.Instance.ExecuteSet(commandeUpdate);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour du client : {ex.Message}");
            }
        }

        public void SupprimerClientDeLaBase(int numClient)
        {
            try
            {
                using (NpgsqlCommand commandeDelete = new NpgsqlCommand("DELETE FROM Client WHERE numclient = @numclient"))
                {
                    commandeDelete.Parameters.AddWithValue("@numclient", numClient);
                    ConnexionBD.Instance.ExecuteSet(commandeDelete);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la suppression du client : {ex.Message}");
            }
        }
        public void InsererNouveauClient(string nom, string prenom, string mail)
        {
            if (VerifierEmailExistant(mail))
            {
                throw new ArgumentException("Cette adresse email existe déjà !");
            }
            else
            {
                try
                {
                    using (NpgsqlCommand commandeInsert = new NpgsqlCommand("INSERT INTO Client (nomClient, prenomClient, mailClient) VALUES (@nom, @prenom, @mail)"))
                    {
                        commandeInsert.Parameters.AddWithValue("@nom", nom);
                        commandeInsert.Parameters.AddWithValue("@prenom", prenom);
                        commandeInsert.Parameters.AddWithValue("@mail", mail);
                        ConnexionBD.Instance.ExecuteInsert(commandeInsert);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erreur lors de l'insertion du client : {ex.Message}");
                }
            }
        }

        public bool VerifierEmailExistant(string email)
        {
            try
            {
                using (NpgsqlCommand commande = new NpgsqlCommand("SELECT COUNT(*) FROM client WHERE LOWER(mailclient) = LOWER(@email)"))
                {
                    commande.Parameters.AddWithValue("@email", email);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(commande);
                    int count = Convert.ToInt32(dt.Rows[0][0]);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}