using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetPrg
{
  public class Server
    {
        
        public static void Main()
        {
            
            string data;
            string marker = "<##!##>";
            StringBuilder buffer = new StringBuilder();
            
            Console.WriteLine("Waiting For Client");
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any,2555);
            
            Socket sock = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            
            sock.Bind(ipep);
            sock.Listen(1);
            Socket client =  sock.Accept();

            IPEndPoint clientipep = (IPEndPoint) client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}",
                clientipep.Address, clientipep.Port);
            NetworkStream netstr = new NetworkStream(client);
            
            StreamReader strread = new StreamReader(netstr);
            StreamWriter strwrite = new StreamWriter(netstr);
            strwrite.WriteLine("Welcome server");
            strwrite.Flush();
           
            
            while(true){
                   try
                   {
                       buffer.Append(Encoding.ASCII.GetString(new byte[] {(byte) netstr.ReadByte()})); //READ BYTE TO CLIENT AND APPEND BUFFER

                       data = buffer.ToString();
                        
                    if (data.Contains(marker))
                    {
                       data =  data.Replace(marker, String.Empty); 
                       
                       Console.WriteLine("Received Message->"+data);

                        buffer.Clear();
                    }


                   }
                   
                   catch (IOException)
                   {
                       break;
                   }
                   
                   
                   strwrite.WriteLine();
                   strwrite.Flush(); 
            } 
            Console.WriteLine("Server Closing");
               strread.Close();
               strwrite.Close();
               netstr.Close();
           
           
           
        }

    }
}
