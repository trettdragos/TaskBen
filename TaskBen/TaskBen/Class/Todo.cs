﻿using MetroFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskBen.Forms;

namespace TaskBen.Class
{
    public class Todo
    {
        private int _id;
        private string _date;
        private string _dateHH;
        private string _dateMM;
        private string _remHH;
        private string _remMM;
        private string _repeat;
        private string _description;
        private string _title;
        private int _checked;
        private int _idGroup;
        private int _idUser;

        public void clear()
        {
            _id = 0;
            _date = "";
            _dateHH = "";
            _dateMM = "";
            _remHH = "";
             _remMM = "";
            _repeat = "";
            _description = "";
            _title = "";
            _checked = 0;
        }

        public Todo()
        {
            _idUser = 0;
            _idGroup = 0;
        }

        public int IdGroup
        {
            set { _idGroup = value; }
            get { return _idGroup; }
        }

        public int IdUser
        {
            set { _idUser = value; }
            get { return _idUser; }
        }

        public string Date
        {
            set { _date = value; }
            get { return _date; }
        }

        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }

        public int Checked
        {
            set { _checked = value; }
            get { return _checked; }
        }
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        public string DateHours
        {
            set { _dateHH = value; }
            get { return _dateHH; }
        }
        public string DateMinutes
        {
            set { _dateMM = value; }
            get { return _dateMM; }
        }
        public string ReminderHours
        {
            set { _remHH = value; }
            get { return _remHH; }
        }
        public string ReminderMinutes
        {
            set { _remMM = value; }
            get { return _remMM; }
        }
        public string Schedule
        {
            set { _repeat = value; }
            get { return _repeat; }
        }

        public void remove_web()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("id", _id.ToString());
            json.Add("idUser", Settings.user.ID.ToString());
            json.Add("action", "remove_task");
            WebServer.post(JsonConvert.SerializeObject(json));
        }

        public void task_get_list()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("idUser", Settings.user.ID.ToString());
            json.Add("action", "get_tasks");

            string list_todo = WebServer.post_get(JsonConvert.SerializeObject(json));

            if (list_todo != "")
                try
                {
                    Settings.taskList = JsonConvert.DeserializeObject<List<Todo>>(list_todo);
                }
                catch
                {
                    MessageBox.Show(list_todo);
                }
        }

        

        public void task_get_list(string x)
        {
            if (x == "")
            {
                task_get_list();
            }
            else
            {
                Dictionary<string, string> json = new Dictionary<string, string>();
                json.Add("words", x);
                json.Add("action", "get_tasks_words");

                string list_todo = WebServer.post_get(JsonConvert.SerializeObject(json));

                if (list_todo != "")
                    try
                    {
                        Settings.taskList = JsonConvert.DeserializeObject<List<Todo>>(list_todo);
                    }
                    catch
                    {
                        MessageBox.Show(list_todo);
                    }
            }
        }

        public void task_get_list_repeat()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("action", "get_tasks_repeat");

            string list_todo = WebServer.post_get(JsonConvert.SerializeObject(json));

            if (list_todo != "")
            {
                try
                {
                    Settings.taskList = JsonConvert.DeserializeObject<List<Todo>>(list_todo);
                }
                catch
                {
                    MessageBox.Show(list_todo);
                }
            }     
        }
       
        public bool add_web()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("description", _description);
            json.Add("title", _title);
            json.Add("idUser", Settings.user.ID.ToString());
            json.Add("date", _date);
            json.Add("dateHours", _dateHH);
            json.Add("dateMinutes", _dateMM);
            json.Add("reminderHours", _remHH);
            json.Add("reminderMinutes", _remMM);
            json.Add("repeat", _repeat);
            json.Add("checked", _checked.ToString());
            json.Add("action", "add_task");
            string rasp = WebServer.post_get(JsonConvert.SerializeObject(json));

            try
            {
                _id = Convert.ToInt32(rasp);
                return true;
            }
            catch
            {
                MetroMessageBox.Show(new ScreenForm(), "The description is corrupted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void update_web()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("description", _description);
            json.Add("title", _title);
            json.Add("idUser", Settings.user.ID.ToString());
            json.Add("id", _id.ToString());
            json.Add("date", _date);
            json.Add("dateHours", _dateHH);
            json.Add("dateMinutes", _dateMM);
            json.Add("reminderHours", _remHH);
            json.Add("reminderMinutes", _remMM);
            json.Add("repeat", _repeat);
            json.Add("checked", _checked.ToString());
            json.Add("action", "update_task");
            string rasp = WebServer.post_get(JsonConvert.SerializeObject(json));

            if (_id == 0)
                _id = Convert.ToInt32(rasp);
        }




        public bool add_web_group()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("description", _description);
            json.Add("title", _title);
            json.Add("idUser", Settings.user.ID.ToString());
            json.Add("date", _date);
            json.Add("dateHours", _dateHH);
            json.Add("dateMinutes", _dateMM);
            json.Add("reminderHours", _remHH);
            json.Add("reminderMinutes", _remMM);
            json.Add("repeat", _repeat);
            json.Add("checked", _checked.ToString());
            json.Add("idGroup", _idGroup.ToString());
            json.Add("action", "add_task_group");
            string rasp = WebServer.post_get(JsonConvert.SerializeObject(json));

            try
            {
                _id = Convert.ToInt32(rasp);
                return true;
            }
            catch
            {
                MetroMessageBox.Show(new ScreenForm(), "The description is corrupted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
