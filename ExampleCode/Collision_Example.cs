using SFML.Graphics;
using SFML.System;
using SFML.Utils;
/*
 * Dieses Projekt ist im Rahmen des Bachelorstudiengangs MultiMediaTechnology an der FH Salzburg entstanden.
 * Diese Plattform ist mein erstes eigenständiges Projekt im Bereich Game Developement.
 * MultiMediaProjekt 1 - Sommersemester 2022
 * Piet Koller
 */
namespace KingInTheCastle
{
    internal class Collider
    {
        Vector2f Position;
        RectangleShape Shape;
        Vector2f Size;
        FloatRect intersectionRect;

        public Collider(Vector2f position, Vector2f size)
        {
            Position = position;
            Size = size;
            Shape = new RectangleShape(Size);
            Shape.Position = Position;
            Shape.OutlineColor = Color.Black;
            Shape.OutlineThickness = 1;
            Shape.FillColor = Color.Transparent;
        }

        public void DrawCollider(RenderWindow window)
        {
            window.Draw(Shape);
        }
        private void CollisionResolve(Player player)
        {
            //Convinience Variables to shorten the Code
            float playerBottom = player.playerSprite.GetGlobalBounds().Top + player.playerSprite.GetGlobalBounds().Height -player.playerInput.Y;
            float rightBoundry = player.playerSprite.Position.X - intersectionRect.Left + intersectionRect.Width;
            float intersectionBottom = intersectionRect.Top + intersectionRect.Height;
            float playerTop = player.playerSprite.GetGlobalBounds().Top - player.playerInput.Y;

            //Bottom Collision
            if (playerBottom <= intersectionRect.Top)
            {
                player.isGrounded = true;
                player.playerSprite.Position += new Vector2f(0f, -player.playerInput.Y);
            }

           // Top Collision
            if (playerTop >= intersectionBottom)
            {
                player.playerSprite.Position += new Vector2f(0f,intersectionRect.Height);
                player.playerInput.Y = 0f;
                return;
            }

            //Right Collision
            if (rightBoundry > 0 && playerBottom > intersectionRect.Top)
            {
               player.playerSprite.Position += new Vector2f(intersectionRect.Width, 0f);  
            }

            //Left Collision
            if (rightBoundry < 0 && playerBottom > intersectionRect.Top)
            {
                player.playerSprite.Position += new Vector2f(-intersectionRect.Width, 0f);
            }     
        }

        public void OnPlayerCollision(Player player)
        {
            if (player.playerSprite.GetGlobalBounds().Intersects(Shape.GetGlobalBounds(), out intersectionRect))
            {
                CollisionResolve(player);
            }
           
        }

        public FloatRect GetFloatRect()
        {
            return Shape.GetGlobalBounds();
        }

    }
}
