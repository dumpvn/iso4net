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
using System.Runtime.Serialization;


namespace ISO4Net.Library {

    /// <summary>
    /// Exception to identify our library exceptions. If needed, additional information can be added here,
    /// but keep the four basic constructors for compatibility
    /// </summary>
    [Serializable]
    public class ISOException : Exception {

        #region ISOException

        public ISOException() : base() { }
        public ISOException(string message) : base(message) { }
        public ISOException(string message, Exception innerException) : base(message, innerException) { }
        public ISOException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

    }
}
