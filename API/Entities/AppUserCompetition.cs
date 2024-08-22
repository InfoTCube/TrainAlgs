using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities;
public class AppUserCompetition
{
    public int AppUserId { get; set; }
    public AppUser? User { get; set; }
    public int CompetitionId { get; set; }
    public Competition? Competition { get; set; }
}
