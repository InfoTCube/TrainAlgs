using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities;

public class Rating
{
    public int Id { get; set; }
    public AppUser? User { get; set; }
    public int UserId { get; set; }
    public AlgTask? Task { get; set; }
    public int TaskId { get; set; }
    public int Rate { get; set; }
}
