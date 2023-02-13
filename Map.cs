using Raylib_cs;

namespace HelloWorld
{
    class Map
    {
        public string[,] map;
        public List<Cell> numbers = new List<Cell> { };
        public static List<Cell> destinations = new List<Cell> { };
        public Map? parent;
        public string? moveThatResultedInThisMap;
        public int numberOfMoves, heuristic, heuristicPlusCost;
        private string EMPTY = "_";
        public static bool blockGeneratingLevel = false;
        static Sound winSFX = Raylib.LoadSound("new-win-gold-special.wav");

        public Map(string[,] savedMap)
        {
            this.map = new String[savedMap.GetLength(0), savedMap.GetLength(1)];
            //parsing the map
            for (int i = 0; i < savedMap.GetLength(0); i++)
            {
                for (int j = 0; j < savedMap.GetLength(1); j++)
                {
                    this.map[i, j] = savedMap[i, j];
                    //cells that have + or - (e.g. +1)
                    if (savedMap[i, j][0] == '+' || savedMap[i, j][0] == '-')
                    {
                        continue;
                    }
                    //cells that have a number and a destination (letter) 
                    else if (savedMap[i, j].Length == 2)
                    {
                        numbers.Add(new Cell(savedMap[i, j][0].ToString(), i, j));
                        destinations.Add(new Cell(savedMap[i, j][1].ToString(), i, j));
                        map[i, j] = savedMap[i, j][0].ToString();
                    }

                    //cells that have a number 
                    else if (int.TryParse(savedMap[i, j], out _))
                        numbers.Add(new Cell(savedMap[i, j], i, j));

                    //cells that have a destination (letter) 
                    else if (savedMap[i, j].Any(x => char.IsLetter(x)))
                    {
                        destinations.Add(new Cell(savedMap[i, j], i, j));
                        map[i, j] = EMPTY;
                    }
                }
            }
            numbers = numbers.OrderBy(o => o.value).ToList();
            destinations = destinations.OrderBy(o => o.value).ToList();
            heuristic = calculateHeuristic();
        }
        public bool canMoveRight()
        {
            numbers = numbers.OrderBy(o => o.y).ToList();
            foreach (Cell num in numbers)
            {
                if (map[num.x, num.y + 1] == EMPTY || map[num.x, num.y + 1][0] == '+' || map[num.x, num.y + 1][0] == '-')
                    return true;
            }
            return false;
        }
        public Map moveRight()
        {
            numbers = numbers.OrderBy(o => o.y).ToList();
            foreach (Cell num in numbers)
            {
                if (map[num.x, num.y + 1] == EMPTY)
                {
                    map[num.x, num.y + 1] = num.value;
                    if (blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                    num.y++;
                }
                else if (map[num.x, num.y + 1][0] == '+' || map[num.x, num.y + 1][0] == '-')
                {
                    int a;
                    if (map[num.x, num.y + 1][0] == '+')
                        a = (int)num.value[0] + (map[num.x, num.y + 1][1] - 48);
                    else
                        a = (int)num.value[0] - (map[num.x, num.y + 1][1] - 48);
                    char c = (char)a;
                    num.value = c.ToString();

                    map[num.x, num.y + 1] = num.value;
                    if (blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                    num.y++;
                }
            }
            return this;
        }
        public bool canMoveLeft()
        {
            numbers = numbers.OrderByDescending(o => o.y).ToList();
            foreach (Cell num in numbers)
            {
                if (map[num.x, num.y - 1] == EMPTY || map[num.x, num.y - 1][0] == '+' || map[num.x, num.y - 1][0] == '-')
                    return true;
            }
            return false;
        }
        public Map moveLeft()
        {
            numbers = numbers.OrderByDescending(o => o.y).ToList();
            foreach (Cell num in numbers)
            {
                if (map[num.x, num.y - 1] == EMPTY)
                {
                    map[num.x, num.y - 1] = num.value;
                    if (blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                    num.y--;
                }
                else if (map[num.x, num.y - 1][0] == '+' || map[num.x, num.y - 1][0] == '-')
                {
                    int a;
                    if (map[num.x, num.y - 1][0] == '+')
                        a = (int)num.value[0] + (map[num.x, num.y - 1][1] - 48);
                    else
                        a = (int)num.value[0] - (map[num.x, num.y - 1][1] - 48);

                    char c = (char)a;
                    num.value = c.ToString();

                    map[num.x, num.y - 1] = num.value;
                    if (blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                    num.y--;
                }

            }
            return this;
        }
        public bool canMoveUp()
        {
            numbers = numbers.OrderByDescending(o => o.x).ToList();
            foreach (Cell num in numbers)
            {
                if (map[num.x - 1, num.y] == EMPTY || map[num.x - 1, num.y][0] == '+' || map[num.x - 1, num.y][0] == '-')
                    return true;
            }
            return false;
        }
        public Map moveUp()
        {
            numbers = numbers.OrderByDescending(o => o.x).ToList();
            foreach (Cell num in numbers)
            {
                if (map[num.x - 1, num.y] == EMPTY)
                {
                    map[num.x - 1, num.y] = num.value;
                    if (blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                    num.x--;
                }
                else if (map[num.x - 1, num.y][0] == '+' || map[num.x - 1, num.y][0] == '-')
                {
                    int a;
                    if (map[num.x - 1, num.y][0] == '+')
                        a = (int)num.value[0] + (map[num.x - 1, num.y][1] - 48);
                    else
                        a = (int)num.value[0] - (map[num.x - 1, num.y][1] - 48);
                    char c = (char)a;
                    num.value = c.ToString();

                    map[num.x - 1, num.y] = num.value;
                    if (blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                    num.x--;
                }
            }
            return this;
        }
        public bool canMoveDown()
        {
            numbers = numbers.OrderBy(o => o.x).ToList();
            foreach (Cell num in numbers)
            {
                if (map[num.x + 1, num.y] == EMPTY || map[num.x + 1, num.y][0] == '+' || map[num.x + 1, num.y][0] == '-')
                    return true;
            }
            return false;
        }
        public Map moveDown()
        {
            numbers = numbers.OrderBy(o => o.x).ToList();
            foreach (Cell num in numbers)
            {
                if (map[num.x + 1, num.y] == EMPTY)
                {
                    map[num.x + 1, num.y] = num.value;
                    if (blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                    num.x++;
                }
                else if (map[num.x + 1, num.y][0] == '+' || map[num.x + 1, num.y][0] == '-')
                {
                    int a;
                    if (map[num.x + 1, num.y][0] == '+')
                        a = (int)num.value[0] + (map[num.x + 1, num.y][1] - 48);
                    else
                        a = (int)num.value[0] - (map[num.x + 1, num.y][1] - 48);
                    char c = (char)a;
                    num.value = c.ToString();

                    map[num.x + 1, num.y] = num.value;
                    if (blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                    num.x++;
                }
            }
            return this;
        }

        public bool isFinal()
        {
            numbers = numbers.OrderBy(o => o.value).ToList();
            for (int i = 0; i < numbers.Count; i++)
            {
                /*
                    subtracts the ascii value of the number and destination
                    if the number and destination match then the value must be 48
                    e.g. 'a' in ascii is 97
                         '1' in ascii is 49
                         'a' - '1' = 97 - 49 = 48 
                */
                Cell? destination = destinations.Find(des => ((des.value[0] - numbers[i].value[0]) == 48)
                                                            && numbers[i].x == des.x && numbers[i].y == des.y);
                if (destination == null) return false;
            }
            Raylib.PlaySound(winSFX);
            return true;
        }
        public List<Map> possibleStates()
        {
            List<Map> children = new List<Map> { };
            if (canMoveUp())
            {
                Map m = deepCopy();
                m.moveThatResultedInThisMap = "U ";
                children.Add(m.moveUp());
            }
            if (canMoveDown())
            {
                Map m = deepCopy();
                m.moveThatResultedInThisMap = "D ";
                children.Add(m.moveDown());
            }
            if (canMoveRight())
            {
                Map m = deepCopy();
                m.moveThatResultedInThisMap = "R ";
                children.Add(m.moveRight());
            }
            if (canMoveLeft())
            {
                Map m = deepCopy();
                m.moveThatResultedInThisMap = "L ";
                children.Add(m.moveLeft());
            }
            return children;
        }
        public bool equals(Map map2)
        {
            for (int i = 0; i < this.map.GetLength(0); i++)
            {
                for (int j = 0; j < this.map.GetLength(1); j++)
                {
                    if (this.map[i, j] != map2.map[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void printMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == EMPTY)
                    {
                        Raylib.DrawRectangle(j * 100, i * 100, 100, 100, Colors.WHITE);
                    }
                    if (map[i, j][0] == '+' || map[i, j][0] == '-')
                    {
                        Raylib.DrawRectangle(j * 100, i * 100, 100, 100, Colors.PLUS_MINUS_BOX);
                        Raylib.DrawText(map[i, j], (j * 100) + 40, (i * 100) + 40, 32, Colors.PLUS_MINUS);
                    }
                }
                foreach (Cell num in numbers)
                {
                    Raylib.DrawRectangle(num.y * 100, num.x * 100, 100, 100, Colors.NUMBER_BOX);
                    Raylib.DrawText(num.value, (num.y * 100) + 40, (num.x * 100) + 40, 32, Colors.NUMBER);
                }
                foreach (Cell dis in destinations)
                {
                    int ascii = dis.value[0] - 48;
                    char c = (char)ascii;
                    Raylib.DrawText(c.ToString(), (dis.y * 100) + 60, (dis.x * 100) + 40, 32, Colors.DISTINATION);
                }
            }
        }
        public Map deepCopy()
        {
            string[,] m = new string[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    m[i, j] = map[i, j];
                }
            }
            return new Map(m);
        }
        public int calculateHeuristic()
        {
            numbers = numbers.OrderBy(o => o.value).ToList();
            int ans = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                int temp = destinations[i].value[0] - numbers[i].value[0];
                if (temp == 48)
                    ans += Math.Abs(numbers[i].x - destinations[i].x) + Math.Abs(numbers[i].y - destinations[i].y);
                else
                    ans += temp;
            }
            return ans;
        }
    }
}