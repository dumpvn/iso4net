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


namespace ISO4Net.Library.Translators {

    /// <summary>
    /// Implementation for ASCII Translation. Strings are converted using <remarks>System.Text.Encoding.ASCII</remarks>
    /// </summary>
    public class TranslatorASCII : ITranslator {


        public static TranslatorASCII Instance = new TranslatorASCII();


        #region TranslatorASCII

        private TranslatorASCII() {
            //
        }

        #endregion
        
        #region ITranslator


        public int EncodedLength(int dataUnits) {
            return dataUnits;
        }
        
        public void Translate(string data, byte[] buffer, int offset) {
            Array.Copy(Encoding.ASCII.GetBytes(data), 0, buffer, offset, data.Length);
        }

        public string TranslateBack(byte[] data, int offset, int length) {

            byte[] retVal = new byte[length];

            try {
                Array.Copy(data, offset, retVal, 0, length);
                return Encoding.ASCII.GetString(retVal);
            }
            catch (IndexOutOfRangeException e) {
                throw new ISOException(string.Format("Required {0} but got only {1} byte(s)", length, data.Length - offset));
            }
            catch (Exception) {
                throw;
            }

        }


        #endregion
     
    }
}
