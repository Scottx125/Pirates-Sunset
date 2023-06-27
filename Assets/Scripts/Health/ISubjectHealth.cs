public interface ISubjectHealth{
    public void AddHealthObserver(IObserverShipMovement observer);
    public void RemoveHealthObserver(IObserverShipMovement observer);
    public void NotifyOnDamage(float percent, AttackTypeEnum type);
}
