﻿namespace QuizRoyaleAPI.Models
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }

        public TokenDTO(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
