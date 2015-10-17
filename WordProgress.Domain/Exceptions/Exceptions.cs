using System;

namespace WordProgress.Domain.Exceptions
{
    public class UserAlreadyRegistered : Exception { }

    public class UserNameTaken : Exception { }

    public class BioTooLong : Exception { }

    public class WriterNotRegistered : Exception { }

    public class ProjectListNotYetRetrieved : Exception { }

    public class ProjectNameAlreadyUsed : Exception { }

    public class ProjectDoesntExist : Exception { }
    
    public class WordCountUpdateDoesntExist : Exception { }

    public class ProjectHasNoWordCountUpdates : Exception { }
}
