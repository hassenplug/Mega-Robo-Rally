using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;



namespace MRR_CLG
{
    class Program
    {

        private static Database rDBConn = new Database();

        private static RRGame rRGame = new RRGame(rDBConn);

        static void Main(string[] args)
        {
            rRGame.StartServers();
        }

    }
}

