/*
 * ISO4Net - http://openeft.codeplex.com/
 * Copyright (C) 2014 Robert Barreiro (rbarreiro@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */



using System;
using System.Collections;
using System.Text;

namespace ISO4Net.Library {

    /// <summary>
    /// Generic class with general purpose functions
    /// </summary>
    public static class Utils {


        #region Hex2BitArray


        public static BitArray Hex2BitArray(BitArray bitmap, byte[] data, int offset) {

            int len = data.Length << 2;

            for (int i = 0; i < len; i++) {
                int d = Convert.ToInt32(((char)data[i >> 2]).ToString(), 16);
                if ((d & (0x08 >> (i % 4))) > 0)
                    bitmap.Set(offset + i + 1, true);
            }
            return bitmap;
        }

        public static BitArray Hex2BitArray(byte[] bitmap, int offset, int size) {

            int length;

            // Determine the size of the bitmap
            if (size > 64) {
                length = Convert.ToInt32(((char)bitmap[offset]).ToString(), 16) & 0x08;

                if (length == 8)
                    length = 128;
                else
                    length = 64;
            }
            else
                length = size;

            // Build the bitmap
            BitArray bitArray = new BitArray(length + 1);

            for (int i = 0; i <= length; i++) {

                int digits = Convert.ToInt32(((char)bitmap[offset + (i >> 2)]).ToString(), 16);
                if ((digits & (0x08 >> (i % 4))) > 0) {

                    bitArray.Set(i + 1, true);

                    if (i == 65 && size > 128)
                        length = 192;
                }
            }

            return bitArray;
        }


        #endregion

        #region BitArray2Byte


        public static byte[] BitArray2Byte(BitArray ba) {

            int len = (((ba.Count + 62) >> 6) << 6);
            byte[] b = new byte[len >> 3];

            for (int i = 1; i < ba.Length - 8; i += 8) {

                if (ba.Get(i)) b[i / 8] |= 0x80;
                if (ba.Get(i + 1)) b[i / 8] |= 0x40;
                if (ba.Get(i + 2)) b[i / 8] |= 0x20;
                if (ba.Get(i + 3)) b[i / 8] |= 0x10;
                if (ba.Get(i + 4)) b[i / 8] |= 0x8;
                if (ba.Get(i + 5)) b[i / 8] |= 0x4;
                if (ba.Get(i + 6)) b[i / 8] |= 0x2;
                if (ba.Get(i + 7)) b[i / 8] |= 0x1;

            }

            if (len > 64)
                    b[0] |= 0x80;
            if (len > 128)
                b[8] |= 0x80;

            return b;
        }


        #endregion

        #region HexString

        public static string HexString(byte[] data) {

            StringBuilder sb = new StringBuilder(data.Length * 2);

            for (int i = 0; i < data.Length; i++) {
                sb.Append(((int)data[i] & 0xFF).ToString("X2"));
            }

            return sb.ToString();
        }

        #endregion

    }
}
