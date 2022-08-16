using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;

public class TaskParams : ElementParams
{
    public Status TaskStatus { get; set; } // 2 - Solved, 1 - Attempted, 0 - NotAttempted 

    public enum Status
    {
        NotAttempted = 0,
        Attempted = 1,
        Solved = 2
    }
}