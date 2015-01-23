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
    /// Generic encoder for field in ASCII format
    /// </summary>
    public class ISOStringFieldEncoder : ISOFieldEncoder {

        #region Private
        
        private ITranslator _translator;
        private IPadder _padder;
        private IPrefix _prefix;

        #endregion

        #region Properties

        public ITranslator Translator {
            set {
                _translator = value;
            }
        }

        public IPadder Padder {
            set {
                _padder = value;
            }
        }

        public IPrefix Prefix {
            set {
                _prefix = value;
            }
        }
        
        #endregion

        #region ISOStringFieldEncoder

        /// <summary>
        /// Default constructor with no padding, no prefix and values are literally interpreted as string
        /// </summary>
        public ISOStringFieldEncoder()
            : base() {

            _padder = PaddingNone.Instance;
            _prefix = PrefixNone.Instance;
            _translator = TranslatorASCII.Instance;
        }

        public ISOStringFieldEncoder(IPadder padder, IPrefix prefix, ITranslator translator)
            : base() {

            _padder = padder;
            _prefix = prefix;
            _translator = translator;
        }

        public ISOStringFieldEncoder(int length, string name, IPadder padder, IPrefix prefix, ITranslator translator)
            : base() {

            base.Length = length;
            Name = name;
            _padder = padder;
            _prefix = prefix;
            _translator = translator;
        }
        
        #endregion
        
        #region ISOFieldEncoder


        public override byte[] Encode(ISOComponent component) {
            try {

                // if data is represented as byte[], we need to convert it to ASCII, otherwise take it as is
                String data;
                if (component.Value is byte[]) {
                    data = System.Text.ASCIIEncoding.ASCII.GetString((byte[])component.Value);
                }
                else {
                    data = component.Value.ToString();
                }

                // Check the length
                if (data.Length > Length)
                    throw new ISOException(string.Format("{0}: Field length {1} is too long. Max {2}", component.Key, data.Length, Length));

                // Pad, Prefix and Translate
                string paddedData = _padder.Pad(data, Length);

                byte[] rawData = new byte[_prefix.EncodedLength + _translator.EncodedLength(paddedData.Length)];
                _prefix.EncodeLength(paddedData.Length, rawData);
                _translator.Translate(paddedData, rawData, _prefix.EncodedLength);
                
                return rawData;
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
                string decodedStr = _translator.TranslateBack(data, offset + lenLen, len);
                component.Value = decodedStr;

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
