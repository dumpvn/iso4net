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
using System;
using System.Text;


namespace ISO4Net.Library {

    /// <summary>
    /// Generic encoder for field in Binary format (BCD)
    /// </summary>
    public class ISOBinaryFieldEncoder : ISOFieldEncoder {

        #region Private

        private ITranslatorBinary _translator;
        private IPrefix _prefix;

        #endregion

        #region Properties

        public ITranslatorBinary Translator {
            set {
                _translator = value;
            }
        }

        public IPrefix Prefix {
            set {
                _prefix = value;
            }
        }
        
        #endregion

        #region ISOBinaryFieldEncoder

        /// <summary>
        /// Default constructor with no padding, no prefix and values are literally interpreted as string
        /// </summary>
        public ISOBinaryFieldEncoder()
            : base() {

            _prefix = PrefixNone.Instance;
            _translator = TranslatorBinary.Instance;
        }

        public ISOBinaryFieldEncoder(IPrefix prefix, ITranslatorBinary translator)
            : base() {

            _prefix = prefix;
            _translator = translator;
        }

        public ISOBinaryFieldEncoder(int length, string name, IPrefix prefix, ITranslatorBinary translator)
            : base() {

            base._length = length;
            Name = name;
            _prefix = prefix;
            _translator = translator;
        }
        
        #endregion
        
        #region ISOFieldEncoder


        public override byte[] Encode(ISOComponent component) {

            try {

                byte[] data = component.GetBytes();

                // Check length
                int encodedLength = _prefix.EncodedLength;
                if (encodedLength == 0) {
                    if (data.Length != Length) {
                        throw new ISOException(string.Format("Binary data length is not the same as encoder length ({0}/{1}", data.Length, Length));
                    }
                }

                byte[] retValue = new byte[_translator.EncodedLength(data.Length) + encodedLength];
                _prefix.EncodeLength(data.Length, retValue);
                _translator.Translate(data, retValue, encodedLength);

                return retValue;
            }
            catch (Exception e) {
                throw new ISOException(string.Format("Exception encoding field {0}", component.Key), e);
            }
        }
        
        public override int Decode(ISOComponent component, byte[] data, int offset) {

            try {

                int len = _prefix.DecodeLength(data, offset);
                if (len == -1) {
                    // if we don't know the length, use the max defined for the field
                    len = Length;
                }
                else if (Length > 0 && len > Length) {
                    throw new ISOException(string.Format("{0}: Field length {1} is too long. Max {2}", component.Key, len, Length));
                }

                int lenLen = _prefix.EncodedLength;
                byte[] decoded = _translator.TranslateBack(data, offset + lenLen, len);
                component.Value = decoded;

                return lenLen + _translator.EncodedLength(len);

            }
            catch (Exception e) {
                throw new ISOException(string.Format("Exception decoding field {0}", component.Key), e);
            }
        }
        
        public override int GetMaxEncodedLength() {
            return (_prefix.EncodedLength + _translator.EncodedLength(Length));
        }


        #endregion

        #region Protected Methods

        protected virtual void CheckLength(int length, int maxLength) {
            if (length > maxLength) {
                throw new ArgumentException(string.Format("Length {0} is too long for {1}", length, this.ToString()));
            }
        }

        #endregion

    }
}
