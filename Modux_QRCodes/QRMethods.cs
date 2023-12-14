using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Windows.Forms.DataFormats;

namespace Modux_QRCodes
{
    internal class QRMethods
    {
        
        public static byte[] V1GetData(bool[][] input)
        {
            bool[][] Tinput = Transpose(input);
            (int EC, int mask) = GetFormatInfo(input, Tinput);
            if (EC == -1)
            {
                bool[][] temp = input;
                input = Tinput;
                Tinput = temp;
                (EC, mask) = GetFormatInfo(input, Tinput);
            }
            if (EC == -1)
            {
                Debug.WriteLine("Invalid Format");
                return [];
            }

            Tinput = Transpose(ApplyMask(input, mask));
            bool?[][] nulled = ReplaceNonDataV1(Tinput);
            nulled = nulled.Reverse().ToArray();

            IEnumerable<bool> dataBuild = [];
            for (int i = 0; i<5; i++)
            {
                dataBuild = dataBuild.Concat(CombineCols(nulled[4 * i].Reverse().ToArray(), nulled[4 * i + 1].Reverse().ToArray()));
                dataBuild = dataBuild.Concat(CombineCols(nulled[4 * i + 2], nulled[4 * i + 3]));
            }

            bool[] fullDataBools = dataBuild.ToArray();
            int byteLength = fullDataBools.Length / 8;
            bool[] dataBools = new bool[8 * byteLength];
            Array.Copy(fullDataBools, dataBools, 8 * byteLength);
            byte[] data = ToBytes(dataBools);

            int p = 0;
            int k = 0;
            int r = 0;
            switch (EC)
            {
                case 1:
                    // L
                    p = 3;
                    k = 19;
                    r = 2;
                    break;
                case 0:
                    // M
                    p = 2;
                    k = 16;
                    r = 4;
                    break;
                case 3:
                    // Q
                    p = 1;
                    k = 13;
                    r = 6;
                    break;
                case 2:
                    // H
                    p = 1;
                    k = 9;
                    r = 8;
                    break;
            }
            dataBools = FromBytes(decodeDataV1(data, p, k, r));
            int mode = GetBitsValue(dataBools, 0, 4);
            int length = GetBitsValue(dataBools, 4, 8);
            bool[] dataBoolsCut = new bool[length * 8];
            Array.Copy(dataBools, 12, dataBoolsCut, 0, length * 8);
            data = ToBytes(dataBoolsCut);
            return data;
        }

        public static byte[] decodeDataV1(byte[] data, int p, int k, int r)
        {
            int syndromes = 26 - k - p;
            //byte a = 0b100011101;
            return data;
        }

        public static byte[] ToBytes(bool[] inputBools)
        {
            //Assumes input is a multiple of 8 bits long
            BitArray input = new BitArray(inputBools);
            int length = input.Length / 8;
            bool[] bools = new bool[length * 8];
            input.CopyTo(bools, 0);
            for (int i = 0; i < length; i++)
            {
                bool[] temp = new bool[8];
                Array.Copy(bools, i * 8, temp, 0, 8);
                temp = temp.Reverse().ToArray();
                Array.Copy(temp, 0, bools, i * 8, 8);
            }
            byte[] result = new byte[length];
            new BitArray(bools).CopyTo(result, 0);
            return result;
        }

        public static bool[] FromBytes(byte[] inputBytes)
        {
            BitArray input = new BitArray(inputBytes);
            int length = inputBytes.Length;
            bool[] bools = new bool[length * 8];
            input.CopyTo(bools, 0);
            for (int i = 0; i < length; i++)
            {
                bool[] temp = new bool[8];
                Array.Copy(bools, i * 8, temp, 0, 8);
                temp = temp.Reverse().ToArray();
                Array.Copy(temp, 0, bools, i * 8, 8);
            }
            bool[] result = new bool[length * 8];
            new BitArray(bools).CopyTo(result, 0);
            return result;
        }

