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


namespace ISO4Net.Library.Padders {

    /// <summary>
    /// Dummy class used as an utility
    /// </summary>
    public class PaddingNone : IPadder {


        public static PaddingNone Instance = new PaddingNone();


        #region PaddingNone

        private PaddingNone() {

        }

        #endregion

        #region IPadder

        public string Pad(string data, int maxLength) {
            return data;
        }

        public string Unpad(string paddedData) {
            return paddedData;
        }

        #endregion

    }
}
