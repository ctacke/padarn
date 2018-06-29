using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;

namespace SampleSite.Providers
{
    internal static class BeerDatabaseConfig
    {
        public static Beer[] SampleData = new Beer[] {
            new Beer(-1, "Big Sky Brewery", "Moose Dro0l", false),
            new Beer(-1, "Big Sky Brewery", "Scape Goat", false),
            new Beer(-1, "Big Sky Brewery", "Trout Slayer", false),
            new Beer(-1, "Big Sky Brewery", "Powder Hound", false),
            new Beer(-1, "New Glarus Brewery", "Stone Soup", false),
            new Beer(-1, "New Glarus Brewery", "Yokel", false),
            new Beer(-1, "New Glarus Brewery", "Hop Heart Ale", false),
            new Beer(-1, "New Glarus Brewery", "Uff-da", false),
            new Beer(-1, "New Glarus Brewery", "Fat Squirrel", false),
            new Beer(-1, "Harvest Moon Brewery", "Pig's Ass Porter", false),
            new Beer(-1, "Lion Brewery", "Gibbon's Famous Lager", true),
            new Beer(-1, "Lion Brewery", "Pocono Lager", true),
            new Beer(-1, "Lion Brewery", "Pocono Blonde", false),
            new Beer(-1, "Lion Brewery", "Pocono Amber", false),
            new Beer(-1, "Lion Brewery", "Pocono Pale Ale", false),
            new Beer(-1, "D.G. Yuengling and Son", "Yuengling Lager", true),
            new Beer(-1, "D.G. Yuengling and Son", "Black and Tan", false),
            new Beer(-1, "Three Floyds Brewing Co", "Drunk Monk", false),
            new Beer(-1, "Three Floyds Brewing Co", "Alpha King", false),
            new Beer(-1, "Three Floyds Brewing Co", "Alpha Kong", false),
            new Beer(-1, "Three Floyds Brewing Co", "Gumballhead", false),
            new Beer(-1, "Three Floyds Brewing Co", "Robert the Bruce", false),
            new Beer(-1, "Three Floyds Brewing Co", "Dreadnaught IPA", false),
            new Beer(-1, "Three Floyds Brewing Co", "Pride and Joy", false),
            new Beer(-1, "Purple Moose Brewery", "Snowdonia Ale", false),
            new Beer(-1, "Purple Moose Brewery", "Dark Side of the Moose", false),
            new Beer(-1, "Purple Moose Brewery", "Madog's Ale", false),
            new Beer(-1, "Purple Moose Brewery", "Glaslyn Ale", false),
            new Beer(-1, "Purple Moose Brewery", "Merry X-Moose", false),
        };

        private const string DatabaseFilePath = "\\TestDB.sdf";

        public static string ConnectionString
        {
            get { return string.Format("Data Source={0}", DatabaseFilePath); }
        }

        public static void CreateDatabase()
        {
            if (File.Exists(DatabaseFilePath)) return;

            SqlCeEngine engine = new SqlCeEngine(ConnectionString);
            engine.CreateDatabase();

            using (SqlCeConnection connection = new SqlCeConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCeCommand createCmd = new SqlCeCommand("CREATE TABLE Beers(ID int identity, Brewery nvarchar(500), Name nvarchar(500), IsLager bit)", connection))
                {
                    createCmd.ExecuteNonQuery();
                }

                using (SqlCeCommand insertCmd = new SqlCeCommand("INSERT INTO Beers (Brewery, Name, IsLager) VALUES (?, ?, ?)", connection))
                {
                    insertCmd.Parameters.Add("brewery", SqlDbType.NVarChar);
                    insertCmd.Parameters.Add("name", SqlDbType.NVarChar);
                    insertCmd.Parameters.Add("isLager", SqlDbType.Bit);
                    insertCmd.Prepare();

                    using (SqlCeCommand identityCmd = new SqlCeCommand("SELECT @@IDENTITY", connection))
                    {
                        identityCmd.Prepare();
                        int identity = -1;

                        foreach (Beer beer in SampleData)
                        {
                            insertCmd.Parameters["brewery"].Value = beer.Brewery;
                            insertCmd.Parameters["name"].Value = beer.Name;
                            insertCmd.Parameters["isLager"].Value = beer.IsLager;

                            if (insertCmd.ExecuteNonQuery() != 1)
                            {
                                throw new Exception("Insert failed");
                            }

                            identity = Convert.ToInt32(identityCmd.ExecuteScalar());
                        }
                    } // using (SqlCeCommand identityCmd...
                } // using (SqlCeCommand insertCmd...

                using (SqlCeCommand indexCmd = new SqlCeCommand())
                {
                    indexCmd.Connection = connection;

                    indexCmd.CommandText = "CREATE INDEX IDX_Brewery ON Beers (Brewery)";
                    indexCmd.ExecuteNonQuery();

                    indexCmd.CommandText = "CREATE INDEX IDX_Name ON Beers (Name)";
                    indexCmd.ExecuteNonQuery();
                } // using (SqlCeCommand indexCmd
            } // using (SqlCeConnection connection...
        }
    }
}
