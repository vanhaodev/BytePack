using System;
using System.Collections.Generic;
using System.Text;

namespace BytePack
{
    public class PacketHelper
    {
        public void WriteInt(byte[] buffer, ref int offset, int value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
            offset += 4;
        }

        public int ReadInt(byte[] buffer, ref int offset)
        {
            int value = BitConverter.ToInt32(buffer, offset);
            offset += 4;
            return value;
        }

        public void WriteFloat(byte[] buffer, ref int offset, float value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
            offset += 4;
        }

        public float ReadFloat(byte[] buffer, ref int offset)
        {
            float value = BitConverter.ToSingle(buffer, offset);
            offset += 4;
            return value;
        }

        public void WriteBool(byte[] buffer, ref int offset, bool value)
        {
            buffer[offset++] = (byte)(value ? 1 : 0);
        }

        public bool ReadBool(byte[] buffer, ref int offset)
        {
            return buffer[offset++] == 1;
        }

        public void WriteString(byte[] buffer, ref int offset, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            WriteInt(buffer, ref offset, bytes.Length);
            Buffer.BlockCopy(bytes, 0, buffer, offset, bytes.Length);
            offset += bytes.Length;
        }

        public string ReadString(byte[] buffer, ref int offset)
        {
            int len = ReadInt(buffer, ref offset);
            string result = Encoding.UTF8.GetString(buffer, offset, len);
            offset += len;
            return result;
        }

        public void WriteDateTime(byte[] buffer, ref int offset, DateTime value)
        {
            WriteLong(buffer, ref offset, value.Ticks);
        }

        public DateTime ReadDateTime(byte[] buffer, ref int offset)
        {
            return new DateTime(ReadLong(buffer, ref offset));
        }

        public void WriteLong(byte[] buffer, ref int offset, long value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 8);
            offset += 8;
        }

        public long ReadLong(byte[] buffer, ref int offset)
        {
            long value = BitConverter.ToInt64(buffer, offset);
            offset += 8;
            return value;
        }
    }

}
