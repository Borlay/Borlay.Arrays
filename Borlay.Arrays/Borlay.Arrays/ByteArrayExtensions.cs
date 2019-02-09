using System;
using System.Linq;

namespace Borlay.Arrays
{
    public static class ByteArrayExtensions
    {
        public static bool IsNullOrEmpty(this ByteArray byteArray)
        {
            return byteArray == null || byteArray.IsEmpty;
        }

        public static byte[] GetBytes(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] GetBytes(ushort value)
        {
            return BitConverter.GetBytes(value).Endian();
        }

        public static byte[] GetBytes(short value)
        {
            return BitConverter.GetBytes(value).Endian();
        }

        public static byte[] GetBytes(uint value)
        {
            return BitConverter.GetBytes(value).Endian();
        }

        public static byte[] GetBytes(int value)
        {
            return BitConverter.GetBytes(value).Endian();
        }

        public static byte[] GetBytes(ulong value)
        {
            return BitConverter.GetBytes(value).Endian(); // todo test with big values
        }

        public static byte[] GetBytes(long value)
        {
            return BitConverter.GetBytes(value).Endian(); // todo test with big values
        }


        public static byte[] GetBytes<T>(T value)
        {
            var size = TypeSize.SizeOf<T>();
            return GetBytes<T>(value, size);
        }

        public static byte[] GetBytes<T>(T value, int size)
        {
            T[] array = new T[] { value };

            byte[] bytes = new byte[size];

            Buffer.BlockCopy(array, 0, bytes, 0, size);
            if (size > 1)
                bytes.Endian();
            return bytes;
        }

        public static void AddBytes(this byte[] bytes, ByteArray value, int index)
        {
            Array.Copy(value.Bytes, 0, bytes, index, value.Length);
        }

        public static void AddBytes(this byte[] bytes, ByteArray value, ref int index)
        {
            Array.Copy(value.Bytes, 0, bytes, index, value.Length);
            index += value.Length;
        }

        public static void TryAddBytes(this byte[] bytes, ByteArray value, int index)
        {
            if (!value.IsNullOrEmpty())
            {
                Array.Copy(value.Bytes, 0, bytes, index, value.Length);
            }
        }

        public static void TryAddBytes(this byte[] bytes, ByteArray value, ref int index)
        {
            if (!value.IsNullOrEmpty())
            {
                Array.Copy(value.Bytes, 0, bytes, index, value.Length);
                index += value.Length;
            }
        }

        public static void AddBytes(this byte[] bytes, byte[] bytesToAdd, ref int index)
        {
            Array.Copy(bytesToAdd, 0, bytes, index, bytesToAdd.Length);
            index += bytesToAdd.Length;
        }

        public static void AddBytes<T>(this byte[] bytes, T value, int index)
        {
            var size = TypeSize.SizeOf<T>();
            AddBytes<T>(bytes, value, size, ref index);
        }

        public static void AddBytes<T>(this byte[] bytes, T value, ref int index)
        {
            var size = TypeSize.SizeOf<T>();
            AddBytes<T>(bytes, value, size, ref index);
        }

        public static void AddBytes<T>(this byte[] bytes, T value, int size, int index)
        {
            AddBytes<T>(bytes, value, size, ref index);
        }

        public static void AddBytes<T>(this byte[] bytes, T value, int size, ref int index)
        {
            T[] array = new T[] { value };

            if (size > 1 && BitConverter.IsLittleEndian)
            {
                var buff = new byte[size];
                Buffer.BlockCopy(array, 0, buff, 0, size);
                Array.Reverse(buff);
                Buffer.BlockCopy(buff, 0, bytes, index, size);
            }
            else
            {
                Buffer.BlockCopy(array, 0, bytes, index, size);
            }

            index += size;
        }

        public static bool ToBool(byte[] bytes)
        {
            var value = BitConverter.ToBoolean(bytes, 0);
            return value;
        }

        public static ushort ToUShort(byte[] bytes)
        {
            var value = BitConverter.ToUInt16(bytes.Endian(), 0);
            return value;
        }

