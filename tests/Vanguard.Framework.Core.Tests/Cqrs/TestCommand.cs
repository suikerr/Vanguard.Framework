﻿using Vanguard.Framework.Core.Cqrs;

namespace Vanguard.Framework.Core.Tests.Cqrs
{
    public class TestCommand : ICommand
    {
        public bool IsHandlerExecuted { get; set; }
    }
}
