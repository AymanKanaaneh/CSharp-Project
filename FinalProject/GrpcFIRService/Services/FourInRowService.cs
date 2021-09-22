using Grpc.Core;
using GrpcFIRService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace GrpcFIRService
{
    public class FourInRowService : FourInRow.FourInRowBase
    {
        private readonly ILogger<FourInRowService> _logger;
        public FourInRowService(ILogger<FourInRowService> logger)
        {
            _logger = logger;           
            if(firstgame)
                falseallgames();
            firstgame = false;
        }
    
        private static bool firstgame=true;

        private static ConcurrentDictionary<string, List<ChatMessage>> users =
            new ConcurrentDictionary<string, List<ChatMessage>>();

        private static ConcurrentDictionary<string, List<GameMessage>> usersGame =
            new ConcurrentDictionary<string, List<GameMessage>>();

        private static ConcurrentDictionary<string, bool> usersGameStatue =
            new ConcurrentDictionary<string, bool>();

        private static List<UserModel> usersOnGame = new List<UserModel>();

        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(0.5);

        // make all ongoing games false when the service is up once
        private void falseallgames()
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {

                    var query = (from g in ctx.Games
                                 where g.Ongoing == true
                                 select g).ToList();
                    foreach(var game in query)
                    {
                        ctx.Games.Remove(game);
                        ctx.SaveChanges();
                        game.Ongoing = false;
                        ctx.Games.Add(game);
                        ctx.SaveChanges();
                    }
                    
                    return;
                }
            }
            catch(Exception ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
        }

        //-------------------------------------------------------------------------------------------------

        //connecting with responseStream gameMessage when players in a game
        public override async Task Peer2peerConnect(GameMessage userGame,
            IServerStreamWriter<GameMessage> responseStream,
            ServerCallContext context) {

            usersGame.TryAdd(userGame.CMessage.FromUser, new List<GameMessage>());
            usersGameStatue.TryAdd(userGame.CMessage.ToUser, false);

            var token = context.CancellationToken;
            while (!token.IsCancellationRequested) {
                if (usersGame[userGame.CMessage.FromUser].Count > 0) {
                    foreach (var item in usersGame[userGame.CMessage.FromUser]) {
                        usersGameStatue[userGame.CMessage.ToUser] = item.Turn;
                        await responseStream.WriteAsync(item);
                    }
                    usersGame[userGame.CMessage.FromUser].Clear();
                }
                await Task.Delay(Interval, token);
            }

        }
        //-------------------------------------------------------------------------------------------------

        //connecting with responseStream ChatMessage when users loging in
        public override async Task Connect(UserModel user,
            IServerStreamWriter<ChatMessage> responseStream,
            ServerCallContext context) {

            if (users.ContainsKey(user.UserName)) {
                throw new RpcException(new Status(
                    StatusCode.AlreadyExists, "user already exists"));
            }
            else {

                users.TryAdd(user.UserName, new List<ChatMessage>());
                var token = context.CancellationToken;
                InformAllUsers(new ChatMessage
                {
                    Type = MessageType.Update,
                    FromUser = "",
                    ToUser = "",
                    Message = ""
                });

                while (!token.IsCancellationRequested) {
                    if (users[user.UserName].Count > 0) {
                        foreach (var item in users[user.UserName]) {
                            await responseStream.WriteAsync(item);
                        }
                        users[user.UserName].Clear();
                    }
                    await Task.Delay(Interval, token);
                }
            }
        }


        public override async Task<Empty> Disconnect(UserModel user,
            ServerCallContext context) {

            Debug.Write("My error message. ");

            var val = new List<ChatMessage>();
            //var val2 = new List<GameMessage>();
            //bool val3 = true;

            users.TryRemove(user.UserName, out val);
            //usersGame.TryRemove(user.UserName, out val2);
            //usersGameStatue.TryRemove(user.UserName, out val3);
            

            InformAllUsers(new ChatMessage
            {
                Type = MessageType.Update,
                FromUser = "",
                ToUser = "",
                Message = ""
            });


            return await Task.FromResult(new Empty());
        }


        //remove the user from the server
        public override async Task<Empty> serverRemoveUser(Users usersToRemove,
            ServerCallContext context) {

            var token = context.CancellationToken;
            var val = new List<ChatMessage>();          
           
            users.TryRemove(usersToRemove.UserNames[0], out val);

            InformAllUsers(new ChatMessage
            {
                Type = MessageType.Update,
                FromUser = "",
                ToUser = "",
                Message = ""
            });

            return await Task.FromResult(new Empty());
        }

        //add user to the server
        public override async Task<Empty> serverAddUser(UserModel user,
            ServerCallContext context) {

            var val = new List<ChatMessage>();
            users.TryAdd(user.UserName, val);
            InformAllUsers(new ChatMessage
            {
                Type = MessageType.Update,
                FromUser = "",
                ToUser = "",
                Message = ""
            });
            return await Task.FromResult(new Empty());
        }

        //-------------------------------------------------------------------------------------------------
         //add a message to all users
        private void InformAllUsers(ChatMessage chatMessage) {
             foreach (var item in users.Keys) {
                 users[item].Add(chatMessage);
             }
         }
        
        //-------------------------------------------------------------------------------------------------
        //get the user from the username (the key)
        public async override Task<UserModel> GetUserByUserName(NameString request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {

                    User user = (from u in ctx.Users
                                   where u.UserName == request.Name                                   
                                   select u).First();

                    return await Task.FromResult(new UserModel
                    {
                        UserName = user.UserName,
                        NumOfGames = user.NumOfGames,
                        NumOfLoses = user.NumOfLoses,
                        NumOFWins = user.NumOfWins,
                        Points = user.Points,
                        HashedPassword = user.HashedPassword
                    });
                }
            }
            catch (Exception ex)
            {

                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        public override async Task<UserModel> GetUserById(UserModel userId, ServerCallContext context) {


            try {
                using (var ctx = new FourInRowDBContext()) {

                    User UserID = (from user in ctx.Users
                                   where user.UserName == userId.UserName
                                   && user.HashedPassword == userId.HashedPassword
                                   select user).First();

                    return await Task.FromResult(new UserModel
                    {
                        UserName = UserID.UserName,
                        HashedPassword = UserID.HashedPassword,
                        NumOfGames = UserID.NumOfGames,
                        NumOFWins = UserID.NumOfWins,
                        NumOfLoses = UserID.NumOfLoses,
                        Points = UserID.Points
                    });
                }
            }
            catch (Exception ex) {

                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //insert a new game to the databaes when its initilized
        public override async Task<Empty> InsertGame(GameModel game2add, ServerCallContext context) {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    var game = (from g in ctx.Games where g.SerialNumber == game2add.Serialnumber
                                select g).ToList();
                    if (game==null)
                    {
                        return await Task.FromResult(new Empty());
                    }
                    Game newGame = new Game
                    {
                        SerialNumber = game2add.Serialnumber,
                        FirstPlayer = game2add.FirstPlayer,
                        SecondPlayer = game2add.SecondPlayer,
                        Winner = game2add.Winner,
                        StartingTime = game2add.StartingTime,
                        Draw = game2add.Draw,
                        Ongoing = game2add.Ongoing,
                        FirstPlayerPoints = game2add.Firstplayerpoints,
                        SecondPlayerPoints = game2add.SecondPlayerpoints
                    };

                    ctx.Games.Add(newGame);
                    ctx.SaveChanges();
                }
                return await Task.FromResult(new Empty());
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
            
        }
        //-------------------------------------------------------------------------------------------------
        //check if a game already exists by its serial number
        public async override Task<boolmessage> CheckSerialNumber(SerialNumber request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    bool reply = false ;
                    var query = (from game in ctx.Games
                                 where game.SerialNumber == request.Serialnumber
                                select game).ToList();
                   if(query.Count() > 0)
                    {
                        reply = true;
                    }
                    
                    return await Task.FromResult(new boolmessage { Istrue = reply});
                }
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------
        //get a new serial number to initilize a new game
        public async override Task<SerialNumber> GetNextSerialNumber(Empty request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    int sn = ctx.Games.Count()+1;
                    return await Task.FromResult(new SerialNumber { Serialnumber = sn });
                }
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //update the data of the game sent by the end of the game
        public async override Task<Empty> UpdateEndGame(GameModel request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                   ctx.Games.Remove((from game in ctx.Games where
                                        game.SerialNumber == request.Serialnumber 
                                        select game).First());
                    ctx.SaveChanges();
                    Game updatedgame = new Game
                    {
                        Draw = request.Draw,
                        FirstPlayer = request.FirstPlayer,
                        Ongoing = false,
                        SecondPlayer = request.SecondPlayer,
                        SerialNumber = request.Serialnumber,
                        StartingTime = request.StartingTime,
                        Winner = request.Winner,
                        FirstPlayerPoints = request.Firstplayerpoints,
                        SecondPlayerPoints = request.SecondPlayerpoints
                    };
                    ctx.Games.Add(updatedgame);
                    ctx.SaveChanges();
                    return await Task.FromResult(new Empty());
                }
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //get the users as string to show in users search
        public override async Task<ShowUsersList> GetShowedUsers(Empty request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    var query = (from u in ctx.Users select u).ToList();
                    List<string> reply = new List<string>();
                    string temp="";
                    foreach (var item in query)
                    {
                        temp += "name: ";
                        temp += item.UserName;
                        temp += " games: ";
                        temp += item.NumOfGames;
                        temp += " wins: ";
                        temp += item.NumOfWins;
                        temp += " loses: ";
                        temp += item.NumOfLoses;
                        temp += " points: ";
                        temp += item.Points;
                        reply.Add(temp);
                        temp = "";
                    }
                    return await Task.FromResult(new ShowUsersList
                    {
                        Usershow = { reply }

                    });
                }
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Unavailable, ex.Message));
            }

        }
        //-------------------------------------------------------------------------------------------------
        //get the games as string to show in gameslist search
        public override async Task<ShowGamesList> GetShowedGames(Empty request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    var query = (from g in ctx.Games where g.Ongoing == false select g).ToList();
                    List<string> gameslist = new List<string>();
                    
                    string temp = "";
                    foreach (var item in query)
                    {
                        temp += item.FirstPlayer + " got ";
                        temp += item.FirstPlayerPoints + " points";
                        temp += " vs ";
                        temp += item.SecondPlayer + " got ";
                        temp += item.SecondPlayerPoints + " points";
                        if (item.Draw == false) 
                        {
                            temp += ",winner is: " + item.Winner;
                        }
                        else
                        {
                            temp += " ,Draw";
                        }
                        temp += " ,game started at " + item.StartingTime;
                        gameslist.Add(temp);
                        temp = "";
                    }
                    return await Task.FromResult(new ShowGamesList
                    {
                        Gameshow = { gameslist }
                    }); 
                }
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Unavailable, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //get the games between 2 players as a string to show in the search      
        public async override Task<Show2players> GetShowedGamesBetween2Users(UsersModel request, ServerCallContext context) {
            try {
                using (var ctx = new FourInRowDBContext()) {
                    var query = (from g in ctx.Games
                                 where (g.FirstPlayer == request.User1.UserName
                                 & g.SecondPlayer == request.User2.UserName)
                                 | (g.FirstPlayer == request.User2.UserName
                                 & g.SecondPlayer == request.User1.UserName)
                                 select g).ToList();
                    List<string> gameslist = new List<string>();
                    string temp = "";
                    int countgames = 0, countp1wins = 0, countp2wins = 0;
                    foreach (var item in query) {
                        temp += "game started at " + item.StartingTime;
                        if (item.Winner != "") {
                            temp += " ," + item.Winner + " won";
                        }
                        else {
                            temp += " ," + "Draw";
                        }
                        gameslist.Add(temp);
                        temp = "";
                        countgames++;
                        if (item.Winner.Equals(request.User1.UserName)) {
                            countp1wins++;
                        }
                        else
                        if (item.Winner.Equals(request.User2.UserName)) {
                            countp2wins++;
                        }
                    }
                    return await Task.FromResult(new Show2players
                    {
                        Gameshow = { gameslist },
                        Gamesnumber = countgames,
                        P1Wins = countp1wins,
                        P2Wins = countp2wins
                    });
                }
            }
            catch (RpcException ex) {
                throw new RpcException(new Status(StatusCode.Unavailable, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //get the usres as string with winning percent
        public override async Task<ShowUsersList> GetShowedPercentUsers(Empty request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    var query = (from u in ctx.Users select u).ToList();
                    List<string> reply = new List<string>();
                    string temp = "";
                    foreach (var item in query)
                    {
                        temp += "name: ";
                        temp += item.UserName;
                        temp += " games: ";
                        temp += item.NumOfGames;
                        temp += " wins: ";
                        temp += item.NumOfWins;
                        temp += item.NumOfWins / item.NumOfGames * 100;
                        temp += "%";
                        temp += " loses: ";
                        temp += item.NumOfLoses;
                        temp += " points: ";
                        temp += item.Points;
                        reply.Add(temp);
                        temp = "";
                    }
                    return await Task.FromResult(new ShowUsersList
                    {
                        Usershow = { reply }

                    });
                }
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Unavailable, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        // get the ongoing games as string 
        public override async Task<ShowGamesList> GetShowedOngoingGames(Empty request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    var query = (from g in ctx.Games
                                 where g.Ongoing == true
                                 select g).ToList();
                    List<string> gameslist = new List<string>();
                    string temp = "";
                    foreach (var item in query)
                    {
                        temp += item.FirstPlayer ;
                        temp += " vs ";
                        temp += item.SecondPlayer ;
                        temp += " game started at " + item.StartingTime;
                        gameslist.Add(temp);
                        temp = "";
                    }
                    return await Task.FromResult(new ShowGamesList
                    {
                        Gameshow = { gameslist }
                    });
                }
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Unavailable, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //get the users as string 
        public override async Task<ShowUsersList> GetAllUsers(Empty request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    var query = (from u in ctx.Users select u).ToList();
                    List<string> reply = new List<string>();
                    string temp = "";
                    foreach (var item in query)
                    {
                        temp += item.UserName;                       
                        reply.Add(temp);
                        temp = "";
                    }
                    return await Task.FromResult(new ShowUsersList
                    {
                        Usershow = { reply }

                    });
                }
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Unavailable, ex.Message));
            }
        }        
        //-------------------------------------------------------------------------------------------------
        //insert a user to the database
        public override async Task<Empty> InsertUser(UserModel user2add, ServerCallContext context) {
            User newUser = new User
            {
                UserName = user2add.UserName,
                HashedPassword = user2add.HashedPassword,
                NumOfGames = user2add.NumOfGames,
                NumOfWins = user2add.NumOFWins,
                Points = user2add.Points,
                NumOfLoses = user2add.NumOfLoses
            };

            try {
                using (var ctx = new FourInRowDBContext()) {
                    ctx.Users.Add(newUser);
                    ctx.SaveChanges();
                }
            }
            catch (RpcException ex) {

                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }


            return await Task.FromResult(new Empty());

        }
        //-------------------------------------------------------------------------------------------------
        //check if the user already exists
        public override async Task<Empty> UserExists(UserModel user,
           ServerCallContext context) {
            if (users.ContainsKey(user.UserName)) {
                throw new RpcException(new Status(StatusCode.AlreadyExists,
                    "user already exists"));
            }
            return await Task.FromResult(new Empty());
        }
        //-------------------------------------------------------------------------------------------------
        //update the users keys
        public override async Task<Users> UpdateUsers(Empty request,
           ServerCallContext context) {
            var reply = new Users
            {
                UserNames = { users.Keys }
            };
            return await Task.FromResult(reply);
        }
        //-------------------------------------------------------------------------------------------------
       //send game message to all users
        public override async Task<Empty> SendGameMessage(GameMessage gameMessage,
            ServerCallContext context) {

            if (usersGame.ContainsKey(gameMessage.CMessage.ToUser)) {
                usersGame[gameMessage.CMessage.ToUser].Add(gameMessage);
            }
            else {
                throw new RpcException(new Status(StatusCode.NotFound, "game not avaliable"));
            }

            return await Task.FromResult(new Empty());
        }
        //-------------------------------------------------------------------------------------------------
        //send message to all users
        public override async Task<Empty> SendMessage(ChatMessage message,
           ServerCallContext context) {
            if (message.Type == MessageType.Update) {
                InformAllUsers(message);
            }
            else if (users.ContainsKey(message.ToUser)) {
                users[message.ToUser].Add(message);
            }
            else {
                throw new RpcException(
                    new Status(StatusCode.NotFound, "user not connected"));
            }
            return await Task.FromResult(new Empty());
        }
        //-------------------------------------------------------------------------------------------------
        //get a game by its serial number
        public async override Task<GameModel> GetGameBySerialNumber(SerialNumber request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    Game game = (from g in ctx.Games
                                      where g.SerialNumber == request.Serialnumber
                                      select g).First();
                    GameModel reply = new GameModel
                    {
                        Serialnumber = game.SerialNumber,
                        Draw = game.Draw,
                        FirstPlayer = game.FirstPlayer,
                        Ongoing = game.Ongoing,
                        SecondPlayer = game.SecondPlayer,
                        StartingTime = game.StartingTime,
                        Winner = game.Winner,
                        Firstplayerpoints = game.FirstPlayerPoints,
                        SecondPlayerpoints = game.SecondPlayerPoints
                    };
                    return await Task.FromResult(reply);
                }
            }
            catch (RpcException ex)
            {

                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //set a user to his game
        public override async Task<Empty> InsertUser2Played(UserModel user2add, ServerCallContext context) {

            usersOnGame.Add(user2add);

            return await Task.FromResult(new Empty());

        }
        //-------------------------------------------------------------------------------------------------
        //remove a user from his game
        public override async Task<Empty> RemoveUserFPlayed(UserModel user2remove, ServerCallContext context) {

            
            if(usersOnGame.Contains(user2remove))
                usersOnGame.Remove(user2remove);
            var val = new List<GameMessage>();
            bool a;
            usersGame.TryRemove(user2remove.UserName,out val);
            usersGameStatue.TryRemove(user2remove.UserName,out a);

            return await Task.FromResult(new Empty());

        }
        //-------------------------------------------------------------------------------------------------
        //check if the user in a game
        public async override Task<boolmessage> CheckUsertInPlayed(UserModel user2check, ServerCallContext context) {

            bool result = false;
            if (usersOnGame.Contains(user2check)) {
                result = true;
            }
            return await Task.FromResult(new boolmessage { Istrue = result });
        }
        //-------------------------------------------------------------------------------------------------
        //check if the game is already updated 
        public async override Task<boolmessage> CheckIfAlreadyUpdatedGame(SerialNumber request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    Game game = (from g in ctx.Games
                                 where g.SerialNumber == request.Serialnumber
                                 select g).First();
                    bool reply = true;
                    if (game.Winner == "" || game.Ongoing == true || game.Draw == true)
                        reply = false;
                    return await Task.FromResult(new boolmessage { Istrue = reply});
                }
            }
            catch (RpcException ex)
            {

                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------
        //update the user by the end of the game
        public async override Task<Empty> UpdateUser(UserModel request, ServerCallContext context)
        {
            try
            {
                using (var ctx = new FourInRowDBContext())
                {
                    ctx.Remove((from u in ctx.Users
                                       where u.UserName.Equals(request.UserName)
                                       select u).First());
                    ctx.SaveChanges();
                    User updateduser = new User
                    {
                        UserName = request.UserName,
                        HashedPassword = request.HashedPassword,
                        NumOfGames = request.NumOfGames,
                        NumOfLoses = request.NumOfLoses,
                        NumOfWins = request.NumOFWins,
                        Points = request.Points
                    };
                    ctx.Users.Add(updateduser);
                    ctx.SaveChanges();
                    return await Task.FromResult(new Empty());
                }
            }
            catch (RpcException ex)
            {

                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
        }
        //-------------------------------------------------------------------------------------------------                
    }
}