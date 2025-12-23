using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class ChemicalDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<ChemicalPathwayDto> Values { get; set; } = new();
    }
}