        public static bool?[][] ReplaceNonDataV1(bool[][] input)
        {
            //Assumes input is of size 21x21
            //Removes timing pattern row and column
            bool?[][] build = new bool?[20][];
            for (int i = 0; i < 6; i++)
            {
                bool?[] row = new bool?[20];
                bool?[] original = input[i].Cast<bool?>().ToArray();
                Array.Copy(original, 9, row, 8, 4);
                build[i] = row;
            }
            for (int i = 7; i < 9; i++)
            {
                bool?[] row = new bool?[20];
                bool?[] original = input[i].Cast<bool?>().ToArray();
                Array.Copy(original, 9, row, 8, 4);
                build[i - 1] = row;
            }
            for (int i = 9; i < 13; i++)
            {
                bool?[] row = new bool?[20];
                bool?[] original = input[i].Cast<bool?>().ToArray();
                Array.Copy(original, row, 6);
                Array.Copy(original, 7, row, 6, 14);
                build[i - 1] = row;
            }
            for (int i = 13; i < 21; i++)
            {
                bool?[] row = new bool?[20];
                bool?[] original = input[i].Cast<bool?>().ToArray();
                Array.Copy(original, 9, row, 8, 12);
                build[i - 1] = row;
            }
            return build;
        }

        public static bool[] CombineCols(bool?[] a, bool?[] b)
        {
            //Assumes the arrays are of the same length
            IEnumerable<bool> build = [];
            int length = a.Length;
            for (int i = 0; i < length; i++)
            {
                if (a[i] != null)
                {
                    build = build.Append((bool)a[i]);
                }
                if (b[i] != null)
                {
                    build = build.Append((bool)b[i]);
                }
            }
            return build.ToArray();
        }

        public static (int, int) GetFormatInfo(bool[][] input, bool[][] Tinput)
        {
            bool[] format1 = new bool[15];
            bool[] format2 = new bool[15];

            bool[] formatRowBools = input[8];
            bool[] formatColools = Tinput[8].Reverse().ToArray();

            Array.Copy(formatRowBools, 0, format1, 0, 6);
            Array.Copy(formatRowBools, 7, format1, 6, 1);
            Array.Copy(formatColools, formatColools.Length - 9, format1, 7, 2);
            Array.Copy(formatColools, formatColools.Length - 6, format1, 9, 6);
            Array.Copy(formatColools, format2, 7);
            Array.Copy(formatRowBools, formatRowBools.Length - 8, format2, 7, 8);

            format1 = DecodeFormatInfo(format1);
            if (format1 != null)
            {
                return (GetBitsValue(format1, 0, 2), GetBitsValue(format1, 2, 3));
            }
            format2 = DecodeFormatInfo(format2);
            if (format2 != null)
            {
                return (GetBitsValue(format2, 0, 2), GetBitsValue(format2, 2, 3));
            }
            return (-1, -1);
        }

        public static bool[] DecodeFormatInfo(bool[] input)
        {
            IEnumerable<int> errorsBuild = [];
            input = ToBools(new BitArray(input).Xor(BitsToBitArray(0b101010000010010, 15)));
            foreach (BitArray format in validFormatInfo)
            {
                int error = 0;
                foreach (bool bit in new BitArray(input).Xor(format))
                {
                    if (bit)
                    {
                        error += 1;
                    }
                }
                if (error == 0)
                {
                    bool[] data = new bool[5];
                    Array.Copy(ToBools(format), data, 5);
                    return data;
                }
                errorsBuild = errorsBuild.Append(error);
            }
            int[] errors = errorsBuild.ToArray();
            if (errors.Min() < 4)
            {
                bool[] data = new bool[5];
                Array.Copy(ToBools(validFormatInfo[Array.IndexOf(errors, errors.Min())]), data, 5);
                return data;
            }
            else
            {
                return null;
            }
        }

