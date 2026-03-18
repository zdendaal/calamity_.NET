using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Calamity
{
    /// <summary>
    /// Prims algorithm for finding the cheapest edges in graph connecting all the districts together to form of tree graph
    /// </summary>
    internal class Prim
    {
        private readonly bool[] _visited;

        private PriorityQueue<int,int> _pqRoads;

        public Prim(int t) 
        {
            _visited = Enumerable.Repeat(false, t).ToArray();
            _pqRoads = new PriorityQueue<int, int>();
        }

        /// <summary>
        /// Using Prim algorithm calculates cost of minimum spanning tree in the district d starting from town with id townId
        /// </summary>
        /// <param name="d">district Id</param>
        /// <param name="townId">town Id</param>
        /// <param name="towns">Array with all towns</param>
        /// <returns>Minimum spanning tree cost</returns>
        public int ReturnDistrictCost(int d, int townId, Town[] towns)
        {
            int cost = 0;
            _pqRoads.Enqueue(townId, 0);

            while(_pqRoads.TryDequeue(out int cheapestTownId, out int cheapestCost))
            {
                if (_visited[cheapestTownId])
                    continue;

                cost += cheapestCost;

                _visited[cheapestTownId] = true;

                foreach(var (town, roadCost) in towns[cheapestTownId].roads)
                {
                    if (town.district == d && !_visited[town.id])
                        _pqRoads.Enqueue(town.id, roadCost);
                }
            }

            _pqRoads.Clear();
            return cost;
        }

        /// <summary>
        /// Using Prim algorithm calculates cost of minimum spanning tree among condensed districts starting from town with id townId
        /// </summary>
        /// <param name="townId">town Id</param>
        /// <param name="towns">Array with all towns</param>
        /// <returns>Minimum spanning tree cost</returns>
        public int ReturnDistrictCost(int townId, Town[] towns)
        {
            int cost = 0;
            _pqRoads.Enqueue(townId, 0);

            while (_pqRoads.TryDequeue(out int cheapestTownId, out int cheapestCost))
            {
                if (_visited[cheapestTownId])
                    continue;

                cost += cheapestCost;

                _visited[cheapestTownId] = true;

                foreach (var (town, roadCost) in towns[cheapestTownId].roads)
                {
                    if (!_visited[town.id])
                        _pqRoads.Enqueue(town.id, roadCost);
                }
            }

            _pqRoads.Clear();
            return cost;
        }
    }
}
