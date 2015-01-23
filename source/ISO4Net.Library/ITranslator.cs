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


namespace ISO4Net.Library {
    
    /// <summary>
    /// Interface for Translator implementation
    /// </summary>
    public interface ITranslator {

        /// <summary>
        /// Returns the length for the specified data units, after the data is translated
        /// </summary>
        /// <param name="dataUnits"></param>
        /// <returns></returns>
        int EncodedLength(int dataUnits);

        /// <summary>
        /// Translate the data into a specified format
        /// </summary>
        /// <param name="data">String to translate</param>
        /// <param name="buffer">Where the translated result will be stored</param>
        /// <param name="offset">Starting offset</param>
        void Translate(string data, byte[] buffer, int offset);

        /// <summary>
        /// Translates back the byte array data into string
        /// </summary>
        /// <param name="data">Data to translate</param>
        /// <param name="offset">Starting offset</param>
        /// <param name="length">Number of bytes to translate</param>
        string TranslateBack(byte[] data, int offset, int length);

    }
}
