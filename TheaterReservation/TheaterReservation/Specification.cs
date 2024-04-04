namespace TheaterReservation;

public interface Specification<T>
{
    bool IsSatisfiedBy(T performanceInventory);
}