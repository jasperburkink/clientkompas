﻿namespace Application.Common.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }

        DateTime GetUtcNow();
    }

}
