﻿using System.IO;
using HELLION.DataStructures.UI;

namespace HELLION.DataStructures.Document
{
    /// <summary>
    /// Defines a class to load and hold data from a JSON .save file and associated metadata.
    /// </summary>
    /// <remarks>
    /// Derived from the Base class for a generic JSON data file.
    /// This is a re-write intended to encapsulate more of the functionality of building node trees
    /// of the correct type and enabling lazy population of node tree branches.
    /// </remarks>
    public class GameSave_Json_File : Json_File_UI
    {
        /// <summary>
        /// Constructor that takes a FileInfo and, if the file exists, triggers the load.
        /// </summary>
        /// <param name="passedFileInfo">The FileInfo representing the file to be loaded.</param>
        public GameSave_Json_File(IParent_Json_File passedParentObject, FileInfo passedFileInfo, int populateDepth)
            : base(passedParentObject, passedFileInfo, populateDepth)
        {
            RootNode.Name = File.Name;
            RootNode.NodeType = Base_TN_NodeType.SaveFile;
        }
    }
}
