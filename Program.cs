using BowlingCalculator.Models;
using System.Text;

namespace BowlingCalculator {
    public class BowlingCalculator {
        public static void Main(string[] args) {
            Console.WriteLine("Welcome to my super cool Bowling Calculator!");
            //Frame testingFrame = new FinalFrame();
            //testingFrame.HitPins(10);
            //testingFrame.SecondLine();

            //StringBuilder sb = new StringBuilder();
            //sb.Append(testingFrame.TopLine());
            //sb.Append(testingFrame.TopLine());
            //sb.Append(testingFrame.TopLine());
            //sb.Append(testingFrame.TopLine());
            //sb.AppendLine(testingFrame.TopLine());

            //sb.Append(testingFrame.SecondLine());
            //sb.Append(testingFrame.SecondLine());
            //sb.Append(testingFrame.SecondLine());
            //sb.Append(testingFrame.SecondLine());
            //sb.AppendLine(testingFrame.SecondLine());

            //sb.Append(testingFrame.ThirdLine());
            //sb.Append(testingFrame.ThirdLine());
            //sb.Append(testingFrame.ThirdLine());
            //sb.Append(testingFrame.ThirdLine());
            //sb.AppendLine(testingFrame.ThirdLine());

            //sb.Append(testingFrame.ScoreFourthLine(270));
            //sb.Append(testingFrame.ScoreFourthLine(20));
            //sb.Append(testingFrame.ScoreFourthLine(2570));
            //sb.Append(testingFrame.ScoreFourthLine(173));
            //sb.AppendLine(testingFrame.ScoreFourthLine(8));

            //sb.Append(testingFrame.FinalLine());
            //sb.Append(testingFrame.FinalLine());
            //sb.Append(testingFrame.FinalLine());
            //sb.Append(testingFrame.FinalLine());
            //sb.AppendLine(testingFrame.FinalLine());

            //Console.WriteLine(sb.ToString());

            Run();
        }






        private static void Run() {
            var running = true;
            var message = "Arrrre youuuu readyyyyyy?! (y)es/(n)o";
            while (running) {
                var response = GetUserInput(message);
                switch (response) {
                    case "y":
                    case "yes":
                        Console.WriteLine("WOOHOO! LETS GO!");
                        //
                        StartGame();
                        //
                        message = "Do you want to go again? (y)es/(n)o";
                        break;
                    case "n":
                    case "no":
                        Console.WriteLine(":(... goodbye");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("That was an invalid response. Please try again :)");
                        break;
                }
            }
        }

        private static string GetUserInput(string message) {
            var response = "";
            var validResponse = false;
            while (!validResponse) {
                Console.WriteLine(message);
                Console.Write("> ");
                response = Console.ReadLine();
                response = response?.Trim().ToLower();
                if (string.IsNullOrEmpty(response)) {
                    Console.WriteLine("You have to give a response. Please try again :)");
                    continue;
                }
                else {
                    validResponse = true;
                }
            }
            return response;
        }

        private static int GetUserNumber(string message) {
            var num = 0;
            var validResponse = false;
            while (!validResponse) {
                var response = GetUserInput(message);
                try {
                    num = int.Parse(response);
                    validResponse = true;
                }
                catch {
                    Console.WriteLine("You have to give a number. Please try again :)");
                }
            }
            return num;
        }

        private static void StartGame() {
            var frames = new IFrame[10];
            //play all our normal frames - don't wanna include the final frame here
            for (int i = 0; i < frames.Length - 1; i++) {
                var frame = frames[i] = new Frame();
                PlayFrame(frame, i + 1);
                PrintScores(frames);
            }
            //play our special case final frame
            var finalFrame = (FinalFrame)(frames[^1] = new FinalFrame());
            PlayFrame(finalFrame, frames.Length);

            PrintScores(frames);
        }

        private static void PlayFrame(IFrame frameToPlay, int frameNumber) {
            Console.WriteLine($"Now playing frame {frameNumber}");
            int i = 1;
            while (frameToPlay.IsPlayable) {
                Console.WriteLine($"This is Round {i++}");
                var validResponse = false;
                while (!validResponse) {
                    int numOfPinsHit = GetUserNumber("How many pins did you hit?");
                    try {
                        frameToPlay.HitPins(numOfPinsHit);
                        validResponse = true;
                    }
                    catch (Exception e) {
                        Console.WriteLine($"That was an invalid number. {e.Message}");
                    }
                }
            }
        }

        private static void PrintScores(IFrame[] frames) {
            var sb = new StringBuilder();
            //first line
            sb.AppendLine(IFrame.FirstLine().Repeat(frames.Length));
            List<int> scores = new();
            //second line
            //We don't want to include the final frame here, so we'll just go to length - 1
            for (int i = 0; i < frames.Length - 1; i++) {
                IFrame frame = frames[i];
                if (frame is null) {
                    for (; i < frames.Length; i++) {
                        sb.Append(IFrame.ThirdLine());
                    }
                    break;
                }
                scores.Add(CalculateScore(frames, i));
                sb.Append(frame.SecondLine());
            }
            //nowww we'll handle the final frame :^)
            var finalFrame = (FinalFrame)frames[^1];
            if (finalFrame != null) {
                scores.Add(finalFrame.Score + finalFrame.BonusScore);
                sb.Append(finalFrame.SecondLine());
            }
            sb.AppendLine();
            //third line
            sb.AppendLine(IFrame.ThirdLine().Repeat(frames.Length));
            //fourth line
            var totalScore = 0;
            foreach (int score in scores) {
                sb.Append(IFrame.ScoreFourthLine(totalScore += score));
            }
            sb.AppendLine();
            //fifth line
            sb.AppendLine(IFrame.FifthFinalLine().Repeat(frames.Length));
            Console.WriteLine(sb.ToString());
        }

        private static int CalculateScore(IFrame[] frames, int index) {
            int extraPoints = CalculateExtraPoints(frames, index);
            return extraPoints + frames[index].Score;
        }

        private static int CalculateExtraPoints(IFrame[] frames, int index) {
            int extraPoints = 0;
            IFrame frame = frames[index];
            if (frame is null) {
                return 0;
            }
            //we're not at the end of the frames
            IFrame nextFrame;
            if (index + 1 < frames.Length && (nextFrame = frames[index + 1]) is not null) {

                IFrame nextNextFrame;
                //if our current was a spare, we only get the first shot of the next frame
                if (frame.WasSpare) {
                    extraPoints = nextFrame.FirstHit;
                }
                else if (frame.WasStrike) { //if we got a strike, we get all the points of the next one instead
                    extraPoints = nextFrame.Score;

                    if (index + 2 < frames.Length && (nextNextFrame = frames[index + 2]) is not null) {

                        if (nextFrame.WasStrike) { //if the next was also strike, howevah, we also get points for the first try of the next next frame.
                                                    //This also means if you get another strike here, you end up with 30 points on that first strike. (x|x|x = 30, x|x|9,1 = 29)
                                                   //                        (current frame -> next frame -> next next frame)
                            extraPoints += nextNextFrame.FirstHit;
                        }
                        //if our next was only a spare, we only get the first shot of the next next frame
                        else if (nextFrame.WasSpare) {
                            extraPoints = nextNextFrame.FirstHit;
                        }
                    }
                }
            }
            return extraPoints;
        }
    }
}
