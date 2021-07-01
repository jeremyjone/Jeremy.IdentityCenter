using System.Collections.Generic;

namespace Jeremy.IdentityCenter.Business.Models.PersistedGrants
{
    public class PersistedGrantsViewModel
    {
        public string SubjectId { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public List<PersistedGrantViewModel> PersistedGrants { get; set; } = new List<PersistedGrantViewModel>();
    }
}
