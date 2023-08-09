using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wing1.Models;
using Microsoft.AspNetCore.Http;
using Wing1.Test;
using System.Linq;
using ClosedXML.Excel;

namespace Wing1.Controllers;

public class LoginController : Controller
{

    private readonly MyContext _context;

        public LoginController(MyContext context)
        {
            _context = context;
        }
    public IActionResult Index()
    {
        var viewKintaiModel = HttpContext.Session.GetObject<ViewKintaiModel>("viewKintaiModel");
        if(viewKintaiModel != null){
            var array = HttpContext.Session.GetObject<Kintai[]>("array");
            var dim = HttpContext.Session.GetObject<int>("dim");
            var mon = HttpContext.Session.GetObject<int>("mon");
            var year = HttpContext.Session.GetObject<int>("year");
            ViewBag.array = array;
            ViewBag.dim = dim;
            ViewBag.mon = mon;
            ViewBag.year = year;
            return View("Kintai",viewKintaiModel);
        }
        Console.WriteLine("通った -- Index[GET] --");
        HttpContext.Session.SetObject("moni", 0);
        HttpContext.Session.SetObject("yeari", 0);
        return View();
    }

    [HttpPost]
        public async Task<IActionResult> Index(Users model,[Bind("Userid, Pass, Username, Kstarttime, Kendtime, adminflag")] Users users)
    {
        var user = HttpContext.Session.GetObject<Users>("user");
        Console.WriteLine("通った -- Index[POST] --");
        if(Request.Form["btn"] == "excelファイルのダウンロード"){
            return RedirectToActionPreserveMethod(actionName: "DownloadExcel");
        }else if(Request.Form["logout"] == "1"){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        string referer = Request.Headers["Referer"].ToString();
        
        
        if((string.IsNullOrEmpty(model.Userid) && referer != "http://localhost:5059/") 
        || ((Request.Form["backmon"] == "-1" || Request.Form["backmon"] == "1") && string.IsNullOrEmpty(model.Userid))
        || referer.Length >= 34
        || Request.Form["home"] == "1"){
            //Console.WriteLine("通った1_1");
            var key = HttpContext.Session.GetObject<Users>("key");
            model = key;
        }else{
            //Console.WriteLine("通った1_2");
            HttpContext.Session.SetObject("key", model);
        }
        //Console.WriteLine("通った1_"+user);
        try{
            user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == model.Userid && m.Pass == model.Pass);
        }catch(Exception e){
            return RedirectToAction("Index");
        }
        
        //Console.WriteLine("通った1_"+user);
        HttpContext.Session.SetObject("user", user);//ユーザーのセッション　エクセル用

        if(user != null)
        {
            //Console.WriteLine("通った2");
            if(!string.IsNullOrEmpty(Request.Form["backmon"])){
                var moni1 = HttpContext.Session.GetObject<int>("moni");
                HttpContext.Session.SetObject("moni", moni1 + int.Parse(Request.Form["backmon"]));
                //moni = int.Parse(Request.Form["backmon"]);
            }
            var yeari = HttpContext.Session.GetObject<int>("yeari");
            var moni2 = HttpContext.Session.GetObject<int>("moni");
            var year = DateTime.Now.Year + yeari;
            var mon = DateTime.Now.Month + moni2;
            if(mon == 0){
                yeari -= 1;
                year = DateTime.Now.Year + yeari;
                HttpContext.Session.SetObject("yeari", yeari);
                while(DateTime.Now.Month + moni2 != 12){
                    moni2++;
                }
                mon = DateTime.Now.Month + moni2;
                HttpContext.Session.SetObject("moni",moni2);
            }else if(mon == 13){
                yeari += 1;
                year = DateTime.Now.Year + yeari;
                HttpContext.Session.SetObject("yeari", yeari);
                while(DateTime.Now.Month + moni2 != 1){
                    moni2--;
                }
                mon = DateTime.Now.Month + moni2;
                HttpContext.Session.SetObject("moni",moni2);
            }

            HttpContext.Session.SetObject("year",year);
            HttpContext.Session.SetObject("mon",mon);

            int dim = DateTime.DaysInMonth(year,mon);

            ViewBag.dim = dim;
            ViewBag.mon = mon;
            ViewBag.year = year;
            HttpContext.Session.SetObject("dim", dim);
            HttpContext.Session.SetObject("mon", mon);
            HttpContext.Session.SetObject("year", year);

            DateTime dts = new DateTime(year, mon,1);
            DateTime dte = new DateTime(year, mon,dim);
            var kintaidate = await _context.Kintai.Where(m => m.Userid == model.Userid && m.Date >= dts && m.Date <= dte).ToListAsync();

            Kintai[] array = new Kintai[dim];//日数分の配列
            for(int i=0; i<dim;i++){
                Kintai test = new Kintai();
                DateTime dt = new DateTime(year, mon, i+1);
                test.Date = dt;
                array[i] = test;
            }
            
            foreach(var row in kintaidate){
                int j =  int.Parse(row.Date.ToString().Substring(8,2));
                array[j-1] = row;
            }

            //var UsersList = user;

            var viewKintaiModel = new ViewKintaiModel
            {
                Users = user,
            };
            ViewBag.array = array;
            HttpContext.Session.SetObject("array", array);
            HttpContext.Session.SetObject("viewKintaiModel", viewKintaiModel);
            return View("Kintai",viewKintaiModel);
        }else
        {
            this.ModelState.AddModelError("Error1", "指定されたユーザー名またはパスワードが正しくありません。");
            return this.View(model);
        }
        
    }
    public async Task<IActionResult> Edit(string id,int date,Users model){   
        string referer = Request.Headers["Referer"].ToString();
        if(referer != "http://localhost:5059/"){
            return View("Error");
        }
        var moni1 = HttpContext.Session.GetObject<int>("moni");
        DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month+moni1, date);
        ViewBag.id = id;
        ViewBag.date = date2;
        HttpContext.Session.SetObject("date2", date2);

