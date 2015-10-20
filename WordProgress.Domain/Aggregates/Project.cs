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
    public class Project : Aggregate,
        IHandleCommand<CreateProject>,
        IHandleCommand<UpdateProject>,
        IHandleCommand<UpdateWordCount>,
        IHandleCommand<DeleteWordCountUpdate>,
        IApplyEvent<ProjectCreated>,
        IApplyEvent<ProjectUpdated>,
        IApplyEvent<WordCountUpdated>,
        IApplyEvent<WordCountUpdateDeleted>
    {
        private string _name;
        private DateTime _created;
        private DateTime _startDate;
        private DateTime _targetCompletionDate;
        private uint _targetWordCount;
        private uint _currentWordCount;

        private List<WordCountUpdate> _wordCountUpdates;

        #region Public properties

        public string Name => _name;

        #endregion

        #region Checks

        private bool WordCountUpdateExists(Guid wordCountUpdateId) => _wordCountUpdates.Any(x => x.Id == wordCountUpdateId);
        private bool ValidNewWordCount(uint newWordCount) => newWordCount > _currentWordCount;

        #endregion

        #region Handling commands

        public IEnumerable Handle(CreateProject c)
        {
            if (IdPopulated())
            {
                throw new ProjectAlreadyCreated();
            }

            yield return new ProjectCreated
            {
                Id = c.Id,
                ProjectId = c.ProjectId,
                Name = c.Name,
                StartDate = c.StartDate,
                TargetCompletionDate = c.TargetCompletionDate,
                TargetWordCount = c.TargetWordCount
            };
        }

        public IEnumerable Handle(UpdateProject c)
        {
            if (!IdPopulated())
            {
                throw new ProjectNotYetCreated();
            }

            yield return new ProjectUpdated
            {
                Id = c.Id,
                Name = c.Name,
                StartDate = c.StartDate,
                TargetCompletionDate = c.TargetCompletionDate,
                TargetWordCount = c.TargetWordCount
            };
        }

        public IEnumerable Handle(UpdateWordCount c)
        {
            if (!IdPopulated())
            {
                throw new ProjectNotYetCreated();
            }

            if (!ValidNewWordCount(c.NewTotalWordCount))
            {
                throw new NewWordCountLessThanCurrentWordCount();
            }

            yield return new WordCountUpdated
            {
                Id = c.Id,
                WordCountUpdateId = c.WordCountUpdateId,
                NewTotalWordCount = c.NewTotalWordCount
            };
        }

        public IEnumerable Handle(DeleteWordCountUpdate c)
        {
            if (!IdPopulated())
            {
                throw new ProjectNotYetCreated();
            }

            if (!WordCountUpdateExists(c.WordCountUpdateId))
            {
                throw new WordCountUpdateDoesntExistForThisProject();
            }

            yield return new WordCountUpdateDeleted
            {
                Id = c.Id,
                WordCountUpdateId = c.WordCountUpdateId
            };
        }

        #endregion

        #region Applying events

        public void Apply(ProjectCreated e)
        {
            Id = e.ProjectId;
            _created = DateTime.Now;
            _name = e.Name;
            _startDate = e.StartDate;
            _targetCompletionDate = e.TargetCompletionDate;
            _targetWordCount = e.TargetWordCount;

            _wordCountUpdates = new List<WordCountUpdate>();
        }

        public void Apply(ProjectUpdated e)
        {
            _name = e.Name;
            _startDate = e.StartDate;
            _targetCompletionDate = e.TargetCompletionDate;
            _targetWordCount = e.TargetWordCount;
        }

        public void Apply(WordCountUpdated e)
        {
            _wordCountUpdates.Add(new WordCountUpdate
            {
                Id = e.WordCountUpdateId,
                WordsAdded = e.NewTotalWordCount - _currentWordCount
            });

            _currentWordCount = e.NewTotalWordCount;
        }

        public void Apply(WordCountUpdateDeleted e)
        {
            _wordCountUpdates.RemoveAll(x => x.Id == e.WordCountUpdateId);
        }

        #endregion
    }
}
