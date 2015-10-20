using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WordProgress.Domain.Commands;
using WordProgress.Domain.Events;
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
        
        #endregion

        public IEnumerable Handle(CreateProject c)
        {
            throw new NotImplementedException();
        }

        public IEnumerable Handle(UpdateProject c)
        {
            throw new NotImplementedException();
        }

        public IEnumerable Handle(UpdateWordCount c)
        {
            throw new NotImplementedException();
        }

        public IEnumerable Handle(DeleteWordCountUpdate c)
        {
            throw new NotImplementedException();
        }

        public void Apply(ProjectCreated e)
        {
            throw new NotImplementedException();
        }

        public void Apply(ProjectUpdated e)
        {
            throw new NotImplementedException();
        }

        public void Apply(WordCountUpdated e)
        {
            throw new NotImplementedException();
        }

        public void Apply(WordCountUpdateDeleted e)
        {
            throw new NotImplementedException();
        }
    }
}
