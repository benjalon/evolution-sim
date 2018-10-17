﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EvolutionSim
{
    public abstract class MovingSprite : Sprite
    {
        private Random _random = new Random();
        private Point _movement;
        private int _movementSpeed;
        private float _detectionRadius;

        private const int MS_PER_DIRECTION_CHANGE = 400;
        private int _msSinceDirectionChange = MS_PER_DIRECTION_CHANGE;

        /// <summary>
        /// Create a moving sprite from a given texture and rectangle
        /// </summary>
        /// <param name="texture">The appearance of the Sprite</param>
        /// <param name="rectangle">The position and size of the Sprite</param>
        /// <param name="movementSpeed">The speed in pixels at which the sprite moves</param>
        /// <param name="detectionRadius">A circular distance around the sprite where the sprite will react to other sprite, if not given then
        /// the sprite will use it's rectangle instead.</param>
        public MovingSprite(ref Texture2D texture, Rectangle rectangle, int movementSpeed = 2, float detectionRadius = -1.0f)
            : base(ref texture, rectangle)
        {
            _movementSpeed = movementSpeed;
            _detectionRadius = detectionRadius;
        }
        
        /// <summary>
        /// Update the Sprite by moving its rectangle
        /// </summary>
        /// <param name="gameTime">Delta</param>
        /// <param name="bounds">The game area boundary</param>
        public void Update(GameTime gameTime, Rectangle bounds, List<Sprite> colliders)
        {
            Move(gameTime, bounds, colliders);
        }

        private void Move(GameTime gameTime, Rectangle bounds, List<Sprite> colliders)
        {
            AttemptDirectionChange(gameTime);

            var plannedPosition = new Point(Rectangle.X + _movement.X, Rectangle.Y + _movement.Y);
            if (CheckOutOfBounds(bounds, plannedPosition) || CheckCollisions(colliders, plannedPosition))
            {
                return; // Give up with this movement and wait for either the collider to move or the next direction change
            }

            // Move the sprite
            _rectangle.X += plannedPosition.X * gameTime.ElapsedGameTime.Milliseconds;
            _rectangle.Y += plannedPosition.Y * gameTime.ElapsedGameTime.Milliseconds;
        }

        /// <summary>
        /// Change the sprite's direction every so often determined by how much time has passed since the last update, if not enough time
        /// has passed then it just updates the timer for the next check.
        /// </summary>
        /// <param name="gameTime">Delta time</param>
        private void AttemptDirectionChange(GameTime gameTime)
        {
            _msSinceDirectionChange += gameTime.ElapsedGameTime.Milliseconds;

            var shouldChangeDirection = _msSinceDirectionChange > MS_PER_DIRECTION_CHANGE;
            if (shouldChangeDirection)
            {
                _msSinceDirectionChange = 0;
                _movement.X = _random.Next(-_movementSpeed, _movementSpeed + 1); // +1 because Random's upper bound is exclusive
                _movement.Y = _random.Next(-_movementSpeed, _movementSpeed + 1);
            }
        }

        /// <summary>
        /// Check whether the planned movement will cause any collisions
        /// </summary>
        /// <param name="colliders">Elements this move could possibly collide with</param>
        /// <param name="plannedPosition">The planned new position</param>
        /// <returns>True for will collide, false for will not collide</returns>
        private bool CheckCollisions(List<Sprite> colliders, Point plannedPosition)
        {
            var collisionRectangle = Rectangle.Empty;
            foreach (var collider in colliders)
            {
                if (collider.Rectangle.Contains(plannedPosition))
                {
                    collisionRectangle = collider.Rectangle; // Collision found
                    break;
                }
            }

            return !collisionRectangle.IsEmpty;
        }

        /// <summary>
        /// Check whether planned movement will cause the sprite to walk out of bounds
        /// </summary>
        /// <param name="bounds">The screen bounds</param>
        /// <param name="plannedPosition">The planned movement</param>
        /// <returns>True for will be out of bounds, false for will not be out of bounds</returns>
        private bool CheckOutOfBounds(Rectangle bounds, Point plannedPosition)
        {
            var outOfBoundsLeftRight = plannedPosition.X <= bounds.Left || plannedPosition.X >= bounds.Right;
            var outOfBoundsTopBottom = plannedPosition.Y <= bounds.Top || plannedPosition.Y >= bounds.Bottom;
            return outOfBoundsLeftRight || outOfBoundsTopBottom;
        }
    }
}
