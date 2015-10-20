using System;
using NUnit.Framework;
using WordProgress.Domain.Aggregates;
using WordProgress.Domain.Commands;
using WordProgress.Domain.Events;
using WordProgress.Domain.Exceptions;

namespace WordProgress.Tests
{
    [TestFixture]
    public class ProjectTests : BDDTest<Project>
    {
        private Guid _commandId;
        private Guid _wordCountUpdateId;
        private Guid _projectId;
        private string _newProjectName;
        private DateTime _newProjectStartDate;
        private uint _newProjectTargetWordCount;
        private DateTime _newProjectTargetCompletionDate;

        [SetUp]
        public void SetUp()
        {
            _commandId = Guid.NewGuid();
            _wordCountUpdateId = Guid.NewGuid();
            _projectId = Guid.NewGuid();
            _newProjectName = "New Project";
            _newProjectStartDate = DateTime.Now;
            _newProjectTargetCompletionDate = DateTime.Now.AddYears(1);
            _newProjectTargetWordCount = 120000;
        }

        #region CreateProject
        [Test]
        public void CanCreateProject()
        {
            Test(
                Given(),
                When(new CreateProject
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount
                }),
                Then(new ProjectCreated
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount
                }));
        }

        [Test]
        public void CanNotCreateNewProjectWhenProjectAlreadyCreated()
        {
            Test(
                Given(new ProjectCreated
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount
                }),
                When(new CreateProject
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount
                }),
                ThenFailWith<ProjectAlreadyCreated>());
        }
        #endregion

        #region UpdateProject

        [Test]
        public void CanUpdateProject()
        {
            Test(
                Given(new ProjectCreated
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount,
                }),
                When(new UpdateProject
                {
                    Id = _commandId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount,
                }),
                Then(new ProjectUpdated
                {
                    Id = _commandId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount,
                }));
        }

        [Test]
        public void CanNotUpdateProjectWhenProjectNotYetCreated()
        {
            Test(
                Given(),
                When(new UpdateProject
                {
                    Id = _commandId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount
                }),
                ThenFailWith<ProjectNotYetCreated>());
        }

        #endregion

        #region UpdateWordCount

        [Test]
        public void CanUpdateWordCount()
        {
            Test(
                Given(new ProjectCreated
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount,
                }),
                When(new UpdateWordCount
                {
                    Id = _commandId,
                    WordCountUpdateId = _wordCountUpdateId,
                    NewTotalWordCount = 56000
                }),
                Then(new WordCountUpdated
                {
                    Id = _commandId,
                    WordCountUpdateId = _wordCountUpdateId,
                    NewTotalWordCount = 56000
                }));
        }

        [Test]
        public void CanNotUpdateWordCountWhenProjectNotCreated()
        {
            Test(
                Given(),
                When(new UpdateWordCount
                {
                    Id = _commandId,
                    NewTotalWordCount = 56000
                }),
                ThenFailWith<ProjectNotYetCreated>());
        }

        [Test]
        public void CanNotUpdateWordCountWhenNewWordCountLessThanCurrentWordCount()
        {
            Test(
                Given(new ProjectCreated
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount,
                },
                new WordCountUpdated
                {
                    Id = _commandId,
                    NewTotalWordCount = 76000
                }),
                When(new UpdateWordCount
                {
                    Id = _commandId,
                    NewTotalWordCount = 56000
                }),
                ThenFailWith<NewWordCountLessThanCurrentWordCount>());
        }

        #endregion

        #region DeleteWordCountUpdate

        [Test]
        public void CanDeleteWordCountUpdate()
        {
            Test(
                Given(new ProjectCreated
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount,
                },
                new WordCountUpdated
                {
                    Id = _commandId,
                    WordCountUpdateId = _wordCountUpdateId,
                    NewTotalWordCount = 76000
                }),
                When(new DeleteWordCountUpdate
                {
                    Id = _commandId,
                    WordCountUpdateId = _wordCountUpdateId
                }),
                Then(new WordCountUpdateDeleted
                {
                    Id = _commandId,
                    WordCountUpdateId = _wordCountUpdateId
                }));
        }

        [Test]
        public void CanNotDeleteWordCountWhenProjectNotCreated()
        {
            Test(
                Given(),
                When(new DeleteWordCountUpdate
                {
                    Id = _commandId,
                    WordCountUpdateId = _wordCountUpdateId
                }),
                ThenFailWith<ProjectNotYetCreated>());
        }

        [Test]
        public void CanNotDeleteWordCountUpdateWhenThereNoWordCountUpdatesExist()
        {
            Test(
                Given(new ProjectCreated
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount,
                }),
                When(new DeleteWordCountUpdate
                {
                    Id = _commandId,
                    WordCountUpdateId = _wordCountUpdateId
                }),
                ThenFailWith<WordCountUpdateDoesntExistForThisProject>());
        }

        [Test]
        public void CanNotDeleteWordCountUpdateWhenWordCountUpdateDoesntExist()
        {
            Test(
                Given(new ProjectCreated
                {
                    Id = _commandId,
                    ProjectId = _projectId,
                    Name = _newProjectName,
                    StartDate = _newProjectStartDate,
                    TargetCompletionDate = _newProjectTargetCompletionDate,
                    TargetWordCount = _newProjectTargetWordCount,
                }),
                When(new DeleteWordCountUpdate
                {
                    Id = _commandId,
                    WordCountUpdateId = _wordCountUpdateId
                }),
                ThenFailWith<WordCountUpdateDoesntExistForThisProject>());
        }

        #endregion
    }
}
