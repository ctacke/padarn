using System;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Text;

namespace SampleSite.Providers
{
    public class BeerManager
    {
        private static readonly string m_conString = BeerDatabaseConfig.ConnectionString;

        public static DataTable GetBeerInfo(int id)
        {
            DataTable product = new DataTable("Product");
            string sql = "SELECT * FROM Beers WHERE ID = @ProductId";
            using (SqlCeDataAdapter sda = new SqlCeDataAdapter())
            {
                SqlCeConnection con = new SqlCeConnection(m_conString);
                SqlCeParameter param = new SqlCeParameter("@ProductId", id);
                sda.SelectCommand = new SqlCeCommand(sql, con);
                sda.SelectCommand.CommandType = CommandType.Text;
                sda.SelectCommand.Parameters.Add(param);

                sda.Fill(product);
            }

            return product;
        }

        public static DataTable GetAllBeers()
        {
            DataTable products = new DataTable("Products");
            string sql = "SELECT * FROM Beers";
            using (SqlCeDataAdapter sda = new SqlCeDataAdapter())
            {
                SqlCeConnection con = new SqlCeConnection(m_conString);
                sda.SelectCommand = new SqlCeCommand(sql, con);
                sda.SelectCommand.CommandType = CommandType.Text;

                sda.Fill(products);
            }

            return products;
        }
    }
}
