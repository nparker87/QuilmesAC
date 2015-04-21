namespace QuilmesAC.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using ViewModels;

    public partial class QuilmesDataContext
    {
        public void Save()
        {
            SubmitChanges();
        }

        public AllTimeRecord GetAllTimeRecord()
        {
            // Do not include bye "wins"
            var matches = Matches.Where(x => x.Opponent.Name != "Bye");

            var allTimeRecord = new AllTimeRecord
            {
                GamesPlayed = matches.Count(),
                Wins = matches.Count(x => x.Result == 'W'),
                Draws = matches.Count(x => x.Result == 'D'),
                Losses = matches.Count(x => x.Result == 'L'),
                GoalsFor = matches.Select(x => x.GoalsFor ?? 0).Sum(),
                GoalsAgainst = matches.Select(x => x.GoalsAgainst ?? 0).Sum(),
                Points = (matches.Count(x => x.Result == 'W') * 3) + (matches.Count(x => x.Result == 'L'))
            };
            return allTimeRecord;
        }

        /* User Methods */

        public void AddUser(UserViewModel submission)
        {
            var encryptedPassword = LoginHelper.EncryptPassword(submission.Username, submission.Password);
            var user = new User
            {
                Username = submission.Username,
                Password = encryptedPassword,
                Email = submission.Email
            };
            Users.InsertOnSubmit(user);
        }

        /// <summary> Returns a single user for the given username </summary>
        /// <remarks> Currently case insensitive </remarks>
        public User GetUserByUsername(string username)
        {
            return Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
        }

        /* Player Methods */

        public Player GetPlayerByID(long? id)
        {
            return Players.FirstOrDefault(x => x.ID == id);
        }

        public void AddPlayer(PlayerViewModel submission)
        {
            var player = new Player
            {
                FirstName = submission.FirstName,
                LastName = submission.LastName,
                Number = submission.Number,
                StatusID = submission.StatusID
            };
            Players.InsertOnSubmit(player);
        }

        public void UpdatePlayer(Player player, PlayerViewModel submission)
        {
            player.FirstName = submission.FirstName;
            player.LastName = submission.LastName;
            player.Number = submission.Number;
            player.StatusID = submission.StatusID;
        }

        public void Delete(Player player)
        {
            var goals = GetGoalsByPlayerID(player.ID);
            var cards = GetCardsByPlayerID(player.ID);

            Goals.DeleteAllOnSubmit(goals);
            Cards.DeleteAllOnSubmit(cards);
            Players.DeleteOnSubmit(player);
        }

        /* Position Methods */

        public List<Position> GetPositions()
        {
            return Positions.ToList();
        }

        /* Status Methods */

        public List<Status> GetStatuses()
        {
            return Status.ToList();
        }

        /* Match Methods */

        public void AddMatch(MatchViewModel submission)
        {
            var match = new Match
            {
                MatchDay = submission.MatchDay,
                MatchDate = submission.MatchDate,
                OpponentID = submission.OpponentID,
                GoalsFor = submission.GoalsFor,
                GoalsAgainst = submission.GoalsAgainst,
                Result = submission.Result,
                SeasonID = submission.SeasonID
            };
            Matches.InsertOnSubmit(match);
        }

        /* Opponent Methods */

        public void Add(OpponentViewModel submission)
        {
            var opponent = new Opponent
            {
                Name = submission.Name
            };
            Opponents.InsertOnSubmit(opponent);
        }

        public Opponent GetOpponentByID(long id)
        {
            return Opponents.FirstOrDefault(x => x.ID == id);
        }

        public void Update(Opponent opponent, OpponentViewModel submission)
        {
            opponent.Name = submission.Name;
        }

        public void Delete(Opponent opponent)
        {
            Opponents.DeleteOnSubmit(opponent);
        }

        public List<Opponent> GetOpponents()
        {
            return Opponents.OrderBy(x => x.Name).ToList();
        }

        /* Season Methods */

        public List<Season> GetSeasons()
        {
            return Seasons.OrderBy(x => x.StartDate).ToList();
        }

        public Season GetSeasonByID(long id)
        {
            return Seasons.FirstOrDefault(x => x.ID == id);
        }

        public void Add(SeasonViewModel submission)
        {
            var season = new Season
            {
                StartDate = submission.StartDate,
                DisplayName = submission.DisplayName,
                DivisionID = submission.DivisionID
            };
            Seasons.InsertOnSubmit(season);
        }

        public void Update(Season season, SeasonViewModel submission)
        {
            season.StartDate = submission.StartDate;
            season.DisplayName = submission.DisplayName;
            season.DivisionID = submission.DivisionID;
        }

        public void Delete(Season season)
        {
            Seasons.DeleteOnSubmit(season);
        }

        /* Goal Methods */

        public Goal GetGoalByID(long id)
        {
            return Goals.FirstOrDefault(x => x.ID == id);
        }

        public List<Goal> GetGoalsByPlayerID(long playerID)
        {
            return Goals.Where(x => x.PlayerID == playerID).ToList();
        }

        public void Add(GoalViewModel submission)
        {
            var goal = new Goal
            {
                PlayerID = submission.PlayerID,
                MatchID = submission.MatchID
            };
            Goals.InsertOnSubmit(goal);
        }

        public void Update(Goal goal, GoalViewModel submission)
        {
            goal.PlayerID = submission.PlayerID;
            goal.MatchID = submission.MatchID;
        }

        public void Delete(Goal goal)
        {
            Goals.DeleteOnSubmit(goal);
        }

        /* Assist Methods */

        public Assist GetAssitByID(long id)
        {
            return Assists.FirstOrDefault(x => x.ID == id);
        }

        public void Add(AssistViewModel submission)
        {
            var assist = new Assist
            {
                PlayerID = submission.PlayerID,
                MatchID = submission.MatchID
            };
            Assists.InsertOnSubmit(assist);
        }

        public void Update(Assist assist, AssistViewModel submission)
        {
            assist.PlayerID = submission.PlayerID;
            assist.MatchID = submission.MatchID;
        }

        public void Delete(Assist assist)
        {
            Assists.DeleteOnSubmit(assist);
        }

        /* Card Methods */

        public Card GetCardByID(long id)
        {
            return Cards.FirstOrDefault(x => x.ID == id);
        }

        public List<Card> GetCardsByPlayerID(long playerID)
        {
            return Cards.Where(x => x.PlayerID == playerID).ToList();
        }

        public void Add(CardViewModel submission)
        {
            var card = new Card
            {
                PlayerID = submission.PlayerID,
                MatchID = submission.MatchID,
                TypeID = submission.TypeID
            };
            Cards.InsertOnSubmit(card);
        }

        public void Update(Card card, CardViewModel submission)
        {
            card.PlayerID = submission.PlayerID;
            card.MatchID = submission.MatchID;
            card.TypeID = submission.TypeID;
        }

        public void Delete(Card card)
        {
            Cards.DeleteOnSubmit(card);
        }

        /* Division Methods */

        public List<Division> GetDivisions()
        {
            return Divisions.OrderBy(x => x.ID).ToList();
        }
    }
}