using MySqlConnector;

namespace MRR_CLG
{
    public class Database
    {
        public MySqlConnection Conn;

        public Database()
        {
            Connect();
        }

        public MySqlConnection Connect()
        {
            //string myConnectionString = "server=robopi;uid=root;pwd=rallypass;database=rally;";
            //string myConnectionString = "server=localhost;uid=root;pwd=rallypass;database=rally;";

            //string myConnectionString = "server=192.168.1.135;uid=srr;pwd=rallypass;database=rally;";
//            string myConnectionString = "server=robopi;uid=srr;pwd=rallypass;database=rally;";
            string myConnectionString = "server=mrobopi3;uid=mrr;pwd=rallypass;database=rally;";
//            string myConnectionString = "server=mrobopi1;uid=srr;pwd=rallypass;database=rally;";

            //string myConnectionString = "server=localhost;uid=srr;pwd=rallypass;database=rally;";
            try
            {
                Conn = new MySqlConnection();
                Conn.ConnectionString = myConnectionString;
                //Conn.ConnectionString = ConfigurationManager.AppSettings;
                Conn.Open();
            }
            catch (MySqlConnector.MySqlException ex)
            //catch ()
            {
                //MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }

            return Conn;
        }

        public void Close()
        {
            Conn.Close();
        }

        public MySqlDataReader Exec(string strSQL)
        {
            if (Conn.State == System.Data.ConnectionState.Open)
            {
                try
                {
                
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = strSQL;
                    cmd.Connection = Conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    return cmd.ExecuteReader();
                }
                catch(Exception e)
                {
                    Console.WriteLine("DBError:{0}" , e);

                }
            }

            return null;
        }

        public int Command(string strSQL)
        {
            if (Conn.State == System.Data.ConnectionState.Open)
            {
                // set next game state...
                //Console.WriteLine(strSQL);
                MySqlConnector.MySqlCommand update = Conn.CreateCommand();  // Game.DBConn.Exec(strSQL);
                update.CommandText = strSQL;
                Console.WriteLine(strSQL);
                return update.ExecuteNonQuery();
                
            }
            return 0;
        }

        public int GetIntFromDB(string strSQL)
        {
            var returnset = this.Exec(strSQL);
            var returnval = 0;
            if (returnset.Read())
            {
                //Console.WriteLine(returnset[0]);
                if (returnset[0] == System.DBNull.Value)
                {
                    returnval = 0;
                }
                else
                {
                    returnval = (int)(long)Convert.ToInt64(returnset[0]) ;
                }
                //var returnval1 = returnset[0];
                //int.TryParse(returnset[0],returnval);
                //var returnval2 = (long)returnset[0];
                //returnval = (int)returnval2;

            }
            returnset.Close();
            return returnval;
        }

        public int[] GetIntList(string strSQL)
        {
            var returnset = this.Exec(strSQL);
            List<int> returnvalset = new List<int>();

            if (returnset.Read())
            {
                for(int f=0;f<returnset.FieldCount;f++)
                {
                    //Console.WriteLine(returnset[0]);
                    var returnval = 0;
                    if (returnset[f] != System.DBNull.Value)
                    {
                        returnval = (int)(long)Convert.ToInt64(returnset[f]) ;
                    }
                    returnvalset.Add(returnval);
                }
            }
            returnset.Close();
            return returnvalset.ToArray();
        }


        public string jsonFromQuery(string strSQL)
        {
            string output = "";
            string comma = "";

            MySqlConnector.MySqlDataReader reader = this.Exec(strSQL);
            while (reader.Read())
            {
                string commain = "";
                string localoutput = "";
                for(int c=0;c<reader.FieldCount;c++)
                {
                    localoutput += commain + "\"" + reader.GetName(c) + "\":\"" + reader.GetValue(c) + "\"";
                    commain = ",";
                }
                output += comma + "{" + localoutput + "}";
                comma = ",";
            }

            output = "[" + output + "]";
            reader.Close();

            //Console.WriteLine("output:" + output);

            return output;
        }

        public string GetHTMLfromQuery(string strSQL)
        {
            string output = "";
            string fields = "";

            MySqlConnector.MySqlDataReader reader = this.Exec(strSQL);
            while (reader.Read())
            {
                fields = "";
                output += "<tr>";

                //GetFieldType(Int32)

                for(int c=0;c<reader.FieldCount;c++)
                {
                    fields += "<td style='background-color:#cccccc;'>" + reader.GetName(c) + "</td>" ;
                    //fields += "<td bgcolor=#080808>" + reader.GetName(c) + "</td>" ;
                    output += "<td style='background-color:#eeeeee;'>" + reader.GetValue(c) + "</td>" ;
                }

                output += "</tr>";
            }

            output = "<table width='100%'><tr>" + fields + "</tr>" + output + "</table>";
            reader.Close();

            //Console.WriteLine("output:" + output);

            return output;
        }
    }
}
