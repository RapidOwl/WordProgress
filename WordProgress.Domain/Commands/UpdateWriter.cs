﻿namespace WordProgress.Domain.Commands
{
    public class UpdateWriter : BaseCommand
    {
        public string Name { get; set; }
        public string Bio { get; set; }
    }
}