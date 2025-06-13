using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class Employe
    {
        private int numEmploye;
        private int numRole;
        private string nom;
        private string prenom;
        private string login;
        private string motDePasse;

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

        public int NumRole
        {
            get { return numRole; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro de rôle doit être positif");
                }
                numRole = value;
            }
        }

        public string Nom
        {
            get { return nom; }
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
                nom = value;
            }
        }

        public string Prenom
        {
            get { return prenom; }
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
                prenom = value;
            }
        }

        public string Login
        {
            get { return login; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le login ne peut pas être vide");
                }
                if (value.Length > 30)
                {
                    throw new ArgumentException("Le login ne peut pas dépasser 30 caractères");
                }
                login = value;
            }
        }

        public string MotDePasse
        {
            get { return motDePasse; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le mot de passe ne peut pas être vide");
                }
                if (value.Length > 100)
                {
                    throw new ArgumentException("Le mot de passe ne peut pas dépasser 100 caractères");
                }
                motDePasse = value;
            }
        }

        public bool EstResponsable
        {
            get { return NumRole == 2; }
        }

        public static Employe Connexion(string login, string password)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM employe WHERE login = @login "))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    DataTable dt = ConnexionBD.Instance.ExecuteSelect(cmd);

                    if (dt.Rows.Count >= 1)
                    {
                        DataRow row = dt.Rows[0];
                        Employe employe = new Employe();

                        // Assignation directe aux champs privés pour éviter les validations
                        employe.numEmploye = (int)row["numemploye"];
                        employe.numRole = (int)row["numrole"];
                        employe.nom = (string)row["nom"];
                        employe.prenom = (string)row["prenom"];
                        employe.login = (string)row["login"];
                        employe.motDePasse = (string)row["mdp"];

                        return employe;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la connexion de l'employé : {ex.Message}");
            }

            return null;
        }
    }
}