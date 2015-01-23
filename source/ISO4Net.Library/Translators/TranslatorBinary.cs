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
    /// Implementation for Binary Translation
    /// </summary>
    public class TranslatorBinary : ITranslatorBinary {


        public static TranslatorBinary Instance = new TranslatorBinary();


        #region TranslatorBinary

        private TranslatorBinary() {
            //
        }

        #endregion

        #region ITranslatorBinary


        public int EncodedLength(int dataUnits) {
            return dataUnits;
        }

        public void Translate(byte[] data, byte[] buffer, int offset) {
            Array.Copy(data, 0, buffer, offset, data.Length);
        }

        public byte[] TranslateBack(byte[] data, int offset, int length) {
            byte[] retVal = new byte[length];
            Array.Copy(data, offset, retVal, 0, length);
            return retVal;
        }


        #endregion

        
    }
}
