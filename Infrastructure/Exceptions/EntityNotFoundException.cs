namespace MAS.Payments.Infrastructure.Exceptions
{
    using System;

    [Serializable]
    public class EntityNotFoundException(
        Type entityType,
        long entityId
    )
        : Exception($"Entity {entityType.Name} with id {entityId} doesn't exist")
    { }
}
