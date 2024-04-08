using System.Threading.Tasks;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Cache;

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

// check for connection to each active robot
// get list of active commands

    public class CommandListProcessor
    {
        private Database DBConn;
        private Players RobotList;
        private CommandList RobotCommandList;
        
        public CommandListProcessor(Database ldb)
        {
            DBConn = ldb;

            RRGame tempgame = new RRGame(DBConn);

            RobotList = new Players(tempgame); // load robot list from db

        }

        public void UpdateCommandList()
        {
            
        }

        public bool CheckPendingCommands()
        {
            string strSQL = "select count(*) from viewCommandListActive;";
            return DBConn.GetIntFromDB(strSQL)>0;
        }

        public void ProcessList()
        {
            while(CheckPendingCommands())  // continue to execute commands while they exist
            {

            }
        }

        // function to wait for reply from robots
        // wait for reply from robots

        // wait for input from users

        // process all commands
        public string ProcessCommand()
        {
            // find list of commands
            // send commands to robots


            while(true)
            {

            }
            //return null;
        }

        public string ProcessCommands()
        {
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

            return null;
        }



    }

}