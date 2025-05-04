using S_EDex365.API.Models;
using S_EDex365.API.Models.Payment;

namespace S_EDex365.API.Interfaces
{
    public interface IWalletService
    {
        Task<bool> InsertWalletAsync(PaymentResponse paymentResponse,Guid userId);
        Task<StudentWallet> GetStudentWalletAsync(Guid userId);
        Task<TeacherWallet> GetTeacherWalletAsync(Guid userId);
    }
}
