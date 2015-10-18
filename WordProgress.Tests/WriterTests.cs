using System;
using NUnit.Framework;
using WordProgress.Domain.Aggregates;
using WordProgress.Domain.Commands;
using WordProgress.Domain.Events;
using WordProgress.Domain.Exceptions;
using WordProgress.Edument;

namespace WordProgress.Tests
{
    [TestFixture]
    public class WriterTests : BDDTest<Writer>
    {
        private string _userName;
        private string _name;
        private string _bio;
        private string _tooLongBio;

        //private IEnumerable<ProjectDto> _projectList;
        //private IEnumerable<ProjectDto> _projectListWithProject;

        private Guid _projectId;
        private string _projectName;
        private DateTime _created;
        private DateTime _startDate;
        private DateTime _targetCompletionDate;
        private int _targetWordCount;

        private Guid _differentProjectId;
        private string _differentProjectName;

        [SetUp]
        public void SetUp()
        {
            _userName = "new_user_name";
            _name = "Test Name";
            _bio = new string('*', 160);
            _tooLongBio = new string('*', 161);

            //_projectList = new List<ProjectDto>();
            //_projectListWithProject = new List<ProjectDto>
            //{
            //    new ProjectDto
            //    {
            //        Id = Guid.NewGuid(),
            //        Created = _created,
            //        Name = _projectName,
            //        StartDate = _startDate,
            //        TargetCompletionDate = _targetCompletionDate,
            //        TargetWordCount = _targetWordCount
            //    }
            //};

            _projectId = Guid.NewGuid();
            _created = new DateTime(2015, 10, 15);
            _projectName = "New Project Name";
            _startDate = new DateTime(2015, 10, 16);
            _targetCompletionDate = new DateTime(2015, 12, 31);
            _targetWordCount = 120000;

            _differentProjectId = Guid.NewGuid();
            _differentProjectName = "Different Project Name";
        }

        #region RegisterWriter

        [Test]
        public void CanCreateNewWriter()
        {
            Test(
                Given(),
                When(new RegisterWriter
                {
                    UserName = _userName,
                    Name = _name
                }),
                Then(new WriterRegistered
                {
                    Id = Guid.NewGuid(),
                    UserName = _userName,
                    Name = _name
                }));
        }

        [Test]
        public void CanNotCreateWriterAgain()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                }),
                When(new RegisterWriter
                {
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
                    UserName = _userName,
                    Name = _name
                }),
                When(new UpdateWriter
                {
                    Name = _name,
                    Bio = _bio
                }),
                Then(new WriterUpdated
                {
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
                    UserName = _userName,
                    Name = _name
                }),
                When(new UpdateWriter
                {
                    Name = _name,
                    Bio = _tooLongBio
                }),
                ThenFailWith<BioTooLong>());
        }
        #endregion

        #region CreateProject

        [Test]
        public void CanCreateProject()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                }),
                When(new CreateProject
                {
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                Then(new ProjectCreated
                {
                    Id = _projectId,
                    Created = _created,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }));
        }

        [Test]
        public void CanNotCreateProjectWhenWriterNotRegistered()
        {
            Test(
                Given(),
                When(new CreateProject
                {
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<WriterNotRegistered>());
        }

        [Test]
        public void CanNotCreateProjectWhenProjectNameAlreadyInUse()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreated
                {
                    Id = _projectId,
                    Created = _created,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new CreateProject
                {
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<ProjectNameAlreadyUsedByThisWriter>());
        }

        #endregion

        #region UpdateProject

        [Test]
        public void CanUpdateProject()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreated
                {
                    Id = _projectId,
                    Created = _created,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new UpdateProject
                {
                    Id = _projectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                Then(new ProjectUpdated
                {
                    Id = _projectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }));
        }

        [Test]
        public void CanNotUpdateProjectWhenWriterNotRegistered()
        {
            Test(
                Given(),
                When(new UpdateProject
                {
                    Id = _projectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<WriterNotRegistered>());
        }

        [Test]
        public void CanNotUpdateProjectWhenNoProjectsCreated()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                }),
                When(new UpdateProject
                {
                    Id = _projectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<ProjectDoesntExistForThisWriter>());
        }

        [Test]
        public void CanNotUpdateProjectWhenProjectDoesntExist()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreated
                {
                    Id = _projectId,
                    Created = _created,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new UpdateProject
                {
                    Id = _differentProjectId,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<ProjectDoesntExistForThisWriter>());
        }

        [Test]
        public void CanNotUpdateProjectWhenNewProjectNameInUseByAnotherProject()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreated
                {
                    Id = _projectId,
                    Created = _created,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                },
                new ProjectCreated
                {
                    Id = _differentProjectId,
                    Created = _created,
                    Name = _differentProjectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new UpdateProject
                {
                    Id = _differentProjectId,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                ThenFailWith<ProjectNameAlreadyUsedByThisWriter>());
        }

        #endregion

        #region DeleteProject

        [Test]
        public void CanDeleteProject()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreated
                {
                    Id = _projectId,
                    Created = _created,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new DeleteProject
                {
                    Id = _projectId
                }),
                Then(new ProjectDeleted()));
        }

        [Test]
        public void CanNotDeleteProjectWhenWriterNotRegistered()
        {
            Test(
                Given(),
                When(new DeleteProject
                {
                    Id = _projectId
                }),
                ThenFailWith<WriterNotRegistered>());
        }

        [Test]
        public void CanNotDeleteProjectWhenNoProjectsExist()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                }),
                When(new DeleteProject
                {
                    Id = _projectId
                }),
                ThenFailWith<ProjectDoesntExistForThisWriter>());
        }

        [Test]
        public void CanNotDeleteProjectWhenProjectDoesntExist()
        {
            Test(
                Given(new WriterRegistered
                {
                    UserName = _userName,
                    Name = _name
                },
                new ProjectCreated
                {
                    Id = _projectId,
                    Created = _created,
                    Name = _projectName,
                    StartDate = _startDate,
                    TargetCompletionDate = _targetCompletionDate,
                    TargetWordCount = _targetWordCount
                }),
                When(new DeleteProject
                {
                    Id = _differentProjectId
                }),
                ThenFailWith<ProjectDoesntExistForThisWriter>());
        }

        #endregion
    }
}
