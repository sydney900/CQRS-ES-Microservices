using CQRS.Core;
using System;

namespace CQRS.Domain
{  
    public class CreateProductCommand : ICommand {
        public readonly Guid Id;
        public readonly string Name;

        public CreateProductCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class RenameProductCommand : ICommand {
        public readonly Guid Id;
        public readonly string NewName;
        public readonly int OriginalVersion;

        public RenameProductCommand(Guid id, string newName, int originalVersion)
        {
            Id = id;
            NewName = newName;
            OriginalVersion = originalVersion;
        }
    }

    public class RemoveProductCommand : ICommand {
        public Guid Id;
        public readonly int OriginalVersion;

        public RemoveProductCommand(Guid id, int originalVersion)
        {
            Id = id;
            OriginalVersion = originalVersion;
        }
    }
}
