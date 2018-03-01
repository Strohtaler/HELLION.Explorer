﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using System.Runtime.CompilerServices;

namespace HELLION.DataStructures
{
    /// <summary>
    /// 
    /// </summary>
    public class HEJsonBlueprintFile : HEJsonBaseFile
    {
        /// <summary>
        /// Constructor that takes a FileInfo and, if the file exists, triggers the load.
        /// </summary>
        /// <param name="passedFileInfo">The FileInfo representing the file to be loaded.</param>
        public HEJsonBlueprintFile(FileInfo passedFileInfo, object passedParentObject, int populateNodeTreeDepth)
        {
            // Blueprint files

            if (passedParentObject == null) throw new NullReferenceException();
            else parent = (IHENotificationReceiver)passedParentObject;

            if (passedFileInfo == null) throw new NullReferenceException();
            else
            {
                fileInfo = passedFileInfo;

                rootNode = new HEBlueprintTreeNode("BLUEPRINT", HETreeNodeType.Blueprint, nodeText: File.Name, nodeToolTipText: File.FullName);
                dataViewRootNode = new HEGameDataTreeNode("BLUEPRINTDATAVIEW", HETreeNodeType.DataView, nodeText: "Data View", nodeToolTipText: "");
                hierarchyViewRootNode = new HESolarSystemTreeNode("HIERARCHYVIEW", HETreeNodeType.BlueprintHierarchyView, nodeText: "Hierarchy View", nodeToolTipText: "");

                //blueprintObject = new HEBlueprint();

                rootNode.Nodes.Add(dataViewRootNode);
                rootNode.Nodes.Add(hierarchyViewRootNode);

                if (!File.Exists) throw new FileNotFoundException();
                else
                {
                    LoadFile();
                    DeserialiseToBlueprintObject();
                    dataViewRootNode.Tag = jData;
                    dataViewRootNode.CreateChildNodesFromjData(populateNodeTreeDepth);
                }

                BuildSolarSystem();

            }
        }

        public new HEBlueprintTreeNode RootNode => rootNode;

        /// <summary>
        /// This class overrides the type of root node to represent a blueprint.
        /// </summary>
        protected new HEBlueprintTreeNode rootNode;

        public HEGameDataTreeNode DataViewRootNode => dataViewRootNode;

        protected HEGameDataTreeNode dataViewRootNode = null;

        protected HESolarSystemTreeNode hierarchyViewRootNode = null;

        protected HEBlueprint blueprintObject = null;


        /// <summary>
        /// Builds tree nodes from the GameData nodes, with cross-references
        /// </summary>
        public void BuildSolarSystem()
        {
            // Basic operation
            //
            // The following types of HESolarSystemTreeNodes to be created as child nodes of the
            // Solar System root node, then the hierarchy will be applied and nodes re-parented
            // to the appropriate place.


            // 3. Create Ship nodes from GameData - save file
            AddSolarSystemObjects();

            // 6. Rehydrate docked ship/module structure.
            // RehydrateDockedShips();

            // 7. Trigger the root node to recursively update the node counts.
            // RootNode.UpdateCounts();

        }

        /// <summary>
        /// Adds Solar System nodes of the specified type to the RootNode, generated from the 
        /// Game Data nodes.
        /// </summary>
        public void AddSolarSystemObjects()
        {
            // Set up the find key
            string findKey = "Structures";

            Debug.Print("dataViewRootNode.CountOfChildNodes " + dataViewRootNode.CountOfChildNodes);

            TreeNode[] tmpMatches = dataViewRootNode.Nodes.Find(findKey, searchAllChildren: true);

            HEGameDataTreeNode sectionRootNode = null;
            HEGameDataTreeNode arrayRootNode = null;

            foreach (var match in tmpMatches)
            {
                sectionRootNode = (HEGameDataTreeNode)match;
                break;
            }
            if (sectionRootNode == null) throw new NullReferenceException("sectionRootNode was null.");

            foreach (var match2 in sectionRootNode.Nodes)
            {
                arrayRootNode = (HEGameDataTreeNode)match2;
                break;
            }
            if (arrayRootNode == null) throw new NullReferenceException("subRootNode was null.");

            foreach (HEGameDataTreeNode node in arrayRootNode.Nodes)
            {
                //HETreeNodeType newNodeType = HETreeNodeType.Asteroid;

                JObject obj = (JObject)node.Tag;
                long newNodeParentGUID = 0;
                JToken testToken = obj["ParentGUID"];
                if (testToken != null)
                {
                    newNodeParentGUID = (long)obj["ParentGUID"];
                }

                HESolarSystemTreeNode newNode = node.CreateLinkedSolarSystemNode(HETreeNodeType.Ship);
                hierarchyViewRootNode.Nodes.Add(newNode);
            }
        }

        /// <summary>
        /// Re-arranges (rehydrates) existing ship nodes by their DockedToShipGUID forming a tree where the
        /// root node is the parent vessel of the docked ships (and is what shows up on radar in-game).
        /// </summary>
        /// <remarks>
        /// Although this particular function is non-recursive, recursive calls are made when calling
        /// the HETreeNode.GetAllNodes() to get sub-nodes.
        /// </remarks>
        public void RehydrateDockedShips()
        {
            IEnumerable<HESolarSystemTreeNode> shipsToBeReparented = RootNode.ListOfAllChildNodes
                .Cast<HESolarSystemTreeNode>()
                .Where(p => (p.NodeType == HETreeNodeType.Ship) && (p.DockedToShipGUID > 0));

            foreach (HESolarSystemTreeNode node in shipsToBeReparented)
            {
                // If this node has a non-zero value for DockedToShipGUID, process it.
                if (node.DockedToShipGUID != 0)
                {
                    // Find the node that has the GUID matching the DockedToShipGUID of this node.
                    // There can be only one!
                    HESolarSystemTreeNode newParentNode = RootNode.ListOfAllChildNodes
                        .Cast<HESolarSystemTreeNode>()
                        .Where(p => p.GUID == node.DockedToShipGUID)
                        .Single();
                    // If the .Single() causes an exception, there's more than one module docked to that port!

                    // Remove the ship to be re-parented from it's current parent's node collection.
                    node.Parent.Nodes.Remove(node);
                    // Add the ship being re-parented to the new parent's node collection.
                    newParentNode.Nodes.Add(node);
                    // As the new parent's node collection has changed, clear it's cache to force regeneration.
                    newParentNode.ClearCachedData();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }


        public void DeserialiseToBlueprintObject()
        {

            blueprintObject = jData.ToObject<HEBlueprint>();
            blueprintObject.ConnectTheDots();

        }

    }


}
