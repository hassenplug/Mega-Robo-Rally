using System.Threading.Tasks;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Cache;
using Iot.Device.Camera.Settings;
using System.Collections.ObjectModel; //ObservableCollection

namespace MRR_CLG
{

/*

Reset status = 1 where status = 4
newstatus = -1
Get all commands where status >= 3 and status <= 4
    If status = 3 
        if robot command
            Send command to robot
            NewStatus = 4 (wait) or 5 (do not wait)
    else //status = 4
        // Check for reply from robots
        if received, newstatus = 5

    if newstatus >-1, update status in database

*/

// command id
// command type
// robot id
// command to send
// status id
// command cat id

// check for connection to each active robot
// get list of active commands

    public class PendingCommands : ObservableCollection<PendingCommand>
    {
        private Database DBConn;
        private Players RobotList;
        private RRGame lGame;
        
        public PendingCommands(Database ldb)
        {
            DBConn = ldb;

            lGame = new RRGame(DBConn);

            RobotList = new Players(lGame); // load robot list from db

        }

        /// <summary>
        /// get current commands that need processed
        /// </summary>
        public PendingCommands UpdateCommandList()
        {
            // call function to mark ready
            // funcMarkCommandsReady()

            this.Clear();

            // if funcMarkCommandsReady == 0, return 0

            string strSQL = "select CommandID,CommandType,RobotID,BTCommand,StatusID,CommandCatID from viewCommandListActive;";
            
            MySqlConnector.MySqlDataReader reader = DBConn.Exec(strSQL);
            while (reader.Read())
            {
                this.Add(new PendingCommand( (int)reader.GetValue(0), // command id
                                            (int)reader.GetValue(1),  // command type
                                            RobotList.GetPlayer((int)reader.GetValue(2)),  // robot id
                                            (string)reader.GetValue(3),  // bt command
                                            (int)reader.GetValue(4),  // status id
                                            (int)reader.GetValue(5)  // command cat id
                ));
            }

            return this;
        }

        public bool ProcessCommands()
        {
            while (UpdateCommandList().Count > 0)
            {
                foreach(PendingCommand onecommand in this)
                {
                    // process command here
                    // update commandstatus
                }
            }
            return false;
        }


        // function to wait for reply from robots
        // wait for reply from robots

        // wait for input from users

        // process all commands
        //public string ProcessCommands()
        //{
            // get database state
            // switch & process it

            // start thread to call function
            //Thread CommandThread = new Thread(new ThreadStart(WebSocket.RunWebSocket));
            //CommandThread.Name = "RunningWebSocket"; // +Name;
            //CommandThread.Start();
            //Thread.Sleep(1);

            //worker = new QueueWorker(queue,Game);
            //Thread t = new Thread(new ThreadStart(worker.Work));
            //t.Start();

            //worker = new WebSocketClass(queue,Game);
            //Thread t = new Thread(new ThreadStart(worker.Start));
            //t.Start();

        //    return null;
        //}

    }

    public class PendingCommand  //: IComparable
    {

        public PendingCommand(int pcommandid, int pcommandType, Player pplayer, string pbtcommand, int pstatusid, int pcomcat) 
        {
            CommandID = pcommandid;
            CommandType = pcommandType;
            RobotConnection = pplayer;
            CommandString = pbtcommand;
            StatusID = pstatusid;
            CommandCatID = pcomcat;
        }

        public int CommandID {get;set;}
        public int CommandType {get;set;}
        public Player RobotConnection {get;set;}
        public string CommandString {get;set;}
        public int StatusID {get;set;}
        public int CommandCatID {get;set;} 

    }

}