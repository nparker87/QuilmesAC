﻿namespace QuilmesAC.Models
{
	using System;
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
			var matches = Matches.Where(x => x.Opponent.Name != "Bye" && x.Result != null);

			var allTimeRecord = new AllTimeRecord
			{
				GamesPlayed = matches.Count(),
				Wins = matches.Count(x => x.Result == 'W'),
				Draws = matches.Count(x => x.Result == 'D'),
				Losses = matches.Count(x => x.Result == 'L'),
				GoalsFor = matches.Select(x => x.GoalsFor ?? 0).Sum(),
				GoalsAgainst = matches.Select(x => x.GoalsAgainst ?? 0).Sum(),
				Points = (matches.Count(x => x.Result == 'W') * 3) + (matches.Count(x => x.Result == 'D'))
			};
			return allTimeRecord;
		}

		/* User Methods */

		public void Add(UserViewModel submission)
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

		public User GetUserByPasswordReset(string passwordReset)
		{
			return Users.FirstOrDefault(x => x.PasswordReset == passwordReset);
		}

		/// <summary> Returns a single user for the given username </summary>
		/// <remarks> Currently case insensitive </remarks>
		public User GetUserByUsername(string username)
		{
			return Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
		}

		public User GetUserByID(int? id)
		{
			return Users.FirstOrDefault(x => x.ID == id);
		}

		public User GetUserByEmail(string email)
		{
			return Users.FirstOrDefault(x => x.Email == email);
		}

		public UserRole GetUserRoleByID(int id)
		{
			return UserRoles.FirstOrDefault(x => x.ID == id);
		}

		public void Add(UserRoleViewModel submission)
		{
			var userRole = new UserRole
			{
				UserID = submission.UserID,
				RoleID = submission.RoleID
			};
			UserRoles.InsertOnSubmit(userRole);
		}

		public void Update(UserRole userRole, UserRoleViewModel submission)
		{
			userRole.UserID = submission.UserID;
			userRole.RoleID = submission.RoleID;
		}

		public void Delete(UserRole userRole)
		{
			UserRoles.DeleteOnSubmit(userRole);
		}

		/* Player Methods */

		public Player GetPlayerByID(int? id)
		{
			return Players.FirstOrDefault(x => x.ID == id);
		}

		public List<Player> GetPlayersByStatusID(int statusID)
		{
			return Players.Where(x => x.StatusID == statusID).ToList();
		}

		public void Add(PlayerViewModel submission)
		{
			var player = new Player
			{
				FirstName = submission.FirstName,
				LastName = submission.LastName,
				Number = submission.Number,
				CreatedDate = DateTime.Now,
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
			var playerPosition = GetPlayerPositionsByPlayerID(player.ID);

			Goals.DeleteAllOnSubmit(goals);
			Cards.DeleteAllOnSubmit(cards);
			PlayerPositions.DeleteAllOnSubmit(playerPosition);
			Players.DeleteOnSubmit(player);
		}

		/* Position Methods */

		public List<Position> GetPositions()
		{
			return Positions.ToList();
		}

		public Position GetPositionByID(int id)
		{
			return Positions.FirstOrDefault(x => x.ID == id);
		}

		public void Add(PositionViewModel submission)
		{
			var position = new Position
			{
				Name = submission.Name,
				ShortName = submission.ShortName
			};
			Positions.InsertOnSubmit(position);
		}

		public void Update(Position position, PositionViewModel submission)
		{
			position.Name = submission.Name;
			position.ShortName = submission.ShortName;
		}

		public void Delete(Position position)
		{
			Positions.DeleteOnSubmit(position);
		}

		/* Player Position Methods */

		public PlayerPosition GetPlayerPositionByID(int id)
		{
			return PlayerPositions.FirstOrDefault(x => x.ID == id);
		}

		public List<PlayerPosition> GetPlayerPositionsByPlayerID(int playerID)
		{
			return PlayerPositions.Where(x => x.PlayerID == playerID).ToList();
		}

		public void Add(PlayerPositionViewModel submission)
		{
			if (submission.PrimaryPosition)
				foreach (var primaryPosition in PlayerPositions.Where(x => x.PlayerID == submission.PlayerID && x.PrimaryPosition))
					primaryPosition.PrimaryPosition = false;

			var playerPosition = new PlayerPosition
			{
				PlayerID = submission.PlayerID,
				PositionID = submission.PositionID,
				PrimaryPosition = submission.PrimaryPosition
			};
			PlayerPositions.InsertOnSubmit(playerPosition);
		}

		public void Update(PlayerPosition playerPosition, PlayerPositionViewModel submission)
		{
			if (submission.PrimaryPosition)
				foreach (var primaryPosition in PlayerPositions.Where(x => x.PlayerID == submission.PlayerID && x.PrimaryPosition))
					primaryPosition.PrimaryPosition = false;

			playerPosition.PlayerID = submission.PlayerID;
			playerPosition.PositionID = submission.PositionID;
			playerPosition.PrimaryPosition = submission.PrimaryPosition;
		}

		public void Delete(PlayerPosition playerPosition)
		{
			PlayerPositions.DeleteOnSubmit(playerPosition);
		}

		/* Status Methods */

		public List<Status> GetStatuses()
		{
			return Status.ToList();
		}

		/* Match Methods */

		public Match GetMatchByID(int id)
		{
			return Matches.FirstOrDefault(x => x.ID == id);
		}

		public List<Match> GetMatchesBySeasonID(int? seasonID)
		{
			return Matches.Where(x => x.SeasonID == seasonID).OrderBy(x => x.MatchDay).ToList();
		}

		public void Add(FixtureViewModel submission)
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

		public void UpdateMatch(Match match, FixtureViewModel submission)
		{
			match.MatchDay = submission.MatchDay;
			match.MatchDate = submission.MatchDate;
			match.OpponentID = submission.OpponentID;
			match.GoalsFor = submission.GoalsFor;
			match.GoalsAgainst = submission.GoalsAgainst;
			match.Result = submission.Result;
			match.SeasonID = submission.SeasonID;
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

		public Opponent GetOpponentByID(int id)
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

		public Season GetSeasonByID(int id)
		{
			return Seasons.FirstOrDefault(x => x.ID == id);
		}

		public Season GetCurrentSeason()
		{
			return Seasons.FirstOrDefault(x => x.IsCurrent == true);
		}

		public void Add(SeasonViewModel submission)
		{
			var season = new Season
			{
				StartDate = submission.StartDate,
				DisplayName = submission.DisplayName,
				DivisionID = submission.DivisionID,
				IsCurrent = submission.IsCurrent
			};
			Seasons.InsertOnSubmit(season);
		}

		public void Update(Season season, SeasonViewModel submission)
		{
			season.StartDate = submission.StartDate;
			season.DisplayName = submission.DisplayName;
			season.DivisionID = submission.DivisionID;
			season.IsCurrent = submission.IsCurrent;
		}

		public void Delete(Season season)
		{
			Seasons.DeleteOnSubmit(season);
		}

		/* Goal Methods */

		public Goal GetGoalByID(int id)
		{
			return Goals.FirstOrDefault(x => x.ID == id);
		}

		public List<Goal> GetGoalsByPlayerID(int playerID)
		{
			return Goals.Where(x => x.PlayerID == playerID).ToList();
		}

		public int GetGoalsBySeasonAndPlayer(int playerID, int? seasonID)
		{
			if (seasonID == null)
				return Goals.Where(x => x.PlayerID == playerID).Count();
			return Goals.Where(x => x.PlayerID == playerID && x.Match.SeasonID == seasonID).Count();
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

		public Assist GetAssitByID(int id)
		{
			return Assists.FirstOrDefault(x => x.ID == id);
		}

        public int GetAssistsBySeasonAndPlayer(int playerID, int? seasonID)
        {
            if (seasonID == null)
                return Assists.Where(x => x.PlayerID == playerID).Count();
            return Assists.Where(x => x.PlayerID == playerID && x.Match.SeasonID == seasonID).Count();
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

		public Card GetCardByID(int id)
		{
			return Cards.FirstOrDefault(x => x.ID == id);
		}

		public List<Card> GetCardsByPlayerID(int playerID)
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

		/* Standings Methods */

		public Standing GetStandingByID(int id)
		{
			return Standings.FirstOrDefault(x => x.ID == id);
		}

		public List<Standing> GetStandingsBySeasonID(int? seasonID)
		{
			return Standings.Where(x => x.SeasonID == seasonID).OrderBy(x => x.Position).ToList();
		}

		public void Add(StandingViewModel submission)
		{
			var standing = new Standing
			{
				SeasonID = submission.SeasonID,
				OpponentID = submission.OpponentID,
				GamesPlayed = submission.GamesPlayed,
				Win = submission.Win,
				Draw = submission.Draw,
				Loss = submission.Loss,
				GoalsFor = submission.GoalsFor,
				GoalsAgainst = submission.GoalsAgainst,
				GoalDifference = submission.GoalsFor - submission.GoalsAgainst,
				Points = (submission.Win * 3) + submission.Draw,
				Position = submission.Position
			};
			Standings.InsertOnSubmit(standing);
		}

		public void Update(Standing standing, StandingViewModel submission)
		{
			standing.SeasonID = submission.SeasonID;
			standing.OpponentID = submission.OpponentID;
			standing.GamesPlayed = submission.GamesPlayed;
			standing.Win = submission.Win;
			standing.Draw = submission.Draw;
			standing.Loss = submission.Loss;
			standing.GoalsFor = submission.GoalsFor;
			standing.GoalsAgainst = submission.GoalsAgainst;
			standing.GoalDifference = submission.GoalsFor - submission.GoalsAgainst;
			standing.Points = (submission.Win * 3) + submission.Draw;
			standing.Position = submission.Position;
		}

		public void Delete(Standing standing)
		{
			Standings.DeleteOnSubmit(standing);
		}

		public Standing GetQuilmesStandingIDBySeason(int seasonID)
		{
			return Standings.FirstOrDefault(x => x.SeasonID == seasonID && x.Opponent.Name == "Quilmes AC");
		}

		/* Division Methods */

		public List<Division> GetDivisions()
		{
			return Divisions.OrderBy(x => x.ID).ToList();
		}

		/* Formation Methods */

		public Formation GetFormationByID(int id)
		{
			return Formations.FirstOrDefault(x => x.ID == id);
		}

		public void Add(FormationViewModel submission)
		{
			var formation = new Formation
			{
				Name = submission.Name
			};
			Formations.InsertOnSubmit(formation);
		}

		public void Update(Formation formation, FormationViewModel submission)
		{
			formation.Name = submission.Name;
		}

		public void Delete(Formation formation)
		{
			Formations.DeleteOnSubmit(formation);
		}

        public List<CurrentRoster> GetCurrentRoster()
        {
            var roster = new List<CurrentRoster>();
            var currentSeason = GetCurrentSeason();

            foreach(var player in Players.Where(x => x.StatusID == 1).OrderBy(x => x.LastName))
            {
                var rosterPlayer = new CurrentRoster
                {
                    PlayerID = player.ID,
                    FullName = String.Format("{0}, {1}", player.LastName, player.FirstName),
                    Number = player.Number,
                    Appearances = player.Appearances.Where(x => x.Match.SeasonID == currentSeason.ID).Count(),
                    Goals = GetGoalsBySeasonAndPlayer(player.ID, currentSeason.ID),
                    Assists = GetAssistsBySeasonAndPlayer(player.ID, currentSeason.ID),
                    YellowCards = GetCardsByPlayerID(player.ID).Where(x => x.CardType.Name == "Yellow" && x.Match.SeasonID == currentSeason.ID).Count(),
                    RedCards = GetCardsByPlayerID(player.ID).Where(x => x.CardType.Name == "Red" && x.Match.SeasonID == currentSeason.ID).Count()
                };
                roster.Add(rosterPlayer);
            }

            return roster;
        }

    }
}