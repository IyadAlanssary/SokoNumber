using Raylib_cs;

namespace HelloWorld
{
    class Map
    {
        public string[,] map;
        public List<Object> numbers = new List<Object> { };
        public static List<Object> letters = new List<Object> { };
        public Map? parent;
        public string? moveThatResultedInThisMap;
        public int numberOfMoves;
        public int heuristic;
        public int heuristicPlusCost;
        private string EMPTY = "_";
        Sound win = Raylib.LoadSound("new-win-gold-special.wav");

        public Map(string[,] savedMap)
        {
            this.map = new String[savedMap.GetLength(0), savedMap.GetLength(1)];
            for (int i = 0; i < savedMap.GetLength(0); i++)
            {
                for (int j = 0; j < savedMap.GetLength(1); j++)
                {
                    this.map[i, j] = savedMap[i, j];
                    if (savedMap[i, j][0] == '+')
                    {
                        continue;
                    }
                    //for input like 2a
                    else if (savedMap[i, j].Length == 2)
                    {
                        numbers.Add(new Object(savedMap[i, j][0].ToString(), i, j));
                        int a = savedMap[i, j][1] - 48;
                        char c = (char)a;
                        letters.Add(new Object(c.ToString(), i, j));
                        map[i, j] = savedMap[i, j][0].ToString();
                    }

                    //for input like 2
                    else if (int.TryParse(savedMap[i, j], out _))
                        numbers.Add(new Object(savedMap[i, j], i, j));

                    //for input like a
                    else if (savedMap[i, j].Any(x => char.IsLetter(x)))
                    {
                        int a = savedMap[i, j][0] - 48;
                        char c = (char)a;
                        letters.Add(new Object(c.ToString(), i, j));

                        //letters.Add(new Object(savedMap[i, j], i, j));
                        map[i, j] = EMPTY;
                    }
                }
            }
            numbers = numbers.OrderBy(o => o.value).ToList();
            letters = letters.OrderBy(o => o.value).ToList();
            heuristic = calculateHeuristic();
        }
        public bool canMoveRight()
        {
            numbers = numbers.OrderBy(o => o.y).ToList();
            foreach (Object num in numbers)
            {
                if (map[num.x, num.y + 1] == EMPTY)
                    return true;
            }
            return false;
        }
        public Map moveRight()
        {
            numbers = numbers.OrderBy(o => o.y).ToList();
            foreach (Object num in numbers)
            {
                if (map[num.x, num.y + 1] == EMPTY)
                {
                    map[num.x, num.y + 1] = num.value;
                    num.y++;
                    if (Program.blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                }
                else if (map[num.x, num.y + 1][0] == '+')
                {
                    int a = (int)num.value[0] + (map[num.x, num.y + 1][1] - 48);
                    char c = (char)a;
                    num.value = c.ToString();

                    map[num.x, num.y + 1] = num.value;
                    num.y++;
                    if (Program.blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                }
            }
            return this;
        }
        public bool canMoveLeft()
        {
            numbers = numbers.OrderByDescending(o => o.y).ToList();
            foreach (Object num in numbers)
            {
                if (map[num.x, num.y - 1] == EMPTY)
                    return true;
            }
            return false;
        }
        public Map moveLeft()
        {
            numbers = numbers.OrderByDescending(o => o.y).ToList();
            foreach (Object num in numbers)
            {
                if (map[num.x, num.y - 1] == EMPTY)
                {
                    map[num.x, num.y - 1] = num.value;
                    num.y--;
                    if (Program.blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                }
                else if (map[num.x, num.y - 1][0] == '+')
                {
                    int a = (int)num.value[0] + (map[num.x, num.y - 1][1] - 48);
                    char c = (char)a;
                    num.value = c.ToString();

                    map[num.x, num.y - 1] = num.value;
                    num.y--;
                    if (Program.blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                }

            }
            return this;
        }
        public bool canMoveUp()
        {
            numbers = numbers.OrderByDescending(o => o.x).ToList();
            foreach (Object num in numbers)
            {
                if (map[num.x - 1, num.y] == EMPTY)
                    return true;
            }
            return false;
        }
        public Map moveUp()
        {
            numbers = numbers.OrderByDescending(o => o.x).ToList();
            foreach (Object num in numbers)
            {
                if (map[num.x - 1, num.y] == EMPTY)
                {
                    map[num.x - 1, num.y] = num.value;
                    num.x--;
                    if (Program.blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                }
                else if (map[num.x - 1, num.y][0] == '+')
                {
                    int a = (int)num.value[0] + (map[num.x - 1, num.y][1] - 48);
                    char c = (char)a;
                    num.value = c.ToString();

                    map[num.x - 1, num.y] = num.value;
                    num.x--;
                    if (Program.blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                }
            }
            return this;
        }
        public bool canMoveDown()
        {
            numbers = numbers.OrderBy(o => o.x).ToList();
            foreach (Object num in numbers)
            {
                if (map[num.x + 1, num.y] == EMPTY)
                    return true;
            }
            return false;
        }
        public Map moveDown()
        {
            numbers = numbers.OrderBy(o => o.x).ToList();
            foreach (Object num in numbers)
            {
                if (map[num.x + 1, num.y] == EMPTY)
                {
                    map[num.x + 1, num.y] = num.value;
                    num.x++;
                    // if (Program.blockGeneratingLevel)
                    //     map[num.x, num.y] = "#";
                    // else
                    map[num.x, num.y] = EMPTY;
                }
                else if (map[num.x + 1, num.y][0] == '+')
                {
                    int a = (int)num.value[0] + (map[num.x + 1, num.y][1] - 48);
                    char c = (char)a;
                    num.value = c.ToString();

                    map[num.x + 1, num.y] = num.value;
                    num.x++;
                    if (Program.blockGeneratingLevel)
                        map[num.x, num.y] = "#";
                    else
                        map[num.x, num.y] = EMPTY;
                }
            }
            return this;
        }

        public bool isFinal()
        {
            numbers = numbers.OrderBy(o => o.value).ToList();
            for (int i = 0; i < numbers.Count; i++)
            {
                Console.WriteLine(i + " : " + numbers[i].value + "  " + letters[i].value);
                for (int j = 0; j < numbers.Count; j++)
                {
                    if (numbers[i].value != letters[j].value || numbers[i].x != letters[j].x || numbers[i].y != letters[j].y)
                        return false;
                }
            }
            Raylib.PlaySound(win);
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
                        Raylib.DrawRectangle(j * 100, i * 100, 100, 100, Color.WHITE);
                    if (map[i, j][0] == '+')
                    {
                        Raylib.DrawRectangle(j * 100, i * 100, 100, 100, Color.WHITE);
                        Raylib.DrawText(map[i, j], (j * 100) + 60, (i * 100) + 40, 26, Color.PURPLE);
                    }
                }
            }
            foreach (Object num in numbers)
            {
                Raylib.DrawRectangle(num.y * 100, num.x * 100, 100, 100, Color.GREEN);
                Raylib.DrawText(num.value, (num.y * 100) + 40, (num.x * 100) + 40, 26, Color.BLUE);
            }
            foreach (Object let in letters)
            {
                Raylib.DrawText(let.value, (let.y * 100) + 60, (let.x * 100) + 40, 26, Color.RED);
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
                ans += Math.Abs(numbers[i].x - letters[i].x) + Math.Abs(numbers[i].y - letters[i].y);
            }
            return ans;
        }
    }
}