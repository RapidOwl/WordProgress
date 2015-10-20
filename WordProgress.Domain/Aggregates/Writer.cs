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
        IHandleCommand<CreateProjectForWriter>,
        IHandleCommand<UpdateProjectForWriter>,
        IHandleCommand<DeleteProjectForWriter>,
        IApplyEvent<WriterRegistered>,
        IApplyEvent<WriterUpdated>,
        IApplyEvent<ProjectCreatedForWriter>,
        IApplyEvent<ProjectUpdatedForWriter>,
        IApplyEvent<ProjectDeletedForWriter>
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

        public IEnumerable Handle(CreateProjectForWriter c)
        {
            if (!UserExists())
            {
                throw new WriterNotRegistered();
            }

            if (!ValidProjectName(c.Name))
            {
                throw new ProjectNameAlreadyUsedByThisWriter();
            }

            yield return new ProjectCreatedForWriter
            {
                Id = c.Id,
                ProjectId = c.ProjectId,
                Name = c.Name,
                StartDate = c.StartDate,
                TargetCompletionDate = c.TargetCompletionDate,
                TargetWordCount = c.TargetWordCount
            };
        }

        public IEnumerable Handle(UpdateProjectForWriter c)
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

            yield return new ProjectUpdatedForWriter()
            {
                Id = c.Id,
                ProjectId = c.ProjectId,
                Name = c.Name,
                StartDate = c.StartDate,
                TargetCompletionDate = c.TargetCompletionDate,
                TargetWordCount = c.TargetWordCount
            };
        }

        public IEnumerable Handle(DeleteProjectForWriter c)
        {
            if (!UserExists())
            {
                throw new WriterNotRegistered();
            }

            if (!ProjectExists(c.ProjectId))
            {
                throw new ProjectDoesntExistForThisWriter();
            }

            yield return new ProjectDeletedForWriter
            {
                Id = c.Id,
                ProjectId = c.ProjectId
            };
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

        public void Apply(ProjectCreatedForWriter e)
        {
            var project = new Project();

            project.ApplyEvents(project.Handle(new CreateProject
            {
                Id = Guid.NewGuid(),
                ProjectId = e.ProjectId,
                Name = e.Name,
                StartDate = e.StartDate,
                TargetCompletionDate = e.TargetCompletionDate,
                TargetWordCount = e.TargetWordCount
            }));

            _projects.Add(project);
        }

        public void Apply(ProjectUpdatedForWriter e)
        {
            var project = _projects.Single(x => x.Id == e.ProjectId);

            project.ApplyEvents(project.Handle(new UpdateProject
            {
                Id = Guid.NewGuid(),
                ProjectId = e.ProjectId,
                Name = e.Name,
                StartDate = e.StartDate,
                TargetCompletionDate = e.TargetCompletionDate,
                TargetWordCount = e.TargetWordCount
            }));
        }

        public void Apply(ProjectDeletedForWriter e)
        {
            _projects.RemoveAll(x => x.Id == e.ProjectId);
        }
        #endregion
    }
}
