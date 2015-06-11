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
    /// Prefix for ASCII data
    /// </summary>
    public class PrefixASCII : IPrefix {

        #region Private

        private int _digits;

        #endregion

        #region Public Static

        /// <summary>
        /// Prefixer for fields up to 9 characters
        /// </summary>
        public static PrefixASCII L = new PrefixASCII(1);

        /// <summary>
        /// Prefixer for fields up to 99 characters
        /// </summary>
        public static PrefixASCII LL = new PrefixASCII(2);

        /// <summary>
        /// Prefixers for fields up to 999 characters
        /// </summary>
        public static PrefixASCII LLL = new PrefixASCII(3);

        #endregion

        #region Ctor

        /// <summary>
        /// Constructor for PrefixASCII
        /// </summary>
        /// <param name="digits">Number of prefix digits</param>
        public PrefixASCII(int digits) {
            this._digits = digits;
        }

        #endregion
        
        #region IPrefix


        public int EncodedLength {
            get { return this._digits; }
        }

        public void EncodeLength(int length, byte[] data) {

            int n = length;
            for (int i = _digits - 1; i >= 0; i--) {
                data[i] = (byte)(n % 10 + '0');
                n = n / 10;
            }

            if (n != 0)
                throw new ISOException(string.Format("Invalid length {0}. Digits = {1}", length, _digits));

        }

        public int DecodeLength(byte[] data, int offset) {

            int l = 0;
            for (int i = 0; i < _digits; i++) {
                l = l * 10 + data[offset + i] - (byte)'0';
            }

            return l;
        }


        #endregion

    }
}
