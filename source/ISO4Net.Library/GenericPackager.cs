
/*
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
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;



namespace ISO4Net.Library {

    /// <summary>
    /// Generic and default message packager implementation. This implementation reads the ISO configuration from a XML file
    /// </summary>
    public class GenericPackager : ISOPackager {
        
        #region Private

        private int _bitmapField = 1;

        #endregion

        #region Properties

        public string FileName { get; set; }

        #endregion

        #region GenericPackager


        public GenericPackager() : base() { }

        public GenericPackager(string cfgFile) : this() {
            FileName = cfgFile;
            ReadConfigurationFile();
        }


        #endregion
    
        #region Protected Methods


        protected void ReadConfigurationFile() {
            if (File.Exists(FileName)) {
                try {
                    string xmlData = File.ReadAllText(FileName);
                    ReadConfigurationData(xmlData);
                }
                catch (Exception e) {
                    throw new IOException(string.Format("Error reading file {0}: {1}", FileName, e.Message), e);
                }
            }
            else
                throw new IOException(string.Format("Cannot find file {0}", FileName));
        }

        protected void ReadConfigurationData(string xmlData) {

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlData);

            XmlNodeList fields = xDoc.GetElementsByTagName("isofield");
            if (fields != null) {

                _fields = new Hashtable(fields.Count);

                // Read xml file content and create the list with the configuration for each field
                for (int i = 0; i < fields.Count; i++) {

                    try {
                        int id = Convert.ToInt32(fields[i].Attributes["id"].Value);
                        int len = Convert.ToInt32(fields[i].Attributes["len"].Value);
                        string name = fields[i].Attributes["name"].Value;
                        string className = fields[i].Attributes["class"].Value;

                        // Create encoder instance
                        // TODO: Change for a more generic method capable of loading classes from other assemblies
                        ISOFieldEncoder fEncoder = (ISOFieldEncoder)Activator.CreateInstance(Type.GetType(className), len, name);

                        _fields.Add(id, fEncoder);

                    }
                    catch (Exception e) {
                        throw new ISOException(string.Format("Exception loading iso field configuration (Index={0})", i), e);
                    }
                }
            }
            else
                throw new ISOException(string.Format("Packager configuration file {0} is not valid", FileName));
        }


        #endregion

    }
}
