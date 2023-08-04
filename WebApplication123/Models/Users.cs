using System. ComponentModel. DataAnnotations;

namespace Wing1.Models;
public class Users
{
    [Key]
    [Display(Name = "ユーザーID")] 
    [Required(ErrorMessage = "ユーザーIDは必須入力です。")]
    public string Userid {get; set;}
    [Display(Name = "パスワード")] 
    [Required(ErrorMessage = "パスワードは必須入力です。")]
    public string Pass {get;set;} = String.Empty;
    [Display(Name = "氏名")] 
    [Required(ErrorMessage = "氏名は必須入力です。")]
    public string Username {get;set;} = String.Empty;
    [Display(Name = "基本勤務開始時間")] 
    [Required(ErrorMessage = "基本勤務開始時間は必須入力です。")]
    [DataType(DataType.Time)]
    public DateTime? Kstarttime{get; set;}
    [Display(Name = "基本勤務終了時間")] 
    [Required(ErrorMessage = "基本勤務終了時間は必須入力です。")]
     [DataType(DataType.Time)]
    public DateTime? Kendtime{get; set;}
    public int? adminflag {get;set;}
    [Display(Name = "実働時間")] 
    public TimeSpan? Jitudo{get; set;}
}