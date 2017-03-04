using ADOFactory;
using System;
using System.Data.SqlClient;

namespace Helper
{
    public class EmailContent
    {
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public string SendStatus { get; set; }
        public DateTime SendDate { get; set; }
        public string Project { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }

        private void IsValidCreateEntryMail()
        {

        }

        private void IsValidEmailSentUpdate()
        {

        }

        public void EmailSentUpdate()
        {
            IsValidEmailSentUpdate();

            var query = "UPDATE EmailContent    " +
                   "SET                         " +
                   "[AlterDate] = @AlterDate    " +
                   ",[SendStatus] = @SendStatus " +
                   ",[SendDate] = @SendDate     " +
                   ",[From] = @From     " +
                   " WHERE Id = @Id             ";

            query += " SELECT CAST(@Id AS int)";

            SqlCommand cmd;
            cmd = new SqlCommand(query);

            cmd.Parameters.AddWithValue("@AlterDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@SendStatus", SendStatus);
            cmd.Parameters.AddWithValue("@SendDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@From", From);
            cmd.Parameters.AddWithValue("@Id", Id);

            using (var db = new Factory(""))
                db.InsertUpdateData(cmd);
        }

        public void CreateEntryMail()
        {
            IsValidCreateEntryMail();

            var query = "INSERT INTO EmailContent" +
                "\n ([AddDate]     " +
                "\n ,[To]          " +
                "\n ,[Body]        " +
                "\n ,[Project]     " +
                "\n ,[IsBodyHtml]  " +
                "\n ,[Subject])       " +
             "\n VALUES            " +
                 "\n (@AddDate     " +
                 "\n ,@To          " +
                 "\n ,@Body        " +
                 "\n ,@Project     " +
                 "\n ,@IsBodyHtml  " +
                 "\n ,@Subject)       ";

            query += "SELECT CAST(scope_identity() AS int)";

            SqlCommand cmd;
            cmd = new SqlCommand(query);

            cmd.Parameters.AddWithValue("@AddDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@To", To);
            cmd.Parameters.AddWithValue("@Body", Body);
            cmd.Parameters.AddWithValue("@Project", Project);
            cmd.Parameters.AddWithValue("@IsBodyHtml", IsBodyHtml);
            cmd.Parameters.AddWithValue("@Subject", Subject);

            using (var db = new Factory("PlanoDeAcao"))
                Id = db.InsertUpdateData(cmd);

        }

    }

}
