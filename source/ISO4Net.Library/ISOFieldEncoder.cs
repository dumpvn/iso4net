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




namespace ISO4Net.Library {

    /// <summary>
    /// Base implementation for all the field encoders defined in ISO4Net.Library.Encoders
    /// </summary>
    public abstract class ISOFieldEncoder {

        #region Protected 

        protected int _length;

        #endregion

        #region Properties

        public virtual int Length {
            get {
                return _length;
            }
            set {
                _length = value;
            }
        }
        public string Name { get; set; }
        public bool Pad { get; set; }

        #endregion

        #region ISOFieldEncoder

        public ISOFieldEncoder() {
            _length = -1;
            Name = string.Empty;
        }

        public ISOFieldEncoder(int length, string name) {
            _length = length;
            Name = name;
        }

        #endregion

        #region Abstract Methods

        
        public abstract byte[] Encode(ISOComponent component);

        public abstract int Decode(ISOComponent component, byte[] data, int offset);

        public abstract int GetMaxEncodedLength();
        

        #endregion

        #region Public Methods

        public virtual ISOComponent CreateComponent(int fieldNumber) {
            return new ISOField(fieldNumber);
        }

        #endregion

        #region ToString()

        public override string ToString() {
            return string.IsNullOrEmpty(Name) ? base.ToString() : Name;
        }

        #endregion

    }
}
