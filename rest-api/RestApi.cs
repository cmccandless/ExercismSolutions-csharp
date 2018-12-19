using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class RestApi
{
    private class User
    {
        public string name;
        private Dictionary<string, double> ledger = new Dictionary<string, double>();
        public Dictionary<string, double> owes =>
            ledger.Where(kvp => kvp.Value < 0)
                .OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => -kvp.Value);
        public Dictionary<string, double> owed_by =>
            ledger.Where(kvp => kvp.Value > 0)
                .OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        public double balance => ledger.Sum(kvp => kvp.Value);
        private void entry(string name, double amount) =>
            ledger[name] = ledger.GetValueOrDefault(name, 0) + amount;
        public void lend(string borrower, double amount) => entry(borrower, amount);
        public void borrowFrom(string lender, double amount) => entry(lender, -amount);
    }

    private Dictionary<string, User> database;

    public RestApi(string database)
    {
        var db_def = new[] {
            new {
                name = "",
                owed_by = new Dictionary<string, double>(),
                owes = new Dictionary<string, double>(),
                balance = 0.0
            }
        };
        this.database = new Dictionary<string, User>();
        var users = JsonConvert.DeserializeAnonymousType(database, db_def);
        foreach(var user_json in users)
        {
            var user = new User { name = user_json.name };
            user_json.owed_by.ToList().ForEach(kvp => user.lend(kvp.Key, kvp.Value));
            user_json.owes.ToList().ForEach(kvp => user.borrowFrom(kvp.Key, kvp.Value));
            this.database[user.name] = user;
        }
    }

    public string Get(string url, string payload = null)
    {
        string response;
        switch(url)
        {
            case "/users":
            {
                List<User> users = this.database.Values.ToList();
                if (payload != null)
                {
                    var payload_def = new { users = new string[0] };
                    var data = JsonConvert.DeserializeAnonymousType(payload, payload_def);
                    users = users.Where(u => data.users.Contains(u.name)).ToList();
                }
                response = JsonConvert.SerializeObject(users);
                break;
            }
            default: throw new ArgumentException("unknown url");
        }
        return response;
    }

    public string Post(string url, string payload)
    {
        string response;
        switch(url)
        {
            case "/add":
            {
                var payload_def = new { user = "" };
                var data = JsonConvert.DeserializeAnonymousType(payload, payload_def);
                this.database[data.user] = new User { name = data.user };
                response = JsonConvert.SerializeObject(this.database[data.user]);
                break;
            }
            case "/iou":
            {
                var payload_def = new { lender = "", borrower = "", amount = 0.0d };
                var data = JsonConvert.DeserializeAnonymousType(payload, payload_def);
                this.database[data.lender].lend(data.borrower, data.amount);
                this.database[data.borrower].borrowFrom(data.lender, data.amount);
                var users = new[] {
                    this.database[data.lender],
                    this.database[data.borrower]
                }.OrderBy(u => u.name);
                response = JsonConvert.SerializeObject(users);
                break;
            }
            default: throw new ArgumentException("unknown url");
        }
        return response;
    }
}
