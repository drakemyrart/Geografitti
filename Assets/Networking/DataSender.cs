using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Networking.Externals;

namespace Assets.Scripts.Networking
{
    

    class DataSender
    {
        public static void SendHelloServer()
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ClientPackets.CHelloServer);
            buffer.WriteString("Thank you, I'm now connected to you!");
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLogin(string username, string password)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ClientPackets.CLogin);
            buffer.WriteString(username);
            buffer.WriteString(password);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();

        }

        public static void SendNewAccount(string username, string password)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ClientPackets.CNewAccount);
            buffer.WriteString(username);
            buffer.WriteString(password);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();

        }

        public static void SendImage(string image, float lat, float lon)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ClientPackets.CStoreImage);
            buffer.WriteString(image);
            buffer.WriteFloat(lat);
            buffer.WriteFloat(lon);
            
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();

        }

        public static void SendPOISearch(int lat, int lon)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ClientPackets.CPOISearch);
            buffer.WriteIntager(lat);
            buffer.WriteIntager(lon);
            
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendImageRequest(int id)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ClientPackets.CRequestImage);
            buffer.WriteIntager(id);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

    }
}
