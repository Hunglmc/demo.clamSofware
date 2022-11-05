using System.ComponentModel.DataAnnotations;

public enum ClassNameEnum
{
    [Display(Name = "100USS")]
    None,
    [Display(Name = "200USD")]
    FirstGrade,
    [Display(Name = "300USD")]
    SecondGrade,
    [Display(Name = "5000VND")]
    GradeThree
}