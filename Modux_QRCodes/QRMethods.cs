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
        /*
        public static byte[] QRDecodeV1(BitArray[] input)
        {
            BitArray[] Tinput = BitArrayTranspose(input);
            int EC1 = GetBitsValue(input[8], 0, 2);
            int EC2 = GetBitsValue(Tinput[8], 0, 2);
        }
        */

        public static (int, int) GetFormatInfo(bool[][] input)
        {
            bool[][] Tinput = Transpose(input);
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

            PrintBools(format1);
            PrintBools(format2);

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
            /*
             * L = 3
             * M = 2
             * Q = 1
             * H = 0
            */ 
        }

        public static bool[] DecodeFormatInfo(bool[] input)
        {
            int[] errors = [];
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
                errors = errors.Append(error).ToArray();
            }
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

        public static BitArray BitsToBitArray(int input, int length)
        {
            bool[] bits = ToBools(new BitArray(new int[] { input })).Reverse().ToArray();
            bool[] result = new bool[length];
            Array.Copy(bits, 32 - length, result, 0, length);
            return new BitArray(result);
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

        public static int GetBitsValue(bool[] input, int start, int length)
        {
            bool[] selected = new bool[length];
            Array.Copy(input, start, selected, 0, length);
            int[] temp = new int[1];
            new BitArray(selected.Reverse().ToArray()).CopyTo(temp, 0);
            return temp[0];
        }

        public static T[][] Transpose<T>(T[][] input)
        {
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
                case 2:
                    return (r + c) % 2 == 0;
                case 3:
                    return r % 2 == 0;
                case 0:
                    return c % 3 == 0;
                case 1:
                    return (r + c) % 3 == 0;
                case 6:
                    return (r / 2 + c / 3) % 2 == 0;
                case 7:
                    return (r * c) % 2 + (r * c) % 3 == 0;
                case 4:
                    return ((r * c) % 3 + r * c) % 2 == 0;
                case 5:
                    return ((r * c) % 3 + r + c) % 2 == 0;
            }
            return false;
        }
    }
}
