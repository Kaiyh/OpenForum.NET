using Microsoft.Ajax.Utilities;
using OpenForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenForum.Controllers
{
    public class UserController : Controller
    {
        private MyDb db = new MyDb();// 数据库上下文类实例

        // GET: User
        public ActionResult Login()
        {
            Request.ContentEncoding = System.Text.Encoding.UTF8;    //请求编码
            Response.ContentEncoding = System.Text.Encoding.UTF8;   //响应编码

            if (Request.HttpMethod == "POST") {
                string username = Request["username"];
                string password = Request["password"];

                UserLogin ul = db.UserLogins.Find(username);
                if (ul == null) {// 没有此用户
                    return Redirect(Url.Action("Error", "Home", new {
                        id = 2
                    }));
                }
                if (ul.password == password) {
                    Session["userID"] = username;
                    return Redirect(Url.Action("MainIndex", "Home"));
                } else {
                    return Redirect(Url.Action("Error", "Home", new {
                        id = 3
                    }));
                }
            }
            ViewBag.error = "提交异常";
            return Redirect(Url.Action("Error", "Home", new {
                id = 1
            }));
        }

        public ActionResult Register() {
            Request.ContentEncoding = System.Text.Encoding.UTF8;    //请求编码
            Response.ContentEncoding = System.Text.Encoding.UTF8;   //响应编码

            if (Request.HttpMethod == "POST") {
                string username = Request["newuser"];
                string password = Request["newpsw"];
                string email = Request["email"];

                UserLogin ul = db.UserLogins.Find(username);// 查询
                if (ul != null) {// 已存在此用户(要求用户名唯一)
                    return Redirect(Url.Action("Error", "Home", new { id = 4}));
                }
                // 将数据插入UserLogin表...
                ul = new UserLogin {
                    username = username,
                    password = password,
                    email = email
                };
                try {
                    db.UserLogins.Add(ul);
                    db.SaveChanges();
                } catch (Exception ex) { }

                Session["userID"] = username;
                return Redirect(Url.Action("MainIndex", "Home"));
            }
            return Redirect(Url.Action("Error", "Home", new {
                id = 1
            }));
        }
    }
}