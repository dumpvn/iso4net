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


using ISO4Net.Library.Padders;
using ISO4Net.Library.Prefixers;
using ISO4Net.Library.Translators;


namespace ISO4Net.Library.Encoders {

    /// <summary>
    /// Fixed length, ASCII Number (Left padded with zeros)
    /// </summary>
    public class FA_Number : ISOStringFieldEncoder {

        #region FA_Number

        public FA_Number() : base(PaddingLeft.ZERO_PADDING, PrefixNone.Instance, TranslatorASCII.Instance) { }

        public FA_Number(int len, string name) : base(len, name, PaddingLeft.ZERO_PADDING, PrefixNone.Instance, TranslatorASCII.Instance) { }

        #endregion

    }
}
