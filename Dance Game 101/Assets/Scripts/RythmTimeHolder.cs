using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hilam
{
    [CreateAssetMenu(menuName = "Create Time Holder")]
    public class RythmTimeHolder : ScriptableObject
    {
        public List<float> RhythmTimes;

        public void DeleteALlTimes()
        {
            RhythmTimes.Clear();
        }

        public void AddTime(float _time) => RhythmTimes.Add(_time);
    }
}
