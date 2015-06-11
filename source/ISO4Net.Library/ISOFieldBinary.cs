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


namespace ISO4Net.Library {

    /// <summary>
    /// Basic implementation for binary fields
    /// </summary>
    public class ISOFieldBinary : ISOComponent, ICloneable {

        #region ISOFieldBinary

        public ISOFieldBinary() {
            base.Key = -1;
        }

        public ISOFieldBinary(int field) {
            base.Key = field;
        }

        public ISOFieldBinary(int field, byte[] value) {
            base.Key = field;
            base.Value = value;
        }

        public ISOFieldBinary(int field, byte[] value, int offset, int length) {
            byte[] b = new byte[length];
            Array.Copy(value, offset, b, 0, length);
            base.Key = field;
            base.Value = b;
        }

        #endregion

        #region ISOComponent


        public override byte[] Encode() {
            throw new NotImplementedException("N/A at this point. Field must be encoded with it's own encoder");
        }

        public override int Decode(byte[] data) {
            throw new NotImplementedException("N/A at this point. Field must be decoded with it's own decoder");
        }

        #endregion

        #region Public Methods


        public override byte[] GetBytes() {
            return (byte[])Value;
        }


        #endregion

        #region ICloneable

        public object Clone() {
            return new ISOFieldBinary();
        }

        #endregion

        #region ToString()

        public override string ToString() {
            return (this.Value != null ? Utils.HexString((byte[])Value) : string.Empty);
        }

        #endregion

    }
}
