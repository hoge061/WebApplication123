using System.Collections.Generic;
namespace Wing1.Models;
public class DetailsTest {
    public DateTime? Starttime {get; set;}
    public DateTime? Endtime {get; set;}
    public string? Content {get; set;}
    public DetailsTest(DateTime? Starttime,DateTime? Endtime,string? Content){
        this.Starttime = Starttime;
        this.Endtime = Endtime;
        this.Content = Content;
    }

}