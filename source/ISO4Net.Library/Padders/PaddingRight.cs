/*
 * 
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
using System.Text;


namespace ISO4Net.Library.Padders {

    /// <summary>
    /// Pads the string to the right filled with the specified char
    /// </summary>
    public class PaddingRight : IPadder {

        #region Public Static

        /// <summary>
        /// Right padding filled with spaces
        /// </summary>
        public static PaddingRight SPACE_PADDING = new PaddingRight(' ');

        /// <summary>
        /// Right padding filled with zeros
        /// </summary>
        public static PaddingRight ZERO_PADDING = new PaddingRight('0');

        #endregion

        #region Private

        private char _padChar;
        
        #endregion

        #region PaddingRight

        public PaddingRight(char padChar) {
            _padChar = padChar;
        }

        #endregion

        #region IPadder

        public string Pad(string data, int maxLength) {

            int len = data.Length;

            if (len < maxLength) {
                StringBuilder padded = new StringBuilder(maxLength);
                padded.Append(data);

                for (; len < maxLength; len++) {
                    padded.Append(_padChar);
                }
                data = padded.ToString();
            }
            else if (len > maxLength) {
                throw new ISOException("Data is too long. Max = " + maxLength);
            }

            return data;
        }

        public string Unpad(string paddedData) {

            int len = paddedData.Length;

            for (int i = len; i > 0; i--) {
                if (paddedData.Substring(i - 1, 1) != _padChar.ToString()) {
                    return paddedData.Substring(0, i);
                }
            }
            return "";
        }

        #endregion

    }
}
