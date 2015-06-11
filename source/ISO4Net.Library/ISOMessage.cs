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
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace ISO4Net.Library {

    /// <summary>
    /// General ISO Message implementation
    /// </summary>
    public class ISOMessage : ISOComponent, ICloneable, IDisposable {

        #region Protected

        protected SortedList<int, object> _fields;                          // Fields collection
        protected int _biggestField;                                        // Used to keep track of the biggest field ID in the collection
        protected bool _changed;                                            // Indicates if there was any change in the message

        #endregion

        #region Properties


        /// <summary>
        /// Number of the biggest field in the collection
        /// </summary>
        public override int TopField {
            get {
                return _biggestField;
            }
        }

        /// <summary>
        /// Message associated packager
        /// </summary>
        public ISOPackager Packager { get; set; }

        /// <summary>
        /// Messsge header
        /// </summary>
        public ISOHeader Header { get; set; }

        /// <summary>
        /// Message Type Indicator
        /// </summary>
        public string MTI {
            get {
                return _fields.ContainsKey(0) ? ((ISOField)_fields[0]).Value.ToString() : string.Empty;
            }
            set {
                Add(0, value);
            }
        }


        #endregion

        #region ISOMessage

        public ISOMessage() {
            _fields = new SortedList<int, object>();
            _biggestField = -1;
        }

        public ISOMessage(string mti) : this() {
            MTI = mti;
        }

        #endregion

        #region Public Methods


        public override void Add(ISOComponent component) {
            if (component != null) {
                int key = (int)component.Key;

                if (_fields.ContainsKey(key))                       // if exists, remove the old field
                    _fields.Remove(key);

                _fields.Add(key, component);

                if (_biggestField < key)
                    _biggestField = key;

                _changed = true;
            }
        }

        public override void Add(int fieldNumber, string value) {
            if (fieldNumber >= 0) {
                if (!(Packager is ISOPackager)) {
                    Add(new ISOField(fieldNumber, value));
                }
                else {
                    // Use the configured Packager
                    object obj = Packager.GetFieldEncoder(fieldNumber);
                    if (!(obj is ISOBinaryField))
                        Add(new ISOField(fieldNumber, value));
                    else
                        Add(new ISOBinaryField(fieldNumber, ASCIIEncoding.ASCII.GetBytes(value)));
                }
            }
        }

        public override void Add(int fieldNumber, byte[] value) {
            if (value != null ) {
                Add(new ISOBinaryField(fieldNumber, value));
            }
        }


        public override void Remove(int fieldNumber) {
            if (_fields.Remove(fieldNumber))
                _changed = true;
        }

        public override void Remove(int[] fields) {
            for (int i = 0; i < fields.Length; i++)
                Remove(fields[i]);
        }


        public override object GetValue(int fieldNumber) {
            ISOComponent c = GetComponent(fieldNumber);
            return c != null ? c.Value : null;
        }

        public override string GetString(int fieldNumber) {
            string s = string.Empty;

            if (ContainsField(fieldNumber)) {
                object obj = ((ISOComponent)_fields[fieldNumber]).Value;
                if (obj is string)
                    s = obj.ToString();
                else
                    s = Utils.HexString((byte[])obj);
            }

            return s;
        }

        public override byte[] GetBytes(int fieldNumber) {
            byte[] b = null;

            if (ContainsField(fieldNumber)) {
                object obj = ((ISOComponent)_fields[fieldNumber]).Value;
                if (obj is string)
                    b = ASCIIEncoding.ASCII.GetBytes(obj.ToString());
                else
                    b = (byte[])obj;
            }

            return b;
        }

        
        public bool ContainsField(int fieldNumber) {
            return _fields.ContainsKey(fieldNumber);
        }

        public bool ContainsFields(int[] fields) {
            for (int i = 0; i < fields.Length; i++) {
                if (!ContainsField(fields[i]))
                    return false;
            }
            return true;
        }


        public override ISOComponent GetComponent(int fieldNumber) {
            return (ISOComponent)_fields[fieldNumber];
        }


        public string Dump(bool includeBitmap = false) {

            StringBuilder sb = new StringBuilder();

            // Header
            if (Header != null)
                sb.AppendFormat("<iso header=\"{0}\">\n", Header.ToString());
            else
                sb.AppendLine("<iso>");

            // Bitmap
            if (_fields.ContainsKey(-1))
                sb.AppendFormat("\t<bitmap>{0}</bitmap>\n", _fields[-1].ToString());

            // MTI
            if (_fields.ContainsKey(0))
                sb.AppendFormat("\t<mti>{0}</mti>\n", ((ISOComponent)_fields[0]).Value);

            // Fields
            IList<int> keys = _fields.Keys;
            for (int i = 0; i < keys.Count; i++) {

                ISOComponent c = (ISOComponent)_fields[keys[i]];
                if ((int)c.Key > 0 && c is ISOField)
                    sb.AppendFormat("\t<field id=\"{0}\" value=\"{1}\" />\n", c.Key, c.Value);
                else if ((int)c.Key > 0 && c is ISOBinaryField)
                    sb.AppendFormat("\t<field id=\"{0}\" value=\"{1}\" />\n", c.Key, Utils.HexString((byte[])c.Value));
            }

            sb.AppendLine("</iso>");

            return sb.ToString();
        }


        #endregion

        #region Private Methods


        /// <summary>
        /// Calculates the bitmap based on the current fields
        /// </summary>
        private void RefreshBitmap() {
            if (_changed) {

                int tp = Math.Min(TopField, 192);
                BitArray bmp = new BitArray((tp + 62 >> 6 << 6) + 1);

                for (int i = 1; i <= tp; i++) {
                    if (_fields.ContainsKey(i))
                        bmp.Set(i, true);
                }

                Add(new ISOBitmap(-1, bmp));
                _changed = false;
            } 
        }


        #endregion

        #region ISOComponent

        public override byte[] Encode() {
            if (Packager != null) {
                RefreshBitmap();
                return Packager.Encode(this);
            }
            else
                throw new ISOException("Can't decode: packager not defined for this component");
        }

        public override int Decode(byte[] data) {
            if (Packager != null)
                return Packager.Decode(this, data);
            else
                throw new ISOException("Can't decode: packager not defined for this component");
        }

        public override SortedList<int, object> GetChildren() {
            return new SortedList<int, object>(_fields);
        }

        #endregion

        #region ICloneable

        public object Clone() {

            ISOMessage m = new ISOMessage();
            m._biggestField = _biggestField;
            
            // Clone each field
            m._fields = new SortedList<int, object>(_fields.Count);
            IList<int> keys = _fields.Keys;

            for (int i = 0; i < keys.Count; i++) {
                m._fields.Add(keys[i], ((ISOField)_fields[keys[i]]).Clone());
            }

            m.Header = Header;
            m.Key = Key;
            m.Value = Value;
            m.Packager = Packager;
            m.RefreshBitmap();

            return m;
        }

        #endregion

        #region IDisposable

        public void Dispose() {
            if (_fields != null) _fields.Clear();
        }

        #endregion

    }
}
