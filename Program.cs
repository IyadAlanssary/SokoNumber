using Raylib_cs;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace HelloWorld
{
    class Program
    {
        public static Map map;
        public static int mapWidth, mapHeight, screenWidth = 900, screenHeight = 900;
        static Stopwatch stopwatch = new Stopwatch();
        private enum stateEnum { Intro, ChoosingMethod, userPlaying, Results };
        private static stateEnum state = stateEnum.Intro;
        public static bool loading = false;
        public static int levelNumber = -1;
        private static int frameCounter = 0;

        static void Main(string[] args)
        {
            Raylib.InitAudioDevice();
            Raylib.SetMasterVolume(0.1f);
            hideConsole();

            Raylib.InitWindow(screenWidth, screenHeight, "SokoNumber");
            Raylib.SetTargetFPS(24);

            Task algorithmTask = null;
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Colors.BACKGROUND);
                switch (state)
                {
                    case stateEnum.Intro:
                        intro();
                        break;
                    case stateEnum.ChoosingMethod:
                        mapWidth = map.map.GetLength(1) * 100;
                        mapHeight = map.map.GetLength(0) * 100;
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO) || Raylib.IsKeyPressed(KeyboardKey.KEY_THREE)
                             || Raylib.IsKeyPressed(KeyboardKey.KEY_FOUR) || Raylib.IsKeyPressed(KeyboardKey.KEY_FIVE))
                        {
                            loading = true;
                        }
                        if (loading)
                        {
                            loadingText();
                            Raylib.DrawText("Visited States: " + Algorithms.visitedStates.ToString(), mapWidth / 2 + 60, mapHeight - 25, 20, Colors.WHITE);
                        }
                        algorithmTask = new Task(new Action(getMethodInput));
                        algorithmTask.Start();

                        choosingMethodPrompts();
                        if (map.isFinal())
                        {
                            loading = false;
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
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_M))
                {
                    restartMainMenu();
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                {
                    restartLevel();
                }
                Raylib.EndDrawing();
            }
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }
        static void intro()
        {
            Raylib.DrawText("SokoNumber", screenWidth / 3 - 30, screenHeight / 3, 60, Colors.WHITE);
            Raylib.DrawText("Guide the numbers to their unique position on the grid", screenWidth / 7, screenHeight - 80, 25, Colors.WHITE);

            guiButtons();
            assignMap();
            if (map != null)
            {
                map.parent = null;
                state = stateEnum.ChoosingMethod;
            }
        }
        static void guiButtons()
        {
            for (int i = 0; i < 10; i++)
            {
                Raylib.DrawRectangle(150 + (i * 60), 440, 50, 50, Colors.BUTTON);
                Raylib.DrawText((i + 1).ToString(), 165 + (i * 60), 450, 30, Colors.BACKGROUND);

                Raylib.DrawRectangle(150 + (i * 60), 520, 50, 50, Colors.BUTTON);
                Raylib.DrawText((i + 11).ToString(), 160 + (i * 60), 530, 30, Colors.BACKGROUND);
            }
            for (int i = 0; i < 5; i++)
            {
                Raylib.DrawRectangle(150 + (i * 60), 600, 50, 50, Colors.BUTTON);
                Raylib.DrawText((i + 21).ToString(), 160 + (i * 60), 610, 30, Colors.BACKGROUND);
            }
        }
        static void assignMap()
        {
            if (Raylib.IsMouseButtonPressed(0))
            {
                System.Numerics.Vector2 mousePos = Raylib.GetMousePosition();
                for (int i = 0; i < 10; i++)
                {
                    if (mousePos.X > (150 + i * 60) && mousePos.X < (200 + i * 60))
                    {
                        if (mousePos.Y > 440 && mousePos.Y < 480)
                        {
                            levelNumber = i + 1;
                        }
                        else if (mousePos.Y > 520 && mousePos.Y < 560)
                        {
                            levelNumber = i + 11;
                        }
                        else if (mousePos.Y > 600 && mousePos.Y < 640)
                        {
                            levelNumber = i + 21;
                        }
                        if (levelNumber != -1)
                            map = new Map(SavedMaps.chooseMap(levelNumber));
                    }
                }
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
            Raylib.DrawText("Choose playing method", screenWidth / 3, 20, 30, Colors.WHITE);
            Raylib.DrawText("1: User Play", 30, 60, 21, Colors.WHITE);
            Raylib.DrawText("2: BFS", 200, 60, 21, Colors.WHITE);
            Raylib.DrawText("3: DFS", 350, 60, 21, Colors.WHITE);
            Raylib.DrawText("4: Uniform Cost", 500, 60, 21, Colors.WHITE);
            Raylib.DrawText("5: A*", 720, 60, 21, Colors.WHITE);
            Raylib.DrawText("R: Restart", 30, screenHeight - 65, 21, Colors.WHITE);
            Raylib.DrawText("M: Main Menu", 30, screenHeight - 90, 21, Colors.WHITE);
            Raylib.DrawText(levelNumber.ToString(), screenWidth - 130, screenHeight - 80, 28, Colors.WHITE);
        }
        static void userPlayPrompts()
        {
            Raylib.DrawText("Move: ARROWS / WASD", 25, 20, 21, Colors.WHITE);
            Raylib.DrawText("R: Restart", 30, screenHeight - 50, 21, Colors.WHITE);
            Raylib.DrawText("M: Main Menu", 30, screenHeight - 80, 21, Colors.WHITE);
            Raylib.DrawText(levelNumber.ToString(), screenWidth - 130, screenHeight - 80, 28, Colors.WHITE);
        }
        static void restartMainMenu()
        {
            Algorithms.visitedStates = 0;
            Algorithms.passedMaps.Clear();
            Algorithms.passedMapsHash.Clear();
            Map.destinations.Clear();
            loading = false;
            Map.blockGeneratingLevel = false;
            stopwatch = new Stopwatch();
            map = null;
            levelNumber = -1;
            state = stateEnum.Intro;
        }
        static void restartLevel()
        {
            Algorithms.visitedStates = 0;
            Algorithms.passedMaps.Clear();
            Algorithms.passedMapsHash.Clear();
            Map.destinations.Clear();
            stopwatch = new Stopwatch();
            map = null;
            loading = false;

            state = stateEnum.ChoosingMethod;
            map = new Map(SavedMaps.chooseMap(levelNumber));
        }
        static void results()
        {
            Raylib.DrawText("You Won!", mapWidth / 2 - 60, 45, 40, Colors.YOU_WON);
            Raylib.DrawText("M: Main Menu", 30, screenHeight - 80, 21, Colors.WHITE);
            Raylib.DrawText("R: Restart", 30, screenHeight - 50, 21, Colors.WHITE);

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
                Raylib.DrawText(stopwatch.Elapsed.TotalSeconds.ToString() + " seconds", 100, mapHeight - 80, 25, Colors.WHITE);
                Raylib.DrawText("Moves: " + new String(allMovesCharArray), 100, mapHeight - 50, 20, Colors.WHITE);
                Raylib.DrawText("Solution Depth: " + map.numberOfMoves.ToString(), 100, mapHeight - 25, 20, Colors.WHITE);
                Raylib.DrawText("Visited States: " + Algorithms.visitedStates.ToString(), mapWidth / 2 + 60, mapHeight - 25, 20, Colors.WHITE);
            }
        }
        static void loadingText()
        {
            if (frameCounter < 24)
            {
                Raylib.DrawText("Loading", screenWidth / 2 - 50, screenHeight - 80, 20, Colors.WHITE);
                frameCounter++;
            }
            else if (frameCounter < 48)
            {
                Raylib.DrawText("Loading.", screenWidth / 2 - 50, screenHeight - 80, 20, Colors.WHITE);
                frameCounter++;
            }
            else if (frameCounter < 72)
            {
                Raylib.DrawText("Loading..", screenWidth / 2 - 50, screenHeight - 80, 20, Colors.WHITE);
                frameCounter++;
            }
            else
            {
                Raylib.DrawText("Loading", screenWidth / 2 - 50, screenHeight - 80, 20, Colors.WHITE);
                frameCounter = 0;
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