using Assets.Scripts.Networking.Externals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Networking
{
    static class ClientHandleData
    {
        private static ByteBuffer playerBuffer;
        public delegate void Packet(byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        public static void InitializePackets()
        {
            packets.Add((int)ServerPackets.SWelcomeMessage, DataReceiver.HandleWelcomeMsg);
            packets.Add((int)ServerPackets.SInstantiatePlayer, DataReceiver.HandleInstantiatePlayer);
            packets.Add((int)ServerPackets.SAlertMsg, DataReceiver.HandleAlertMsg);
            packets.Add((int)ServerPackets.SLoginOK, DataReceiver.HandleLoginOK);
            packets.Add((int)ServerPackets.SPlayerData, DataReceiver.HandlePlayerData);
            packets.Add((int)ServerPackets.SPOIData, DataReceiver.HandlePOIData);
            packets.Add((int)ServerPackets.SSearchFin, DataReceiver.HandleSearchFin);
            packets.Add((int)ServerPackets.SImage, DataReceiver.HandleImage);
            //packets.Add((int)ServerPackets.SChat, DataReceiver.HandleChatMsg);
            //packets.Add((int)ServerPackets.SMatching, DataReceiver.HandleMatching);
            //packets.Add((int)ServerPackets.STurn, DataReceiver.HandleTurn);
            //packets.Add((int)ServerPackets.SCardData, DataReceiver.HandleCardData);
        }

        public static void HandleData( byte[] data)
        {
            byte[] buffer = (byte[])data.Clone();
            int pLength = 0;

            if (playerBuffer == null)
            {
                playerBuffer = new ByteBuffer();
            }

            playerBuffer.WriteBytes(buffer);
            if (playerBuffer.Count() == 0)
            {
                playerBuffer.Clear();
                return;
            }

            if (playerBuffer.Length() >= 4)
            {
                pLength = playerBuffer.ReadIntager(false);
                if (pLength <= 0)
                {
                    playerBuffer.Clear();
                    return;
                }
            }

            while (pLength > 0 && pLength <= playerBuffer.Length() - 4)
            {
                if (pLength <= playerBuffer.Length() - 4)
                {
                    playerBuffer.ReadIntager();
                    data = playerBuffer.ReadBytes(pLength);
                    HandleDataPackets(data);
                }

                pLength = 0;
                if (playerBuffer.Length() >= 4)
                {
                    pLength = playerBuffer.ReadIntager(false);
                    if (pLength <= 0)
                    {
                        playerBuffer.Clear();
                        return;
                    }
                }
            }

            if (pLength <= 1)
            {
                playerBuffer.Clear();
            }
        }

        private static void HandleDataPackets(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            buffer.Dispose();
            if (packets.TryGetValue(packetID, out Packet packet))
            {
                packet.Invoke(data);
            }
        }
    }
}
