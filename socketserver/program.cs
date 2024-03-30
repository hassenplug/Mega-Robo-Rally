//
// csc wsserver.cs
// wsserver.exe
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;


class Server 
{

//    public static ObservableCollection<TcpClient> clientlist = new ObservableCollection<TcpClient>();
    private static Dictionary<System.Net.IPAddress, TcpClient> clientlist = new Dictionary<System.Net.IPAddress, TcpClient>();

    public static void Main() {
    //static void ServerStart(){
        string ip = "0.0.0.0";
        int port = 5050;
        var server = new TcpListener(IPAddress.Parse(ip), port);    

            
    //    TcpListener listener = new TcpListener(IPAddress.Any, 4480);

        server.Start();
        Console.WriteLine("Listening on port: 5050");

        while (true)
        {
            // Accept the client connection before creating the thread.
            TcpClient client = server.AcceptTcpClient();

            ThreadPool.QueueUserWorkItem(state => HandleConnection(client));
        }
    }

    static void HandleConnection(TcpClient client)
    {
        var addr = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

        if (!clientlist.ContainsKey(addr))
        {
            clientlist.Add(addr,client);
            Console.WriteLine("Client: {0} {1} ",addr,clientlist.Count().ToString());
            // Insert your code here. (Do not accept socket again here)
            client.Close();
        }
    }
}





// private void Connectnattk_DoWork(object sender, DoWorkEventArgs e)
// {
//     try
//     {
//         IPAddress ipAd = IPAddress.Parse(IPV4.Text); //use local m/c IP address, and use the same in the client

//         /* Initializes the Listener */
//         TcpListener myList = new TcpListener(ipAd, 8001);

//         /* Start Listeneting at the specified port */
//         myList.Start();

//         MessageBox.Show("The server is running at port 8001...");
//         MessageBox.Show("The local End point is  :" + myList.LocalEndpoint);
//         MessageBox.Show("Looking for other computer");

//         Socket s = myList.AcceptSocket();
//         Console.WriteLine("Found buddy " + s.RemoteEndPoint);

//         ASCIIEncoding asen = new ASCIIEncoding();
//         s.Send(asen.GetBytes(satt.Text));
//         MessageBox.Show("The command " + satt.Text + " was sent to the computer with IP address " + IPV4.Text);

//         byte[] b = new byte[100];
//         int k = s.Receive(b);
//         for (int i = 0; i < k; i++)
//             Console.Write(Convert.ToChar(b[i]));

//         void ServerStart()
//         {

//             TcpListener listener = new TcpListener(IPAddress.Any, 8001);

//             listener.Start();
//             Console.WriteLine("Listening on port: 8001");

//             while (true)
//             {
//                 TcpClient client = listener.AcceptTcpClient();

//                 ThreadPool.QueueUserWorkItem(state => HandleConnection(client));
//             }
//         }

//     void HandleConnection(TcpClient client)
//         {
//             Socket s = myList.AcceptSocket();
//             Console.WriteLine("Found buddy " + s.RemoteEndPoint);

//             ASCIIEncoding asen = new ASCIIEncoding();
//             s.Send(asen.GetBytes(satt.Text));

//             Console.WriteLine("Number of connected buddies: " + connectedbuddies++);

//             client.Close();
//         }


//         /* clean up */

//         s.Close();
//         myList.Stop();

//     }
// }




// class Server {
//     public static void Main() {
//         string ip = "0.0.0.0";
//         int port = 5050;
//         var server = new TcpListener(IPAddress.Parse(ip), port);

//         server.Start();
//         Console.WriteLine("Server has started on {0}:{1}, Waiting for a connection…", ip, port);

//         TcpClient client = server.AcceptTcpClient();
//         Console.WriteLine("A client connected.");

//         NetworkStream stream = client.GetStream();

//         // enter to an infinite cycle to be able to handle every change in stream
//         while (true) {
//             while (!stream.DataAvailable);
//             while (client.Available < 3); // match against "get"

//             byte[] bytes = new byte[client.Available];
//             stream.Read(bytes, 0, bytes.Length);
//             string s = Encoding.UTF8.GetString(bytes);
//             //Console.WriteLine(s);

//             if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase)) {
//                 Console.WriteLine("=====Handshaking from client=====\n{0}", s);

//                 // 1. Obtain the value of the "Sec-WebSocket-Key" request header without any leading or trailing whitespace
//                 // 2. Concatenate it with "258EAFA5-E914-47DA-95CA-C5AB0DC85B11" (a special GUID specified by RFC 6455)
//                 // 3. Compute SHA-1 and Base64 hash of the new value
//                 // 4. Write the hash back as the value of "Sec-WebSocket-Accept" response header in an HTTP response
//                 string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
//                 string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
//                 byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
//                 string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

//                 // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
//                 byte[] response = Encoding.UTF8.GetBytes(
//                     "HTTP/1.1 101 Switching Protocols\r\n" +
//                     "Connection: Upgrade\r\n" +
//                     "Upgrade: websocket\r\n" +
//                     "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");

//                 stream.Write(response, 0, response.Length);
//             } else {
//                 bool fin = (bytes[0] & 0b10000000) != 0,
//                     mask = (bytes[1] & 0b10000000) != 0; // must be true, "All messages from the client to the server have this bit set"
//                 int opcode = bytes[0] & 0b00001111, // expecting 1 - text message
//                     offset = 2;
//                 ulong msglen = bytes[1] & (ulong)0b01111111;

//                 if (msglen == 126) {
//                     // bytes are reversed because websocket will print them in Big-Endian, whereas
//                     // BitConverter will want them arranged in little-endian on windows
//                     msglen = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0);
//                     offset = 4;
//                 } else if (msglen == 127) {
//                     // To test the below code, we need to manually buffer larger messages — since the NIC's autobuffering
//                     // may be too latency-friendly for this code to run (that is, we may have only some of the bytes in this
//                     // websocket frame available through client.Available).
//                     msglen = BitConverter.ToUInt64(new byte[] { bytes[9], bytes[8], bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2] },0);
//                     offset = 10;
//                 }

//                 if (msglen == 0) {
//                     Console.WriteLine("msglen == 0");
//                 } else if (mask) {
//                     byte[] decoded = new byte[msglen];
//                     byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] };
//                     offset += 4;

//                     for (ulong i = 0; i < msglen; ++i)
//                         decoded[i] = (byte)(bytes[offset + (int)i] ^ masks[i % 4]);

//                     string text = Encoding.UTF8.GetString(decoded);
//                     Console.WriteLine("{0}", text);
//                 } else
//                     Console.WriteLine("mask bit not set");

//                 Console.WriteLine();
//             }
//         }
//     }
// }
