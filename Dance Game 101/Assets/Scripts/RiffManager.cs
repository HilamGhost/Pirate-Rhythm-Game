using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hilam
{
    [CreateAssetMenu(menuName = "Create Riff")]
    public class RiffManager : ScriptableObject
    {
        public List<RythmPoint> rhythmPoints;
        [Space(50)] 
        public float time;
    }
}
