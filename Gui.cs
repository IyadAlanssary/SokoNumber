using Raylib_CsLo;

namespace HelloWorld
{
    class Gui
    {

        public static void guii()
        {

            for (int i = 0; i < 10; i++)
            {
                Raylib.DrawRectangle(85 + (i * 55), 400, 40, 40, Raylib.ColorFromHSV(1, 1, 1));
                Raylib.DrawRectangle(90 + (i * 55), 405, 30, 30, Raylib.BROWN);
                Raylib.DrawText((i + 1).ToString(), 95 + (i * 55), 405, 30, Raylib.WHITE);

                Raylib.DrawRectangle(85 + (i * 55), 480, 40, 40, Raylib.ColorFromHSV(1, 1, 1));
                Raylib.DrawText((i + 11).ToString(), 90 + (i * 55), 485, 30, Raylib.WHITE);

            }
            for (int i = 0; i < 5; i++)
            {
                Raylib.DrawRectangle(85 + (i * 55), 560, 40, 40, Raylib.ColorFromHSV(1, 1, 1));
                Raylib.DrawText((i + 21).ToString(), 90 + (i * 55), 565, 30, Raylib.WHITE);

            }
        }
    }
}