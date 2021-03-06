﻿using Microsoft.Azure.Mobile.Server;

namespace XamarinChallengeDemoService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }

        public string Name { get; set; }
    }
}