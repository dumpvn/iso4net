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


namespace ISO4Net.Library.Prefixers {

    /// <summary>
    /// Prefix using BCD data format
    /// </summary>
    public class PrefixBCD : IPrefix {

        #region Private

        private int _digits;

        #endregion

        #region Public Static

        public static PrefixBCD L = new PrefixBCD(1);

        public static PrefixBCD LL = new PrefixBCD(2);

        public static PrefixBCD LLL = new PrefixBCD(3);

        #endregion

        #region PrefixBCD

        public PrefixBCD(int digits) {
            _digits = digits;
        }

        #endregion

        #region IPrefix


        public int EncodedLength {
            get { return (_digits + 1) >> 1; }
        }

        public void EncodeLength(int length, byte[] data) {
            for (int i = ((IPrefix)this).EncodedLength - 1; i >= 0; i--) {
                int twoDigits = length % 100;
                length /= 100;
                data[i] = (byte)(((twoDigits / 10) << 4) + twoDigits % 10);
            }
        }

        public int DecodeLength(byte[] data, int offset) {
            int l = 0;
            for (int i = 0; i < (_digits + 1) / 2; i++) {
                l = 100 * l + ((data[offset + i] & 0xF0) >> 4) * 10 + ((data[offset + i] & 0x0F));
            }
            return l;
        }


        #endregion

    }
}
