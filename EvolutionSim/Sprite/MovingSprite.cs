﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
        public void Update(GameTime gameTime, Rectangle bounds)
        {
            Move(gameTime, bounds);
        }

        private void Move(GameTime gameTime, Rectangle bounds)
        {
            AttemptDirectionChange(gameTime);
            ConstrainMovementToGameArea(bounds);

            // Move the sprite
            _rectangle.X += _movement.X;
            _rectangle.Y += _movement.Y;
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
                _movement.X = _random.Next(-_movementSpeed, _movementSpeed + 1); // +1 because Random's upper bound is -1 for some reason
                _movement.Y = _random.Next(-_movementSpeed, _movementSpeed + 1);
            }
        }

        /// <summary>
        /// Adjust movement so the sprite won't walk out of bounds
        /// </summary>
        /// <param name="bounds">The screen bounds</param>
        private void ConstrainMovementToGameArea(Rectangle bounds)
        {
            var outOfBoundsLeftRight = _rectangle.X + _movement.X <= bounds.Left || _rectangle.X + _movement.X >= bounds.Right;
            var outOfBoundsTopBottom = _rectangle.Y + _movement.Y <= bounds.Top || _rectangle.Y + _movement.Y >= bounds.Bottom;
            if (outOfBoundsLeftRight)
            {
                _movement.X = -_movement.X;
            }
            if (outOfBoundsTopBottom)
            {
                _movement.Y = -_movement.Y;
            }
        }
    }
}
