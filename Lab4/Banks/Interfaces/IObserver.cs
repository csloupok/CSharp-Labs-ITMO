namespace Banks.Interfaces;

public interface IObserver
{
    void Subscribe();
    void Unsubscribe();
    void Update(ISubject subject);
}