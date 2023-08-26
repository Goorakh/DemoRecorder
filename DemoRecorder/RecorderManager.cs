using RoR2;
using RoR2.Networking;
using System;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine.Networking;

namespace DemoRecorder
{
    public static class RecorderManager
    {
        static int findFreeConnectionId()
        {
            ReadOnlyCollection<NetworkConnection> connections = NetworkServer.connections;
            for (int i = 1; i < connections.Count; i++)
            {
                if (connections[i] == null)
                {
                    return i;
                }
            }

            return connections.Count;
        }

        public static bool IsRecording { get; private set; }
        static FileStream _file;

        public static void StartRecording(string outputPath)
        {
            if (IsRecording)
                return;

            _file = File.Create(outputPath);

            RecorderConnection recorderConnection = new RecorderConnection(new BinaryWriter(_file));
            recorderConnection.ForceInitialize(NetworkServer.hostTopology);
            recorderConnection.connectionId = findFreeConnectionId();

            if (NetworkServer.AddExternalConnection(recorderConnection))
            {
                Log.Debug("Created recorder connection");

                NetworkManagerSystem.singleton.OnServerAddPlayer(recorderConnection, 0);

                IsRecording = true;
            }
            else
            {
                Log.Error("Failed to add recorder connection");

                _file.Dispose();
                _file = null;
                File.Delete(outputPath);
            }
        }

        public static void StopRecording()
        {
            if (!IsRecording)
                return;

            IsRecording = false;

            _file.Flush();
            _file.Dispose();
            _file = null;
        }
    }
}
