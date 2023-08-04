using System. ComponentModel. DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace Wing1.Models;

public class Details{
    [Display(Name = "ユーザID")]
    public string Userid {get; set;} = String.Empty;
    [Display(Name = "日付")]
    [DisplayFormat(DataFormatString = "{0:yyyy年M月d日}")]
    public DateTime Date {get; set;}
    [Display(Name = "開始1")]
    [DataType(DataType.Time)]
    public DateTime? Starttime1{get; set;}
    [Display(Name = "終了1")]
    [DataType(DataType.Time)]
    public DateTime? Endtime1{get; set;}
    [Display(Name = "内容1")]
    public string? Content1 {get; set;} = String.Empty;
    [Display(Name = "開始2")]
    [DataType(DataType.Time)]
    public DateTime? Starttime2{get; set;}
    [Display(Name = "終了2")]
    [DataType(DataType.Time)]
    public DateTime? Endtime2{get; set;}
    [Display(Name = "内容2")]
    public string? Content2 {get; set;} = String.Empty;
    [Display(Name = "開始3")]
    [DataType(DataType.Time)]
    public DateTime? Starttime3{get; set;}
    [Display(Name = "終了3")]
    [DataType(DataType.Time)]
    public DateTime? Endtime3{get; set;}
    [Display(Name = "内容3")]
    public string? Content3 {get; set;} = String.Empty;
    [Display(Name = "開始4")]
    [DataType(DataType.Time)]
    public DateTime? Starttime4{get; set;}
    [Display(Name = "終了4")]
    [DataType(DataType.Time)]
    public DateTime? Endtime4{get; set;}
    [Display(Name = "内容4")]
    public string? Content4 {get; set;} = String.Empty;
    [Display(Name = "開始5")]
    [DataType(DataType.Time)]
    public DateTime? Starttime5{get; set;}
    [Display(Name = "終了5")]
    [DataType(DataType.Time)]
    public DateTime? Endtime5{get; set;}
    [Display(Name = "内容5")]
    public string? Content5 {get; set;} = String.Empty;
    [Display(Name = "開始6")]
    [DataType(DataType.Time)]
    public DateTime? Starttime6{get; set;}
    [Display(Name = "終了6")]
    [DataType(DataType.Time)]
    public DateTime? Endtime6{get; set;}
    [Display(Name = "内容6")]
    public string? Content6 {get; set;} = String.Empty;
    [Display(Name = "開始7")]
    [DataType(DataType.Time)]
    public DateTime? Starttime7{get; set;}
    [Display(Name = "終了7")]
    [DataType(DataType.Time)]
    public DateTime? Endtime7{get; set;}
    [Display(Name = "内容7")]
    public string? Content7 {get; set;} = String.Empty;
    [Display(Name = "開始8")]
    [DataType(DataType.Time)]
    public DateTime? Starttime8{get; set;}
    [Display(Name = "終了8")]
    [DataType(DataType.Time)]
    public DateTime? Endtime8{get; set;}
    [Display(Name = "内容8")]
    public string? Content8 {get; set;} = String.Empty;
    [Display(Name = "開始9")]
    [DataType(DataType.Time)]
    public DateTime? Starttime9{get; set;}
    [Display(Name = "終了9")]
    [DataType(DataType.Time)]
    public DateTime? Endtime9{get; set;}
    [Display(Name = "内容9")]
    public string? Content9 {get; set;} = String.Empty;
    [Display(Name = "開始10")]
    [DataType(DataType.Time)]
    public DateTime? Starttime10{get; set;}
    [Display(Name = "終了10")]
    [DataType(DataType.Time)]
    public DateTime? Endtime10{get; set;}
    [Display(Name = "内容10")]
    public string? Content10 {get; set;} = String.Empty;
}