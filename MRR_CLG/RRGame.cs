// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Windows;
// using System.ComponentModel;//INotifyPropertyChanged
// using System.Threading;
// using System.Xml.Serialization;  // serializer
// using System.Reflection;
// using System.IO;
// using System.Collections.ObjectModel; // needed for enum?


// namespace MRR_CLG
// {

//     public class RRGame // : INotifyPropertyChanged
//     {

//         public RRGame(Database ldb)
//         {
//         }

//         public RRGame()
//         {
//         }

//         public Database DBConn { get; set; }


//         public Communication rCommunication;

//         public string BoardFileName { get; set; }

//         public int BoardID { get; set; }

//         public int GameState { get; set; }

//         public int RulesVersion { get; set; }

//         public int PhaseCount { get; set; }

//         public int AutoExecute { get; set; }

//         public Players AllPlayers { get; set; }

//         public CommandList ListOfCommands { get; set; }

//         public CardList GameCards { get; set; }

//         [XmlIgnore]
//         public OptionCardList OptionCards { get; set; }

//         public Dictionary<int,string> OptionCardNames = new Dictionary<int, string>();

//         public BoardElementCollection g_BoardElements { get; set; } // = new BoardElementCollection(0, 0);

//         public int CurrentPhase  { get; set; } = 0;
        
//         public int CurrentTurn  { get; set; } = 0;

//         public bool IsOptionsEnabled
//         {
//             get
//             {
//                 return (OptionsOnStartup > -1);
//             }
//             set
//             {
//                 if (value)
//                 {
//                     OptionsOnStartup = 1;
//                 }
//                 else
//                 {
//                     OptionsOnStartup = -1;
//                 }
//             }
//         }

//         public int OptionsOnStartup  { get; set; } = -1;

//         public int LaserDamage  { get; set; } = 1;

//         public int TotalFlags  { get; set; } = 4;
        
//         public PendingCommands rPendingCommands;



//     }

// }
