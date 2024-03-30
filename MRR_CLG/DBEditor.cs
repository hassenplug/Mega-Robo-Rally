using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace MRR_CLG
{
    public class DBEditor
    {
        private static Database rDBConn = new Database();

        public DBEditor(Database DBConn)
        {
            rDBConn = DBConn;
        }

        public string GetTableNames(string usetable)
        {
            string strSQL = "select TABLE_NAME  from information_schema.TABLES t where TABLE_SCHEMA ='rally' order by TABLE_TYPE , TABLE_NAME  ;";
            string output = "";

            MySqlDataReader reader = rDBConn.Exec(strSQL);
            while (reader.Read())
            {
                output += "<option value='" +  reader[0] + "'";
                if ((string)reader[0] == usetable) output += " selected ";
                output += ">" +  reader[0] + "</option>";
            }

            output = "<select id='tables' onchange='changeToTable();'>" + output + "</select>";
            reader.Close();
            return output;
        }

        public string GetEditor(string readdata)
        {

            var sout = readdata.Split("/");
            var newQuery =  sout[sout.Length-1] ;
//            return rDBConn.GetHTMLfromQuery(newQuery);

            string output = "<html><head>";
            output += "<script src='/jscode.js' type='text/javascript' charset='utf-8'></script>";
            output += "</head><body>";
//            output += "<h1>Database Editor</h1>";
            output +=  GetTableNames(newQuery);
            newQuery = "Select * from " + newQuery;
            output += rDBConn.GetHTMLfromQuery(newQuery);
            output += "</body></html>";

            //Console.WriteLine("output:" + output);

            return output;
        }
    }
}