        public static uint ToUInt(byte[] bytes)
        {
            var value = BitConverter.ToUInt32(bytes.Endian(), 0);
            return value;
        }

        public static ulong ToULong(byte[] bytes)
        {
            var value = BitConverter.ToUInt64(bytes.Endian(), 0);
            return value;
        }

        public static short ToShort(byte[] bytes)
        {
            var value = BitConverter.ToInt16(bytes.Endian(), 0);
            return value;
        }

        public static int ToInt(byte[] bytes)
        {
            var value = BitConverter.ToInt32(bytes.Endian(), 0);
            return value;
        }

        public static long ToLong(byte[] bytes)
        {
            var value = BitConverter.ToInt64(bytes.Endian(), 0);
            return value;
        }

        public static T GetValue<T>(this byte[] bytes, int index)
        {
            var size = TypeSize.SizeOf<T>();
            return GetValue<T>(bytes, size, ref index);
        }

        public static T GetValue<T>(this byte[] bytes, ref int index)
        {
            var size = TypeSize.SizeOf<T>();
            return GetValue<T>(bytes, size, ref index);
        }

        public static T GetValue<T>(this byte[] bytes, int size, int index)
        {
            return GetValue<T>(bytes, size, ref index);
        }

        public static T GetValue<T>(this byte[] bytes, int size, ref int index)
        {
            T[] value = new T[1];

            if (size > 1 && BitConverter.IsLittleEndian)
            {
                var buff = new byte[size];
                Buffer.BlockCopy(bytes, index, buff, 0, size);
                Array.Reverse(buff);
                Buffer.BlockCopy(buff, 0, value, 0, size);
            }
            else
            {
                Buffer.BlockCopy(bytes, index, value, 0, size);
            }

            index += size;
            return value[0];
        }


        public static byte[] Endian(this byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        public static ByteArray ToByteArray(this byte[] bytes, ushort maxCount, ref ushort index)
        {
            if (bytes == null) return null;

            var count = BitConverter.ToUInt16(bytes, index);
            index += 2;
            if (count > maxCount) throw new ArgumentOutOfRangeException(nameof(count), $"Max count of array is {maxCount} but was {count}");

            var byteArray = ToByteArray(bytes, index, count);
            index += count;
            return byteArray;
        }

        public static ByteArray ToByteArrayExactly(this byte[] bytes, ushort mustCount, ref ushort index)
        {
            if (bytes == null) return null;

            var count = BitConverter.ToUInt16(bytes, index);
            index += 2;
            if (count != mustCount) throw new ArgumentOutOfRangeException(nameof(count), $"Count of array must be {mustCount} but was {count}");

            var byteArray = ToByteArray(bytes, index, count);
            index += count;
            return byteArray;
        }

        public static ByteArray ToByteArrayExactlyOrEmpty(this byte[] bytes, ushort mustCount, ref ushort index)
        {
            if (bytes == null) return null;

            var count = BitConverter.ToUInt16(bytes, index);
            index += 2;
            if (count == 0) return null;
            if (count != mustCount) throw new ArgumentOutOfRangeException(nameof(count), $"Count of array must be {mustCount} but was {count}");

            var byteArray = ToByteArray(bytes, index, count);
            index += count;
            return byteArray;
        }

        public static ByteArray ToByteArray(this byte[] bytes, int start, int count)
        {
            if (bytes == null) return null;
            if (count == 0) return null;

            if ((bytes.Length - count) < start) throw new IndexOutOfRangeException("Byte array length is less than needed");

            var array = new byte[count];
            Array.Copy(bytes, start, array, 0, count);
            var byteArray = new ByteArray(array);
            return byteArray;
        }

        public static ByteArray ToByteArray(this byte[] bytes)
        {
            if (bytes == null) return null;
            if (bytes.Length == 0) return null;
            return new ByteArray(bytes);
        }

        public static byte[] ToArray(this ByteArray byteArray)
        {
            return byteArray.IsNullOrEmpty() ? null : byteArray.Bytes.ToArray();
        }
    }
}
