using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;



namespace MRR_CLG
{
    class Program
    {

        private static TcpListener myListener;
        private static int port = 5050;
//        private static IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        private static IPAddress localAddr = IPAddress.Parse("0.0.0.0");
        private static string WebServerPath = "../web";
        private static string serverEtag = Guid.NewGuid().ToString("N");

        private static Database rDBConn = new Database();

        private static RRGame rRGame = new RRGame(rDBConn);

        private static LEDs rLED = new LEDs(rDBConn);

        private static DBEditor rdbeditor = new DBEditor(rDBConn);

        private static PendingCommands rPendingCommands = new PendingCommands(rDBConn);

        private static Dictionary<IPAddress, TcpClient> clientlist = new Dictionary<IPAddress, TcpClient>();


        static void Main(string[] args)
        {
            try
            {

                myListener = new TcpListener(localAddr, port);
                myListener.Start();
                Console.WriteLine($"Web Server Running on {localAddr.ToString()} on port {port}... Press ^C to Stop...");
                Thread th = new Thread(new ThreadStart(StartListen));
                th.Start();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        private static string GetQuery(string query)
        {
            var sout = query.Split("/");
            //Console.WriteLine("query:" + sout[sout.Length-1]);
            var newQuery = "Select * from " + sout[sout.Length-1] + ";";
            //var newQuery = "" + sout[sout.Length-1] + ";";
            return rDBConn.GetHTMLfromQuery(newQuery);
        }

        private static string GetLED(string query)
        {
            var sout = query.Split("/");
            int iout;
            int.TryParse(sout[sout.Length-1], out iout);
            //Console.WriteLine("query:" + sout[sout.Length-1]);
            return rLED.Animation(iout);
        }

        // commands
        // 2 = validate position
        // 1 = update card /player/card id/ position

        private static string UpdatePlayer(string request)
        {
            Console.WriteLine("Update: " + request);
            // update/player/card/removefrom/
            
            string[] requestSplit = request.Split('/');
            string commandID = requestSplit[2];
            string playerid = requestSplit[3];
            switch (commandID)
            {
                case "1": 
                    string cardid = requestSplit[4];
                    string position = requestSplit[5];
                    rDBConn.Command("call procUpdateCardPlayed(" + playerid + "," + cardid + "," + position + ");");
                    // check to see if we an go to next state
                    break;
                case "2":
                    string positionValid = requestSplit[4];
                    // clear message
                    rDBConn.Command("update Robots set PositionValid=" + positionValid + " where RobotID=" + playerid + ";");
                    break;
                case "3":
                    int markcommand = rDBConn.GetIntFromDB("Select MessageCommandID from Robots where RobotID=" + playerid);
                    rDBConn.Command("update Robots set MessageCommandID=null where RobotID=" + playerid + ";");
                    rDBConn.Command("update CommandList set StatusID=6 where CommandID=" + markcommand + ";");
                    break;

            }
            // check to see if we an go to next state
            //select funcGetNextGameState();
            
            //var gamestate = rDBConn.Exec("select funcGetNextGameState();"); //going to next state?
            var gamestate = rDBConn.Command("select funcGetNextGameState();"); //going to next state?
            
            if (rRGame.UpdateGameState() == 6)
            {
                rRGame.ExecuteTurn();
            }
           // Console.WriteLine(gamestate);
            //Console.WriteLine( "Game State:",gamestate);
            //if (gamestate == 6)
            //{
            //    Console.WriteLine( rRGame.ExecuteTurn());
            //}
            //rDBConn.Command("select funcGetNextGameState();"); //going to next state?
            return MakeRobotsJson(request);

        }

        static void HandleConnection(TcpClient client)
        {

            NetworkStream stream = client.GetStream();

            //read request 
            byte[] requestBytes = new byte[1024];
            int bytesRead = stream.Read(requestBytes, 0, requestBytes.Length);

            string request = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
            var requestHeaders = ParseHeaders(request);

            string[] requestFirstLine = requestHeaders.requestType.Split(" ");
            string httpVersion = requestFirstLine.LastOrDefault();
            string contentType = requestHeaders.headers.GetValueOrDefault("Accept");
            string contentEncoding = requestHeaders.headers.GetValueOrDefault("Acept-Encoding");

            if (!request.StartsWith("GET"))
            {
                SendHeaders(httpVersion, 405, "Method Not Allowed", contentType, contentEncoding, 0, ref stream);
            }
            else
            {
                var requestedPath = requestFirstLine[1];
                if (requestedPath == "/ws")
                {
                    var addr = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                    if (!clientlist.ContainsKey(addr))
                    {
                        clientlist.Add(addr,client);
                        Console.WriteLine("Client: {0} {1} ",addr,clientlist.Count.ToString());
                        // Insert your code here. (Do not accept socket again here)
                        client.Close();
                        return;
                    }
                }

                var fileContent = GetContent(requestedPath);
                if(fileContent is not null)
                {
                    SendHeaders(httpVersion, 200, "OK", contentType, contentEncoding, 0, ref stream);
                    stream.Write(fileContent, 0, fileContent.Length);
                }
                else
                {
                    SendHeaders(httpVersion, 404, "Page Not Found", contentType, contentEncoding, 0, ref stream);
                }
            }

            client.Close();

        }

        private static void StartListen()
        {
            while (true)
            {
             
                TcpClient client = myListener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(state => HandleConnection(client));

            }
        }

        private static byte[] GetContent(string requestedPath)
        {
            if (requestedPath == "/") requestedPath = "index.html";
            string filePath = Path.Join(WebServerPath, requestedPath);

            //Console.WriteLine(requestedPath + " " + requestedPath.Length);
            Console.WriteLine(requestedPath);

            if (requestedPath == "/executeturn")
            {
                return  Encoding.ASCII.GetBytes( rRGame.ExecuteTurn());
            }
            else if (requestedPath == "/makerobotsjson")
            {
                return  Encoding.ASCII.GetBytes(MakeRobotsJson(filePath));
            }
            else if (requestedPath.Contains("/query"))
            {
                return  Encoding.ASCII.GetBytes(GetQuery(requestedPath));
            }
            else if (requestedPath.Contains("/nextstate"))
            {
                return  Encoding.ASCII.GetBytes(NextState());
            }
            else if (requestedPath.Contains("/startgame"))
            {
                return  Encoding.ASCII.GetBytes(StartGame(requestedPath));
            }
            else if (requestedPath.Contains("/processcommands"))
            {
                rPendingCommands.ProcessCommands();
                return  Encoding.ASCII.GetBytes(MakeRobotsJson(filePath));
            }
            else if (requestedPath.Contains("led"))
            {
                return  Encoding.ASCII.GetBytes(GetLED(requestedPath));
            }
            else if (requestedPath.Contains("dbeditor"))
            {
               return  Encoding.ASCII.GetBytes(rdbeditor.GetEditor(requestedPath));
            }
            else if (requestedPath.Length > 13 && requestedPath.Substring(1,12) == "updatePlayer")
            {
                return  Encoding.ASCII.GetBytes(UpdatePlayer(requestedPath)) ; //Encoding.ASCII.GetBytes(MakeRobotsJson(filePath));
            }
            else if (!File.Exists(filePath)) 
            {
                return null;
            }
            else
            {
                byte[] file = System.IO.File.ReadAllBytes(filePath);
                return file;
            }
        }

//        private static void SendHeaders(string? httpVersion, int statusCode, string statusMsg, string? contentType, string? contentEncoding,
        private static void SendHeaders(string httpVersion, int statusCode, string statusMsg, string contentType, string contentEncoding,
            int byteLength, ref NetworkStream networkStream)
        {
            string responseHeaderBuffer = "";

            responseHeaderBuffer = $"HTTP/1.1 {statusCode} {statusMsg}\r\n" +
                $"Connection: Keep-Alive\r\n" +
                $"Date: {DateTime.UtcNow.ToString()}\r\n" +
                $"Server: MacOs PC \r\n" +
                $"Etag: \"{serverEtag}\"\r\n" +
                $"Content-Encoding: {contentEncoding}\r\n" +
                "X-Content-Type-Options: nosniff"+
                $"Content-Type: application/signed-exchange;v=b3\r\n\r\n";

            byte[] responseBytes = Encoding.UTF8.GetBytes(responseHeaderBuffer);
            networkStream.Write(responseBytes, 0, responseBytes.Length);
        }

        private static (Dictionary<string, string> headers, string requestType) ParseHeaders(string headerString)
        {
            var headerLines = headerString.Split('\r', '\n');
            string firstLine = headerLines[0];
            var headerValues = new Dictionary<string, string>();
            foreach (var headerLine in headerLines)
            {
                var headerDetail = headerLine.Trim();
                var delimiterIndex = headerLine.IndexOf(':');
                if (delimiterIndex >= 0)
                {
                    var headerName = headerLine.Substring(0, delimiterIndex).Trim();
                    var headerValue = headerLine.Substring(delimiterIndex + 1).Trim();
                    headerValues.Add(headerName, headerValue);
                }
            }
            return (headerValues, firstLine);
        }

        public static string MakeCardJson(int playerID)
        {
            string strSQL = "Select * from MoveCards where Owner=" + playerID.ToString() + " order by CardID";
            string result = rDBConn.jsonFromQuery(strSQL);
//            result = result.Replace("\"Cardlist["+playerID+"]\"", result1);
            return result;

        }


        public static string MakeRobotsJson(string filename)
        {
            string strSQL = "select * from viewRobots;";
            string result = rDBConn.jsonFromQuery(strSQL);

            for (int c=1;c<9;c++)
            {
                //string result1 = MakeCardJson(c);
                //result = result.Replace("\"Cardlist["+c+"]\"", result1);

            }

            return result;
        }

        public static string NextState()
        {
            var newstate = rDBConn.GetIntFromDB("select funcGetNextGameState(); ");
            Console.WriteLine("next:" + newstate.ToString());
            return "State:" + newstate.ToString();
        }

        public static string StartGame(string request)
        {
            string[] sout = request.Split('/');
            if (sout[sout.Length-1] != "startgame")
            {
                rDBConn.Command("Update CurrentGameData set iValue = " + sout[sout.Length-1] + " where iKey = 26;");  // set game state
            }

            rDBConn.Command("Update CurrentGameData set iValue = 0 where iKey = 10;");  // set state to 0

            var startstate = rDBConn.GetIntFromDB("select funcGetNextGameState(); ");
            //Console.WriteLine("next:" + newstate.ToString());
            return "New Game:" + startstate.ToString();
        }


    }
}

