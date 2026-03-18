using System;
using System.Collections.Generic;
using System.Text;

namespace Calamity
{
    internal class District
    {
        private int district; // id of the starting town of the district

        public IList<(int town, int road)> intersect_roads = new List<(int town, int road)>();  // list of cross-district roads, with cost of the road
        public IList<int> districtTowns = new List<int>(); // list of district towns, used for quick access

        private int _layerOffset = 0;   // offset for the next layer of towns to be added to the district, used for growing the district in layers


        public District(int startTown)
        {
            this.district = startTown;
            districtTowns.Add(startTown);
        }

        public bool CoverNextDistrictLevel(Town[] towns)
        {
            int size = districtTowns.Count;
            int id;
            for (int i = _layerOffset; i < size; i++)
            {
                id = districtTowns[i];
                for (int j = 0; j < towns[id].roads.Count; j++)
                {
                    if (towns[id].roads[j].town.district == Town.NO_DISTRICT)
                    {
                        towns[id].roads[j].town.district = district;
                        districtTowns.Add(towns[id].roads[j].town.id);
                    }
                    else if (district < towns[id].roads[j].town.district)
                    {
                        intersect_roads.Add((towns[id].roads[j].town.district, towns[id].roads[j].cost));
                    }
                }
            }
            if (_layerOffset == size) return true;

            _layerOffset = size;
            return false;
        }
    }
}
