using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    internal class Config
    {
        public static List<Types.Candidate> Candidates { get; set; }
        public static Dictionary<string, int> VotingStatus { get; set; }
        public static int VoterCount { get => Database.Voters.Count; }
        public static int VotersVoted { get; set; }
    }
}
