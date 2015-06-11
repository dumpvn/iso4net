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
using System.Text;


namespace ISO4Net.Library.Padders {

    public class PaddingLeft : IPadder {

        #region Public Static

        /// <summary>
        /// Left padding filled with spaces
        /// </summary>
        public static PaddingLeft SPACE_PADDING = new PaddingLeft(' ');

        /// <summary>
        /// Left padding filled with zeros
        /// </summary>
        public static PaddingLeft ZERO_PADDING = new PaddingLeft('0');

        #endregion

        #region Private

        private char _padChar;

        #endregion

        #region PaddingLeft

        public PaddingLeft(char padChar) {
            _padChar = padChar;
        }

        #endregion

        #region IPadder


        public string Pad(string data, int maxLength) {

            StringBuilder padded = new StringBuilder(maxLength);
            int len = data.Length;
            if (len > maxLength) {
                throw new ISOException(string.Format("Data is too long. Max= {0}", len));
            }
            else {
                for (int i = maxLength - len; i > 0; i--) {
                    padded.Append(_padChar);
                }
                padded.Append(data);
            }

            return padded.ToString();
        }

        public string Unpad(string paddedData) {

            int i = 0;
            int len = paddedData.Length;

            while (i < len) {
                if ( !paddedData.Substring(i, 1).Equals(_padChar) ) {
                    return paddedData.Substring(i, paddedData.Length - i);
                }
                i++;
            }

            return "";
        }


        #endregion

    }
}
