/*
 * 
 * ISO4Net - http://iso4net.codeplex.com/
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


using ISO4Net.Library;
using System;
using System.Collections;
using System.Text;


namespace ISO4Net.Library.Encoders {

    /// <summary>
    /// Fixed Length, ASCII Bitmap
    /// </summary>
    public class FA_Bitmap : ISOBitmapEncoder {

        #region FA_Bitmap

        public FA_Bitmap() : base() { }

        public FA_Bitmap(int length, string name) : base(length, name) { }

        #endregion

        #region ISOBitmapEncoder


        public override byte[] Encode(ISOComponent component) {

            byte[] b = Utils.BitArray2Byte((BitArray)component.Value);
            return ASCIIEncoding.ASCII.GetBytes(Utils.HexString(b));

        }

        public override int Decode(ISOComponent component, byte[] data, int offset) {

            int length;

            //BitArray bitmap = Utils.Hex2BitArray(data, offset, (Length / 2) * 8);           // 2 chars represents 1 byte and has 8 bits
            BitArray bitmap = Utils.Hex2BitArray(data, offset, (Length << 3));
            component.Value = bitmap;

            // check if we have secondary bitmap
            if (bitmap.Get(1))
                length = 128;
            else
                length = 64;

            // check if we have extended bitmap (more than 128 fields)
            if (Length > 16 && bitmap.Get(65))
                length = 192;

            return (length >> 2);
        }

        public override int GetMaxEncodedLength() {
            throw new NotImplementedException();
        }


        #endregion

    }
}
