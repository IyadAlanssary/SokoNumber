namespace HelloWorld
{
    class Algorithms
    {
        public static Map map = Program.map;
        public static List<Map> passedMaps = new List<Map>() { };
        public static HashSet<int> passedMapsHash = new HashSet<int>();
        public static int? visitedStates = 0;
        public static void DFS()
        {
            map = Program.map;
            Stack<Map> stack = new Stack<Map>() { };
            stack.Push(map);
            visitedStates++;
            map.numberOfMoves = 0;
            while (stack.Count != 0)
            {
                Map peeky = stack.Peek();
                if (peeky.isFinal())
                {
                    Program.map = peeky;
                    return;
                }
                else
                {
                    stack.Pop();
                    if (passedB4(peeky))
                        continue;
                    passedMaps.Add(peeky);
                    // if(!passedMapsHash.Add(peeky.map.GetHashCode()))
                    //     continue;
                    visitedStates++;
                    List<Map> possibles = peeky.possibleStates();
                    foreach (Map m in possibles)
                    {
                        m.parent = peeky;
                        stack.Push(m);
                        m.numberOfMoves = peeky.numberOfMoves + 1;
                    }
                }
            }
        }
        public static void BFS()
        {
            map = Program.map;
            Queue<Map> queue = new Queue<Map> { };
            queue.Enqueue(map);
            visitedStates++;
            while (queue.Count != 0)
            {
                Map peeky = queue.Peek();

                if (peeky.isFinal())
                {
                    Program.map = peeky;
                    return;
                }
                else
                {
                    queue.Dequeue();

                    if (passedB4(peeky))
                        continue;
                    passedMaps.Add(peeky);
                    visitedStates++;

                    List<Map> possibles = peeky.possibleStates();
                    foreach (Map m in possibles)
                    {
                        m.parent = peeky;
                        queue.Enqueue(m);
                        m.numberOfMoves = peeky.numberOfMoves + 1;
                    }
                }
            }
        }
        public static void UCS()
        {
            map = Program.map;
            PriorityQueue<Map, int> queue = new PriorityQueue<Map, int> { };
            queue.Enqueue(map, map.numberOfMoves);
            map.numberOfMoves = 0;
            visitedStates++;
            while (queue.Count != 0)
            {
                Map peeky = queue.Peek();

                if (peeky.isFinal())
                {
                    Program.map = peeky;
                    return;
                }
                else
                {
                    queue.Dequeue();
                    // if (passedB4(peeky))
                    //     continue;
                    passedMaps.Add(peeky);
                    visitedStates++;

                    List<Map> possibles = peeky.possibleStates();
                    foreach (Map m in possibles)
                    {
                        m.parent = peeky;
                        m.numberOfMoves = peeky.numberOfMoves + 1;
                        queue.Enqueue(m, m.numberOfMoves);
                    }
                }
            }
        }
        public static void AStar()
        {
            map = Program.map;
            List<Map> list = new List<Map> { };
            
            list.Add(map);
            map.numberOfMoves = 0;
            visitedStates++;
            while (list.Count != 0)
            {
                list = list.OrderBy(o => o.heuristicPlusCost).ToList();

                List<Map> list2 = new List<Map>() { };
                for (int i = 0; i < list.Count; i++)
                {
                    int t = list[0].heuristicPlusCost;
                    if (list[i].heuristicPlusCost == t)
                    {
                        list2.Add(list[i]);
                    }
                }
                list2 = list2.OrderBy(o => o.heuristic).ToList();

                Map peeky = list2.First();

                if (peeky.isFinal())
                {
                    Program.map = peeky;
                    return;
                }
                else
                {
                    list.Remove(peeky);
                    // if (passedB4(peeky))
                    //     continue;
                    passedMaps.Add(peeky);
                    visitedStates++;

                    List<Map> possibles = peeky.possibleStates();
                    foreach (Map m in possibles)
                    {
                        m.parent = peeky;
                        m.numberOfMoves = peeky.numberOfMoves + 1;
                        m.heuristicPlusCost = m.numberOfMoves + m.heuristic;
                        list.Add(m);
                    }
                }
            }
        }
        static bool passedB4(Map m)
        {
            // if(passedMapsHash.Contains(hash))
            //     return true;
            // return false;
            foreach (Map passedMap in passedMaps)
            {
                if (passedMap.equals(m))
                {
                    return true;
                }
            }
            return false;
        }
    }
}