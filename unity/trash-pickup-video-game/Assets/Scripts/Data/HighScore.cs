using System;
using Models;
using UnityEngine;

namespace Data
{
    public class HighScore : ScriptableObject, IHighScore
    {
        public float score = 0f;
        public string initials = "";
        public DateTime dateTime = DateTime.Now;

        public float Score => score;
        public string Initials => initials;
        public DateTime Date => dateTime;
    }
}