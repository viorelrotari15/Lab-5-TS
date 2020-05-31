using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab5
{
    class Program

    {

        private static IPAddress remoteIPAddress;
        private static int remotePort;
        private static int localPort;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                
                Console.WriteLine("Introduceti Local Port");
                localPort = Convert.ToInt16(Console.ReadLine());

                Console.WriteLine("Introduceti protul clientului 2");
                remotePort = Convert.ToInt16(Console.ReadLine());

                Console.WriteLine("Introduceti IP adresa clientului 2");
                remoteIPAddress = IPAddress.Parse(Console.ReadLine());

                // Cream thred nou
                Thread tRec = new Thread(new ThreadStart(Receiver));
                tRec.Start();

                while (true)
                {
                    Send(Console.ReadLine());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        private static void Send(string datagram)
        {
            // cream  UdpClient
            UdpClient sender = new UdpClient();

            // initiam endPoint pe informatia despre hostul separat
            IPEndPoint endPoint = new IPEndPoint(remoteIPAddress, remotePort);

            try
            {
                // converatm datele in masiv
                byte[] bytes = Encoding.UTF8.GetBytes(datagram);

                // Trimitem datele
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally
            {
                // derminam conexiunea
                sender.Close();
            }
        }

        public static void Receiver()
        {
            // initiem UdpClient pentru citirea datelor de intrare
            UdpClient receivingUdpClient = new UdpClient(localPort);

            IPEndPoint RemoteIpEndPoint = null;

            try
            {
                Console.WriteLine(
                   "\n-----------*******Chat comun*******-----------");

                while (true)
                {
                    // asteptam datele
                    byte[] receiveBytes = receivingUdpClient.Receive(
                       ref RemoteIpEndPoint);

                    // convertam datele in date citabile
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    Console.WriteLine(" --> " + returnData.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

    }
    
}
