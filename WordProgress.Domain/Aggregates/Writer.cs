using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WordProgress.Domain.Commands;
using WordProgress.Domain.Events;
using WordProgress.Domain.Exceptions;
using WordProgress.Edument;

namespace WordProgress.Domain.Aggregates
{
    public class Writer : Aggregate,
        IHandleCommand<RegisterWriter>,
        IHandleCommand<UpdateWriter>,
        IHandleCommand<CreateProject>,
        IHandleCommand<UpdateProject>,
        IHandleCommand<DeleteProject>,
        IApplyEvent<WriterRegistered>,
        IApplyEvent<WriterUpdated>,
        IApplyEvent<ProjectCreated>,
        IApplyEvent<ProjectUpdated>,
        IApplyEvent<ProjectDeleted>
    {
        private const int MaxBioLength = 160;

        private bool _registered;
        private DateTime _registeredOn;
        private string _userName;
        private string _name;
        private string _bio;

        private List<Project> _projects;

        #region Checks

        private bool UserExists() => IdPopulated() && _registered && _userName != string.Empty;
        private bool BioTooLong(string bio) => bio.Length > MaxBioLength;
        private bool ValidProjectName(string newProjectName) => _projects != null && _projects.All(x => x.Name != newProjectName);

        private bool ValidUpdatedProjectName(Guid projectId, string newProjectName) => _projects != null && !_projects.Any(x => x.Id != projectId && x.Name == newProjectName);
        private bool ProjectExists(Guid projectId) => _projects != null && _projects.Any(x => x.Id == projectId);
        private bool HasProjects() => _projects != null && _projects.Any();

        #endregion

        #region Handling Commands
        public IEnumerable Handle(RegisterWriter c)
        {
            if (UserExists())
            {
                throw new UserAlreadyRegistered();
            }

            yield return new WriterRegistered
            {
                Id = c.Id,
                Name = c.Name,
                UserName = c.UserName
            };
        }

        public IEnumerable Handle(UpdateWriter c)
        {
            if (!UserExists())
            {
                throw new WriterNotRegistered();
            }

            if (BioTooLong(c.Bio))
            {
                throw new BioTooLong(MaxBioLength);
            }

            yield return new WriterUpdated
            {
                Id = c.Id,
                Name = c.Name,
                Bio = c.Bio
            };
        }

        public IEnumerable Handle(CreateProject c)
        {
            if (!UserExists())
            {
                throw new WriterNotRegistered();
            }

            if (!ValidProjectName(c.Name))
            {
                throw new ProjectNameAlreadyUsedByThisWriter();
            }

            yield return new ProjectCreated
            {
                Id = c.Id,
                Name = c.Name,
                StartDate = c.StartDate,
                TargetCompletionDate = c.TargetCompletionDate,
                TargetWordCount = c.TargetWordCount
            };
        }

        public IEnumerable Handle(UpdateProject c)
        {
            if (!UserExists())
            {
                throw new WriterNotRegistered();
            }

            if (!ProjectExists(c.ProjectId))
            {
                throw new ProjectDoesntExistForThisWriter();
            }

            if (!ValidUpdatedProjectName(c.ProjectId, c.Name))
            {
                throw new ProjectNameAlreadyUsedByThisWriter();
            }

            yield return new ProjectUpdated()
            {
                Id = c.Id,
                ProjectId = c.ProjectId,
                Name = c.Name,
                StartDate = c.StartDate,
                TargetCompletionDate = c.TargetCompletionDate,
                TargetWordCount = c.TargetWordCount
            };
        }

        public IEnumerable Handle(DeleteProject c)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Apply Events
        public void Apply(WriterRegistered e)
        {
            Id = Guid.NewGuid();
            _registered = true;
            _registeredOn = DateTime.Now;
            _name = e.Name;
            _userName = e.UserName;

            _projects = new List<Project>();
        }

        public void Apply(WriterUpdated e)
        {
            _name = e.Name;
            _bio = e.Bio;
        }

        public void Apply(ProjectCreated e)
        {
            var project = new Project();

            project.Handle(new CreateProject
            {
                Id = Guid.NewGuid(),
                Name = e.Name,
                StartDate = e.StartDate,
                TargetCompletionDate = e.TargetCompletionDate,
                TargetWordCount = e.TargetWordCount
            });
            
            _projects.Add(project);
        }

        public void Apply(ProjectUpdated e)
        {
            //var project = _projects.
        }

        public void Apply(ProjectDeleted e)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
