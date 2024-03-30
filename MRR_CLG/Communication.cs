// communicate with robots
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel; //ObservableCollection
using System.ComponentModel;
using System.Data; //INotifyPropertyChanged

namespace MRR_CLG
{

    public class RobotConnections : ObservableCollection<RobotConnection>
    {
        public RobotConnections()
            : base()
        {
        }

        public RobotConnection AddRobot(int pRobotID)
        {
            RobotConnection newbot = new RobotConnection(pRobotID);
            this.Add(newbot);
            return newbot;
        }

        public bool SendMessageToRobot(int robotID, string MessageToSend)
        {
            RobotConnection useRobot = this.FirstOrDefault(r=>r.RobotID == robotID);
            return useRobot.SendMessage(MessageToSend);
        }
    }

    public class RobotConnection
    {
        public RobotConnection(int pRobotID):base()
        {
            RobotID = pRobotID;
            // get mac id for robot
            // and connect
        }

        public bool SendMessage(string MessageToSend)
        {
            return false;
        }

        public int RobotID { get; set; }
        public bool ISConnected { get; set; }

    }

}