using System.Collections.Generic;
using Data;
using Models;
using UnityEngine;

namespace Behaviors
{
    public class HighScoreScreen : MonoBehaviour
    {
        public List<IHighScore> highScores = new List<IHighScore>();
        //public List<HighScore> highScores = new List<HighScore>();
    }
}