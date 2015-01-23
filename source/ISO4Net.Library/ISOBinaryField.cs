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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISO4Net.Library {

    /// <summary>
    /// Basic implementation for Binary fields
    /// </summary>
    public class ISOBinaryField : ISOComponent {

        #region ISOBinaryField

        public ISOBinaryField() {
            base.Key = -1;
        }

        public ISOBinaryField(int field) {
            base.Key = field;
        }

        public ISOBinaryField(int field, byte[] value) {
            base.Key = field;
            base.Value = value;
        }

        public ISOBinaryField(int field, byte[] value, int offset, int length) {
            byte[] b = new byte[length];
            Array.Copy(value, offset, b, 0, length);
            base.Key = field;
            base.Value = b;
        }

        #endregion

        #region ISOComponent

        public override byte[] Encode() {
            throw new NotImplementedException();
        }

        public override int Decode(byte[] data) {
            throw new NotImplementedException();
        }

        public override byte[] GetBytes() {
            return Value != null ? (byte[])Value : null;
        }

        #endregion

        #region ToString()

        public override string ToString() {
            return Utils.HexString((byte[])Value);
        }

        #endregion

    }
}
