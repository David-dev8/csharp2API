﻿using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Result
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        [MaxLength(100)]
        public Mode Mode { get; set; } // todo mode als string

        public int Position { get; set; }

        public int PlayerId { get; set; }

        public Result(Mode mode, int position)
        {
            Mode = mode;
            Position = position;
            Time = DateTime.Now;
        }
    }

    public enum Mode
    {
        QUIZ_ROYALE
    }
}
