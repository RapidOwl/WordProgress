using System;
using NUnit.Framework;
using WordProgress.Domain.Aggregates;
using WordProgress.Domain.Commands;
using WordProgress.Domain.Events;
using WordProgress.Domain.Exceptions;

namespace WordProgress.Tests
{
    [TestFixture]
    public class WriterTests : BDDTest<Writer>
    {
        private Guid _commandId;

        private const int MaxBioLength = 160;

        private Guid _userId;
        private string _userName;
        private string _name;
        private string _bio;
        private string _tooLongBio;

        private Guid _projectId;
        private string _projectName;
        private DateTime _startDate;
        private DateTime _targetCompletionDate;
        private uint _targetWordCount;

        private Guid _differentProjectId;
        private string _differentProjectName;
        
        [SetUp]
        public void SetUp()
        {
            _commandId = Guid.NewGuid();

            _userId = Guid.NewGuid();
            _userName = "new_user_name";
            _name = "Test Name";
            _bio = new string('*', MaxBioLength);
            _tooLongBio = new string('*', MaxBioLength + 1);

            _projectId = Guid.NewGuid();
            _projectName = "New Project Name";
            _startDate = new DateTime(2015, 10, 16);
            _targetCompletionDate = new DateTime(2015, 12, 31);
            _targetWordCount = 120000;

            _differentProjectId = Guid.NewGuid();
            _differentProjectName = "Different Project Name";
        }

        #region RegisterWriter

        [Test]
        public void CanRegisterNewWriter()
        {
            Test(
                Given(),
                When(new RegisterWriter
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }),
                Then(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }));
        }

        [Test]
        public void CanNotRegisterWriterAgain()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }),
                When(new RegisterWriter
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }),
                ThenFailWith<UserAlreadyRegistered>());
        }

        #endregion

        #region UpdateWriter

        [Test]
        public void CanUpdateWriter()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }),
                When(new UpdateWriter
                {
                    Id = _commandId,
                    Name = _name,
                    Bio = _bio
                }),
                Then(new WriterUpdated
                {
                    Id = _commandId,
                    Name = _name,
                    Bio = _bio
                }));
        }

        [Test]
        public void CanNotUpdateWriterWhenWriterNotRegistered()
        {
            Test(
                Given(),
                When(new UpdateWriter
                {
                    Id = _commandId,
                    Name = _name,
                    Bio = _bio
                }),
                ThenFailWith<WriterNotRegistered>());
        }

        [Test]
        public void CanNotUpdateWriterWhenBioTooLong()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }),
                When(new UpdateWriter
                {
                    Id = _commandId,
                    Name = _name,
                    Bio = _tooLongBio
                }),
                ThenFailWith<BioTooLong>());
        }
        #endregion

        #region CreateProjectForWriter

        [Test]
        public void CanCreateProjectForWriter()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }),
                When(new CreateProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                Then(new ProjectCreatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }));
        }

        [Test]
        public void CanNotCreateProjectForWriterWhenWriterNotRegistered()
        {
            Test(
                Given(),
                When(new CreateProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<WriterNotRegistered>());
        }

        [Test]
        public void CanNotCreateProjectForWriterWhenProjectNameAlreadyInUse()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new CreateProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<ProjectNameAlreadyUsedByThisWriter>());
        }

        #endregion

        #region UpdateProjectForWriter

        [Test]
        public void CanUpdateProjectForWriter()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new UpdateProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                Then(new ProjectUpdatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }));
        }

        [Test]
        public void CanNotUpdateProjectForWriterWhenWriterNotRegistered()
        {
            Test(
                Given(),
                When(new UpdateProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<WriterNotRegistered>());
        }

        [Test]
        public void CanNotUpdateProjectForWriterWhenNoProjectsCreated()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }),
                When(new UpdateProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<ProjectDoesntExistForThisWriter>());
        }

        [Test]
        public void CanNotUpdateProjectForWriterWhenProjectDoesntExist()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new UpdateProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _differentProjectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<ProjectDoesntExistForThisWriter>());
        }

        [Test]
        public void CanNotUpdateProjectForWriterWhenNewProjectNameInUseByAnotherProject()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                },
                new ProjectCreatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _differentProjectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new UpdateProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _differentProjectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<ProjectNameAlreadyUsedByThisWriter>());
        }

        #endregion

        #region DeleteProjectForWriter

        [Test]
        public void CanDeleteProjectForWriter()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new DeleteProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId
                }),
                Then(new ProjectDeletedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId
                }));
        }

        [Test]
        public void CanNotDeleteProjectForWriterWhenWriterNotRegistered()
        {
            Test(
                Given(),
                When(new DeleteProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId
                }),
                ThenFailWith<WriterNotRegistered>());
        }

        [Test]
        public void CanNotDeleteProjectForWriterWhenNoProjectsExist()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                }),
                When(new DeleteProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId
                }),
                ThenFailWith<ProjectDoesntExistForThisWriter>());
        }

        [Test]
        public void CanNotDeleteProjectForWriterWhenProjectDoesntExist()
        {
            Test(
                Given(new WriterRegistered
                {
                    Id = _commandId,
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreatedForWriter
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new DeleteProjectForWriter
                {
                    Id = _commandId,
                    ProjectId = _differentProjectId
                }),
                ThenFailWith<ProjectDoesntExistForThisWriter>());
        }

        #endregion
    }
}
