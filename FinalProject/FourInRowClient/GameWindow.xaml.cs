using Grpc.Core;
using Grpc.Net.Client;
using GrpcFIRService;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FourInRowClient
{
    public partial class GameWindow : Window
    {
        #region FIELDS
        private int serialnumber = -1;
        private DispatcherTimer BallDown;
        private Ellipse currentCircle;
        private int currentColumn;
        private int currentRow;
        private double colWidthF;
        private double rowHeightF;

        public string UserName { get; set; }
        public string VsUserName { get; set; }
        public bool TurnStatue { get; set; }
        public string FirstPlayer { get; set; }

        private int fills = 0;

        private SolidColorBrush backgroundColor = new SolidColorBrush(Colors.Black);

        public SolidColorBrush UserEllipseColor;
        public SolidColorBrush VsUserEllipseColor;
        public SolidColorBrush animationEllipseColor;


        private static readonly int ROW = 6;
        private static readonly int COLUMN = 7;

        private Ellipse[,] Board = new Ellipse[ROW, COLUMN];

        public GrpcChannel Channel { get; set; }
        public FourInRow.FourInRowClient Client { get; set; }

        public AsyncServerStreamingCall<GameMessage> Listener { get; set; }

        public Func<IAsyncStreamReader<GameMessage>, CancellationToken, Task> ListenerDelegate;
        #endregion
        //---------------------------------------------------------------------------------
        //constructor to initilize client, chanel, serial number and the rest of the fields
        public GameWindow(string userName, string vsUserName, bool userSwitch, int serialnumber, GrpcChannel _Channel, FourInRow.FourInRowClient _Client)
        {
            InitializeComponent();

            this.Title = userName + " Vs " + vsUserName;

            this.serialnumber = serialnumber;
            Channel = _Channel;
            Client = _Client;

            UserName = userName;
            VsUserName = vsUserName;

            TurnStatue = userSwitch;

            initProperty();
            checkAndInitGameAsync();
            BallDown = new DispatcherTimer();
            BallDown.Interval = new TimeSpan(0, 0, 0, 0, 1);
            BallDown.Start();
        }
        //---------------------------------------------------------------------------------
        //check if the game already initilized 
        //and if not initilize one by getting new serial number from the server
        private async void checkAndInitGameAsync()
        {
            try
            {
                var ongoing = (await Client.CheckSerialNumberAsync(new SerialNumber
                { Serialnumber = serialnumber }));
                if (!ongoing.Istrue)
                //check if serial number already exist so we do only 1 game
                {
                    GameModel newgame = new GameModel
                    {
                        Serialnumber = serialnumber,
                        Draw = true,
                        FirstPlayer = UserName,
                        SecondPlayer = VsUserName,
                        Ongoing = true,
                        StartingTime = DateTime.Now.ToString(),
                        Winner = "",
                        Firstplayerpoints = 0,
                        SecondPlayerpoints = 0
                    };
                    await Client.InsertGameAsync(newgame);
                }
            }
            catch (RpcException ex) {
                MessageBox.Show($"{ex.StatusCode}: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //---------------------------------------------------------------------------------        
        //initilize property (caled by the constructor)
        private void initProperty()
        {

            if (TurnStatue)
            {
                UserEllipseColor = new SolidColorBrush(Colors.AliceBlue);
                VsUserEllipseColor = new SolidColorBrush(Colors.DarkBlue);
                SPTurn.Background = VsUserEllipseColor;
                FirstPlayer = UserName;


                ChatMessage startGameMessage = new ChatMessage
                {
                    Type = MessageType.Approved,
                    FromUser = UserName,
                    ToUser = VsUserName,
                    Message = ""
                };
                AsyncServerStreamingCall<GameMessage> Listener =
                    Client.Peer2peerConnect(new GameMessage
                    {
                        Turn = true,
                        CMessage = startGameMessage
                    });

                ListenAsync(Listener.ResponseStream, new CancellationTokenSource().Token);
            }
            else
            {
                UserEllipseColor = new SolidColorBrush(Colors.DarkBlue);
                VsUserEllipseColor = new SolidColorBrush(Colors.AliceBlue);
                SPTurn.Background = UserEllipseColor;
                FirstPlayer = VsUserName;

                ChatMessage startGameMessage = new ChatMessage
                {
                    Type = MessageType.Approved,
                    FromUser = UserName,
                    ToUser = VsUserName,
                    Message = ""
                };

                AsyncServerStreamingCall<GameMessage> Listener =
                       Client.Peer2peerConnect(new GameMessage
                       {
                           Turn = false,
                           CMessage = startGameMessage
                       });

                ListenAsync(Listener.ResponseStream, new CancellationTokenSource().Token);
            }

            ellipse.Fill = UserEllipseColor;

        }
        //---------------------------------------------------------------------------------
        //listener to get messages from other users by the server
        public async Task ListenAsync(IAsyncStreamReader<GameMessage> stream, CancellationToken token)
        {

            string[] rowCol;
            int row;
            int col;
            await foreach (GameMessage info in stream.ReadAllAsync(token))
            {

                if (info.CMessage.Message.Equals("win"))
                {
                    //update winner player
                    UserModel originaluser = (await Client.GetUserByUserNameAsync
                       (new NameString { Name = UserName }));
                    UserModel udateduser = new UserModel
                    {
                        UserName = originaluser.UserName,
                        HashedPassword = originaluser.HashedPassword,
                        NumOfGames = originaluser.NumOfGames + 1,
                        NumOfLoses = originaluser.NumOfLoses,
                        NumOFWins = originaluser.NumOFWins + 1,
                        Points = originaluser.Points + 1000 + checkallcolumns()
                    };
                    await Client.UpdateUserAsync(udateduser);
                    MessageBox.Show("You win");
                    this.Close();

                }
                if (info.CMessage.Message.Equals("lose"))
                {

                    MessageBox.Show("You lose");
                    this.Close();

                }
                if (info.CMessage.Message.Equals("Draw"))
                {
                    MessageBox.Show("Draw");
                    this.Close();

                }
                if (info.CMessage.Type == MessageType.Message && info.CMessage.ToUser == UserName)
                {
                    //MessageBox.Show(UserName);
                    rowCol = info.CMessage.Message.Split(',');
                    row = Int32.Parse(rowCol[0]);
                    col = Int32.Parse(rowCol[1]);
                    Board[row, col].Fill = VsUserEllipseColor;
                    //++fills;
                    animationEllipseColor = VsUserEllipseColor;
                    DrawCircle(row, col);
                    TurnStatue = true;
                    SPTurn.Background = VsUserEllipseColor;
                }
            }
        }
        //---------------------------------------------------------------------------------
        //check if every column has at least one elipse when true return 1000 else 0
        private int checkallcolumns()
        {
            int count = 0;
            for (int i = 0; i < COLUMN; i++)
            {
                for (int j = 0; j < ROW; j++)
                {
                    if (Board[j, i].Fill == UserEllipseColor)
                    {
                        count++;
                        break;
                    }
                }
            }
            return count == COLUMN ? 100 : 0;
        }
        //---------------------------------------------------------------------------------
        // update the game data when someone close not by regular ending game      
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await Client.RemoveUserFPlayedAsync(new UserModel { UserName = UserName });
            GameModel game = await Client.GetGameBySerialNumberAsync(new SerialNumber { Serialnumber = serialnumber });
            //should be in win message
            if (game.Ongoing == false)
            {
                return;
            }

            MessageBox.Show("You lose");
            int firstpoints;
            int secondpoints;
            if (game.FirstPlayer.Equals(game.Winner))
            {
                firstpoints = 1000 + checkallcolumns();
                secondpoints = ((fills - 1) * 10) + checkallcolumns();
            }
            else
            {
                secondpoints = 1000 + checkallcolumns();
                firstpoints = (fills * 10) + checkallcolumns();
            }

            if (game.Ongoing == true)
            {
                await Client.UpdateEndGameAsync(new GameModel
                {
                    Ongoing = false,
                    Winner = VsUserName,
                    Draw = false,
                    Serialnumber = serialnumber,
                    StartingTime = game.StartingTime,
                    FirstPlayer = game.FirstPlayer,
                    SecondPlayer = game.SecondPlayer,
                    Firstplayerpoints = firstpoints,
                    SecondPlayerpoints = secondpoints
                });               
            }
            //update loser player
            UserModel originalvsuser = (await Client.GetUserByUserNameAsync
               (new NameString { Name = UserName }));
            UserModel updatedvsuser = new UserModel
            {
                UserName = originalvsuser.UserName,
                HashedPassword = originalvsuser.HashedPassword,
                NumOfGames = originalvsuser.NumOfGames + 1,
                NumOfLoses = originalvsuser.NumOfLoses + 1,
                NumOFWins = originalvsuser.NumOFWins,
                Points = originalvsuser.Points + fills * 10 + checkallcolumns()
            };
            await Client.UpdateUserAsync(updatedvsuser);

            if ((await Client.CheckUsertInPlayedAsync(new UserModel { UserName = VsUserName })).Istrue)
            {
                ChatMessage winningmessege = new ChatMessage
                {
                    Type = MessageType.Message,
                    FromUser = UserName,
                    ToUser = VsUserName,
                    Message = "win"
                };
                await Client.SendGameMessageAsync(new GameMessage
                {
                    Turn = true,
                    CMessage = winningmessege
                });
            }
        }
        //---------------------------------------------------------------------------------
        //initilize new board to play on 6 * 7
        private void FourInARowBord_Initialized(object sender, EventArgs e)
        {
            FourInARowBord.Background = new SolidColorBrush(Colors.Gray);
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COLUMN; j++)
                {
                    Board[i, j] = new Ellipse
                    {
                        Fill = backgroundColor,
                        Height = 43,
                        Width = 43
                    };
                    Panel.SetZIndex(Board[i, j], 3);

                    Grid.SetColumn(Board[i, j], j);
                    Grid.SetRow(Board[i, j], i);

                    GameBoardGrid.Children.Add(Board[i, j]);
                }
            }
        }
        //---------------------------------------------------------------------------------
        //get the top elipse in the column paramater
        private int getTopEllipse(int column)
        {
            int row = ROW - 1;
            for (int i = 0; i < ROW; i++)
            {
                if (Board[row, column].Fill != backgroundColor)
                {
                    --row;
                }
                else
                {
                    break;
                }
            }
            return row;
        }
        //---------------------------------------------------------------------------------
        //check if the elipse i,j is the fourth elipse in row , column or diagonal
        private bool checkLine(int i, int j)
        {
            int leftSide = j;
            int topSide = i;
            int rightSide = j;

            int leftDiagonal = j;
            int topLeftDiagonal = i;

            int rightDiagonal = j;
            int topRightDiagonal = i;

            int leftSideLineCount = 0;
            int topSideLineCount = 0;

            int leftDiagonalLineCount = 0;
            int rightDiagonalLineCount = 0;
            //horizontal check
            //get left edge
            //m=0,m<=3
            for (int m = 0; m < 3; m++)
            {
                if (leftSide > 0 && Board[i, leftSide - 1].Fill == UserEllipseColor)
                {
                    --leftSide;
                }
                else
                {
                    break;
                }
            }
            for (int c = leftSide; c < leftSide + 4 && c < COLUMN; c++)
            {
                if (Board[i, c].Fill == UserEllipseColor)
                {
                    ++leftSideLineCount;
                }
            }

            for (int r = topSide; r < topSide + 4 && r < ROW; r++)
            {
                if (Board[r, j].Fill == UserEllipseColor)
                {
                    ++topSideLineCount;
                }
            }


            //top left diagonal check
            for (int d = 0; d < 3; d++)
            {
                if (topLeftDiagonal == 0 || leftDiagonal == 0)
                {
                    break;
                }
                if (Board[topLeftDiagonal - 1, leftDiagonal - 1].Fill == UserEllipseColor)
                {
                    --topLeftDiagonal;
                    --leftDiagonal;
                }
                else
                {
                    break;
                }
            }
            for (int d = 0; d < 4; d++)
            {
                if (topLeftDiagonal > ROW - 1 || leftDiagonal > COLUMN - 1)
                {
                    break;
                }
                if (Board[topLeftDiagonal, leftDiagonal].Fill == UserEllipseColor)
                {
                    ++leftDiagonalLineCount;
                }
                ++topLeftDiagonal;
                ++leftDiagonal;
            }
            //top right diagonal check
            for (int d = 0; d < 3; d++)
            {
                if (topRightDiagonal == 0 || rightDiagonal == COLUMN - 1)
                {
                    break;
                }
                if (Board[topRightDiagonal - 1, rightDiagonal + 1].Fill == UserEllipseColor)
                {
                    --topRightDiagonal;
                    ++rightDiagonal;
                }
                else
                {
                    break;
                }
            }

            for (int d = 0; d < 4; d++)
            {
                if (topRightDiagonal > ROW - 1 || rightDiagonal < 0)
                {
                    break;
                }
                if (Board[topRightDiagonal, rightDiagonal].Fill == UserEllipseColor)
                {
                    ++rightDiagonalLineCount;
                }
                ++topRightDiagonal;
                --rightDiagonal;
            }
            return ((leftSideLineCount == 4) || (topSideLineCount == 4) ||
                (leftDiagonalLineCount == 4) || (rightDiagonalLineCount == 4));
        }
        //---------------------------------------------------------------------------------
        //send chat message to the user
        private async Task sendM(ChatMessage message)
        {
            await Client.SendMessageAsync(message);
        }
        //---------------------------------------------------------------------------------
        //handle the clicking on the boarder
        private async void BoardClick(object sender, MouseButtonEventArgs e)
        {
            if (TurnStatue)
            {

                var point = Mouse.GetPosition(GameBoardGrid);

                int row = 0;
                int col = 0;
                int row2 = 0;
                double rowHeight = 0;
                double colWidth = 0;
                double accumulatedHeight = 0.0;
                double accumulatedWidth = 0.0;
                // calc row mouse was over
                foreach (var rowDefinition in GameBoardGrid.RowDefinitions)
                {
                    accumulatedHeight += rowDefinition.ActualHeight;
                    if (accumulatedHeight >= point.Y)
                    {
                        rowHeight = rowDefinition.ActualHeight;
                        rowHeightF = rowHeight;
                        break;
                    }
                    row++;
                }
                // calc col mouse was over
                foreach (var columnDefinition in GameBoardGrid.ColumnDefinitions)
                {
                    accumulatedWidth += columnDefinition.ActualWidth;
                    if (accumulatedWidth >= point.X)
                    {
                        colWidth = columnDefinition.ActualWidth;
                        colWidthF = colWidth;
                        break;
                    }
                    col++;
                }

                if (point.Y < row * rowHeight + 10 || point.Y > (row + 1) * rowHeight - 10 ||
                    point.X < col * colWidth + 10 || point.X > (col + 1) * colWidth - 10)
                {
                    MessageBox.Show("Please click on empty ellipse!");
                    return;
                }

                row2 = row;
                row = getTopEllipse(col);//warning row < 0 value expected
                if (row < 0)
                {
                    MessageBox.Show("Full!, choose another column");
                    return;
                }

                ChatMessage Cordinatesmessage = new ChatMessage
                {
                    Type = MessageType.Message,
                    FromUser = UserName,
                    ToUser = VsUserName,
                    Message = row + "," + col
                };

                currentRow = row;
                currentColumn = col;

                animationEllipseColor = UserEllipseColor;
                Board[currentRow, currentColumn].Fill = animationEllipseColor;
                DrawCircle(row, col);
                Board[row, col].Fill = UserEllipseColor;
                GameModel game = await Client.GetGameBySerialNumberAsync//unhandled exception bassam
                           (new SerialNumber { Serialnumber = serialnumber });
                if (game.Ongoing == false)
                {
                    this.Close();
                }
                //check winning
                if (checkLine(row, col))
                {                   
                    UserModel originaluser = (await Client.GetUserByUserNameAsync
                        (new NameString { Name = UserName }));
                    UserModel udateduser = new UserModel
                    {
                        UserName = originaluser.UserName,
                        HashedPassword = originaluser.HashedPassword,
                        NumOfGames = originaluser.NumOfGames + 1,
                        NumOfLoses = originaluser.NumOfLoses,
                        NumOFWins = originaluser.NumOFWins + 1,
                        Points = originaluser.Points + 1000 + checkallcolumns()
                    };
                    await Client.UpdateUserAsync(udateduser);

                    UserModel originalvsuser = (await Client.GetUserByUserNameAsync
                       (new NameString { Name = VsUserName }));
                    UserModel updatedvsuser = new UserModel
                    {
                        UserName = originalvsuser.UserName,
                        HashedPassword = originalvsuser.HashedPassword,
                        NumOfGames = originalvsuser.NumOfGames + 1,
                        NumOfLoses = originalvsuser.NumOfLoses + 1,
                        NumOFWins = originalvsuser.NumOFWins,
                        Points = originalvsuser.Points + fills * 10 + checkallcolumns()
                    };
                    await Client.UpdateUserAsync(updatedvsuser);

                    if (!(await Client.CheckIfAlreadyUpdatedGameAsync(new SerialNumber { Serialnumber = serialnumber })).Istrue)
                    {
                        GameModel originalgame = await Client.GetGameBySerialNumberAsync
                                (new SerialNumber { Serialnumber = serialnumber });
                        GameModel updatedgame = new GameModel
                        {
                            Serialnumber = serialnumber,
                            Winner = UserName,
                            Draw = false,
                            FirstPlayer = originalgame.FirstPlayer,
                            SecondPlayer = originalgame.SecondPlayer,
                            StartingTime = originalgame.StartingTime,
                            Ongoing = false,
                        };
                        if (updatedgame.FirstPlayer.Equals(UserName))
                        {
                            updatedgame.Firstplayerpoints = 1000 + checkallcolumns();
                            updatedgame.SecondPlayerpoints = (fills - 1) * 10 + checkallcolumns();
                        }
                        else
                        {
                            updatedgame.SecondPlayerpoints = 1000 + checkallcolumns();
                            updatedgame.Firstplayerpoints = (fills - 1) * 10 + checkallcolumns();
                        }
                        await Client.UpdateEndGameAsync(updatedgame);
                    }

                    ChatMessage losegmessege = new ChatMessage
                    {
                        Type = MessageType.Message,
                        FromUser = UserName,
                        ToUser = VsUserName,
                        Message = "lose"
                    };

                    await Client.SendGameMessageAsync(new GameMessage
                    {
                        Turn = false,
                        CMessage = Cordinatesmessage
                    });

                    await Client.SendGameMessageAsync(new GameMessage
                    {
                        Turn = true,
                        CMessage = losegmessege
                    });
                    MessageBox.Show("You win");
                    this.Close();                   
                }
                else
                {
                    //check draw
                    if (checkdraw())
                    {
                        await Client.UpdateEndGameAsync(new GameModel
                        {
                            Draw = true,
                            FirstPlayer = game.FirstPlayer,
                            SecondPlayer = game.SecondPlayer,
                            Firstplayerpoints = fills * 10 + checkallcolumns(),
                            SecondPlayerpoints = fills * 10 + checkallcolumns(),
                            Winner = "",
                            Ongoing = false,
                            Serialnumber = serialnumber,
                            StartingTime = game.StartingTime
                        });
                        await Client.SendGameMessageAsync(new GameMessage
                        {
                            Turn = false,
                            CMessage = Cordinatesmessage
                        });
                        ChatMessage drawmessege = new ChatMessage
                        {
                            Type = MessageType.Message,
                            FromUser = UserName,
                            ToUser = VsUserName,
                            Message = "Draw"
                        };
                        await Client.SendGameMessageAsync(new GameMessage
                        {
                            Turn = true,
                            CMessage = drawmessege
                        });
                       
                        MessageBox.Show("Draw");

                        this.Close();
                    }                   
                    TurnStatue = false;
                    SPTurn.Background = UserEllipseColor;
                    try
                    {
                        await Client.SendGameMessageAsync(new GameMessage
                        {
                            Turn = true,
                            CMessage = Cordinatesmessage
                        });
                    }
                    catch (RpcException ex) {
                        MessageBox.Show($"{ex.StatusCode}: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        //---------------------------------------------------------------------------------
        //check the board is full with elipses and call it draw
        private bool checkdraw()
        {
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COLUMN; j++)
                {
                    if (Board[i, j].Fill != UserEllipseColor && Board[i, j].Fill != VsUserEllipseColor)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //---------------------------------------------------------------------------------        
        //draw the circle in the right place
        private void DrawCircle(int row, int col)
        {
            currentRow = row;
            currentColumn = col;

            Ellipse circle = new Ellipse();
            circle.Height = 43;
            circle.Width = 43;

            circle.Fill = animationEllipseColor;

            Canvas.SetTop(circle, 0);
            Canvas.SetLeft(circle, (currentColumn * colWidthF) + 9);
            FourInARowBord.Children.Add(circle);
            currentCircle = circle;
            BallDown.Tick += DropCircleAnimation;
        }
        //---------------------------------------------------------------------------------
        //make the elipse fall from the top to its place
        private void DropCircleAnimation(object sender, EventArgs e)
        {
            double dropLength = currentRow * rowHeightF + 5;
            int dropRate = 10;
            //SolidColorBrush color;
            if (Canvas.GetTop(currentCircle) < dropLength)
            {
                Canvas.SetTop(currentCircle, Canvas.GetTop(currentCircle) + dropRate);
            }
            else
            {
                BallDown.Tick -= DropCircleAnimation;
                fills++;

                Board[currentRow, currentColumn].Fill = animationEllipseColor;
                FourInARowBord.Children.Remove(currentCircle);
            }
        }
    }
}