        public static BitArray[] validFormatInfo = new BitArray[]
        {
            BitsToBitArray(0b000000000000000, 15),
            BitsToBitArray(0b000010100110111, 15),
            BitsToBitArray(0b000101001101110, 15),
            BitsToBitArray(0b000111101011001, 15),
            BitsToBitArray(0b001000111101011, 15),
            BitsToBitArray(0b001010011011100, 15),
            BitsToBitArray(0b001101110000101, 15),
            BitsToBitArray(0b001111010110010, 15),
            BitsToBitArray(0b010001111010110, 15),
            BitsToBitArray(0b010011011100001, 15),
            BitsToBitArray(0b010100110111000, 15),
            BitsToBitArray(0b010110010001111, 15),
            BitsToBitArray(0b011001000111101, 15),
            BitsToBitArray(0b011011100001010, 15),
            BitsToBitArray(0b011100001010011, 15),
            BitsToBitArray(0b011110101100100, 15),
            BitsToBitArray(0b100001010011011, 15),
            BitsToBitArray(0b100011110101100, 15),
            BitsToBitArray(0b100100011110101, 15),
            BitsToBitArray(0b100110111000010, 15),
            BitsToBitArray(0b101001101110000, 15),
            BitsToBitArray(0b101011001000111, 15),
            BitsToBitArray(0b101100100011110, 15),
            BitsToBitArray(0b101110000101001, 15),
            BitsToBitArray(0b110000101001101, 15),
            BitsToBitArray(0b110010001111010, 15),
            BitsToBitArray(0b110101100100011, 15),
            BitsToBitArray(0b110111000010100, 15),
            BitsToBitArray(0b111000010100110, 15),
            BitsToBitArray(0b111010110010001, 15),
            BitsToBitArray(0b111101011001000, 15),
            BitsToBitArray(0b111111111111111, 15),
        };

        public static bool[][] ApplyMask(bool[][] input, int maskCode)
        {
            for (int r = 0; r < input.Length; r++)
            {
                for (int c = 0; c < input[r].Length; c++)
                {
                    input[r][c] = input[r][c] ^ Mask(r, c, maskCode);
                }
            }
            return input;
        }

        public static BitArray BitsToBitArray(int input, int length)
        {
            bool[] bits = ToBools(new BitArray(new int[] { input })).Reverse().ToArray();
            bool[] result = new bool[length];
            Array.Copy(bits, 32 - length, result, 0, length);
            return new BitArray(result);
        }

        public static void PrintBools(bool?[] input)
        {
            string output = "0b";
            foreach (bool? bit in input)
            {
                if (bit == null)
                {
                    output += " ";
                }
                else if (bit == true)
                {
                    output += "1";
                }
                else
                {
                    output += "0";
                }
            }
            Debug.WriteLine(output);
        }
        public static void PrintBools(bool[] input)
        {
            string output = "0b";
            foreach (bool bit in input)
            {
                if (bit)
                {
                    output += "1";
                }
                else
                {
                    output += "0";
                }
            }
            Debug.WriteLine(output);
        }

        public static void PrintBoolArrs(bool?[][] input)
        {
            foreach (bool?[] row in input)
            {
                PrintBools(row);
            }
        }
        public static void PrintBoolArrs(bool[][] input)
        {
            foreach (bool[] row in input)
            {
                PrintBools(row);
            }
        }

        public static int GetBitsValue(bool[] input, int start, int length)
        {
            //Assumes length is at most 32 bits
            bool[] selected = new bool[length];
            Array.Copy(input, start, selected, 0, length);
            int[] temp = new int[1];
            new BitArray(selected.Reverse().ToArray()).CopyTo(temp, 0);
            return temp[0];
        }

        public static T[][] Transpose<T>(T[][] input)
        {
            //Assumes all rows have the same length
            int length = input[0].Length;
            T[][] newMat = [];
            for (int i = 0; i < length; i++)
            {
                T[] col = [];
                foreach (T[] row in input)
                {
                    col = col.Append(row[i]).ToArray();
                }
                newMat = newMat.Append(col).ToArray();
            }
            return newMat;
        }

        public static bool[] ToBools(BitArray input)
        {
            bool[] bools = new bool[input.Length];
            input.CopyTo(bools, 0);
            return bools;
        }

        public static bool Mask(int r, int c, int code)
        {
            switch (code)
            {
                case 0:
                    return (r + c) % 2 == 0;
                case 1:
                    return r % 2 == 0;
                case 2:
                    return c % 3 == 0;
                case 3:
                    return (r + c) % 3 == 0;
                case 4:
                    return (r / 2 + c / 3) % 2 == 0;
                case 5:
                    return (r * c) % 2 + (r * c) % 3 == 0;
                case 6:
                    return ((r * c) % 3 + r * c) % 2 == 0;
                case 7:
                    return ((r * c) % 3 + r + c) % 2 == 0;
            }
            return false;
        }
    }
}
