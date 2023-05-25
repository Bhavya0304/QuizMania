using QuizMania.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizMania.DBContext;

namespace QuizMania.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        // GET: Quiz
        public ActionResult ShowAll()
        {
            List<Quiz> data;
            using(BJBhavyaJoshiNewEntities _db = new BJBhavyaJoshiNewEntities())
            {
                data = _db.Quizs.Include("Questions").ToList();
            }
            return View(data);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                using (BJBhavyaJoshiNewEntities _db = new BJBhavyaJoshiNewEntities())
                {
                    quiz.UserId = new UserMemberShip().GetId(User.Identity.Name);
                    _db.Quizs.Add(quiz);
                    _db.SaveChanges();
                    TempData["QuizId"] = quiz.Id;
                    return RedirectToAction("AddQuestions");
                }
            }
            return View(quiz);
        }

        public ActionResult AddQuestions()
        {
            if(TempData["QuizId"] != null)
            {
                ViewBag.QuizId = TempData["QuizId"];
                TempData.Keep();
                return View();
            }
            else
            {
                return RedirectToAction("Add");
            }
        }
        [HttpPost]
        public ActionResult AddQuestions(Models.Question data,string Save)
        {

            try
            {
                TempData.Keep();
                ViewBag.QuizId = TempData["QuizId"];
                using (BJBhavyaJoshiNewEntities _db = new BJBhavyaJoshiNewEntities())
                {
                    var question = new DBContext.Question
                    {
                        Question1 = data.QuestionName,
                        Mark = 1,
                        QuizId = data.QuizId,

                    };
                    _db.Questions.Add(question);
                    _db.SaveChanges();
                    foreach (var opt in data.Options)
                    {
                        var options = new DBContext.Option
                        {
                            Option1 = opt.OptionName,
                            QuestionId = question.Id
                        };
                        _db.Options.Add(options);
                        _db.SaveChanges();
                        if (data.Answer == opt.OptionName)
                        {
                            question.AnswerId = options.Id;
                            _db.Entry(question).State = System.Data.Entity.EntityState.Modified;
                            _db.SaveChanges();
                        }

                    }
                }
                ModelState.Clear();
                if(Save != "Save And Next")
                {
                    return RedirectToAction("ShowAll");
                }
                return View();
            }
            catch (Exception)
            {
                return View(data); 
            }
        }

        public ActionResult Play(int id)
        {
            using (BJBhavyaJoshiNewEntities db = new BJBhavyaJoshiNewEntities())
            {
                int userid = new UserMemberShip().GetId(User.Identity.Name);
                if(db.Results.Any(x=>x.QuizId == id && x.UserId == userid))
                {
                    return RedirectToAction("ShowAll");
                }
                Quiz quizData = db.Quizs.Include("Questions").Where(x=>x.Id == id).FirstOrDefault();
                int index = 0;
                foreach(var ques in quizData.Questions)
                {
                    quizData.Questions.ElementAt(index).Options = db.Options.Where(x => x.QuestionId == ques.Id).ToList();
                    index++;
                }
                return View(quizData);
            }
        }

        [HttpPost]
        public ActionResult Play(QuizResponse response)
        {

            try
            {
                int result = 0;
                int total = 0;
                UserMemberShip user = new UserMemberShip();
                string username = User.Identity.Name;
                using (BJBhavyaJoshiNewEntities db = new BJBhavyaJoshiNewEntities())
                {
                    Quiz quizData = db.Quizs.Include("Questions").Where(x => x.Id == response.QuizId).FirstOrDefault();
                    foreach (var ques in response.ResponseLists)
                    {
                        DBContext.Question dataQues = quizData.Questions.Where(x => x.Id == ques.quesId).FirstOrDefault();
                        total += Convert.ToInt32(dataQues.Mark);
                        result += dataQues.AnswerId == Convert.ToInt32(ques.optionId) ? Convert.ToInt32(dataQues.Mark) : 0;
                    }
                    db.Results.Add(new Result
                    {
                        UserId = user.GetId(username),
                        QuizId = response.QuizId,
                        TotalMarks = result,
                        OutOf = total
                    });
                    db.SaveChanges();
                    return Json("/Quiz/ShowResult/" + response.QuizId);
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Play",new { id = response.QuizId });
               
            }
        }

        public ActionResult ShowResult(int id)
        {
            int userid = new UserMemberShip().GetId(User.Identity.Name);
            User usr = new UserMemberShip().getUser(User.Identity.Name);
            using (BJBhavyaJoshiNewEntities db = new BJBhavyaJoshiNewEntities())
            {
                Quiz qz = db.Quizs.Where(x => x.Id == id).FirstOrDefault();
                Result rs = db.Results.Where(x => x.QuizId == id && x.UserId == userid).FirstOrDefault();
                Report r = new Report() {
                    user = usr,
                    quiz = qz,
                    res = rs
                };
            return View(r);
            }
        }

        public ActionResult ShowAllResult()
        {
            int userId = new UserMemberShip().GetId(User.Identity.Name);
            using (BJBhavyaJoshiNewEntities db = new BJBhavyaJoshiNewEntities())
            {
                List<Result> res = db.Results.Include("Quiz").Where(x => x.UserId == userId).ToList();
                return View(res);

            }

        }
    }
}