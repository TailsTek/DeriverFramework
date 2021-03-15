using DF.In;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DF.Classes
{
    public class CacheMemory : IDisposable
    {
        internal string Key;
        internal byte[] Data;

        public long Length { get { return Data.Length; } }

        /// <summary>
        /// Compress current data.
        /// </summary>
        /// <param name="compressionLevel">Compression Level</param>
        public void Compress(CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, compressionLevel))
            {
                dstream.Write(Data, 0, Data.Length);
            }
            var dt = output.ToArray();
            Data = dt;
        }
        public void SetKey(string key)
        {
            Key = key;
        }
        /// <summary>
        /// Encrypting Byte Array.
        /// </summary>
        public void Encrypt()
        {
            Data = RijndaelHandler.Encrypt(Data, Key);
        }
        /// <summary>
        /// Decrypting Byte Array.
        /// </summary>
        public void Decrypt()
        {
            Data = RijndaelHandler.Decrypt(Data, Key);
        }
        /// <summary>
        /// Decompress current data.
        /// </summary>
        public void Decompress()
        {
            MemoryStream input = new MemoryStream(Data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            Data = output.ToArray();
        }
        /// <summary>
        /// Reading bytes.
        /// </summary>
        /// <param name="buffer">Buffer of bytes.</param>
        /// <param name="offset">Offset in cache.</param>
        /// <param name="count">Count of bytes in cache.</param>
        /// <returns></returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            //return Stream.Read(buffer, offset, count);
            
            byte[] newbuf = new byte[count];
            var c = 0;
            for (var i = 0; i != count; i++)
            {
                newbuf[i] = Data[i + offset];
                c += 1;
            }
            buffer = newbuf;
            return c;
            
        }
        /// <summary>
        /// Writing bytes.
        /// </summary>
        /// <param name="buffer">Buffer of bytes.</param>
        /// <param name="offset">Offset in cache.</param>
        /// <param name="count">Count of bytes in cache.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            //Stream.Write(buffer, offset, count);
            
            byte[] newbuf = new byte[count];
            var c = 0;
            for (var i = 0; i != count; i++)
            {
                newbuf[i] = buffer[i + offset];
                c += 1;
            }
            Data = newbuf;
        }
        public byte[] ToByteArray()
        {
            return Data;
        }

        public void Dispose()
        {
            Data = null;
        }

        public CacheMemory(byte[] buffer, string key)
        {
            Data = buffer;
            Key = key;
        }
    }
}
