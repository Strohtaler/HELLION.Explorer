﻿using System;
using Newtonsoft.Json.Linq;

/// <summary>
/// Defines a derived HETreeNode to handle objects in the Solar System view.
/// Also defines a node sorter that sorts by Semi-Major axis instead of by name which is
/// the default on a TreeView control, and a class to hold the orbital data.
/// </summary>
namespace HELLION.DataStructures
{
    /// <summary>
    /// Defines a class to hold orbital data as used by the game.
    /// </summary>
    /// <remarks>
    /// May need to be expanded to handle additional functions related to new
    /// coordinate generation, for moving or spawning of ships/modules.
    /// </remarks>
    public class OrbitalData
    {
        public long? ParentGUID { get; set; } = null;
        public double? Eccentricity { get; set; } = null;
        public double? SemiMajorAxis { get; set; } = null;
        public double? LongitudeOfAscendingNode { get; set; } = null;
        public double? ArgumentOfPeriapsis { get; set; } = null;
        public double? Inclination { get; set; } = null;
        public double? TimeSincePeriapsis { get; set; } = null;
        public double? SolarSystemPeriapsisTime { get; set; } = null;

        /// <summary>
        /// Basic constructor used to initialise an empty object.
        /// </summary>
        public OrbitalData()
        { }

        /// <summary>
        /// Constructor that takes a JObject representing the OrbitData.
        /// </summary>
        public OrbitalData(JObject orbitData)
        {
            if (orbitData != null)
            {
                ParentGUID = (long)orbitData["ParentGUID"];
                SemiMajorAxis = (double)orbitData["SemiMajorAxis"];
                Inclination = (double)orbitData["Inclination"];
                Eccentricity = (double)orbitData["Eccentricity"];
                ArgumentOfPeriapsis = (double)orbitData["ArgumentOfPeriapsis"];
                LongitudeOfAscendingNode = (double)orbitData["LongitudeOfAscendingNode"];

                // Celestial Bodies don't have the following two properties, so additional checking
                // will be required to prevent null reference exceptions.
                JToken testToken = orbitData["TimeSincePeriapsis"];
                if (testToken != null)
                {
                    TimeSincePeriapsis = (double)orbitData["TimeSincePeriapsis"];
                }
                else TimeSincePeriapsis = -1;

                testToken = orbitData["SolarSystemPeriapsisTime"];
                if (testToken != null)
                {
                    SolarSystemPeriapsisTime = (double)orbitData["SolarSystemPeriapsisTime"];
                }
                else SolarSystemPeriapsisTime = -1;
            }
        }

        /// <summary>
        /// Constructor that copies its parameters from another object, used by the
        /// Clone() routine to 'shallow clone' the object
        /// </summary>
        /// <param name="another">The OrbitalData object to be cloned.</param>
        private OrbitalData(OrbitalData another)
        {
            ParentGUID = another.ParentGUID;
            Eccentricity = another.Eccentricity;
            SemiMajorAxis = another.SemiMajorAxis;
            LongitudeOfAscendingNode = another.LongitudeOfAscendingNode;
            ArgumentOfPeriapsis = another.ArgumentOfPeriapsis;
            Inclination = another.Inclination;
            TimeSincePeriapsis = another.TimeSincePeriapsis;
            SolarSystemPeriapsisTime = another.SolarSystemPeriapsisTime;
        }

        /// <summary>
        /// Builds and returns a clone of the current OrbitalData object
        /// </summary>
        /// <returns></returns>
        public OrbitalData Clone() => new OrbitalData(this);

        /// <summary>
        /// Placeholder for a clone routine that generates an imperfect clone, used to generate similar
        /// coordinates to be used in spawning or moving of objects.
        /// </summary>
        /// <returns></returns>
        public OrbitalData FuzzyClone()
        {
            throw new NotImplementedException("Fuzzy clone is not yet implemented.");
        }

    }
}
