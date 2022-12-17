using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;

public class TaskParams : ElementParams
{
    public Status TaskStatus { get; set; }

    public enum Status
    {
        Default = 0,
        NotAttempted = 1,
        Attempted = 2,
        Solved = 3
    }
}