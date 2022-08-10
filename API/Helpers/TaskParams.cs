using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;

public class TaskParams : ElementParams
{
    public int TaskStatus { get; set; } // 2 - Solved, 1 - Attempted, 0 - NotAttempted 
}