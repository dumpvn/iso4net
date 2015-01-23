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
using System.Collections;
using System.Collections.Generic;



namespace ISO4Net.Library {

    /// <summary>
    /// Basic Packager implementation
    /// </summary>
    public abstract class ISOPackager {

        #region Protected Members

        protected Hashtable _fields;

        #endregion

        #region Properties


        public int HeaderLength { get; set; }

        public bool HeaderASCII { get; set; }

        public bool HasBitmap {
            get {
                return _fields != null ? _fields[1] is ISOBitmapEncoder : false;
            }
        }

        public int FirstField {
            get {
                return _fields.Count > 1 ? (_fields[1] is ISOBitmapEncoder ? 2 : 1) : 0;
            }
        }


        #endregion

        #region Encode

        public byte[] Encode(ISOComponent component) {
            
            // Loop in each field, encode it a put into a list, where the maximum number may be 128
            List<byte[]> list = new List<byte[]>(128);

            SortedList<int, object> children = component.GetChildren();
            if (children != null) {

                int len = 0;
                byte[] b;
                byte[] header = null;
                ISOComponent c;

                // if it's message get header length
                if (component is ISOMessage) {
                    if (HeaderLength > 0 && ((ISOMessage)component).Header != null) {
                        header = ((ISOMessage)component).Header.Value;
                        if (header != null) 
                            len += header.Length;
                    }
                }

                // MTI
                c = (ISOComponent)children[0];
                if (FirstField > 0 && c != null) {
                    b = ((ISOFieldEncoder)_fields[0]).Encode(c);
                    len += b.Length;
                    list.Add(b);
                }

                // Bitmap
                if (HasBitmap) {
                    c = (ISOComponent)children[-1];
                    if (c != null) {
                        b = GetBitmapEncoder().Encode(c);
                        len += b.Length;
                        list.Add(b);
                    }
                }

                // Fields
                foreach (int k in children.Keys) {
                    if (k >= FirstField) {
                        if ((c = (ISOComponent)children[k]) != null) {

                            ISOFieldEncoder fe = (ISOFieldEncoder)_fields[k];
                            if (fe == null)
                                throw new ISOException(string.Format("No encoder defined for field {0}", k));

                            b = fe.Encode(c);
                            len += b.Length;
                            list.Add(b);
                        }
                    }
                }



                // TODO: ++ Process Extended Bitmap ++



                // Build final message
                int count = 0;
                byte[] msg = new byte[len];

                if (header != null) {
                    Array.Copy(header, 0, msg, 0, header.Length);
                    count += header.Length;
                }

                for (int i = 0; i < list.Count; i++) {
                    Array.Copy(list[i], 0, msg, count, list[i].Length);
                    count += list[i].Length;
                }

                return msg;

            }

            return null;
        }

        #endregion
        
        #region Decode

        public int Decode(ISOComponent component, byte[] data) {

            int offset = 0;

            #region - HEADER -

            if (component is ISOMessage && HeaderLength > 0) {
                byte[] header = new byte[HeaderLength];
                Array.Copy(data, 0, header, 0, HeaderLength);
                
                ((ISOMessage)component).Header = new ISOHeader(header);
                ((ISOMessage)component).Header.ASCIIEncoding = this.HeaderASCII;
                
                offset += HeaderLength;
            }

            #endregion

            #region - MTI -

            if (!(_fields[0] is ISOBitmapEncoder)) {
                ISOComponent mti = ((ISOFieldEncoder)_fields[0]).CreateComponent(0);
                offset += ((ISOFieldEncoder)_fields[0]).Decode(mti, data, offset);

                component.Add(mti);
            }

            #endregion

            #region - BITMAP -

            BitArray bitArray = null;
            int totalFields = _fields.Count;

            if (HasBitmap) {
                
                ISOBitmap bitmap = new ISOBitmap(-1);
                offset += GetBitmapEncoder().Decode(bitmap, data, offset);
                bitArray = (BitArray)bitmap.Value;

                component.Add(bitmap);
                totalFields = Math.Min(totalFields, bitArray.Length);
            }

            #endregion

            #region - FIELDS -

            for (int i = FirstField; i < totalFields; i++) {

                if (bitArray[i]) {              // bit array starts at 0

                    if (_fields[i] == null)
                        throw new ISOException(string.Format("Field encoder not defined for {0}", i));

                    try {
                        ISOComponent c = ((ISOFieldEncoder)_fields[i]).CreateComponent(i);
                        offset += ((ISOFieldEncoder)_fields[i]).Decode(c, data, offset);

                        component.Add(c);
                    }
                    catch (ISOException e) {
                        e = new ISOException(string.Format("Error decoding field {0}: {1} ({2}). Bytes consumed: {3} ", i, e.Message, e.InnerException.Message, offset));
                        throw;
                    }
                }


            }

            #endregion


            // TODO: Do something if still there're bytes to process...?


            return offset;
        }

        #endregion
        
        #region Public Methods


        public string GetFieldName(ISOComponent component, int fieldNumber) {
            if (_fields != null) {
                if (_fields[fieldNumber] is ISOFieldEncoder) {
                    return ((ISOFieldEncoder)_fields[fieldNumber]).Name;
                }
            }
            return string.Empty;
        }

        public ISOFieldEncoder GetFieldEncoder(int fieldNumber) {
            if (_fields != null) {
                if (_fields[fieldNumber] is ISOFieldEncoder) {
                    return (ISOFieldEncoder)_fields[fieldNumber];
                }
            }
            return null;
        }

        public ISOFieldEncoder GetBitmapEncoder() {
            if (_fields != null) {
                if (_fields[1] is ISOBitmapEncoder) {
                    return (ISOFieldEncoder)_fields[1];
                }
            }
            return null;
        }

        public void SetFieldEncoder(int fieldNumber, ISOFieldEncoder encoder) {
            if (_fields != null) {
                if (_fields.ContainsKey(fieldNumber)) {
                    _fields[fieldNumber] = encoder;
                }
            }
        }

        public ISOMessage CreateISOMessage() {
            ISOMessage m = new ISOMessage();
            m.Packager = this;
            return m;
        }


        #endregion

    }
}

