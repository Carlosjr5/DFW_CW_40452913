using DFW_CW_40452913.Data;
using Microsoft.AspNetCore.Identity;

namespace DFW_CW_40452913.Models
{
    public class UserVote
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PetitionId { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Petition Petition { get; set; }
    }
}
