using System;
using System.Net.Sockets;
using Assets.Scripts.Networking.Externals;
using UnityEngine;

namespace Assets.Scripts.Networking
{
    static class ClientTCP
    {
        private static TcpClient clientSocket;
        private static NetworkStream myStream;
        private static byte[] recBuff;

        public static void InitializingNetworking()
        {
            clientSocket = new TcpClient();
            clientSocket.ReceiveBufferSize = 4096;
            clientSocket.SendBufferSize = 4096;
            recBuff = new byte[4096 * 2];
            clientSocket.BeginConnect("SERVERIP", 5557, new AsyncCallback(ClientConnectCallback), clientSocket);
        }

        private static void ClientConnectCallback(IAsyncResult result)
        {
            clientSocket.EndConnect(result);
            if(clientSocket.Connected == false)
            {
                return;
            }
            else
            {
                clientSocket.NoDelay = true;
                myStream = clientSocket.GetStream();
                myStream.BeginRead(recBuff, 0, 4096 * 2, ReceiveCallback, null);
            }
        }

        private static void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int length = myStream.EndRead(result);
                if(length <= 0)
                {
                    Console.WriteLine("Disconnected from server");
                    UnityThread.executeInUpdate(() => { Application.Quit(); });
                    return;
                }

                byte[] newBytes = new byte[length];
                Array.Copy(recBuff, newBytes, length);
                UnityThread.executeInFixedUpdate(() => { ClientHandleData.HandleData(newBytes); });
                myStream.BeginRead(recBuff, 0, 4096 * 2, ReceiveCallback, null);
            }
            catch (Exception)
            {
                Console.WriteLine("Disconnected from server");
                UnityThread.executeInUpdate(() => { Application.Quit(); });
                return;
            }
        }

        public static void SendData(byte[] data)
        {
            try
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteIntager((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
                buffer.WriteBytes(data);
                myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
                buffer.Dispose();
            }
            catch (Exception)
            {

                Debug.Log("Disconnected from the server.");
                Application.Quit();

            }
            
        }

        public static bool IsConnected()
        {
            if(clientSocket == null) { return false;  }
            if (clientSocket.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Disconnect()
        {
            clientSocket.Close();
        }
    }
}
