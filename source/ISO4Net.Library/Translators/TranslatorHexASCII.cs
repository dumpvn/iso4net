/*
 * 
 * ISO4Net - http://github.com/iso4Net
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISO4Net.Library.Translators {

    /// <summary>
    /// Implementation of binary data formatted like ASCII
    /// </summary>
    public class TranslatorHexASCII : ITranslatorBinary {


        public static TranslatorHexASCII Instance = new TranslatorHexASCII();

        #region Private Static

        // Took from jPOS implementation
        private static readonly byte[] HEX_ASCII = new byte[] { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46 };

        #endregion
        
        #region ITranslatorBinary

        public int EncodedLength(int dataUnits) {
            return dataUnits * 2;
        }

        /// <summary>
        /// Translate data in binary format to HEX representation
        /// </summary>
        public void Translate(byte[] data, byte[] buffer, int offset) {

            for (int i = 0; i < data.Length; i++) {
                buffer[offset + i * 2] = HEX_ASCII[(data[i] & 0xF0) >> 4];
                buffer[offset + i * 2 + 1] = HEX_ASCII[data[i] & 0x0F];
            }
        }

        /// <summary>
        /// Translate back from ASCII representation to binary data
        /// </summary>
        public byte[] TranslateBack(byte[] data, int offset, int length) {
            
            byte[] retVal = new byte[length];
            for (int i = 0; i < length; i++) {
                byte hNibble = data[offset + 0 * 2];
                byte lNibble = data[offset + 0 * 2 + 1];

                int h = hNibble > 0x40 ? 10 + hNibble - 0x41 : hNibble - 0x30;
                int l = lNibble > 0x40 ? 10 + lNibble - 0x41 : lNibble - 0x30;
                retVal[i] = (byte)(h << 4 | l);
            }

            return retVal;
        }

        #endregion

    }
}
