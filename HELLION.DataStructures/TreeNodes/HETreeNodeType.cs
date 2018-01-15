﻿/// <summary>
/// Defines an enum of node types applicable to the HETreeNode class.
/// </summary>

namespace HELLION.DataStructures
{
    /// <summary>
    /// Defines an enum of HETreeNode types
    /// </summary>
    public enum HETreeNodeType
    {
        Unknown = 0,        // Default for new nodes
        SolarSystemView,    // Node type for the root of the solar system view tree
        DataView,           // Node type for the root of the data view tree
        SearchResultsView,  // Node type for the root of the search results view tree
        Asteroid,           // Node type for Asteroids (loaded from save file, data usually loaded from Asteroids.json)
        Ship,               // Node type for Ships including modules (loaded from save file, data usually loaded from Structures.json)
        Player,             // Node type for player characters, probably includes corpses yet to de-spawn
        //DynamicObject,      // Node type for Dynamic Objects (loaded from save file, data usually loaded from DynamicObjects.json) - MAY BE REDUNDANT
        ExpansionAvailable, // Node type used temporarily to indicate that an item can be evaluated further on-demand, replaced by real data
        JsonArray,          // Node type for a json Array
        JsonObject,         // Node type for a json Object
        JsonProperty,       // Node type for a json Property
        JsonValue,          // Node type for a json Value
        SaveFile,           // Node type for the save file as represented in the node tree
        SaveFileError,      // Node type for the save file in error state as represented in the node tree
        DataFolder,         // Node type for the data folder
        DataFolderError,    // Node type for the data folder
        DataFile,           // Node type for a data file
        DataFileError,      // Node type for a data file
        SolSysStar,         // Node type for the star in the Solar System view (GUID of the star)
        SolSysPlanet,       // Node type for a planet (parent GUID of the star)
        SolSysMoon,         // Node type for a moon (not the star, or orbiting it directly)
    };
}