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


namespace ISO4Net.Library {


    public abstract class ISOBitmapEncoder : ISOFieldEncoder {

        #region ISOBitmapEncoder

        public ISOBitmapEncoder() : base() { }

        public ISOBitmapEncoder(int length, string name) : base(length, name) { }

        #endregion

        #region Public Methods

        public override ISOComponent CreateComponent(int fieldNumber) {
            return new ISOBitmap(fieldNumber);
        }

        #endregion
        
    }
}
