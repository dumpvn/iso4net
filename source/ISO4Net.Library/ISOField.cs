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
using System.Text;
using System.Collections.Generic;



namespace ISO4Net.Library {

    /// <summary>
    /// Basic implementation for fields
    /// </summary>
    public class ISOField : ISOComponent, ICloneable {
        
        #region ISOField

        public ISOField() {
            base.Key = -1;
        }

        public ISOField(int field) {
            //Key = field;
            base.Key = field;
        }

        public ISOField(int field, string value) {
            base.Key = field;
            base.Value = value;
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


        /// <summary>
        /// Returns byte array with the field's value representation
        /// </summary>
        public override byte[] GetBytes() {
            return Value != null ? ASCIIEncoding.ASCII.GetBytes(Value.ToString()) : new byte[] { };
        }


        #endregion

        #region ToString()

        public override string ToString() {
            return (Value != null && Key != null) ? string.Format("{0} = {1}", Key, Value) : base.ToString();
        }

        #endregion

        #region ICloneable

        public object Clone() {
            return new ISOField((int)Key, Value.ToString());
        }

        #endregion

    }
}
