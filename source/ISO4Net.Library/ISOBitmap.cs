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
using System.Text;



namespace ISO4Net.Library {

    /// <summary>
    /// Basic Bitmap representation
    /// </summary>
    public class ISOBitmap : ISOComponent, ICloneable {

        #region ISOBitmap

        public ISOBitmap(int fields) {
            base.Key = fields;
            //Key = fields;
        }

        public ISOBitmap(int fields, BitArray bits) {
            base.Key = fields;
            base.Value = bits;
            //Key = fields;
            //Value = bits;
        }

        #endregion
        
        #region ISOComponent

        public override byte[] Encode() {
            throw new NotImplementedException("N/A at this point. Bitmap must be encoded with it's own encoder");
        }

        public override int Decode(byte[] data) {
            throw new NotImplementedException("N/A at this point. Bitmap must be decoded with it's own decoder");
        }
        
        #endregion

        #region ToString()


        public override string ToString() {

            // Hexadecimal representation
            if (Value is BitArray) {

                BitArray b = (BitArray)Value;
                StringBuilder sb = new StringBuilder((b.Count / 8) * 2);
                StringBuilder sbAux = new StringBuilder(2);

                for (int i = 1; i < b.Count - 8; i += 8) {
                    sbAux.Clear();
                    for (int j = 0; j < 8; j++) {
                        sbAux.Append(b[i + j] == true ? "1" : "0");
                    }

                    int byteVal = Convert.ToInt32(sbAux.ToString(), 2);
                    sb.Append(byteVal.ToString("X2"));
                }

                return sb.ToString();
            }
            else
                return base.ToString();
        }

        public string ToStringBinary() {

            // Binary representation (bits)
            if (Value is BitArray) {

                BitArray b = (BitArray)Value;
                StringBuilder sb = new StringBuilder(b.Count);

                for (int i = 0; i < b.Count; i++) {
                    if (b[i])
                        sb.Append("1");
                    else
                        sb.Append("0");
                }

                return sb.ToString();
            }
            else
                return base.ToString();
        }


        #endregion

        #region ICloneable

        public object Clone() {
            ISOBitmap b = new ISOBitmap((int)Key);
            b.Value = Value;

            return b;
        }

        #endregion

    }
}
