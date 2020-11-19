namespace QuilmesAC.Models
{ 
    using System;

    public partial class CountdownDataContext
    {
        public void Save()
        {
            SubmitChanges();
        }

        public void Add(Event submission)
        {
            var countdownEvent = new Event
            {
                Name = submission.Name,
                Description = submission.Description,
                StartDate = submission.StartDate,
                EndDate = submission.EndDate,
                ThemeImage = submission.ThemeImage,
                DateAdded = DateTime.Now,
                DateChanged = DateTime.Now
            };
            Events.InsertOnSubmit(countdownEvent);
        }
    }
}