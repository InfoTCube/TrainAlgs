using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;

public class TaskParams : ElementParams
{
    public Status TaskStatus { get; set; } //Solved, Attempted, NotAttempted 

    public enum Status
    {
        NotAttempted = 0,
        Attempted = 1,
        Solved = 2
    }
}