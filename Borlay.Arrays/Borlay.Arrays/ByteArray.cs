using System;
using System.Collections;
using System.Collections.Generic;

namespace Borlay.Arrays
{
    public class ByteArray : IEnumerable<byte>
    {
        private byte[] bytes = null;
        private int hashCode;

        public byte[] Bytes
        {
            get
            {
                if (bytes == null)
                    throw new Exception("Not initialized");
                return bytes;
            }
            set
            {
                if (bytes != null)
                    throw new Exception("Cannot set bytes twice");

                this.bytes = value ?? throw new ArgumentNullException(nameof(Bytes), "Cannot set null Byte array to ByteArray.Bytes");
                this.hashCode = ComputeHash(bytes);
            }
        }

        public bool IsSet => bytes != null;

        public bool IsEmpty => bytes == null || bytes.Length == 0;

        public ByteArray(byte[] bytes)
        {
            Bytes = bytes;
        }

        /// <summary>
        /// For serialization only
        /// </summary>
        public ByteArray()
        {
        }

        public byte this[int index]
        {
            get
            {
                return bytes[index];
            }
        }

        public int Length => Bytes.Length;

        public static ByteArray Create(byte[] bytes)
        {
            return new ByteArray(bytes);
        }

        public static bool operator ==(ByteArray left, ByteArray right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            if (left.hashCode != right.hashCode) return false;
            return SequenceEqual(left.Bytes, right.Bytes);
        }

        public static bool operator !=(ByteArray left, ByteArray right)
        {
            if (left is null && right is null) return false;
            if (left is null || right is null) return true;
            if (left.hashCode != right.hashCode) return true;
            return !SequenceEqual(left.Bytes, right.Bytes);
        }

        public static bool operator >(ByteArray left, ByteArray right)
        {
            if (left == right) return false;


            var count = left.Length;
            if (count > right.Length)
                count = right.Length;

            for (int i = 0; i < count; i++)
            {
                if (left[i] > right[i])
                    return true;
                else if (left[i] < right[i])
                    return false;
            }

            return left.Length > right.Length;
        }

        public static bool operator <(ByteArray left, ByteArray right)
        {
            if (left == right) return false;


            var count = left.Length;
            if (count > right.Length)
                count = right.Length;

            for (int i = 0; i < count; i++)
            {
                if (left[i] < right[i])
                    return true;
                else if (left[i] > right[i])
                    return false;
            }

            return left.Length < right.Length;
        }

        public static bool operator <=(ByteArray left, ByteArray right)
        {
            if (left == right) return true;
            return left < right;
        }

        public static bool operator >=(ByteArray left, ByteArray right)
        {
            if (left == right) return true;
            return left > right;
        }

        public static unsafe bool SequenceEqual(byte[] left, byte[] right)
        {
            if (left is null || right is null) return left == right;
            int length = left.Length;
            if (length != right.Length) return false;
            fixed (byte* str = left)
            {
                byte* chPtr = str;
                fixed (byte* str2 = right)
                {
                    byte* chPtr2 = str2;
                    while (length >= 20)
                    {
                        if ((((*(((int*)chPtr)) != *(((int*)chPtr2))) ||
                        (*(((int*)(chPtr + 4))) != *(((int*)(chPtr2 + 4))))) ||
                        ((*(((int*)(chPtr + 8))) != *(((int*)(chPtr2 + 8)))) ||
                        (*(((int*)(chPtr + 12))) != *(((int*)(chPtr2 + 12)))))) ||
                        (*(((int*)(chPtr + 16))) != *(((int*)(chPtr2 + 16))))) break;

                        chPtr += 20;
                        chPtr2 += 20;
                        length -= 20;
                    }

                    while (length >= 4)
                    {
                        if (*(((int*)chPtr)) != *(((int*)chPtr2))) break;
                        chPtr += 4;
                        chPtr2 += 4;

                        length -= 4;
                    }

                    while (length > 0)
                    {
                        if (*chPtr != *chPtr2) break;
                        chPtr++;
                        chPtr2++;
                        length--;
                    }

                    return (length <= 0);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var other = (ByteArray)obj;
            if (other.hashCode != this.hashCode) return false;
            return SequenceEqual(Bytes, other.Bytes);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public static int ComputeHash(byte[] data)
        {
            unchecked
            {
                var hash = 0;
                for (var i = 0; i < data.Length; i += 4)
                {
                    hash = (hash << 4) + data[i];
                }
                return hash;
            }
        }

        public static ByteArray New()
        {
            return New(32);
        }

        public static ByteArray New(int size)
        {
            var bytes = new byte[size];
            var rnd = System.Security.Cryptography.RandomNumberGenerator.Create();
            rnd.GetBytes(bytes);
            var byteArray = new ByteArray(bytes);
            return byteArray;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)Bytes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Bytes.GetEnumerator();
        }
    }
}