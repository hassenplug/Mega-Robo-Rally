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
//     public partial class GameController 
//     {


//         public void SaveCommandList(string SaveFileName = "")
//         {
//             //if (SaveFileName.Length == 0) SaveFileName = Properties.Settings.Default.RecoveryPath + "CommandList.xml";
//             if (SaveFileName.Length == 0) SaveFileName = "" + "CommandList.xml";
//             //SaveFile(SaveFileName, ListOfCommands);
//             //Console.WriteLine("Saved " + ListOfCommands.Count + " commands");
//         }

//         public void SaveFile(string FileName, Object SerializeObject)
//         {
//             DateTime starttime = DateTime.Now;
//             XmlSerializer serialPlay = new XmlSerializer(SerializeObject.GetType());
//             System.IO.StreamWriter csvfile = new System.IO.StreamWriter(FileName, false);  // do not append
//             serialPlay.Serialize(csvfile, SerializeObject);
//             csvfile.Close();

//             //Console.WriteLine("Save " + SerializeObject.GetType().ToString() + " ET:" + (DateTime.Now - starttime).ToString());
//         }


//         public Object LoadFile(Type FileType, string FileName)
//         {
//             if (!File.Exists(FileName))
//             {
//                 return null;
//             }
//             //XmlDeserializationEvents
//             DateTime starttime = DateTime.Now;
//             XmlSerializer serialPlay = new XmlSerializer(FileType);
//             System.IO.StreamReader csvfile = new System.IO.StreamReader(FileName);
//             Object localfile = serialPlay.Deserialize(csvfile);
//             csvfile.Close();
//             //Console.WriteLine("Load " + FileType.ToString() + " ET:" + (DateTime.Now - starttime).ToString());

//             return localfile;
//         }



//     }
// }
