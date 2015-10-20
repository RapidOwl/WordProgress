using System;

namespace WordProgress.Domain.Exceptions
{
    #region Writer Exceptions

    public class UserAlreadyRegistered : Exception
    {
        public UserAlreadyRegistered() : base("This user has already been registered.") { }
    }

    public class UserNameTaken : Exception
    {
        public UserNameTaken() : base("This user name has already been taken.") { }
    }

    public class BioTooLong : Exception
    {
        public BioTooLong(int maxBioLength) : base($"The bio you have provided must be less than {maxBioLength} characters in length.") { }
    }

    public class WriterNotRegistered : Exception
    {
        public WriterNotRegistered () : base("You must register the writer before performing any other operations.") { }
    }

    public class ProjectNameAlreadyUsedByThisWriter : Exception
    {
        public ProjectNameAlreadyUsedByThisWriter () : base("This project name is already in use.") { }
    }

    public class ProjectDoesntExistForThisWriter : Exception
    {
        public ProjectDoesntExistForThisWriter () : base("The supplied ID does not resolve to any project owned by this writer.") { }
    }

    #endregion

    #region Project Exceptions

    public class ProjectAlreadyCreated : Exception
    {
        public ProjectAlreadyCreated() : base("This project has already been created.") { }
    }

    public class ProjectNotYetCreated : Exception
    {
        public ProjectNotYetCreated() : base("This project has not been created.") { }
    }

    public class NewWordCountLessThanCurrentWordCount : Exception
    {
        public NewWordCountLessThanCurrentWordCount() : base("The supplied new word count is less than the current word count.") { }
    }

    public class WordCountUpdateDoesntExistForThisProject : Exception
    {
        public WordCountUpdateDoesntExistForThisProject () : base("The supplied ID does not resolve to a word count update for this project.") { }
    }

    public class ProjectHasNoWordCountUpdates : Exception
    {
        public ProjectHasNoWordCountUpdates () : base("This project has no word count updates.") { }
    }

    #endregion
}
