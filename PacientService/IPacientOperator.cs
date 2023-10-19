namespace PacientService
{
    public interface IPacientOperator
    {
        Task ProcessPacient(int orderId);
    }
}