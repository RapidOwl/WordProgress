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
    public class ProjectTests : BDDTest<Project>
    {
        private Guid _wordCountUpdateId;

        [SetUp]
        public void SetUp()
        {
            _wordCountUpdateId = Guid.NewGuid();
        }

        #region UpdateWordCount

        [Test]
        public void CanUpdateWordCount()
        {
            Test(
                Given(),
                When(new UpdateWordCount
                {
                    NewTotalWordCount = 56000
                }),
                Then(new WordCountUpdated
                {
                    NewTotalWordCount = 76000,
                    WordsAdded = 20000
                }));
        }

        [Test]
        public void CanNotUpdateWordCountWhenNewWordCountLessThanCurrentWordCount()
        {
            Test(
                Given(new WordCountUpdated
                {
                    NewTotalWordCount = 76000,
                    WordsAdded = 20000
                }),
                When(new UpdateWordCount
                {
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
                Given(new WordCountUpdated
                {
                    Id = _wordCountUpdateId,
                    NewTotalWordCount = 76000,
                    WordsAdded = 20000
                }),
                When(new DeleteWordCountUpdate
                {
                    Id = _wordCountUpdateId
                }),
                Then(new WordCountUpdateDeleted()));
        }

        [Test]
        public void CanNotDeleteWordCountUpdateWhenWordCountUpdateDoesntExist()
        {
            Test(
                Given(),
                When(new DeleteWordCountUpdate
                {
                    Id = _wordCountUpdateId
                }),
                ThenFailWith<WordCountUpdateDoesntExistForThisProject>());
        }

        #endregion
    }
}
