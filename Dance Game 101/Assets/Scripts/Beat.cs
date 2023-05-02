using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hilam
{
    public class Beat
    {
        public Beat(Vector2 _direction, BeatTypes _beatType)
        {
            Direction = _direction;
            BeatType = _beatType;

            SetHitTime();

        }

        public Vector2 Direction;
        public BeatTypes BeatType;

        public float[] PerfectHitTime = new float[2];
        public float HitTime;
        private float perfectHitTimeHalf;

        public BeatStatus BeatStatus = BeatStatus.Playing;

        void SetHitTime()
        {
            HitTime = BeatType switch
            {
                BeatTypes.Eighth => 0.5f,
                BeatTypes.Quarter => 1,
                BeatTypes.Half => 2,
                BeatTypes.Whole => 4,
            };

            SetPerfectHitTime();
        }

        void SetPerfectHitTime()
        {
            perfectHitTimeHalf = HitTime / 2;
            PerfectHitTime[0] = perfectHitTimeHalf - 0.1f;
            PerfectHitTime[1] = perfectHitTimeHalf + 0.1f;
        }

        public void MissBeat()
        {
            BeatStatus = BeatStatus.Missed;
        }

        public void PlayBeat(Vector2 _direction, float _time)
        {
            if (_direction != Direction)
            {
                MissBeat();
                return;
            }

            CheckBeatStatus(_time);
        }

        void CheckBeatStatus(float _time)
        {
            if (_time > HitTime)
            {
                MissBeat();
                return;
            }

            if (_time < (PerfectHitTime[0] - 0.1f))
            {
                MissBeat();
                return;
            }

            if (_time > (PerfectHitTime[1] + 0.1f))
            {
                MissBeat();
                return;
            }

            if (_time < PerfectHitTime[0])
            {
                BeatStatus = BeatStatus.Early;
                return;
            }

            if (_time > PerfectHitTime[1])
            {
                BeatStatus = BeatStatus.Late;
                return;
            }

            if (_time >= PerfectHitTime[0] && _time <= PerfectHitTime[1])
            {
                BeatStatus = BeatStatus.Perfect;
                return;
            }

            Debug.LogError("There is Error");


        }


        public string BeatDirection()
        {
            float _dirX = Direction.x;
            float _dirY = Direction.y;
            string _directionName = "";

            if (Mathf.Approximately(_dirX, 0))
            {
                if (_dirY > 0) _directionName = "Up";
                if (_dirY < 0) _directionName = "Down";
            }
            else
            {
                if (_dirX > 0) _directionName = "Right";
                if (_dirX < 0) _directionName = "Left";
            }

            return _directionName;
        }
    }

    public enum BeatTypes
    {
        Quarter,
        Half,
        Eighth,
        Whole,
    }
}
