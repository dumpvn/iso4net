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
using System.Collections.Generic;
using System.Collections;



namespace ISO4Net.Library {

    /// <summary>
    /// Basic component with the basic methods shared by Messages, Headers, Bitmaps and Fields
    /// </summary>
    public abstract class ISOComponent {

        #region Virtual Properties

        public virtual int TopField {
            get {
                return 0;
            }
        }

        #endregion

        #region Abtract

        public virtual object Key { get; set; }
        public virtual object Value { get; set; }

        public abstract byte[] Encode();
        public abstract int Decode(byte[] data);

        #endregion
        
        #region Virtual Methods


        public virtual void Add(ISOComponent component) {
            throw new NotImplementedException("N/A at this point.");
        }

        public virtual void Add(int fieldNumber, string value) {
            throw new NotImplementedException("N/A at this point.");
        }

        public virtual void Add(int fieldNumber, byte[] value) {
            throw new NotImplementedException("N/A at this point.");
        }


        public virtual void Remove(int fieldNumber) {
            throw new NotImplementedException("N/A at this point.");
        }

        public virtual void Remove(int[] fields) {
            throw new NotImplementedException("N/A at this point.");
        }


        public virtual ISOComponent GetComponent(int fieldNumber) {
            throw new NotImplementedException("N/A at this point.");
        }

        public virtual object GetValue(int fieldNumber) {
            throw new NotImplementedException("N/A at this point.");
        }

        public virtual string GetString(int fieldNumber) {
            throw new NotImplementedException("N/A at this point.");
        }

        public virtual byte[] GetBytes(int fieldNumber) {
            throw new NotImplementedException("N/A at this point.");
        }

        public virtual byte[] GetBytes() {
            throw new NotImplementedException("N/A at this point.");
        }

        public virtual SortedList<int, object> GetChildren() {
            return null;
        }
        

        #endregion

    }

}
