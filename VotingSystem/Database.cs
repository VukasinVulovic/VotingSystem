using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Security.Cryptography;

namespace Types
{
    internal class Voter
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
    }

    internal class Candidate
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public override string ToString() => $"{{\"title\":\"{Title.Replace("\\", "\\\\").Replace("\"", "\\\"")}\",\"description\":\"{Description.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"}}";
    }

    internal class Token
    {
        public string Value { get; set; }
        public long Date { get; set; }
    }
}

namespace DbTools
{
    internal class DatabaseInstance
    {
        private static Random rng;
        private string pathName;
        private SQLiteConnection conn;
        private Dictionary<string, Types.Voter> voters;
        public Dictionary<string, Types.Voter> Voters { get => voters; set => voters = value; }

        public DatabaseInstance(string path)
        {
            pathName = path;
            conn = new SQLiteConnection($"Data Source={pathName}; Version=3; New=True; Compress=True;");

            voters = new Dictionary<string, Types.Voter>();

            conn.Open();
        } //connect to database

        public bool LoadVoters()
        {
            voters.Clear();

            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand("SELECT * FROM Voters", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return false;
            }

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Types.Voter voter = new Types.Voter();

                voter.Id = reader.GetInt32(0);
                voter.Token = reader.IsDBNull(1) ? null : reader.GetString(1);
                voter.FName = reader.GetString(2);
                voter.LName = reader.GetString(3);
                voter.Hash = reader.GetString(4);
                voter.Salt = reader.GetString(5);

                voters[voter.Id.ToString()] = voter;
            }

            cmd.Dispose();
            return true;
        } //load voters from db

        public Types.Voter GetVoter(string id, string key)
        {
            if (voters.ContainsKey(id) && (voters[id].Hash == App.Crypto.HashSha256(key + voters[id].Salt)))//if voter is loaded / exists; and key is ok
                return voters[id];

            return null;
        } //get voter if all criteria is met

        public Types.Token GetToken(int id)
        {
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand($"SELECT Tokens.Token, Tokens.Date FROM Voters INNER JOIN Tokens ON Voters.Token=Tokens.Token WHERE Voters.Id={id};", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return null;
            }

            SQLiteDataReader reader = cmd.ExecuteReader();
            Types.Token token = new Types.Token();

            if (!reader.HasRows)
            {
                if (rng == null)
                    rng = new Random();

                token.Value = App.Crypto.HashSha256(rng.Next() + "");
                token.Date = 0;

                CreateToken(token.Value, id);

                cmd.Dispose();
                return token;
            }

            while (reader.Read())
            {
                token.Value = reader.IsDBNull(0) ? null : reader.GetString(0);
                token.Date = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
            }

            cmd.Dispose();
            return token;
        }

        public long GetTokenDate(string token)
        {
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand($"SELECT Token, Date FROM Tokens WHERE Token='{token}';", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return -1;
            }

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cmd.Dispose();
                return reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
            }

            cmd.Dispose();
            return -1;
        }

        public bool CreateToken(string token, int id)
        {
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand($"INSERT INTO Tokens(token, date) VALUES('{token}', NULL)", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return false;
            }

            try
            {
                cmd = new SQLiteCommand($"UPDATE Voters SET Token = '{token}' WHERE Id = {id};", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return false;
            }

            cmd.Dispose();
            return true;
        }

        public bool UpdateTokenDate(string token, long date)
        {
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand($"UPDATE Tokens SET Date = {date} WHERE Token = '{token}';", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return false;
            }

            cmd.Dispose();
            return true;
        }

        public bool RemoveToken(string token)
        {
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand($"DELETE FROM Tokens WHERE Token = '{token}'; UPDATE Voters SET Token = NULL WHERE Token = '{token}';", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return false;
            }

            cmd.Dispose();
            return true;
        }

        public bool RemoveAllTokens()
        {
            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand($"DELETE FROM Tokens; UPDATE Voters SET Token = NULL;", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                cmd.Dispose();
                return false;
            }

            cmd.Dispose();
            return true;
        }
    }
}

namespace App
{
    internal class Database
    {
        private static DbTools.DatabaseInstance dbInstance;
        public static Dictionary<string, Types.Voter> Voters { 
            get => (dbInstance == null) ? null : dbInstance.Voters;
        }

        public static void Init(string path)
        {
            dbInstance = new DbTools.DatabaseInstance(path);
        }

        public static bool LoadVoters() => dbInstance.LoadVoters();

        public static Types.Voter GetVoter(string id, string key) => dbInstance.GetVoter(id, key);

        public static Types.Token GetToken(int id) => dbInstance.GetToken(id);

        public static long GetTokenDate(string token) => dbInstance.GetTokenDate(token);

        public static bool UpdateTokenDate(string token, long date) => dbInstance.UpdateTokenDate(token, date);

        public static bool RemoveAllTokens() => dbInstance.RemoveAllTokens();
    }
}
