using System;
using System.Runtime.Serialization;

namespace MAS.Payments.Infrastructure.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        private EntityNotFoundException() { }

        public EntityNotFoundException(Type entityType, long entityId)
        : base($"Entity {entityType.Name} with id {entityId} doesn't exist") { }

        protected EntityNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
