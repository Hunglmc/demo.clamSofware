namespace demo.clamSofware.Models
{
    /// <summary>
    /// Để khai báo lớp kế thừa dùng cú pháp, lớp cơ sở viết sau ký hiệu :
    /// Kế thừa lại các thuộc tính của  EmployeesCreateViewModel tránh viết lại nhiều lần gây code không clen
    /// </summary>
    public class EmployeesEditVidewModel : EmployeesCreateViewModel
    {
        public int Id { get; set; }

        public string ExistingPhotoPath { get; set; }
    }
}
