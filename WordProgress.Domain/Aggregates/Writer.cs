using System;
using System.Collections.Generic;
using System.Linq;
using WordProgress.Edument;

namespace WordProgress.Domain.Aggregates
{
    public class Writer : Aggregate
    {
        private bool _registered;
        private DateTime _created;
        private string _userName;
        private string _name;
        private string _bio;

        private List<Project> _projects;

        // TODO Bio too long checks + exception.

        #region Checks

        private bool UserExists() => _registered && _created != null && _userName != string.Empty;
        private bool ProjectsLoaded() => _projects != null;
        private bool ValidProjectName(string newProjectName) => _projects.All(x => x.Name != newProjectName);
        private bool ProjectExists(Guid projectId) => _projects.Any(x => x.Id == projectId);
        private bool HasProjects() => _projects.Any();

        #endregion

        // TODO When implementing the update project code, 
        // make sure we check that it's only other projects
        // and not this one that need to have different names.
    }
}
