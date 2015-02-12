namespace lib.test.a.b.Vehicals.TwoWheeled.MotorCycles
{
    public class Harley<T> : IHog<T>
    {
        public Trim MyTrim { get; set; }
        public T Type { get; set; }
    }

    public enum Trim
    {
        Chome,
        Matte
    }

    public interface IHog<T>
    {

    }
}