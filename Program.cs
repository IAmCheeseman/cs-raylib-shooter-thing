using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;
using static Raylib_cs.Color;

namespace Game
{   
    class Bullet
    {
        private Vector2 position;
        private Vector2 direction;

        public Bullet(Vector2 pos, Vector2 dir) 
        {
            position = pos;
            direction = dir;
        } 

        public void Process()
        {
            position += direction * 6;
        }

        public void Draw()
        {
            DrawCircleV(position, 8f, YELLOW);
        }
    }

    class Player
    {
        private Vector2 position;
        public List<Bullet> bullets;

        public Player(Vector2 pos)
        {
            position = pos;
        }

        public void Process()
        {
            // WASD movement
            Vector2 vel = new Vector2(0, 0);

            if (IsKeyDown(KEY_W)) vel.Y -= 1;
            if (IsKeyDown(KEY_A)) vel.X -= 1;
            if (IsKeyDown(KEY_S)) vel.Y += 1;
            if (IsKeyDown(KEY_D)) vel.X += 1;

            // Normalizing
            float velLength = (float)Math.Sqrt(vel.X * vel.X + vel.Y * vel.Y);
            if (vel != new Vector2(0,0) && velLength != 0) vel /= velLength;

            position += vel * 5;

            // Shooting
            if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON)) {
                Vector2 direction = new Vector2(0, 0);
                Vector2 mousePos = GetMousePosition();

                direction = mousePos - position;
                float dirLength = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                if (direction != new Vector2(0,0) && dirLength != 0) direction /= dirLength;

                Bullet newBullet = new Bullet(
                    new Vector2(position.X, position.Y) + (direction * 32f),
                    direction
                );

                bullets.Add(newBullet);
            }
        }

        public void Draw()
        {
            DrawCircleV(position, 32f, WHITE);
        }
    }

    static class Program
    {
        public static void Main()
        {
            InitWindow(800, 480, "Hello World");
            SetTargetFPS(60);

            Player player = new Player(new Vector2(50, 50));
            List<Bullet> bullets = new List<Bullet>();

            player.bullets = bullets;

            while (!WindowShouldClose())
            {
                // PROCESSING
                player.Process();

                for (int i=0;i<bullets.Count;i++) bullets[i].Process();

                // DRAWING
                BeginDrawing();
                ClearBackground(GREEN);

                DrawText("Hello, world!", 12, 12, 20, BLACK);
                
                player.Draw();

                for (int i=0;i<bullets.Count;i++) bullets[i].Draw();

                EndDrawing();
            }

            CloseWindow();
        }
    }
}