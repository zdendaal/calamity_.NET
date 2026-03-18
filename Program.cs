using System.Runtime.CompilerServices;

namespace Calamity
{
    class Program
    {
        public static int GetLineNum([CallerLineNumber] int line = 0) => line;


        static void Main(string[] args)
        {
            // loading first row t,d,r
            string? input = Console.ReadLine();
            string[] parts = input?.Split(' ') ?? throw new ArgumentNullException("Source variable: " + nameof(input) + "\tline: " + GetLineNum());

            if (parts.Length < 3) return;

            int t = int.Parse(parts[0]);
            int d = int.Parse(parts[1]);
            int r = int.Parse(parts[2]);

            // district towns
            Town[] towns = new Town[t];
            District[] districts = new District[d];
            LinkedList<int> districtsIds = new LinkedList<int>();
            for (int i = 0; i < d; i++) // loading district towns
            {
                towns[i] = new Town(i, i);
                districts[i] = new District(i);
                districtsIds.AddLast(i);
            }

            // regular towns
            for (int i = d; i < t; i++)
                towns[i] = new Town(i, Town.NO_DISTRICT);

            // loading roads
            int t1, t2, c;
            for(int i = 0; i < r; i++)
            {
                input = Console.ReadLine();
                parts = input?.Split(' ') ?? throw new ArgumentNullException("Source variable: " + nameof(input) + "\tline: " + GetLineNum());

                t1 = int.Parse(parts[0]);
                t2 = int.Parse(parts[1]);
                c = int.Parse(parts[2]);

                towns[t1 - 1].roads.Add((towns[t2 - 1], c));
                towns[t2 - 1].roads.Add((towns[t1 - 1], c));
            }

            int cost = Search(districts, towns, districtsIds);
            Console.WriteLine(cost);
        }


        /// <summary>
        /// Returns the cost of the minimum spanning tree of two phases: 
        ///     1. sum of cost of each district's minimum spanning tree, 
        ///     2. cost of the minimum spanning tree of the collapsed graph of districts
        /// </summary>
        /// <param name="districts">Empty districts for be grown up around district towns</param>
        /// <param name="towns">Array of nodes in graph</param>
        /// <param name="districtIds">Ids of district towns</param>
        /// <returns></returns>
        public static int Search(District[] districts, Town[] towns, LinkedList<int> districtIds)
        {
            LinkedListNode<int> node = districtIds.First;
            LinkedListNode<int> nextNode;

            // asingingtowns to their districts by growing the districts from startTowns until all towns are assigned to a district
            while (districtIds.Count > 0)
            {
                nextNode = node.Next ?? districtIds.First;

                if (districts[node.Value].CoverNextDistrictLevel(towns))
                    districtIds.Remove(node);

                node = nextNode;
            }

            int cost = 0;
            Prim prim = new Prim(towns.Length);
            for(int i = 0; i < districts.Length; i++)
            {
                cost += prim.ReturnDistrictCost(i, i, towns);
            }

            Town[] collapsed = new Town[districts.Length];
            for(int i = 0; i < districts.Length; i++)
            {
                collapsed[i] = new Town(i, i);
            }

            int targetTown;
            for (int i = 0; i < districts.Length; i++)
            {
                towns[i] = new Town(i, i);
                for (int r = 0; r < districts[i].intersect_roads.Count; r++)
                {
                    targetTown = districts[i].intersect_roads[r].town;

                    collapsed[i].roads.Add((collapsed[targetTown], districts[i].intersect_roads[r].road));
                    collapsed[targetTown].roads.Add((collapsed[i], districts[i].intersect_roads[r].road));
                }
            }

            Prim primCollapsed = new Prim(districts.Length);
            cost += primCollapsed.ReturnDistrictCost(0, collapsed);

            return cost;
        }
    }
}