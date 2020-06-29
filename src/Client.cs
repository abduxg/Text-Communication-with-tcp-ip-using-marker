using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetPrg
{
   public class Client
    {
        public static void Main()
        {
            string banner;
            string input;
            string marker = "<##!##>";
            char [] data;
            byte[] datas;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2555);

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                server.Connect(ipep);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            NetworkStream netstr = new NetworkStream(server);
            StreamReader strread = new StreamReader(netstr);
            StreamWriter strwrite = new StreamWriter(netstr);
            
            banner = strread.ReadLine();
            Console.WriteLine(banner);

            while (true)
            {
                Console.Write("Message->");
                input = Console.ReadLine();
                data = input.ToCharArray();
               
                datas = Encoding.ASCII.GetBytes(data);
                
                try
                {
                    
                    for (int i = 0; i < datas.Length; i++)
                    {
                        netstr.WriteByte(datas[i]);
                        netstr.Flush();
                       
                    }
                   
                    netstr.WriteByte(60);//<
                    netstr.WriteByte(35);//#
                    netstr.WriteByte(35);//#
                    netstr.WriteByte(33);//!
                    netstr.WriteByte(35);//#
                    netstr.WriteByte(35);//#
                    netstr.WriteByte(62);//>
                    netstr.Flush();
                    
                    
                }
                catch (IOException e)
                {
                  break;
                }
                
            }
       
            Console.WriteLine("Exiting");
            strread.Close();
            strwrite.Close();
            netstr.Close();

            server.Shutdown(SocketShutdown.Both);
            server.Close();
            
            

        }
    }
}
