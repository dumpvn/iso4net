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
using ISO4Net.Library;


namespace ISO4Net.Library {

    /// <summary>
    /// Basic ISO Header implementation
    /// </summary>
    public class ISOHeader : ICloneable {

        #region Protected

        protected byte[] _header;

        #endregion
        
        #region Properties


        public int Length {
            get {
                return _header != null ? _header.Length : 0;
            }
        }

        public bool ASCIIEncoding { get; set; }

        public byte[] Value {
            get {
                return _header;
            }
        }


        #endregion

        #region ISOHeader


        public ISOHeader() {
            _header = null;
            ASCIIEncoding = true;
        }

        public ISOHeader(byte[] header) {
            _header = header;
            ASCIIEncoding = true;
        }


        #endregion

        #region Public Methods


        public byte[] Encode() {
            return _header;
        }

        public int Decode(byte[] header) {
            _header = header;
            return _header != null ? _header.Length : 0;
        }


        #endregion

        #region ICloneable

        public object Clone() {
            ISOHeader h = new ISOHeader();
            if (_header != null)
                h._header = _header;

            return h;
        }

        #endregion

        #region ToString()

        public override string ToString() {
            if (ASCIIEncoding)
                return _header != null ? System.Text.ASCIIEncoding.ASCII.GetString(_header) : base.ToString();
            else
                return Utils.HexString(_header);
        }

        #endregion

    }
}
