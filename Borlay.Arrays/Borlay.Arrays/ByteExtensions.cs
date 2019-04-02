using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Borlay.Arrays
{
    public static class ByteExtensions
    {
        public static bool is64 = RuntimeInformation.ProcessArchitecture == Architecture.X64 || RuntimeInformation.ProcessArchitecture == Architecture.Arm64;



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

        //public static unsafe bool ContainsSequence(this byte[] left, byte[] right)
        //{
            
        //    if (is64)
        //        return ContainsSequence64(left, right);
        //    else
        //        return ContainsSequence32(left, right);
        //}

        public static unsafe bool ContainsSequence32(this byte[] left, byte[] right)
        {
            if (left is null || right is null) return false;
            if (left.Length < right.Length) return false;

            int length = right.Length;

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

        public static unsafe bool ContainsSequence64(this byte[] left, byte[] right)
        {
            if (left is null || right is null) return false;
            if (left.Length < right.Length) return false;

            int length = right.Length;

            fixed (byte* str = left)
            {
                byte* chPtr = str;
                fixed (byte* str2 = right)
                {
                    byte* chPtr2 = str2;
                    while (length >= 32)
                    {
                        if (((
                            (*(((long*)chPtr + 0)) != *(((long*)chPtr2 + 0))) ||
                            (*(((long*)(chPtr + 8))) != *(((long*)(chPtr2 + 8))))
                        ) ||
                        (
                        (*(((long*)(chPtr + 16))) != *(((long*)(chPtr2 + 16)))) ||
                        (*(((long*)(chPtr + 24))) != *(((long*)(chPtr2 + 24))))
                        ))
                        ) break;

                        chPtr += 32;
                        chPtr2 += 32;
                        length -= 32;
                    }

                    while (length >= 16)
                    {
                        if (
                            (*((long*)chPtr) != *((long*)chPtr2))
                            ||
                            (*((long*)chPtr + 8) != *((long*)chPtr2 + 8))
                            ) break;

                        chPtr += 16;
                        chPtr2 += 16;

                        length -= 16;
                    }

                    while (length >= 8)
                    {
                        if (*(((long*)chPtr)) != *(((long*)chPtr2))) break;
                        chPtr += 8;
                        chPtr2 += 8;

                        length -= 8;
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
    }
}
