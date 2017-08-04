using OpenForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenForum.Controllers {
    public class HomeController : Controller {
        private MyDb db = new MyDb();// 数据库上下文类实例

        public ActionResult MainIndex() {
            List<Question> que_list = db.Questions.ToList();
            return PartialView(que_list);

            //Debug.....
            //String s = "";
            //foreach (var v in que_list) {
            //    s += "问题：" + v.content + "  时间：" + v.time + "\n<br />";
            //}
            //return Content(s);
        }

        public ActionResult SpecialColumn() {
            List<Question> que_list = db.Questions.ToList();
            return PartialView(que_list);
        }

        public ActionResult Modules() {
            return PartialView();
        }

        public ActionResult Users(string id="1") {
            var userID = Session["userID"];
            if (userID == null) {
                return PartialView();
            } else {
                // 第一个模型
                UserInfo ui = db.UserInfoes.Find(userID);
                // 第二个模型
                string questionID = ui.questionID;
                string[] questions = questionID.Split('+');
                List<Question> que_list = new List<Question>();
                for (int i = 0; i < questions.Length - 1; i++) {// 问题ID以"+"分割，最后一个符号为"+"
                    string v = questions[i];
                    int qid = int.Parse(v);
                    Question que = db.Questions.Find(qid);
                    que_list.Add(que);
                }
                return PartialView(Tuple.Create(ui, que_list));
            }
        }


        /// <summary>
        /// 登录与退出处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Error(int id) {
            switch (id) {
                case 1:
                    ViewBag.error = "提交异常...";
                    break;
                case 2:
                    ViewBag.error = "不存在此用户！";
                    break;
                case 3:
                    ViewBag.error = "密码错误！";
                    break;
                case 4:
                    ViewBag.error = "用户名已存在！";
                    break;
            }
            return PartialView();
        }

        public ActionResult Logout() {
            Session["userId"] = null;
            return Redirect("MainIndex");
        }


        /// <summary>
        /// 新提问
        /// </summary>
        /// <returns></returns>
        public ActionResult Ask() {
            Request.ContentEncoding = System.Text.Encoding.UTF8;    //请求编码
            Response.ContentEncoding = System.Text.Encoding.UTF8;   //响应编码

            if (Request.HttpMethod == "POST") {
                string _kind = Request["kind"];
                string _content = Request["content"];
                string _master = (string)Session["userID"];
                string _time = DateTime.Now.ToString();
                // 查询最新的问题号
                List<Question> ques = db.Questions.ToList();
                int _id = ques.Count + 1;

                // Step 1: 在问题表中插入新记录
                Question que = new Question() {
                    id = _id,
                    content = _content,
                    time = _time,
                    master = _master,
                    aID = null,
                    kind = _kind
                };
                db.Questions.Add(que);
                db.SaveChanges();

                // Step 2: 更新UserInfo表中questionID字段--------don't forget!!!
                UserInfo ui = db.UserInfoes.Find(_master);
                ui.questionID += _id.ToString() + "+";
                db.SaveChanges();
                
                return Redirect("MainIndex");
            } else {
                return Redirect(Url.Action("Error", new {
                    id = 1
                }));
            }
        }


        /// <summary>
        /// 每个提问的页面控制
        /// 参数id为问题编号(也即Question表的主键)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult QuePage(string id) {
            int qid = int.Parse(id);
            // 根据问题的id查找到对应的所有回答id
            Question que = db.Questions.Find(qid);
            string s = que.aID;
            if (s == null || s.Length == 0) {// 注意"回答回答数为0"的情况
                return PartialView(Tuple.Create(que, new List<Answer>()));
            }
            
            // 根据回答id找到所有回答
            string[] sp = s.Split('+');
            List<Answer> ans_list = new List<Answer>();
            for (int i = 0; i < sp.Length - 1; i++) {
                int aid = int.Parse(sp[i]);
                Answer ans = db.Answers.Find(aid);
                ans_list.Add(ans);
            }
            return PartialView(Tuple.Create(que, ans_list));
        }


        /// <summary>
        /// 回帖
        /// </summary>
        /// <returns></returns>
        public ActionResult Reply() {
            Request.ContentEncoding = System.Text.Encoding.UTF8;    //请求编码
            Response.ContentEncoding = System.Text.Encoding.UTF8;   //响应编码

            if (Request.HttpMethod == "POST") {
                int _qid = int.Parse(Request["qid"]);
                string _solver = (string)Session["userID"];
                string _content = Request["content"];
                string _solvertime = DateTime.Now.ToString();
                // 查询最新的问题号id
                List<Answer> ans_list = db.Answers.ToList();
                int _id = ans_list.Count + 1;

                // Step 1: 在Answer表中插入新记录
                Answer ans = new Answer() {
                    id = _id,
                    qid = _qid,
                    solver = _solver,
                    content = _content,
                    solvertime = _solvertime
                };
                db.Answers.Add(ans);
                db.SaveChanges();

                // Step 2: 更新Question表中问题的aID字段
                Question que = db.Questions.Find(_qid);
                que.aID += _id.ToString() + "+";
                db.SaveChanges();


                return Redirect(Url.Action("QuePage", new {
                    id = _qid
                }));// 返回对应的问题页面
            } else {
                return Redirect(Url.Action("Error", new {
                    id = 1
                }));
            }
        }
    }
}