using System;
using System.Collections.Generic;
using System.Text;

namespace Calamity
{
    /// <summary>
    /// Stores properties af town
    /// </summary>
    internal class Town
    {
        public readonly int id; // identifier of the town
        public int district;   // indentifier of the district the town belongs to
        public static int NO_DISTRICT = -1; // constant for town with no district
        public IList<(Town town, int cost)> roads; // list of roads that connect this town to other towns, with the cost of the road

        public Town(int id, int district) 
        {
            this.id = id;
            this.district = district;
            roads = new List<(Town town, int cost)>();
        }
    }
}
