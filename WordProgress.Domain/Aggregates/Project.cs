using System;
using System.Collections.Generic;
using System.Linq;
using WordProgress.Edument;

namespace WordProgress.Domain.Aggregates
{
    public class Project : Aggregate
        // IHandleCommand<Command>
        // IApplyEvent<Event>
    {
        private string _name;
        private DateTime _created;
        private DateTime _startDate;
        private DateTime _targetCompetionDate;
        private uint _targetWordCount;
        private uint _currentWordCount;

        private List<WordCountUpdate> _wordCountUpdates;

        #region Public properties

        // TODO Should this be in here?
        public string Name => _name;

        #endregion

        #region Checks

        private bool WordCountUpdateExists(Guid wordCountUpdateId) => _wordCountUpdates.Any(x => x.Id == wordCountUpdateId);
        
        #endregion
    }
}
