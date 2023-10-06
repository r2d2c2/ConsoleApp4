using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ConsoleApp4
{
    internal class Program
    {
        private static List<string> allText=new List<string>();
        

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            RunAsyncSocketServer().Wait();
        }
        static async Task RunAsyncSocketServer()
        {
            try
            {
                string host = Dns.GetHostName();
                IPHostEntry entry = Dns.GetHostEntry(host);
                IPAddress iPAddress = entry.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(iPAddress, 7777);
                TcpListener tcpListener = new TcpListener(endPoint);
                tcpListener.Start();

                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                NetworkStream networks = tcpClient.GetStream();

                while (true)
                {
                    
                    BinaryReader binReader = new BinaryReader(networks);
                    allText.Add(binReader.ReadString());
                    foreach (var item in allText)
                    {
                        BinaryWriter binaryw = new BinaryWriter(networks);
                        binaryw.Write(item);
                    }
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                throw;
            }
           

            /*int MAX_SIZE = 1024;  // 가정

            // (1) 소켓 객체 생성 (TCP 소켓)
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // (2) 포트에 바인드
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 7777);
            sock.Bind(ep);

            // (3) 포트 Listening 시작
            sock.Listen(100);

            while (true)
            {
                // (4) 비동기 소켓 Accept
                Socket clientSock = await Task.Factory.FromAsync(sock.BeginAccept, sock.EndAccept, null);

                // (5) 비동기 소켓 수신
                var buff = new byte[MAX_SIZE];

                int nCount = await Task.Factory.FromAsync<int>(
                           clientSock.BeginReceive(buff, 0, buff.Length, SocketFlags.None, null, clientSock),
                           clientSock.EndReceive);

                if (nCount > 0)
                {
                    string msg = Encoding.ASCII.GetString(buff, 0, nCount);
                    Console.WriteLine(msg);

                    // (6) 비동기 소켓 송신
                    await Task.Factory.FromAsync(
                            clientSock.BeginSend(buff, 0, buff.Length, SocketFlags.None, null, clientSock),
                            clientSock.EndSend);
                }

                // (7) 소켓 닫기
                clientSock.Close();
            }*/
        }
    }
}