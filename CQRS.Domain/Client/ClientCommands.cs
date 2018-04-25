using CQRS.Core;
using System;

namespace CQRS.Domain
{  
    public class CreateClientCommand : ICommand {
        public readonly Guid Id;
        public readonly string Name;

        public CreateClientCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class RenameClientCommand : ICommand {
        public readonly Guid Id;
        public readonly string NewName;
        public readonly int OriginalVersion;

        public RenameClientCommand(Guid id, string newName, int originalVersion)
        {
            Id = id;
            NewName = newName;
            OriginalVersion = originalVersion;
        }
    }

    public class RemoveClientCommand : ICommand {
        public Guid Id;
        public readonly int OriginalVersion;

        public RemoveClientCommand(Guid id, int originalVersion)
        {
            Id = id;
            OriginalVersion = originalVersion;
        }
    }
}