        var kintai = await _context.Kintai.FirstOrDefaultAsync(m => m.Userid == id && m.Date == date2);
        var viewKintaiModel = new ViewKintaiModel
            {
                Users = HttpContext.Session.GetObject<Users>("user"),
                Kintai = kintai
            };
        return View(viewKintaiModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([Bind("Userid,Date,Workstyle,Starttime,Endtime,Break1start"+
    ",Break1end,Break2start,Break2end,Break3start,Break3end,Break4start,Break4end,biko")] Kintai kintai){

        try
        {
            if(Request.Form["btn"] == "更新"){
                    if(KintaiExists(kintai.Userid,kintai.Date)){//レコードが存在するか
                        _context.Update(kintai);
                    }else{
                        _context.Add(kintai);
                    }
            }else if(Request.Form["btn"] == "削除"){
                if(KintaiExists(kintai.Userid,kintai.Date)){
                    _context.Kintai.Remove(kintai);
                }
                var details = await _context.Details.FirstOrDefaultAsync(m => m.Userid == kintai.Userid && m.Date == kintai.Date);
                if(details !=null) _context.Details.Remove(details);
                await _context.SaveChangesAsync();
                return RedirectToActionPreserveMethod(actionName: "Index");
            }else if(Request.Form["btn"] == "1日の作業内容詳細"){
                HttpContext.Session.SetObject("euser", kintai);
                //return RedirectToActionPreserveMethod(actionName: "Detail");
                return RedirectToAction(nameof(Detail));
            }

            
            await _context.SaveChangesAsync();
        }catch (DbUpdateConcurrencyException)
        {
            if (!KintaiExists(kintai.Userid, kintai.Date))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        ViewBag.id = kintai.Userid;
        ViewBag.date = kintai.Date;
        return RedirectToActionPreserveMethod(actionName: "Index");
    }
    public async Task<IActionResult> Detail(){
        string referer = Request.Headers["Referer"].ToString();
        Console.WriteLine("確認:"+referer.Substring(0, 36));
        try{
            if(string.IsNullOrEmpty(referer) ||
            (referer.Substring(0,33) != "http://localhost:5059/Login/Edit/" && (referer.Substring(0,35) != "http://localhost:5059/Login/DetailEdit")))
            {
                return View("Error");
            }
        }catch(ArgumentOutOfRangeException e){
            if(referer != "http://localhost:5059/Login/Detail"){
                return View("Error");
            }
        }


        Kintai euser;
        Details DetailsModel;
        try{
            euser = HttpContext.Session.GetObject<Kintai>("euser");
            DetailsModel = await _context.Details.FirstOrDefaultAsync(m => m.Userid == euser.Userid && m.Date == euser.Date);
            if(DetailsModel == null){
                DetailsModel = new Details();
                DetailsModel.Userid = euser.Userid;
                DetailsModel.Date = euser.Date;
            }

            var date2 = HttpContext.Session.GetObject<DateTime>("date2");
            ViewBag.date = date2;
        }catch(InvalidOperationException e){
            return View("Error");
        }

        return View(DetailsModel);
    }
    [HttpPost]
    public async Task<IActionResult> Detail([Bind("Userid,Date,Starttime1,Endtime1,Content1,Starttime2,Endtime2,Content2,Starttime3,Endtime3,Content3,Starttime4,Endtime4,Content4,Starttime5,Endtime5,Content5,Starttime6,Endtime6,Content6,Starttime7,Endtime7,Content7,Starttime8,Endtime8,Content8,Starttime9,Endtime9,Content9,Starttime10,Endtime10,Content10")] Details detail){
        var date2 = HttpContext.Session.GetObject<DateTime>("date2");
        ViewBag.date = date2;
        if(Request.Form["btn"] == "追加"){   
            if(int.Parse(Request.Form["num"]) > 10){
                ViewBag.message1 = "これ以上追加できません。";
                return this.View(detail);
            }         
            return RedirectToAction(nameof(DetailEdit),new { num = Request.Form["num"] });
        }else if(Request.Form["btn"] == "編集"){
            if(string.IsNullOrEmpty(Request.Form["check"])){
                ViewBag.message1 = "項目を選択してください";
                return this.View(detail);
            }
            return RedirectToAction(nameof(DetailEdit),new { num = Request.Form["check"] });
        }else if(Request.Form["btn"] == "削除"){
            if(string.IsNullOrEmpty(Request.Form["check"])){
                ViewBag.message1 = "項目を選択してください";
                return this.View(detail);
            }
            var test_model = await _context.Details.FirstOrDefaultAsync(m => m.Userid == detail.Userid && m.Date == detail.Date);
            var num = int.Parse(Request.Form["check"]);
            var list1 = new ArrayList();
            list1.Add(new DetailsTest(test_model.Starttime1,test_model.Endtime1,test_model.Content1));
            list1.Add(new DetailsTest(test_model.Starttime2,test_model.Endtime2,test_model.Content2));
            list1.Add(new DetailsTest(test_model.Starttime3,test_model.Endtime3,test_model.Content3));
            list1.Add(new DetailsTest(test_model.Starttime4,test_model.Endtime4,test_model.Content4));
            list1.Add(new DetailsTest(test_model.Starttime5,test_model.Endtime5,test_model.Content5));
            list1.Add(new DetailsTest(test_model.Starttime6,test_model.Endtime6,test_model.Content6));
            list1.Add(new DetailsTest(test_model.Starttime7,test_model.Endtime7,test_model.Content7));
            list1.Add(new DetailsTest(test_model.Starttime8,test_model.Endtime8,test_model.Content8));
            list1.Add(new DetailsTest(test_model.Starttime9,test_model.Endtime9,test_model.Content9));
            list1.Add(new DetailsTest(test_model.Starttime10,test_model.Endtime10,test_model.Content10));


            list1.RemoveAt(num-1);
            list1.Add(new DetailsTest(null,null,null));
            
            test_model.Starttime1 = ((DetailsTest)list1[0]).Starttime;
            test_model.Endtime1 = ((DetailsTest)list1[0]).Endtime;
            test_model.Content1 = ((DetailsTest)list1[0]).Content;
            test_model.Starttime2 = ((DetailsTest)list1[1]).Starttime;
            test_model.Endtime2 = ((DetailsTest)list1[1]).Endtime;
            test_model.Content2 = ((DetailsTest)list1[1]).Content;
            test_model.Starttime3 = ((DetailsTest)list1[2]).Starttime;
            test_model.Endtime3 = ((DetailsTest)list1[2]).Endtime;
            test_model.Content3 = ((DetailsTest)list1[2]).Content;
            test_model.Starttime4 = ((DetailsTest)list1[3]).Starttime;
            test_model.Endtime4 = ((DetailsTest)list1[3]).Endtime;
            test_model.Content4 = ((DetailsTest)list1[3]).Content;
            test_model.Starttime5 = ((DetailsTest)list1[4]).Starttime;
            test_model.Endtime5 = ((DetailsTest)list1[4]).Endtime;
            test_model.Content5 = ((DetailsTest)list1[4]).Content;
            test_model.Starttime6 = ((DetailsTest)list1[5]).Starttime;
            test_model.Endtime6 = ((DetailsTest)list1[5]).Endtime;
            test_model.Content6 = ((DetailsTest)list1[5]).Content;
            test_model.Starttime7 = ((DetailsTest)list1[6]).Starttime;
            test_model.Endtime7 = ((DetailsTest)list1[6]).Endtime;
            test_model.Content7 = ((DetailsTest)list1[6]).Content;
            test_model.Starttime8 = ((DetailsTest)list1[7]).Starttime;
            test_model.Endtime8 = ((DetailsTest)list1[7]).Endtime;
            test_model.Content8 = ((DetailsTest)list1[7]).Content;
            test_model.Starttime9 = ((DetailsTest)list1[8]).Starttime;
            test_model.Endtime9 = ((DetailsTest)list1[8]).Endtime;
            test_model.Content9 = ((DetailsTest)list1[8]).Content;
            test_model.Starttime10 = ((DetailsTest)list1[9]).Starttime;
            test_model.Endtime10 = ((DetailsTest)list1[9]).Endtime;
            test_model.Content10 = ((DetailsTest)list1[9]).Content;
            _context.Update(test_model);
            await _context.SaveChangesAsync();
            //return RedirectToActionPreserveMethod(actionName: "Index");
            return RedirectToAction(nameof(Detail));
        }
        return RedirectToActionPreserveMethod(actionName: "Index");
    }
    public async Task<IActionResult> DetailEdit(){
        var date2 = HttpContext.Session.GetObject<DateTime>("date2");
        ViewBag.date = date2;
        string num = HttpContext.Request.Query["num"]; //クエリ文字から値を取得
        ViewBag.num = num;
        var euser = HttpContext.Session.GetObject<Kintai>("euser");
        Details DetailsModel;

        try{
            
            DetailsModel = await _context.Details.FirstOrDefaultAsync(m => m.Userid == euser.Userid && m.Date == euser.Date);
            if(DetailsModel == null){
                DetailsModel = new Details();
                DetailsModel.Userid = euser.Userid;
                DetailsModel.Date = euser.Date;
            }

        }catch(InvalidOperationException e){
            return View("Error");
        }

        return View(DetailsModel);
    }
    [HttpPost]
    public async Task<IActionResult> DetailEdit([Bind("Userid,Date,Starttime1,Endtime1,Content1,Starttime2,Endtime2,Content2,Starttime3,Endtime3,Content3,Starttime4,Endtime4,Content4,Starttime5,Endtime5,Content5,Starttime6,Endtime6,Content6,Starttime7,Endtime7,Content7,Starttime8,Endtime8,Content8,Starttime9,Endtime9,Content9,Starttime10,Endtime10,Content10")] Details detail){
        var date2 = HttpContext.Session.GetObject<DateTime>("date2");
        ViewBag.date = date2;
        
        var DetailsModel = await _context.Details.FirstOrDefaultAsync(m => m.Userid == detail.Userid && m.Date == detail.Date);
        if(DetailsModel == null){
            DetailsModel = new Details();
            DetailsModel.Userid = detail.Userid;
            DetailsModel.Date = detail.Date;
        }
        var num = int.Parse(Request.Form["num"]);
        ViewBag.num = Request.Form["num"];

        if(num == 1){
            if(detail.Starttime1 == null || detail.Endtime1 == null || detail.Content1 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime1 = detail.Starttime1;
            DetailsModel.Endtime1 = detail.Endtime1;
            DetailsModel.Content1 = detail.Content1;
        }else if(num == 2){
            if(detail.Starttime2 == null || detail.Endtime2 == null || detail.Content2 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime2 = detail.Starttime2;
            DetailsModel.Endtime2 = detail.Endtime2;
            DetailsModel.Content2 = detail.Content2;
        }else if(num == 3){
            if(detail.Starttime3 == null || detail.Endtime3 == null || detail.Content3 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime3 = detail.Starttime3;
            DetailsModel.Endtime3 = detail.Endtime3;
            DetailsModel.Content3 = detail.Content3;
        }else if(num == 4){
            if(detail.Starttime4 == null || detail.Endtime4 == null || detail.Content4 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime4 = detail.Starttime4;
            DetailsModel.Endtime4 = detail.Endtime4;
            DetailsModel.Content4 = detail.Content4;
        }else if(num == 5){
            if(detail.Starttime5== null || detail.Endtime5 == null || detail.Content5 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime5 = detail.Starttime5;
            DetailsModel.Endtime5 = detail.Endtime5;
            DetailsModel.Content5 = detail.Content5;
        }else if(num == 6){
            if(detail.Starttime6 == null || detail.Endtime6 == null || detail.Content6 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime6 = detail.Starttime6;
            DetailsModel.Endtime6 = detail.Endtime6;
            DetailsModel.Content6 = detail.Content6;
        }else if(num == 7){
            if(detail.Starttime7 == null || detail.Endtime7 == null || detail.Content7 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime7 = detail.Starttime7;
            DetailsModel.Endtime7 = detail.Endtime7;
            DetailsModel.Content7 = detail.Content7;
        }else if(num == 8){
            if(detail.Starttime8 == null || detail.Endtime8 == null || detail.Content8 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime8 = detail.Starttime8;
            DetailsModel.Endtime8 = detail.Endtime8;
            DetailsModel.Content8 = detail.Content8;
        }else if(num == 9){
            if(detail.Starttime9 == null || detail.Endtime9 == null || detail.Content9 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime9 = detail.Starttime9;
            DetailsModel.Endtime9 = detail.Endtime9;
            DetailsModel.Content9 = detail.Content9;
        }else if(num == 10){
            if(detail.Starttime10 == null || detail.Endtime10 == null || detail.Content10 == null){
                ViewBag.message1 = "未入力の項目があります。";
                return this.View(detail);
            }
            DetailsModel.Starttime10 = detail.Starttime10;
            DetailsModel.Endtime10 = detail.Endtime10;
            DetailsModel.Content10 = detail.Content10;
        }
        if(DetailsExists(detail.Userid,detail.Date)){
            _context.Update(DetailsModel);
        }else{
            _context.Add(DetailsModel);
        }
        await _context.SaveChangesAsync();
        //return RedirectToActionPreserveMethod(actionName: "Index");
        return RedirectToAction(nameof(Detail));
    }

    public async Task<IActionResult> Admintop()
    {
        string referer = Request.Headers["Referer"].ToString();
        if(referer != "http://localhost:5059/" && referer != "http://localhost:5059/Login/Adduser"
        && referer != "http://localhost:5059/Login/Admintop" && referer != "http://localhost:5059/Login/AuthManage"){
            return View("Error");
        }

        var alldata = await _context.Users.ToListAsync();
        //Console.WriteLine(alldata.GetType());
        
        int yearAd = HttpContext.Session.GetObject<int>("yearAd");
        int monthAd = HttpContext.Session.GetObject<int>("monthAd");
        Console.WriteLine("tesuto"+yearAd);
        Console.WriteLine("tesuto"+monthAd);
        if(yearAd == 0){
            yearAd = DateTime.Now.Year;
            monthAd = DateTime.Now.Month;
            HttpContext.Session.SetObject("yearAd", yearAd);
            HttpContext.Session.SetObject("monthAd", monthAd);
        }
        ViewBag.year = yearAd;
        ViewBag.mon = monthAd;

        int dim = DateTime.DaysInMonth(yearAd,monthAd);
        DateTime dts = new DateTime(yearAd, monthAd,1);
        DateTime dte = new DateTime(yearAd, monthAd,dim);
        Dictionary<string, List<double>> totaltimeList = new Dictionary<string, List<double>>();

        foreach(var data in alldata){
            TimeSpan totalKado = TimeSpan.Parse("0:0:0");;
            TimeSpan totalZangyo = TimeSpan.Parse("0:0:0");;

            var kintai = await _context.Kintai.Where(m => m.Userid == data.Userid && m.Date >= dts && m.Date <= dte).ToListAsync();
            foreach(var data2 in kintai){
                var time = data2.Endtime - data2.Starttime;
                if(data2.Break1end != null && data2.Break1start != null){
                    var resttime = ((DateTime)data2.Break1end).TimeOfDay - ((DateTime)data2.Break1start).TimeOfDay;
                    time = time - resttime;
                }
                if(data2.Break2end != null && data2.Break2start != null){
                    var resttime = ((DateTime)data2.Break2end).TimeOfDay - ((DateTime)data2.Break2start).TimeOfDay;
                    time = time - resttime;
                }
                if(data2.Break3end != null && data2.Break3start != null){
                    var resttime = ((DateTime)data2.Break3end).TimeOfDay - ((DateTime)data2.Break3start).TimeOfDay;
                    time = time - resttime;
                }
                if(data2.Break4end != null && data2.Break4start != null){
                    var resttime = ((DateTime)data2.Break4end).TimeOfDay - ((DateTime)data2.Break4start).TimeOfDay;
                    time = time - resttime;
                }

                if(time != null){
                    totalKado += (TimeSpan)time;
                    TimeSpan span = TimeSpan.Parse("0:0:0");
                    if(time - data.Jitudo >= span){
                        totalZangyo += (TimeSpan)(time - data.Jitudo);
                    }
                }
                
            }
            totaltimeList.Add(data.Userid, new List<double> { Math.Floor(totalKado.TotalHours*100)/100, Math.Floor(totalZangyo.TotalHours*100)/100 });
        }
        ViewBag.timeList = totaltimeList;
        return View(alldata);
    }

    [HttpPost]
        public async Task<IActionResult> Admintop(int kari)
    {
        int num = int.Parse(Request.Form["backmon"]);
        int yearAd = HttpContext.Session.GetObject<int>("yearAd");
        int monthAd = HttpContext.Session.GetObject<int>("monthAd");
        if(monthAd == 1 && num == -1){
            yearAd--;
            monthAd = 12;
        }else if(monthAd == 12 && num == 1){
            yearAd++;
            monthAd = 1;
        }else{
            monthAd+= num;
        }
        HttpContext.Session.SetObject("yearAd", yearAd);
        HttpContext.Session.SetObject("monthAd", monthAd);
        return RedirectToAction("Admintop");
    }
        public IActionResult Adduser()
    {
        string referer = Request.Headers["Referer"].ToString();
        if(referer != "http://localhost:5059/Login/Admintop" && referer != "http://localhost:5059/Login/Adduser"){
            return View("Error");
        }
        if(referer == "http://localhost:5059/Login/Adduser"){
            ViewBag.message1 = "登録完了しました。";
        }
        return View();
    }

    [HttpPost]
        public async Task<IActionResult> Adduser(Users user)
    {
            if(user.Userid == null || user.Pass == null || user.Username == null
            || user.Kstarttime == null || user.Kendtime == null){
                return this.View(user);
            }
            try{
                _context.Add(user);
                await _context.SaveChangesAsync();
            }catch(DbUpdateException e){
                Console.WriteLine("重複エラー");
                this.ModelState.AddModelError(string.Empty, "ユーザーIDが重複しています。");
                return this.View(user);
            }

            return RedirectToAction(nameof(Adduser));
    }
    public async Task<IActionResult> AuthManage(){
        var alldata = await _context.Users.ToListAsync();
        return View(alldata);
    }
    [HttpPost]
    public async Task<IActionResult> AuthManage(int kari){
        var alldata = await _context.Users.ToListAsync();
        var check = Request.Form["checkad"];
        Console.WriteLine(Request.Form["checkad"]);
        return View(alldata);
    }

    public async Task<IActionResult> DownloadExcel(){ //エクセル出力用
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("新しいシート名");
        var user = HttpContext.Session.GetObject<Users>("user");
        var year = HttpContext.Session.GetObject<int>("year");
        var mon = HttpContext.Session.GetObject<int>("mon");

        var cell = ws.Cell(1, 1);
        cell.Value = "氏名：";
        cell.Style.Font.FontSize = 16;
        cell = ws.Cell(1, 2);
        cell.Value = user.Username;
        cell.Style.Font.FontSize = 16;
        cell = ws.Cell(1, 4);
        cell.Style.Font.FontSize = 16;
        cell.Value = year + "年" + mon + "月";

        cell = ws.Cell(2, 1);
        cell.Value = "日付";
        cell = ws.Cell(2, 2);
        cell.Value = "曜日";
        cell = ws.Cell(2, 3);
        cell.Value = "勤務形態";
        cell = ws.Cell(2, 4);
        cell.Value = "開始";
        cell = ws.Cell(2, 5);
        cell.Value = "終了";
        cell = ws.Cell(2, 6);
        cell.Value = "稼働時間(時間:分)";
        cell = ws.Cell(2, 7);
        cell.Value = "残業時間(時間:分)";
        cell = ws.Cell(2, 8);
        cell.Value = "備考";
        
        ws.Style.Font.FontName = "游ゴシック";
        ws.Column("A").Width = 10;
        ws.Column("B").Width = 5;
        ws.Column("F").Width = 20;
        ws.Column("G").Width = 20;
        ws.Column("H").Width = 20;

        int dim = DateTime.DaysInMonth(year,mon);
        DateTime dts = new DateTime(year, mon,1);
        DateTime dte = new DateTime(year, mon,dim);

        

        var kintaidate = await _context.Kintai.Where(m => m.Userid == user.Userid && m.Date >= dts && m.Date <= dte).ToListAsync();

        Kintai[] array = new Kintai[dim];//日数分の配列
        for(int i=0; i<dim;i++){
            Kintai test = new Kintai();
            DateTime dt = new DateTime(year, mon, i+1);
            test.Date = dt;
            array[i] = test;
        }
        
        foreach(var row in kintaidate){
            int j =  int.Parse(row.Date.ToString().Substring(8,2));
            array[j-1] = row;
        }
        
        var k = 3;
        foreach(var data in array){
            cell = ws.Cell(k, 1);
            cell.Value = data.Date.Day;
            cell = ws.Cell(k, 2);

            cell.Value = data.Date.ToString("ddd");
            if(data.Date.ToString("ddd") == "土"){
                ws.Range(k,1,k,8).Style.Fill.SetBackgroundColor(XLColor.LightBlue);
            }else if(data.Date.ToString("ddd") == "日"){
                ws.Range(k,1,k,8).Style.Fill.SetBackgroundColor(XLColor.Pink);
            }
            cell = ws.Cell(k, 3);
            cell.Value = data.Workstyle;
            cell = ws.Cell(k, 4);
            if(data.Starttime != null){
                cell.Value = ((DateTime)data.Starttime).TimeOfDay.ToString().Substring(0,5);
            }
            cell = ws.Cell(k, 5);
            if(data.Endtime != null){
                cell.Value = ((DateTime)data.Endtime).TimeOfDay.ToString().Substring(0,5);
            }

            TimeSpan span = TimeSpan.Parse("0:0:0");

            /*稼働時間*/
            cell = ws.Cell(k, 6);
            TimeSpan kado = TimeSpan.Parse("0:0:0");
            if(data.Starttime != null && data.Endtime != null){
                kado = ((DateTime)data.Endtime).TimeOfDay - ((DateTime)data.Starttime).TimeOfDay;
                if(data.Break1end != null && data.Break1start != null){
                    var resttime = ((DateTime)data.Break1end).TimeOfDay - ((DateTime)data.Break1start).TimeOfDay;
                    kado = kado - resttime;
                }
                if(data.Break2end != null && data.Break2start != null){
                    var resttime = ((DateTime)data.Break2end).TimeOfDay - ((DateTime)data.Break2start).TimeOfDay;
                    kado = kado - resttime;
                }
                if(data.Break3end != null && data.Break3start != null){
                    var resttime = ((DateTime)data.Break3end).TimeOfDay - ((DateTime)data.Break3start).TimeOfDay;
                    kado = kado - resttime;
                }
                if(data.Break4end != null && data.Break4start != null){
                    var resttime = ((DateTime)data.Break4end).TimeOfDay - ((DateTime)data.Break4start).TimeOfDay;
                    kado = kado - resttime;
                }
                
                if (kado < span)//稼働時間がマイナスだった場合
                {
                    kado = TimeSpan.Parse("1") + kado;
                }
                cell.Value = kado;
                cell.Style.DateFormat.Format = "[h]:mm";
            }
            /*稼働時間ここまで*/
            /*残業時間*/
            cell = ws.Cell(k, 7);

            
            if(kado - user.Jitudo > span){
                cell.Value = kado - user.Jitudo;
                cell.Style.DateFormat.Format = "[h]:mm";
            }
            /*残業時間ここまで*/
            cell = ws.Cell(k, 8);
            cell.Value = data.biko;
            k++;
        }

        for(int i=2; i<k; i++){
            for(int j=1; j<=8; j++){
                cell = ws.Cell(i,j);
                if(i == 2){
                    cell.Style.Fill.SetBackgroundColor(XLColor.LightGray);
                }
                if(i == 2){
                    cell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                }else{
                    cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                }
                
                if(i == k-1){
                    cell.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                }else{
                    cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                
                if(j == 1){
                    cell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                }else{
                    cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                }
                
                if(j == 2 || j == 8){
                    cell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                }else{
                    cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                }
                

            }
        }

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        //ASP.NET MVCの場合
        string moji = user.Username + "_" + year + "年" + mon + "月" + "勤務表.xlsx";
        return File(ms.ToArray(), "application/msexcel", moji);
    }

    public async Task<IActionResult> UserSetting(){
        string referer = Request.Headers["Referer"].ToString();
        if(referer != "http://localhost:5059/"){
            return View("Error");
        }
        var user = HttpContext.Session.GetObject<Users>("user");
        if(user.Jitudo != null){
            ViewBag.hour = ((TimeSpan)(user.Jitudo)).Hours;
            ViewBag.minute = ((TimeSpan)(user.Jitudo)).Minutes;
        }
        return View(user);      
    }  
[HttpPost]
    public async Task<IActionResult> UserSetting([Bind("Kstarttime, Kendtime, Jitudo")] Users model){
        var user = HttpContext.Session.GetObject<Users>("user");
        if(Request.Form["btn"] == "変更"){
            user.Kstarttime = model.Kstarttime;
            user.Kendtime = model.Kendtime;
            try{
            TimeSpan time = TimeSpan.Parse(Request.Form["hour"] + ":" + Request.Form["minute"]);
            user.Jitudo = time;
            }catch(FormatException e){
                if(user.Jitudo != null){
                    ViewBag.hour = ((TimeSpan)(user.Jitudo)).Hours;
                    ViewBag.minute = ((TimeSpan)(user.Jitudo)).Minutes;
                }
                return View(user);
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        ViewBag.hour = ((TimeSpan)(user.Jitudo)).Hours;
        ViewBag.minute = ((TimeSpan)(user.Jitudo)).Minutes;
        return View(user);      
    }  


    

    
    private bool KintaiExists(String id,DateTime date)
    {
        return (_context.Kintai?.Any(e => e.Userid == id && e.Date == date)).GetValueOrDefault();
    }
    private bool DetailsExists(String id,DateTime date)
    {
        return (_context.Details?.Any(e => e.Userid == id && e.Date == date)).GetValueOrDefault();
    }

}