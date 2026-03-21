namespace MAS.Payments.Queries.Measurements
{
    using System;

    using MAS.Payments.DataBase;
    using MAS.Payments.Infrastructure.Query;

    public enum GetSiblingMeasurementDirection
    {
        Previous = 1,
        Next = 2
    }

    public class GetSiblingMeasurementQuery : IQuery<MeterMeasurement>
    {
        public Guid TypeId { get; }

        public DateTime Date { get; }

        public GetSiblingMeasurementDirection Direction { get; }

        public GetSiblingMeasurementQuery(Guid typeId, DateTime date, GetSiblingMeasurementDirection direction)
        {
            if (typeId == default)
            {
                throw new ArgumentException(null, nameof(typeId));
            }

            if (date == default)
            {
                throw new ArgumentException(null, nameof(date));
            }

            if (direction == default)
            {
                throw new ArgumentNullException(nameof(direction));
            }

            TypeId = typeId;
            Date = date;
            Direction = direction;
        }
    }
}
