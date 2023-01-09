using System;
using Npgsql;
using Newtonsoft.Json;
using System.IO;

namespace LabaDb2
{
    public class Config
    {
        public static string ConfigPath { get; } = AppDomain.CurrentDomain.BaseDirectory;

        [JsonProperty(Required = Required.Always)]
        public string Host { get; private set; }
        [JsonProperty(Required = Required.Always)]
        public string User { get; private set; }
        [JsonProperty(Required = Required.Always)]
        public string DBname { get; private set; }
        [JsonProperty(Required = Required.Always)]
        public string Password { get; private set; }
        [JsonProperty(Required = Required.Always)]
        public string Port { get; private set; }

        public static Config Read(out string error)
        {
            Config cfg = null;
            error = "";
            if (!File.Exists(ConfigPath + "config.json"))
            {
                error = "Config dont exist.";
                return cfg;
            }

            string str = File.ReadAllText(ConfigPath + "config.json");
            try
            {
                cfg = JsonConvert.DeserializeObject<Config>(str);
            }
            catch (Exception e)
            {
                error = e.Message;
                return cfg;
            }

            return cfg;
        }
    }

    public class DB_helper
    {
        public Config config;
        public NpgsqlConnection npgsqlConnection;

        public DB_helper(Config config)
        {
            this.config = config;
        }

        public bool Connect(out string error)
        {
            error = "";
            string connString = $"Server={config.Host};Username={config.User};Database={config.DBname};Port={config.Port};Password={config.Password};SSLMode=Prefer";
            npgsqlConnection = new NpgsqlConnection(connString);

            try
            {
                npgsqlConnection.Open();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

            return true;
        }

        public NpgsqlDataReader StrResponseCommand(string args, out string error)
        {
            error = "";

            try
            {
                using (var command = new NpgsqlCommand(args, npgsqlConnection))
                {
                    var reader = command.ExecuteReader();
                    return reader;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }


        public int IntResponseCommand(string args, out string error)
        {
            error = "";

            try
            {
                using (var command = new NpgsqlCommand(args, npgsqlConnection))
                {
                    int nRows = command.ExecuteNonQuery();
                    return nRows;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return -1;
            }
        }


        public bool Close(out string error)
        {
            error = "";

            try
            {
                npgsqlConnection.Close();
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

            return true;
        }
    }
}
