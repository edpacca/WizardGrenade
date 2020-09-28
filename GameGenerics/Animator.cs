using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WizardGrenade
{
    class Animator
    {
        private int _currentFrameIndex = 0;
        private Dictionary<string, int[]> _animationStates;
        private int _frameWidth;
        float elapsedFrameTime = 0;
        bool sequenceReset = true;

        public Animator(Dictionary<string, int[]> animationStates, int frameWidth)
        {
            _animationStates = animationStates;
            _frameWidth = frameWidth;
        }

        public int GetAnimationFrames(string state, float targetFrameRate, GameTime gameTime)
        {
            return GetFramePosition(GetCurrentFrame(state, targetFrameRate, gameTime));
        }

        public int GetSingleFrame(string state)
        {
            return GetFramePosition(_animationStates[state][0]);
        }

        public int GetFramePosition(int currentFrame)
        {
            return currentFrame * _frameWidth;
        }

        public int GetCurrentFrame(string state, float targetFrameRate, GameTime gameTime)
        {
            int numberOfFrames = _animationStates[state].Length;
            elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_currentFrameIndex >= numberOfFrames)
                _currentFrameIndex = 0;

            if (elapsedFrameTime < (1 / targetFrameRate))
                return _animationStates[state][_currentFrameIndex];
            else
            {
                _currentFrameIndex = (_currentFrameIndex + 1) % numberOfFrames;
                elapsedFrameTime = 0;
                return _animationStates[state][_currentFrameIndex];
            }
        }

        public int GetCurrentFrameSingleSequence(string state, float targetFrameRate, GameTime gameTime)
        {
            int numberOfFrames = _animationStates[state].Length;
            elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_currentFrameIndex >= numberOfFrames)
                _currentFrameIndex = 0;

            if (elapsedFrameTime < (1 / targetFrameRate))
                return _animationStates[state][_currentFrameIndex];
            else
            {
                _currentFrameIndex++;
                elapsedFrameTime = 0;

                if (_currentFrameIndex < numberOfFrames)
                    return _animationStates[state][_currentFrameIndex];
                else
                {
                    _currentFrameIndex = 0;
                    return 255;
                }
            }
        }

        public void ResetSequence()
        {
            sequenceReset = true;
        }

        public int GetAnimationFrameSequence(string state1, string state2, float targetFrameRate1, float targetFrameRate2, GameTime gameTime)
        {
            int currentSequenceFrame;

            if (sequenceReset)
            {
                currentSequenceFrame = GetCurrentFrameSingleSequence(state1, targetFrameRate1, gameTime);

                if (currentSequenceFrame == 255)
                {
                    sequenceReset = false;
                    currentSequenceFrame = GetCurrentFrame(state2, targetFrameRate2, gameTime);
                }
            }
            else
                currentSequenceFrame = GetCurrentFrame(state2, targetFrameRate2, gameTime);

            return GetFramePosition(currentSequenceFrame);
        }
    }
}
