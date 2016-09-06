using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloonyIntegrationTaskUpdater.Models {
    class OrganisationalSet {
        public string id { get; set; }
        public IEnumerable<Project> Projekte { get; set; }
    }
}
