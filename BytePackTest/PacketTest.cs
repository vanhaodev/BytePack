using BytePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytePackTest
{
    public static class PacketTest
    {
        public static void Run(int iterations = 100_000)
        {
            Console.WriteLine($"=== BYTE TEST x{iterations} ===");

            var helper = new PacketHelper();
            byte[] buffer = new byte[128];

            long totalBytesSent = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < iterations; i++)
            {
                // --- Build packet ---
                int offset = 0;

                helper.WriteInt(buffer, ref offset, 2001);                      // Message ID
                helper.WriteFloat(buffer, ref offset, i * 0.1f);               // PosX
                helper.WriteFloat(buffer, ref offset, i * 0.2f);               // PosY
                helper.WriteBool(buffer, ref offset, (i % 2 == 0));            // State
                helper.WriteDateTime(buffer, ref offset, DateTime.UtcNow);    // Time

                var segment = new ArraySegment<byte>(buffer, 0, offset);
                totalBytesSent += segment.Count;

                // --- Parse packet ---
                int readOffset = segment.Offset;
                byte[] data = segment.Array!;

                int msgId = helper.ReadInt(data, ref readOffset);
                float posX = helper.ReadFloat(data, ref readOffset);
                float posY = helper.ReadFloat(data, ref readOffset);
                bool state = helper.ReadBool(data, ref readOffset);
                DateTime time = helper.ReadDateTime(data, ref readOffset);
            }

            stopwatch.Stop();

            Console.WriteLine($"✅ DONE: {iterations} packets");
            Console.WriteLine($"📦 Total bytes sent: {totalBytesSent} bytes ({totalBytesSent / 1024f:0.00} KB)");
            Console.WriteLine($"⏱ Time: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"⚡ Avg per packet: {stopwatch.Elapsed.TotalMilliseconds / iterations * 1_000_000:0.00} ns");
            Console.WriteLine($"💡 GC-free, shared PacketHelper instance, no per-session state");
        }
    }
}
