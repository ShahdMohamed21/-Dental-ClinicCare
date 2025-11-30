using Final_project.Models;

namespace Final_project.ViewModels
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }

        // ⭐ اقتراح إضافة خاصية المريض ⭐
        // لعرض تفاصيل المريض الرئيسية (قد يكون null إذا لم يكن هناك سجل مريض مرتبط)
        public Patient? Patient { get; set; }

        public List<Appointments> Appointments { get; set; }
    }
}