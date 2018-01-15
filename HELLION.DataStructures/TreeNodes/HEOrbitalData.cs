﻿/// <summary>
/// Defines a derived HETreeNode to handle objects in the Solar System view.
/// Also defines a node sorter that sorts by Semi-Major axis instead of by name which is
/// the default on a TreeView control, and a class to hold the orbital data.
/// </summary>
namespace HELLION.DataStructures
{
    /// <summary>
    /// Defines a class to hold orbital data as used by the game.
    /// </summary>
    public class HEOrbitalData
    {
        public long ParentGUID { get; set; } = 0;
        public long VesselID { get; set; } = 0;
        public long VesselType { get; set; } = 0;
        public double Eccentricity { get; set; } = 0;
        public double SemiMajorAxis { get; set; } = 0;
        public double LongitudeOfAscendingNode { get; set; } = 0;
        public double ArgumentOfPeriapsis { get; set; } = 0;
        public double Inclination { get; set; } = 0;
        public double TimeSincePeriapsis { get; set; } = 0;
        public double SolarSystemPeriapsisTime { get; set; } = 0;


        /// <summary>
        /// Basic constructor used to initialise an empty object.
        /// </summary>
        public HEOrbitalData()
        { }

        /// <summary>
        /// Constructor that copies its parameters from another object, used by the
        /// Clone() routine to 'shallow clone' the object
        /// </summary>
        /// <param name="another">The HEOrbitalData object to be cloned.</param>
        private HEOrbitalData(HEOrbitalData another)
        {
            ParentGUID = another.ParentGUID;
            VesselID = another.VesselID;
            VesselType = another.VesselType;
            Eccentricity = another.Eccentricity;
            SemiMajorAxis = another.SemiMajorAxis;
            LongitudeOfAscendingNode = another.LongitudeOfAscendingNode;
            ArgumentOfPeriapsis = another.ArgumentOfPeriapsis;
            Inclination = another.Inclination;
            TimeSincePeriapsis = another.TimeSincePeriapsis;
            SolarSystemPeriapsisTime = another.SolarSystemPeriapsisTime;
        }

        /// <summary>
        /// Builds and returns a clone of the current HEOrbitalData object
        /// </summary>
        /// <returns></returns>
        public HEOrbitalData Clone()
        {
            return new HEOrbitalData(this);
        }
    }
}