using RoR2;
using System.IO;
using UnityEngine.Networking;

namespace DemoRecorder
{
    public class RecorderConnection : NetworkConnection
    {
        readonly BinaryWriter _writer;

        public RecorderConnection(BinaryWriter writer) : base()
        {
            _writer = writer;
        }

        public override bool SendByChannel(short msgType, MessageBase msg, int channelId)
        {
            _writer.Write(Run.instance.fixedTime);
            _writer.Write((byte)EventType.SendByChannel);

            _writer.Write(msgType);
            _writer.Write(channelId);

            NetworkWriter writer = new NetworkWriter();
            msg.Serialize(writer);
            byte[] msgBytes = writer.AsArray();
            _writer.Write(msgBytes.Length);
            _writer.Write(msgBytes);

            return true;
        }

        public override bool SendBytes(byte[] bytes, int numBytes, int channelId)
        {
            _writer.Write(Run.instance.fixedTime);
            _writer.Write((byte)EventType.SendBytes);

            _writer.Write(channelId);
            _writer.Write(numBytes);
            _writer.Write(bytes);

            return true;
        }

        public override bool SendWriter(NetworkWriter writer, int channelId)
        {
            _writer.Write(Run.instance.fixedTime);
            _writer.Write((byte)EventType.SendWriter);

            _writer.Write(channelId);

            byte[] writerBytes = writer.AsArray();
            _writer.Write(writerBytes.Length);
            _writer.Write(writerBytes);

            return true;
        }

        public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
        {
            _writer.Write(Run.instance.fixedTime);
            _writer.Write((byte)EventType.TransportSend);

            _writer.Write(channelId);

            _writer.Write(numBytes);
            _writer.Write(bytes, 0, numBytes);

            error = 0;
            return true;
        }
    }
}
