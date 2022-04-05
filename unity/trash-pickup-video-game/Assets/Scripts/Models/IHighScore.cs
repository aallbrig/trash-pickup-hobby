using System;

namespace Models
{
    public interface IHighScore
    {
        float Score { get; }
        string Initials { get; }
        DateTime Date { get; }
    }
}