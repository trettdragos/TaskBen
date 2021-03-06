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
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> groupMembers { get; set; } 
        public string MemberEmail { get; set; }
        public bool IsNew { get; set; }

        public Group()
        {
            Id = 0;
            IsNew = true;
        }

        public bool validate_data(ref string broken_rule)
        {
            if (Name == "" || Name == null)
            {
                broken_rule = "The Name is empty!";
                return false;
            }
            return true;
        }

        public void remove()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("id", Id.ToString());
            json.Add("action", "remove_group");
            WebServer.post(JsonConvert.SerializeObject(json));
        }

        public List<Todo> get_group_tasks()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("idGroup", this.Id.ToString());
            json.Add("action", "get_group_tasks");

            string list_todo = WebServer.post_get(JsonConvert.SerializeObject(json));

            if (list_todo != "")
                try
                {
                    return JsonConvert.DeserializeObject<List<Todo>>(list_todo);
                }
                catch
                {
                    MessageBox.Show(list_todo);
                }

            return null;
        }

        public List<Group> get_groups(int userid)
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("idUser", userid.ToString());
            json.Add("action", "get_groups");

            string list_todo = WebServer.post_get(JsonConvert.SerializeObject(json));

            if (list_todo != "")
                try
                {
                    return JsonConvert.DeserializeObject<List<Group>>(list_todo);
                }
                catch
                {
                    return null;
                }
            return null;
        }

        public bool save(ref string broken_rule)
        {
            if (validate_data(ref broken_rule))
            {
                try{
                    Dictionary<string, string> json = new Dictionary<string, string>();
                    json.Add("Name",Name);
                    json.Add("Id", Id.ToString());
                    json.Add("Description", Description);
                    json.Add("action", "save_group");
                    json.Add("Members", JsonConvert.SerializeObject(groupMembers));
                    
                    string rasp = WebServer.post_get(JsonConvert.SerializeObject(json));
                    json = JsonConvert.DeserializeObject<Dictionary<string, string>>(rasp);

                    if (json["Error"].ToString() == "")
                    {
                        if(Id==0)
                            Id = Convert.ToInt32(json["Id"]);
                        return true;
                    }
                    else
                    {
                        broken_rule = json["Error"].ToString();
                        return false;
                    }
                }
                catch(Exception ex){
                    broken_rule = ex.Message;
                    return false;
                }
            }
            else
                return false;
        }

        public bool VerifyMember()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("memberName", MemberEmail);
            json.Add("action", "member_existence");
            string rasp = WebServer.post_get(JsonConvert.SerializeObject(json));

            try
            {
                json = JsonConvert.DeserializeObject<Dictionary<string, string>>(rasp);
                if (json["Error"].ToString() == "" && json["Answer"].ToString() == "1")
                    return true;
                else 
                    return false;
            }
            catch
            {
                MessageBox.Show(rasp);
            }

            return false;
        }

        public bool SameMember()
        {
            foreach (string name in groupMembers)
            {
                if (name == this.MemberEmail)
                    return false;
            }
            return true;
        }

        public void AddMember()
        {

        }

        public void load_members()
        {
            List<Dictionary<string, string>> list_member = new List<Dictionary<string, string>>();
            Dictionary<string, string> json = new Dictionary<string, string>();
            json.Add("id", Id.ToString());
            json.Add("action", "get_group_members");
            string rasp = WebServer.post_get(JsonConvert.SerializeObject(json));

            try
            {
                groupMembers = new List<string>();
                list_member = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(rasp); ;
                foreach(Dictionary<string, string> item in list_member)
                {
                    groupMembers.Add(item["Email"]);
                }
            }
            catch
            {
                MessageBox.Show(rasp);
            }
        }
    }
}
