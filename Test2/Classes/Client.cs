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
    }
}