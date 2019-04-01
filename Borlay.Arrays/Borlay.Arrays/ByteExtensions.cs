using System;
using System.Collections.Generic;
using System.Text;

namespace Borlay.Arrays
{
    public static class ByteExtensions
    {
        public static void CopyFrom(this byte[] bytes, ByteArray source, ref int index)
        {
            CopyFrom(bytes, source.Bytes, ref index);
        }

        public static void CopyFrom(this byte[] bytes, byte[] source, ref int index)
        {
            Buffer.BlockCopy(source, 0, bytes, index, source.Length);
            index += source.Length;
        }

        public static void CopyFrom(this byte[] bytes, byte[] source, int count, ref int index)
        {
            Buffer.BlockCopy(source, 0, bytes, index, count);
            index += count;
        }

        public static void CopyFrom(this byte[] bytes, byte[] source, int srcIndex, int count, ref int index)
        {
            Buffer.BlockCopy(source, srcIndex, bytes, index, count);
            index += count;
        }
    }
}
