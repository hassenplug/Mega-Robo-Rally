using System.Threading.Tasks;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Cache;

namespace MRR_CLG
{
    public class WebServer
    {
        private Database DBConn;
        
        public WebServer(Database ldb)
        {
            DBConn = ldb;
        }

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



    }
}

