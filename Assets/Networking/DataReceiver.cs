using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Networking.Externals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mapbox.Utils;
using Mapbox.Unity.Map;

namespace Assets.Scripts.Networking
{
    
    static class DataReceiver
    {
        public static void HandleWelcomeMsg(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            string msg = buffer.ReadString();
            buffer.Dispose();

            Debug.Log(msg);
            DataSender.SendHelloServer();
        }

        public static void HandleInstantiatePlayer(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            int index = buffer.ReadIntager();
            buffer.Dispose();

            NetworkManager.instance.InstantiatePlayer(index);
        }

        public static void HandleAlertMsg(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            string msg = buffer.ReadString();
            buffer.Dispose();

            Debug.Log(msg);
            
        }

        public static void HandleLoginOK(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            NetworkManager.myIndex = buffer.ReadIntager();
            Debug.Log("You've logged in successfully");
            buffer.Dispose();

            
        }

        public static void HandlePlayerData(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();

            GameManager.instance.username = buffer.ReadString();
            GameManager.instance.level = buffer.ReadIntager();
            GameManager.instance.exp = buffer.ReadIntager();
            GameManager.instance.gold = buffer.ReadIntager();
            GameManager.instance.rating = buffer.ReadIntager();
            GameManager.instance.wins = buffer.ReadIntager();
            GameManager.instance.losses = buffer.ReadIntager();

            GameManager.instance.Loading(1);
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            if(sceneName == "HomeScene")
            {
                MenuManager.instance.UpdateUI();
            }

            buffer.Dispose();
        }

        public static void HandlePOIData(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();

            int key = buffer.ReadIntager();
            double lat = (double)buffer.ReadFloat();
            double lon = (double)buffer.ReadFloat();

            GraffitiPointOnMap._tempLocationsD.Add(key, new Vector2d(lat, lon));

            buffer.Dispose();
        }

        public static void HandleSearchFin(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            int count = buffer.ReadIntager();
            buffer.Dispose();
          
            GraffitiPointOnMap.instance.StartCheck(count);
        }

        public static void HandleImage(byte[] data)
        {
            
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            int id = buffer.ReadIntager();
            string image = buffer.ReadString();
            string username = buffer.ReadString();

            GraffitiPointOnMap.instance.LoadImage(id, image, username);

            buffer.Dispose();

            
        }

        
    }
}
