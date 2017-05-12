﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Diagnostics;

namespace HELLION.DataStructures
{
    // Derive a class for the .save game data file from the HEJsonFile class
    // This is used to change the LoadFile to output to a JObject instead of a JArray and new save functionality

    public class HEGameFile : HEJsonFile
    {
        // Derived from the Base class for a generic JSON data file

        // public string FileName { get; set; }
        public new JObject JData { get; set; } // Master save is loaded in to a JObject (Data files use JArrays instead)

        // Externally read-only properties
        public bool IsFileWritable { get; private set; }
        // public bool IsFileLoaded { get; set; }
        // public bool LoadError { get; set; }
        // public bool LogToDebug { get; set; }
        //private new bool SkipLoading; // Not used, hidden at this level


        public HEGameFile()
        {
            // Basic Constructor
            FileName = "";
            JData = null;
            IsFileLoaded = false;
            IsFileWritable = false;
            LoadError = false;
            LogToDebug = false;
            SkipLoading = false;
        }

        public HEGameFile(string sFileName)
        {
            // Constructor that allows the file name to be set
            FileName = sFileName;
            JData = null;
            IsFileLoaded = false;
            IsFileWritable = false;
            LoadError = false;
            LogToDebug = false;
            SkipLoading = false;
        }

        public void MakeReadWrite()
        {
            // Changes the IsFileWritable to true - may need some additional checks here though
            if (IsFileLoaded && !LoadError)
            {
                IsFileWritable = true;
            }
            else
            {
                IsFileWritable = false;
            }
        } // End of MakeReadWrite()

        public void MakeReadOnly()
        {
            // Changes the IsFileWritable to false
            IsFileWritable = false;
        } // End of MakeReadOnly()

        public new bool LoadFile()
        {
            // Load file data from FileName and parse to the JData JObject of type IOrderedEnumerable<JToken>
            // Returns true if there was a loading error

            if (!SkipLoading)
            {
                if (System.IO.File.Exists(FileName))
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(FileName))
                        {
                            // Process the stream with the JSON Text Reader in to a JArray; was previously an IOrderedEnumerable<JToken> JObject
                            using (JsonTextReader jtr = new JsonTextReader(sr))
                            {
                                JData = (JObject)JToken.ReadFrom(jtr);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // Some error handling to be implemented here
                        LoadError = true;
                        if (LogToDebug) Debug.Print("Exception caught during StreamReader or JsonTextReader while processing " + FileName
                            + Environment.NewLine + e.ToString());
                    }

                    if (JData == null || JData.Count == 0)
                    {
                        // The data didn't load
                        LoadError = true;
                        if (LogToDebug) Debug.Print("JData is null or empty: " + FileName);
                    }
                    else
                    {
                        // We should have some data in the array
                        IsFileLoaded = true;
                        if (LogToDebug) Debug.Print("Processed " + JData.Count + " objects from: " + FileName);
                    }
                }
                else
                {
                    // Invalid file name
                    LoadError = true;
                    if (LogToDebug) Debug.Print("Invalid file name passed: " + FileName);
                }
            }
            else
            {
                if (LogToDebug) Debug.Print("Skipping file: " + FileName);
            } // End of SkipLoading check
                
            // Return the value of LoadError
            return LoadError;
        } // End of LoadFile()

        public void SaveFile()
        {
            // Will save this file (not yet implemented)

            if (false)
            {
                //
            }
            /*
            // Check to see if this fle already exists
            if (MainFile.Exists(FileName))
            {
                // MainFile already exists, create a backup copy (.save.bak)

                // do stuff
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(FileName))
                {
                    // Read the contents of the file
                    String file = sr.ReadToEnd();
                    // Parse the contents of the file in to the Data JArray
                    JData = JArray.Parse(file);
                }
            }
            catch (IOException)
            {
                // Some error handling to be implemented here
            }

            if (JData == null || JData.Count == 0)
            {
                // The data didn't load
            }
            else
            {
                // We should have some data in the array
                IsFileDirty = false;
            }

            */

        } // End of SaveFile()


    } // End of class HEjsonFile



} // End of namespace HELLION.DataStructures
