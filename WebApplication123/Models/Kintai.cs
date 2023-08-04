using System. ComponentModel. DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace Wing1.Models;

//[PrimaryKey(nameof(Userid), nameof(Date))]
public class Kintai
{
    //[Key]
    [Display(Name = "ユーザID")]
    public string Userid {get; set;} = String.Empty;
    //[PrimaryKey]
    [Display(Name = "日付")]
    [DisplayFormat(DataFormatString = "{0:yyyy年M月d日}")]
    public DateTime Date {get; set;}
    [Display(Name = "勤務形態")]
    public string? Workstyle {get; set;}
    [Display(Name = "開始")]
    [DataType(DataType.Time)]
    public DateTime? Starttime{get; set;}
    [Display(Name = "終了")]
    [DataType(DataType.Time)]
    public DateTime? Endtime{get; set;}
    [Display(Name = "休憩時間1開始")]
    [DataType(DataType.Time)]
    public DateTime? Break1start{get; set;}
    [Display(Name = "休憩時間1終了")]
    [DataType(DataType.Time)]
    public DateTime? Break1end{get; set;}
    [Display(Name = "休憩時間2開始")]
    [DataType(DataType.Time)]
    public DateTime? Break2start{get; set;}
    [Display(Name = "休憩時間2終了")]
    [DataType(DataType.Time)]
    public DateTime? Break2end{get; set;}
    [Display(Name = "休憩時間3開始")]
    [DataType(DataType.Time)]
    public DateTime? Break3start{get; set;}
    [Display(Name = "休憩時間3終了")]
    [DataType(DataType.Time)]
    public DateTime? Break3end{get; set;}
    [Display(Name = "休憩時間4開始")]
    [DataType(DataType.Time)]
    public DateTime? Break4start{get; set;}
    [Display(Name = "休憩時間4終了")]
    [DataType(DataType.Time)]
    public DateTime? Break4end{get; set;}
    [Display(Name = "備考")]
    public string? biko {get; set;} = String.Empty;
    
    public static List<SelectListItem> WorkstyleList { get; } = new List<SelectListItem>
    {
        new SelectListItem{Value = "", Text = "--選択してください--"},
        new SelectListItem{Value = "勤務", Text = "勤務"},
        new SelectListItem{Value = "遅刻", Text = "遅刻"},
        new SelectListItem{Value = "早退", Text = "早退"},
        new SelectListItem{Value = "有休", Text = "有休"},
        new SelectListItem{Value = "代休", Text = "代休"},
        new SelectListItem{Value = "振替", Text = "振替"}
    };
}