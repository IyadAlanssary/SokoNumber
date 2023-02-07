using Raylib_cs;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HelloWorld
{
    class Program
    {
        public static Map map;
        private static int width, height;
        static Stopwatch stopwatch = new Stopwatch();
        private enum stateEnum { Intro, ChoosingMethod, userPlaying, Results };
        private static stateEnum state = stateEnum.Intro;
        public static bool blockGeneratingLevel = false;
        static void Main(string[] args)
        {
            blockGeneratingLevel = false;
            Raylib.InitAudioDevice();
            Raylib.SetMasterVolume(0.1f);
            hideConsole();
            // string startupPath = System.IO.Directory.GetCurrentDirectory() + "\\level_1";
            // Console.WriteLine(startupPath);

            // string[] lines = System.IO.File.ReadAllLines(startupPath);

            // foreach (string line in lines)
            // {
            //     Console.WriteLine("\t" + line);
            // }
            Raylib.InitWindow(700, 700, "SokoNumber");
            Raylib.SetTargetFPS(24);

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DARKBROWN);
                switch (state)
                {
                    case stateEnum.Intro:
                        intro();
                        break;
                    case stateEnum.ChoosingMethod:
                        width = map.map.GetLength(1) * 100;
                        height = map.map.GetLength(0) * 100;
                        choosingMethodPrompts();
                        getMethodInput();
                        if (map.isFinal())
                        {
                            state = stateEnum.Results;
                        }
                        map.printMap();
                        break;
                    case stateEnum.userPlaying:
                        if (map.isFinal())
                        {
                            state = stateEnum.Results;
                        }
                        userPlayPrompts();
                        userPlayInput();
                        map.printMap();
                        break;
                    case stateEnum.Results:
                        results();
                        map.printMap();
                        break;
                    default:
                        break;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                {
                    restart();
                }
                Raylib.EndDrawing();
            }
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }
        static void intro()
        {
            Raylib.DrawText("SokoNumber", 700 / 3, 700 / 3, 45, Color.WHITE);
            for (int i = 0; i < 10; i++)
            {
                Raylib.DrawRectangle(30 + (i* 30), 350, 30, 30, Color.LIGHTGRAY);
                Raylib.DrawText(i.ToString(), 30 + (i * 20), 350, 20, Color.WHITE);
            }
            string chosenMap = ((char)Raylib.GetKeyPressed()).ToString();
            assignMap(chosenMap);
            if (map != null)
            {
                map.parent = null;
                state = stateEnum.ChoosingMethod;
            }
        }
        static void assignMap(String chosenMap)
        {
            int i = 0;
            System.Numerics.Vector2 mouse = Raylib.GetMousePosition();
            if (mouse.X > i * 20 && mouse.Y > i * 20)
            {

            }
            switch (chosenMap)
            {
                case "1":
                    map = new Map(SavedMaps.Level_1);
                    break;
                case "2":
                    map = new Map(SavedMaps.Level_2);
                    break;
                case "3":
                    map = new Map(SavedMaps.Level_3);
                    break;
                case "4":
                    map = new Map(SavedMaps.Level_4);
                    break;
                case "5":
                    map = new Map(SavedMaps.Level_5);
                    break;
                case "6":
                    map = new Map(SavedMaps.Level_6);
                    break;
                case "7":
                    map = new Map(SavedMaps.Level_7);
                    break;
                case "8":
                    blockGeneratingLevel = true;
                    map = new Map(SavedMaps.Level_8);
                    break;
                case "9":
                    map = new Map(SavedMaps.Level_9);
                    break;
                case "10":
                    map = new Map(SavedMaps.Level_10);
                    break;
                case "11":
                    map = new Map(SavedMaps.Level_11);
                    break;
                case "12":
                    map = new Map(SavedMaps.Level_12);
                    break;
                case "13":
                    map = new Map(SavedMaps.Level_13);
                    break;
                case "14":
                    map = new Map(SavedMaps.Level_14);
                    break;
                default:
                    break;
            }
        }
        static void getMethodInput()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ONE))
            {
                state = stateEnum.userPlaying;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO))
            {
                stopwatch.Start();
                Algorithms.BFS();
                stopwatch.Stop();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_THREE))
            {
                stopwatch.Start();
                Algorithms.DFS();
                stopwatch.Stop();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_FOUR))
            {
                stopwatch.Start();
                Algorithms.UCS();
                stopwatch.Stop();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_FIVE))
            {
                stopwatch.Start();
                Algorithms.AStar();
                stopwatch.Stop();
            }
        }
        static void userPlayInput()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) || Raylib.IsKeyPressed(KeyboardKey.KEY_S))
            {
                map.moveDown();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP) || Raylib.IsKeyPressed(KeyboardKey.KEY_W))
            {
                map.moveUp();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT) || Raylib.IsKeyPressed(KeyboardKey.KEY_A))
            {
                map.moveLeft();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyPressed(KeyboardKey.KEY_D))
            {
                map.moveRight();
            }
        }
        static void choosingMethodPrompts()
        {
            Raylib.DrawText("Choose playing method", 220, 10, 28, Color.WHITE);
            Raylib.DrawText("1: User Play", 20, 50, 21, Color.WHITE);
            Raylib.DrawText("2: BFS", 200, 50, 21, Color.WHITE);
            Raylib.DrawText("3: DFS", 200, 75, 21, Color.WHITE);
            Raylib.DrawText("4: Uniform Cost", 350, 50, 21, Color.WHITE);
            Raylib.DrawText("5: A*", 350, 75, 21, Color.WHITE);
            Raylib.DrawText("R: Restart", 580, 50, 21, Color.WHITE);
        }
        static void userPlayPrompts()
        {
            Raylib.DrawText("Move: ARROWS / WASD", 25, 20, 21, Color.WHITE);
            Raylib.DrawText("Restart: R", 25, 50, 21, Color.WHITE);
        }
        static void restart()
        {
            map = null;
            Algorithms.visitedStates = 0;
            Algorithms.passedMaps.Clear();
            Algorithms.passedMapsHash.Clear();
            Map.letters.Clear();
            blockGeneratingLevel = false;
            Console.Clear();
            state = stateEnum.Intro;
        }
        static void results()
        {
            Raylib.DrawText("You Won!", width / 3, height - 75, 40, Color.GREEN);
            Raylib.DrawText("R: Restart", 25, 50, 21, Color.WHITE);

            List<string> moves = new List<string>() { };
            string movesString = "";
            Map t = map;
            while (t.parent != null)
            {
                movesString += t.moveThatResultedInThisMap;
                t = t.parent;
            }
            char[] allMovesCharArray = movesString.ToCharArray();
            Array.Reverse(allMovesCharArray);
            if (allMovesCharArray.Length > 1)
            {
                Raylib.DrawText(stopwatch.Elapsed.TotalSeconds.ToString() + " seconds",
                    20, height - 30, 25, Color.WHITE);
                Raylib.DrawText("Moves: " + new string(allMovesCharArray), 20, height, 20, Color.WHITE);
                Raylib.DrawText("Solution Depth: " + map.numberOfMoves.ToString(), 20, height + 30, 20, Color.WHITE);
                Raylib.DrawText("Visited States: " + Algorithms.visitedStates.ToString(), width / 2, height + 30, 20, Color.WHITE);
            }
        }
        static void hideConsole()
        {
            [DllImport("kernel32.dll")]
            static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            const int SW_SHOWMINIMIZED = 2;

            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_SHOWMINIMIZED);
        }
    }
}