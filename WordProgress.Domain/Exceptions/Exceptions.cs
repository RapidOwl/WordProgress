using System;

namespace WordProgress.Domain.Exceptions
{
    public class UserAlreadyRegistered : Exception { }

    public class UserNameTaken : Exception { }

    public class UserDoesntExist : Exception { }

    public class ProjectNameAlreadyUsed : Exception { }

    public class ProjectDoesntExist : Exception { }

    public class UserHasNoProjects : Exception { }
    
    public class WordCountUpdateDoesntExist : Exception { }

    public class ProjectHasNoWordCountUpdates : Exception { }
}
