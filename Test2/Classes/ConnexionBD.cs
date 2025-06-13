using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppliNicolas.Classes
{
    public class ConnexionBD
    {
        private string login;
        private string password;

        

        private static readonly ConnexionBD instance = new ConnexionBD();
        //private readonly string connectionString = "Host=srv-peda-new;Port=5433;Username=gibertk;Password=TfRfKc;Database=bd_nicolas;Options='-c search_path=nicolas'";


        //Chaine de connection bdd local 
        private string connectionString;


        private NpgsqlConnection connection;

        public static ConnexionBD Instance
        {
            get
            {
                return instance;
            }
        }

        public void ConfigurerConnexion(string login, string password)
        {
            if (login == "home")
                connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=SAE201";
            else
            {
                string chaine = $"Host=srv-peda-new;Port=5433;Username={login};Password={password};Database=bd_nicolas;Options='-c search_path=nicolas'";
                connectionString = chaine;
            }
            
            connection = new NpgsqlConnection(connectionString);
        }

        //  Constructeur privé pour empêcher l'instanciation multiple
        private ConnexionBD()
        {
        }


        // pour récupérer la connexion (et l'ouvrir si nécessaire)
        public NpgsqlConnection GetConnection()
        {
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                    throw;
                }
            }


            return connection;
        }

        //  pour requêtes SELECT et retourne un DataTable ( table de données en mémoire)
        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = GetConnection();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL");
                throw;
            }
            return dataTable;
        }

        //   pour requêtes INSERT et renvoie l'ID généré

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                if (cmd == null)
                {
                    throw new ArgumentNullException(nameof(cmd), "La commande ne peut pas être null");
                }

                var connection = GetConnection();
                if (connection == null)
                {
                    throw new InvalidOperationException("Impossible d'obtenir une connexion à la base de données");
                }

                cmd.Connection = connection;
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete insert " + cmd?.CommandText);
                throw;
            }
            return nb;
        }

        //  pour requêtes UPDATE, DELETE
        public int ExecuteSet(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                throw;
            }
            return nb;

        }

        // pour requêtes avec une seule valeur retour  (ex : COUNT, SUM) 
        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            object res = null;
            try
            {
                cmd.Connection = GetConnection();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete select " + cmd.CommandText);
                throw;
            }
            return res;

        }

        //  Fermer la connexion 
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
