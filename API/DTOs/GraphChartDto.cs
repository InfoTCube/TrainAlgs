using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;

public class GraphChartDto
{
    public IEnumerable<Tuple<string, int>>? Solutions { get; set; }
    public int SolvedAllTime { get; set; }
    public int SolvedLastYear { get; set; }
    public int SolvedLastMonth { get; set; }
    public int SolvedInRowAllTime { get; set; }
    public int SolvedInRowLastYear { get; set; }
    public int SolvedInRowLastMonth { get; set; }
}
